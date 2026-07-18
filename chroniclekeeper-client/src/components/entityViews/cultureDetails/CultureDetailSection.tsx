import { useCallback, useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
    Button,
    OrnateCheckbox,
    OrnateField,
    OrnateSelect,
    OrnateTextArea,
    OrnateTextInput,
    SelectOption,
} from "../../ornate";
import { getCultures } from "../../../api/cultures";
import { apiErrorMessage } from "../../../utils/apiError";
import type { CultureDetailDto, Descriptor, FieldDef } from "./descriptors";
import s from "./styles.module.css";

interface Props {
    descriptor: Descriptor;
    worldId: number;
    /** Hub mode: preset + filter by this culture. Omit for world-scoped standalone mode. */
    cultureId?: number;
    canEdit: boolean;
}

type FormValues = Record<string, string | boolean>;

function defaultValues(d: Descriptor, cultureId?: number): FormValues {
    const v: FormValues = { name: "", description: "" };
    for (const f of d.fields) {
        v[f.key] = f.kind === "bool" ? false : f.kind === "number" ? "0" : "";
    }
    v.cultureId = cultureId ? String(cultureId) : "";
    return v;
}

function valuesFromItem(d: Descriptor, item: CultureDetailDto): FormValues {
    const bag = item as unknown as Record<string, unknown>;
    const v: FormValues = {
        name: item.name ?? "",
        description: item.description ?? "",
    };
    for (const f of d.fields) {
        const raw = bag[f.key];
        if (f.kind === "bool") v[f.key] = Boolean(raw);
        else v[f.key] = raw === null || raw === undefined ? "" : String(raw);
    }
    v.cultureId = item.cultureId ? String(item.cultureId) : "";
    return v;
}

function buildPayload(
    d: Descriptor,
    values: FormValues,
    worldId: number
): Record<string, unknown> {
    const p: Record<string, unknown> = {
        name: values.name,
        description: values.description,
        worldId,
        historyId: null,
    };
    const cid = values.cultureId as string;
    p.cultureId = cid === "" ? null : Number(cid);
    for (const f of d.fields) {
        const raw = values[f.key];
        if (f.kind === "bool") p[f.key] = Boolean(raw);
        else if (f.kind === "number") p[f.key] = Number(raw) || 0;
        else if (f.kind === "select")
            p[f.key] = raw === "" ? null : Number(raw);
        else p[f.key] = raw;
    }
    return p;
}

