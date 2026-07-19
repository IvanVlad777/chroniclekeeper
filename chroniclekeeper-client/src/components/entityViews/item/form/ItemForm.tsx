import { useEffect, useState, type FormEvent } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
    Button,
    OrnateCheckbox,
    OrnateField,
    OrnateSelect,
    OrnateTextArea,
    OrnateTextInput,
} from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import {
    EntityPicker,
    type EntityOption,
} from "../../../quickCreate/EntityPicker";
import {
    createItem,
    deleteItem,
    getItemById,
    updateItem,
} from "../../../../api/items";
import { getCharacters } from "../../../../api/characters";
import { getLocations } from "../../../../api/locations";
import { getFactions } from "../../../../api/factions";
import {
    ItemCategory,
    ItemRarity,
    ItemUpdateDto,
    itemCategories,
    itemRarities,
} from "../../../../interfaces/loreInterfaces";
import { useWorld } from "../../../../hooks/useWorld";
import { useAuth } from "../../../../hooks/useAuth";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

interface FormState {
    name: string;
    description: string;
    category: ItemCategory;
    isUnique: boolean;
    material: string;
    specialProperties: string;
    rarity: ItemRarity;
    currentOwnerId: string;
    storedAtId: string;
    factionId: string;
}

const emptyForm: FormState = {
    name: "",
    description: "",
    category: "Weapon",
    isUnique: false,
    material: "",
    specialProperties: "",
    rarity: "Common",
    currentOwnerId: "",
    storedAtId: "",
    factionId: "",
};

const toId = (v: string): number | null => (v ? Number(v) : null);

function toDto(f: FormState): ItemUpdateDto {
    return {
        name: f.name.trim(),
        description: f.description,
        category: f.category,
        isUnique: f.isUnique,
        material: f.material,
        specialProperties: f.specialProperties,
        rarity: f.rarity,
        currentOwnerId: toId(f.currentOwnerId),
        storedAtId: toId(f.storedAtId),
        factionId: toId(f.factionId),
    };
}

