import { useCallback, useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { DisplayGrid, OrnateDisplayBox } from "../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../feedback";
import { LinkEditor } from "../../linking/LinkEditor";
import { ArchitectureStyleDetailsDto, ReferenceDto } from "../../../interfaces/loreInterfaces";
import {
    addArchitectureStyleLocation,
    getArchitectureStyleById,
    removeArchitectureStyleLocation,
} from "../../../api/architectureStyles";
import { getLocations } from "../../../api/locations";
import { useAuth } from "../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];
const glyph = "🏛";

export default function ArchitectureStyleDetail() {
    const { id } = useParams<{ id: string }>();
    const { t } = useTranslation("cultureDetails");
    const { userInfo } = useAuth();
    const canEdit = userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [item, setItem] = useState<ArchitectureStyleDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);
    const [locCandidates, setLocCandidates] = useState<ReferenceDto[] | null>(null);

    useEffect(() => {
        const styleId = Number(id);
        if (!styleId) {
            setNotFound(true);
            setLoading(false);
            return;
        }
        let cancelled = false;
        setLoading(true);
        getArchitectureStyleById(styleId)
            .then((data) => !cancelled && setItem(data))
            .catch((err) => {
                if (cancelled) return;
                if (err?.response?.status === 404) setNotFound(true);
                else setError(t("detail.loadError"));
            })
            .finally(() => !cancelled && setLoading(false));
        return () => {
            cancelled = true;
        };
    }, [id, t, reloadKey]);

    if (loading) return <LoadingSkeleton variant="block" rows={5} />;
    if (notFound)
        return (
            <EmptyState
                glyph={glyph}
                title={t("detail.notFound")}
                action={
                    <Link to="/storymap/cultures" className={s.backLink}>
                        ← {t("detail.backToCultures")}
                    </Link>
                }
            />
        );
    if (error || !item) return <ErrorState onRetry={refetch} detail={error} />;

    const dash = "—";
    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/cultures">{t("detail.cultures")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span>{item.name}</span>
            </div>
            <div className={s.kicker}>{t("sections.architectureStyle")}</div>
            <h1 className={s.name}>{item.name}</h1>

            <div className={s.facts}>
                <DisplayGrid cols={3}>
                    <OrnateDisplayBox
                        label={t("cultureLabel")}
                        value={
                            item.culture ? (
                                <Link
                                    className={s.refLink}
                                    to={`/storymap/cultures/${item.culture.id}`}
                                >
                                    {item.culture.name}
                                </Link>
                            ) : (
                                dash
                            )
                        }
                    />
                    <OrnateDisplayBox
                        label={t("fields.materialsUsed")}
                        value={item.materialsUsed || dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.designFeatures")}
                        value={item.designFeatures || dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.isFortified")}
                        value={item.isFortified ? "✓" : dash}
                    />
                </DisplayGrid>
            </div>

            {item.description && <p className={s.prose}>{item.description}</p>}

            <div className={s.section}>
                <div className={s.sectionHead}>
                    <span className={s.sectionTitle}>{t("links.typicalLocations")}</span>
                    <span className={s.sectionLine} />
                </div>
                <LinkEditor
                    items={item.typicalLocations}
                    candidates={locCandidates}
                    onLoadCandidates={() =>
                        getLocations(item.worldId).then((ls) =>
                            setLocCandidates(
                                ls.map((l) => ({ id: l.id, name: l.name }))
                            )
                        )
                    }
                    onAdd={(locId) => addArchitectureStyleLocation(item.id, locId)}
                    onRemove={(locId) =>
                        removeArchitectureStyleLocation(item.id, locId)
                    }
                    onChanged={refetch}
                    canEdit={canEdit}
                    linkTo={(locId) => `/storymap/locations/${locId}`}
                    addLabel={t("links.add")}
                    noneLabel={t("none")}
                    pickLabel={t("links.pick")}
                    cancelLabel={t("actions.cancel")}
                    confirmLabel={t("links.confirm")}
                    removeLabel={(name) => t("links.remove", { name })}
                    addFailedLabel={t("links.addFailed")}
                    removeFailedLabel={t("links.removeFailed")}
                />
            </div>
        </div>
    );
}