export default function CultureDetailSection({
    descriptor,
    worldId,
    cultureId,
    canEdit,
}: Props) {
    const { t } = useTranslation("cultureDetails");
    const [items, setItems] = useState<CultureDetailDto[] | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    // null = closed, "new" = create, number = editing that id
    const [editing, setEditing] = useState<"new" | number | null>(null);
    const [values, setValues] = useState<FormValues>(() =>
        defaultValues(descriptor, cultureId)
    );
    const [saving, setSaving] = useState(false);
    const [formError, setFormError] = useState<string | null>(null);
    const [fkOptions, setFkOptions] = useState<Record<string, SelectOption[]>>(
        {}
    );
    const [cultureOptions, setCultureOptions] = useState<SelectOption[]>([]);

    useEffect(() => {
        let cancelled = false;
        setError(null);
        descriptor
            .list(worldId)
            .then((all) => {
                if (cancelled) return;
                setItems(
                    cultureId != null
                        ? all.filter((x) => x.cultureId === cultureId)
                        : all
                );
            })
            .catch(
                (e) =>
                    !cancelled && setError(apiErrorMessage(e, t("actions.failed")))
            );
        return () => {
            cancelled = true;
        };
    }, [descriptor, worldId, cultureId, reloadKey, t]);

    const openForm = useCallback(
        (item?: CultureDetailDto) => {
            setFormError(null);
            setValues(
                item
                    ? valuesFromItem(descriptor, item)
                    : defaultValues(descriptor, cultureId)
            );
            setEditing(item ? item.id : "new");
            // Lazily load FK select options + (world mode) culture options.
            for (const f of descriptor.fields) {
                if (f.kind === "select" && f.loadOptions && !fkOptions[f.key]) {
                    f.loadOptions(worldId)
                        .then((opts) =>
                            setFkOptions((prev) => ({ ...prev, [f.key]: opts }))
                        )
                        .catch(() => {});
                }
            }
            if (cultureId == null && cultureOptions.length === 0) {
                getCultures(worldId)
                    .then((cs) =>
                        setCultureOptions(
                            cs.map((c) => ({
                                value: String(c.id),
                                label: c.name,
                            }))
                        )
                    )
                    .catch(() => {});
            }
        },
        [descriptor, worldId, cultureId, fkOptions, cultureOptions.length]
    );

    const setField = (key: string, val: string | boolean) =>
        setValues((prev) => ({ ...prev, [key]: val }));

    const save = async () => {
        setSaving(true);
        setFormError(null);
        try {
            const payload = buildPayload(descriptor, values, worldId);
            if (editing === "new") {
                await descriptor.create(payload);
            } else if (typeof editing === "number") {
                const { worldId: _omit, ...rest } = payload;
                void _omit;
                await descriptor.update(editing, rest);
            }
            setEditing(null);
            refetch();
        } catch (e) {
            setFormError(apiErrorMessage(e, t("actions.failed")));
        } finally {
            setSaving(false);
        }
    };

    const remove = async (item: CultureDetailDto) => {
        if (!window.confirm(t("actions.confirmDelete", { name: item.name })))
            return;
        try {
            await descriptor.remove(item.id);
            refetch();
        } catch (e) {
            setError(apiErrorMessage(e, t("actions.failed")));
        }
    };

    const renderField = (f: FieldDef) => {
        const label = t(`fields.${f.key}`);
        if (f.kind === "bool") {
            return (
                <OrnateCheckbox
                    key={f.key}
                    label={label}
                    checked={Boolean(values[f.key])}
                    onChange={(e) => setField(f.key, e.target.checked)}
                />
            );
        }
        if (f.kind === "select") {
            return (
                <OrnateField key={f.key} label={label}>
                    <OrnateSelect
                        value={String(values[f.key] ?? "")}
                        onChange={(e) => setField(f.key, e.target.value)}
                        options={fkOptions[f.key] ?? []}
                        placeholder={t("selectNone")}
                    />
                </OrnateField>
            );
        }
        if (f.kind === "textarea") {
            return (
                <OrnateField key={f.key} label={label}>
                    <OrnateTextArea
                        value={String(values[f.key] ?? "")}
                        onChange={(e) => setField(f.key, e.target.value)}
                        rows={3}
                    />
                </OrnateField>
            );
        }
        return (
            <OrnateField key={f.key} label={label}>
                <OrnateTextInput
                    type={f.kind === "number" ? "number" : "text"}
                    value={String(values[f.key] ?? "")}
                    onChange={(e) => setField(f.key, e.target.value)}
                />
            </OrnateField>
        );
    };

    return (
        <div className={s.section}>
            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>
                    {t(`sections.${descriptor.key}`)}
                </span>
                <span className={s.sectionLine} />
                {canEdit && editing == null && (
                    <Button
                        variant="ghost"
                        size="sm"
                        onClick={() => openForm()}
                    >
                        + {t("actions.add")}
                    </Button>
                )}
            </div>

            {error && <p className={s.error}>{error}</p>}

            {items && items.length === 0 && editing == null && (
                <p className={s.none}>{t("none")}</p>
            )}

            {items && items.length > 0 && (
                <ul className={s.list}>
                    {items.map((item) => (
                        <li key={item.id} className={s.row}>
                            <span className={s.rowName}>
                                {descriptor.detailRoute ? (
                                    <Link
                                        to={`${descriptor.detailRoute}/${item.id}`}
                                        className={s.rowLink}
                                    >
                                        {item.name}
                                    </Link>
                                ) : (
                                    item.name
                                )}
                            </span>
                            {canEdit && editing == null && (
                                <span className={s.rowActions}>
                                    <button
                                        type="button"
                                        className={s.linkBtn}
                                        onClick={() => openForm(item)}
                                    >
                                        {t("actions.edit")}
                                    </button>
                                    <button
                                        type="button"
                                        className={s.linkBtn}
                                        onClick={() => remove(item)}
                                    >
                                        {t("actions.delete")}
                                    </button>
                                </span>
                            )}
                        </li>
                    ))}
                </ul>
            )}

            {editing != null && (
                <div className={s.form}>
                    <OrnateField label={t("fields.name")} required>
                        <OrnateTextInput
                            value={String(values.name ?? "")}
                            onChange={(e) => setField("name", e.target.value)}
                        />
                    </OrnateField>
                    <OrnateField label={t("fields.description")}>
                        <OrnateTextArea
                            value={String(values.description ?? "")}
                            onChange={(e) =>
                                setField("description", e.target.value)
                            }
                            rows={2}
                        />
                    </OrnateField>
                    {cultureId == null && (
                        <OrnateField label={t("cultureLabel")}>
                            <OrnateSelect
                                value={String(values.cultureId ?? "")}
                                onChange={(e) =>
                                    setField("cultureId", e.target.value)
                                }
                                options={cultureOptions}
                                placeholder={t("selectNone")}
                            />
                        </OrnateField>
                    )}
                    {descriptor.fields.map(renderField)}

                    {formError && <p className={s.error}>{formError}</p>}

                    <div className={s.formActions}>
                        <Button onClick={save} disabled={saving}>
                            {t("actions.save")}
                        </Button>
                        <Button
                            variant="ghost"
                            onClick={() => setEditing(null)}
                            disabled={saving}
                        >
                            {t("actions.cancel")}
                        </Button>
                        {descriptor.detailRoute && typeof editing === "number" && (
                            <Link
                                to={`${descriptor.detailRoute}/${editing}`}
                                className={s.manageLink}
                            >
                                {t("actions.manageLinks")} →
                            </Link>
                        )}
                    </div>
                </div>
            )}
        </div>
    );
}
