import { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import {
    Button,
    OrnateCheckbox,
    OrnateField,
    OrnateTextInput,
    Tag,
} from "../../../ornate";
import { PrivilegeLawDto } from "../../../../interfaces/loreInterfaces";
import {
    createPrivilegeLaw,
    deletePrivilegeLaw,
    getPrivilegeLaws,
    updatePrivilegeLaw,
} from "../../../../api/privilegeLaws";
import { apiErrorMessage } from "../../../../utils/apiError";
import s from "./styles.module.css";

interface DraftState {
    name: string;
    grantsLegalImmunity: boolean;
    grantsLandOwnershipRights: boolean;
    allowsPrivateArmies: boolean;
}

const emptyDraft: DraftState = {
    name: "",
    grantsLegalImmunity: false,
    grantsLandOwnershipRights: false,
    allowsPrivateArmies: false,
};

interface Props {
    socialClassId: number;
    canEdit: boolean;
}

/** Inline CRUD za zakone o privilegijama nekog društvenog sloja (JobRank obrazac). */
export default function PrivilegeLawsSection({ socialClassId, canEdit }: Props) {
    const { t } = useTranslation("socialClass");
    const [laws, setLaws] = useState<PrivilegeLawDto[]>([]);
    const [editing, setEditing] = useState<number | "new" | null>(null);
    const [draft, setDraft] = useState<DraftState>(emptyDraft);
    const [error, setError] = useState<string | null>(null);
    const [busy, setBusy] = useState(false);

    const setD = <K extends keyof DraftState>(key: K, value: DraftState[K]) =>
        setDraft((d) => ({ ...d, [key]: value }));

    useEffect(() => {
        let cancelled = false;
        getPrivilegeLaws(undefined, socialClassId)
            .then((data) => {
                if (!cancelled) setLaws(data);
            })
            .catch((err) =>
                console.error("Failed to load privilege laws:", err)
            );
        return () => {
            cancelled = true;
        };
    }, [socialClassId]);

    function startNew() {
        setDraft(emptyDraft);
        setError(null);
        setEditing("new");
    }

    function startEdit(law: PrivilegeLawDto) {
        setDraft({
            name: law.name,
            grantsLegalImmunity: law.grantsLegalImmunity,
            grantsLandOwnershipRights: law.grantsLandOwnershipRights,
            allowsPrivateArmies: law.allowsPrivateArmies,
        });
        setError(null);
        setEditing(law.id);
    }

    async function save() {
        if (!draft.name.trim()) return;
        setBusy(true);
        setError(null);
        try {
            if (editing === "new") {
                const created = await createPrivilegeLaw({
                    name: draft.name.trim(),
                    description: "",
                    socialClassId,
                    grantsLegalImmunity: draft.grantsLegalImmunity,
                    grantsLandOwnershipRights: draft.grantsLandOwnershipRights,
                    allowsPrivateArmies: draft.allowsPrivateArmies,
                    historyId: null,
                });
                setLaws((ls) => [...ls, created]);
            } else if (typeof editing === "number") {
                const updated = await updatePrivilegeLaw(editing, {
                    name: draft.name.trim(),
                    description: "",
                    grantsLegalImmunity: draft.grantsLegalImmunity,
                    grantsLandOwnershipRights: draft.grantsLandOwnershipRights,
                    allowsPrivateArmies: draft.allowsPrivateArmies,
                    historyId: null,
                });
                setLaws((ls) =>
                    ls.map((l) => (l.id === updated.id ? updated : l))
                );
            }
            setEditing(null);
        } catch (err) {
            console.error("Failed to save privilege law:", err);
            setError(apiErrorMessage(err, t("privilegeLaws.saveFailed")));
        } finally {
            setBusy(false);
        }
    }

    async function remove(id: number) {
        if (!window.confirm(t("privilegeLaws.deleteConfirm"))) return;
        setBusy(true);
        setError(null);
        try {
            await deletePrivilegeLaw(id);
            setLaws((ls) => ls.filter((l) => l.id !== id));
        } catch (err) {
            console.error("Failed to delete privilege law:", err);
            setError(apiErrorMessage(err, t("privilegeLaws.deleteFailed")));
        } finally {
            setBusy(false);
        }
    }

    const traits = (l: PrivilegeLawDto) =>
        [
            l.grantsLegalImmunity && t("privilegeLaws.grantsLegalImmunity"),
            l.grantsLandOwnershipRights &&
                t("privilegeLaws.grantsLandOwnershipRights"),
            l.allowsPrivateArmies && t("privilegeLaws.allowsPrivateArmies"),
        ].filter((v): v is string => Boolean(v));

    return (
        <>
            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>
                    {t("privilegeLaws.label")}
                </span>
                <span className={s.sectionLine} />
                <span className={s.sectionStar}>✦</span>
            </div>

            {laws.length === 0 && editing !== "new" && (
                <p className={s.none}>{t("privilegeLaws.none")}</p>
            )}

            <div className={s.lawList}>
                {laws.map((law) =>
                    editing === law.id ? (
                        <PrivilegeLawEditor
                            key={law.id}
                            draft={draft}
                            setD={setD}
                            busy={busy}
                            onSave={save}
                            onCancel={() => setEditing(null)}
                            t={t}
                        />
                    ) : (
                        <div key={law.id} className={s.lawRow}>
                            <span className={s.lawName}>{law.name}</span>
                            <span className={s.lawTraits}>
                                {traits(law).length ? (
                                    traits(law).map((tr) => (
                                        <Tag key={tr} tone="neutral">
                                            {tr}
                                        </Tag>
                                    ))
                                ) : (
                                    <span className={s.muted}>—</span>
                                )}
                            </span>
                            {canEdit && (
                                <span className={s.lawActions}>
                                    <Button
                                        variant="ghost"
                                        onClick={() => startEdit(law)}
                                    >
                                        {t("privilegeLaws.edit")}
                                    </Button>
                                    <Button
                                        variant="danger"
                                        disabled={busy}
                                        onClick={() => remove(law.id)}
                                    >
                                        {t("privilegeLaws.delete")}
                                    </Button>
                                </span>
                            )}
                        </div>
                    )
                )}

                {editing === "new" && (
                    <PrivilegeLawEditor
                        draft={draft}
                        setD={setD}
                        busy={busy}
                        onSave={save}
                        onCancel={() => setEditing(null)}
                        t={t}
                    />
                )}
            </div>

            {error && (
                <p className={s.formError} role="alert">
                    {error}
                </p>
            )}

            {canEdit && editing === null && (
                <Button variant="ghost" onClick={startNew}>
                    {t("privilegeLaws.add")}
                </Button>
            )}
        </>
    );
}

interface EditorProps {
    draft: DraftState;
    setD: <K extends keyof DraftState>(key: K, value: DraftState[K]) => void;
    busy: boolean;
    onSave: () => void;
    onCancel: () => void;
    t: (key: string) => string;
}

function PrivilegeLawEditor({ draft, setD, busy, onSave, onCancel, t }: EditorProps) {
    return (
        <div className={s.lawEditor}>
            <OrnateField label={t("privilegeLaws.name")} required>
                <OrnateTextInput
                    value={draft.name}
                    maxLength={100}
                    onChange={(e) => setD("name", e.target.value)}
                />
            </OrnateField>
            <div className={s.lawChecks}>
                <OrnateCheckbox
                    label={t("privilegeLaws.grantsLegalImmunity")}
                    checked={draft.grantsLegalImmunity}
                    onChange={(e) => setD("grantsLegalImmunity", e.target.checked)}
                />
                <OrnateCheckbox
                    label={t("privilegeLaws.grantsLandOwnershipRights")}
                    checked={draft.grantsLandOwnershipRights}
                    onChange={(e) =>
                        setD("grantsLandOwnershipRights", e.target.checked)
                    }
                />
                <OrnateCheckbox
                    label={t("privilegeLaws.allowsPrivateArmies")}
                    checked={draft.allowsPrivateArmies}
                    onChange={(e) => setD("allowsPrivateArmies", e.target.checked)}
                />
            </div>
            <div className={s.lawEditorActions}>
                <Button variant="ghost" disabled={busy} onClick={onCancel}>
                    {t("privilegeLaws.cancel")}
                </Button>
                <Button disabled={busy || !draft.name.trim()} onClick={onSave}>
                    {t("privilegeLaws.save")}
                </Button>
            </div>
        </div>
    );
}