/** Zajednička forma za /items/new i /items/:id/edit. */
export default function ItemForm() {
    const { id } = useParams<{ id: string }>();
    const editId = id ? Number(id) : null;
    const isEdit = editId != null;

    const navigate = useNavigate();
    const { t } = useTranslation("item");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canCreate =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;
    const canDelete = canCreate;

    const [form, setForm] = useState<FormState>(emptyForm);
    const [characterOptions, setCharacterOptions] = useState<EntityOption[]>([]);
    const [locationOptions, setLocationOptions] = useState<EntityOption[]>([]);
    const [factionOptions, setFactionOptions] = useState<EntityOption[]>([]);
    const [loading, setLoading] = useState(true);
    const [loadError, setLoadError] = useState<string | null>(null);
    const [saveError, setSaveError] = useState<string | null>(null);
    const [busy, setBusy] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);

    const set = <K extends keyof FormState>(key: K, value: FormState[K]) =>
        setForm((f) => ({ ...f, [key]: value }));

    useEffect(() => {
        if (worldLoading || !selectedWorld) return;

        let cancelled = false;
        setLoading(true);
        setLoadError(null);

        Promise.all([
            getCharacters(selectedWorld.id),
            getLocations(selectedWorld.id),
            getFactions(selectedWorld.id),
        ])
            .then(async ([chars, locs, facs]) => {
                if (cancelled) return;
                setCharacterOptions(
                    chars.map((c) => ({ value: c.id, label: c.name }))
                );
                setLocationOptions(
                    locs.map((l) => ({ value: l.id, label: l.name }))
                );
                setFactionOptions(
                    facs.map((f) => ({ value: f.id, label: f.name }))
                );
                if (isEdit) {
                    const i = await getItemById(editId);
                    if (cancelled) return;
                    setForm({
                        name: i.name ?? "",
                        description: i.description ?? "",
                        category: (i.category as ItemCategory) ?? "Weapon",
                        isUnique: i.isUnique,
                        material: i.material ?? "",
                        specialProperties: i.specialProperties ?? "",
                        rarity: (i.rarity as ItemRarity) ?? "Common",
                        currentOwnerId: i.currentOwnerId
                            ? String(i.currentOwnerId)
                            : "",
                        storedAtId: i.storedAtId ? String(i.storedAtId) : "",
                        factionId: i.factionId ? String(i.factionId) : "",
                    });
                }
            })
            .catch((err) => {
                console.error("Failed to load item form data:", err);
                if (!cancelled) setLoadError(t("loaderror"));
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });

        return () => {
            cancelled = true;
        };
    }, [selectedWorld, worldLoading, isEdit, editId, t, reloadKey]);

    async function onSubmit(e: FormEvent) {
        e.preventDefault();
        if (!selectedWorld) return;
        if (!form.name.trim()) {
            setSaveError(t("form.requiredMissing"));
            return;
        }
        setSaveError(null);
        setBusy(true);
        try {
            let targetId: number;
            if (isEdit) {
                await updateItem(editId, toDto(form));
                targetId = editId;
            } else {
                const created = await createItem({
                    ...toDto(form),
                    worldId: selectedWorld.id,
                });
                targetId = created.id;
            }
            navigate(`/storymap/items/${targetId}`);
        } catch (err) {
            console.error("Failed to save item:", err);
            setSaveError(apiErrorMessage(err, t("form.saveFailed")));
        } finally {
            setBusy(false);
        }
    }

    async function onDelete() {
        if (!isEdit) return;
        if (!window.confirm(t("form.deleteConfirm", { name: form.name }))) {
            return;
        }
        setSaveError(null);
        setBusy(true);
        try {
            await deleteItem(editId);
            navigate("/storymap/items");
        } catch (err) {
            console.error("Failed to delete item:", err);
            setSaveError(apiErrorMessage(err, t("form.deleteFailed")));
            setBusy(false);
        }
    }

    if (worldLoading || loading) {
        return <LoadingSkeleton variant="block" rows={8} />;
    }
    if (!selectedWorld) {
        return (
            <EmptyState
                glyph="⚔"
                title={t("states.noWorldTitle", { ns: "common" })}
                text={t("states.noWorldText", { ns: "common" })}
            />
        );
    }
    if (loadError) {
        return (
            <ErrorState
                onRetry={() => setReloadKey((k) => k + 1)}
                detail={loadError}
            />
        );
    }

    return (
        <form className={s.page} onSubmit={onSubmit} noValidate>
            <h1 className={s.title}>
                {isEdit ? t("form.editTitle") : t("form.newTitle")}
            </h1>
            <p className={s.note}>{t("form.requiredNote")}</p>

            <div className={s.grid}>
                <div className={s.col}>
                    <OrnateField label={t("columns.name")} required>
                        <OrnateTextInput
                            value={form.name}
                            display
                            maxLength={100}
                            onChange={(e) => set("name", e.target.value)}
                        />
                    </OrnateField>
                    <OrnateField label={t("fields.description")}>
                        <OrnateTextArea
                            value={form.description}
                            rows={6}
                            maxLength={4000}
                            onChange={(e) =>
                                set("description", e.target.value)
                            }
                        />
                    </OrnateField>
                    <OrnateField label={t("fields.material")}>
                        <OrnateTextInput
                            value={form.material}
                            maxLength={100}
                            onChange={(e) => set("material", e.target.value)}
                        />
                    </OrnateField>
                    <OrnateField label={t("fields.specialProperties")}>
                        <OrnateTextArea
                            value={form.specialProperties}
                            rows={3}
                            maxLength={500}
                            onChange={(e) =>
                                set("specialProperties", e.target.value)
                            }
                        />
                    </OrnateField>
                    <OrnateCheckbox
                        label={t("fields.isUnique")}
                        checked={form.isUnique}
                        onChange={(e) => set("isUnique", e.target.checked)}
                    />
                </div>

                <div className={s.col}>
                    <OrnateField label={t("columns.category")} required>
                        <OrnateSelect
                            value={form.category}
                            onChange={(e) =>
                                set("category", e.target.value as ItemCategory)
                            }
                        >
                            {itemCategories.map((c) => (
                                <option key={c} value={c}>
                                    {t(`categories.${c}`)}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateField label={t("columns.rarity")} required>
                        <OrnateSelect
                            value={form.rarity}
                            onChange={(e) =>
                                set("rarity", e.target.value as ItemRarity)
                            }
                        >
                            {itemRarities.map((r) => (
                                <option key={r} value={r}>
                                    {t(`rarities.${r}`)}
                                </option>
                            ))}
                        </OrnateSelect>
                    </OrnateField>
                    <OrnateField label={t("fields.currentOwner")}>
                        <EntityPicker
                            kind="character"
                            worldId={selectedWorld.id}
                            canCreate={canCreate}
                            noneLabel={t("none")}
                            value={form.currentOwnerId}
                            options={characterOptions}
                            onChange={(v) => set("currentOwnerId", v)}
                            onCreated={(c) =>
                                setCharacterOptions((prev) => [
                                    ...prev,
                                    { value: c.id, label: c.name },
                                ])
                            }
                        />
                    </OrnateField>
                    <OrnateField label={t("fields.storedAt")}>
                        <EntityPicker
                            kind="location"
                            worldId={selectedWorld.id}
                            canCreate={canCreate}
                            noneLabel={t("none")}
                            value={form.storedAtId}
                            options={locationOptions}
                            onChange={(v) => set("storedAtId", v)}
                            onCreated={(l) =>
                                setLocationOptions((prev) => [
                                    ...prev,
                                    { value: l.id, label: l.name },
                                ])
                            }
                        />
                    </OrnateField>
                    <OrnateField label={t("fields.faction")}>
                        <EntityPicker
                            kind="faction"
                            worldId={selectedWorld.id}
                            canCreate={canCreate}
                            noneLabel={t("none")}
                            value={form.factionId}
                            options={factionOptions}
                            onChange={(v) => set("factionId", v)}
                            onCreated={(f) =>
                                setFactionOptions((prev) => [
                                    ...prev,
                                    { value: f.id, label: f.name },
                                ])
                            }
                        />
                    </OrnateField>
                </div>
            </div>

            {saveError && (
                <p className={s.formError} role="alert">
                    {saveError}
                </p>
            )}

            <div className={s.footer}>
                {isEdit && canDelete && (
                    <Button
                        variant="danger"
                        disabled={busy}
                        onClick={onDelete}
                    >
                        {t("form.delete")}
                    </Button>
                )}
                <span className={s.footerSpacer} />
                <Button
                    variant="ghost"
                    disabled={busy}
                    onClick={() =>
                        navigate(
                            isEdit
                                ? `/storymap/items/${editId}`
                                : "/storymap/items"
                        )
                    }
                >
                    {t("form.cancel")}
                </Button>
                <Button type="submit" disabled={busy}>
                    {busy ? t("form.saving") : t("form.save")}
                </Button>
            </div>
        </form>
    );
}
