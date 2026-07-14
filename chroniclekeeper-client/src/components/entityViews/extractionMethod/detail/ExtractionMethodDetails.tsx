import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, DisplayGrid, OrnateDisplayBox } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { ExtractionMethodDetailsDto } from "../../../../interfaces/loreInterfaces";
import { getExtractionMethodById } from "../../../../api/extractionMethods";
import { useAuth } from "../../../../hooks/useAuth";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];
const glyph = "⛏";

export default function ExtractionMethodDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("extractionMethod");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [extractionMethod, setExtractionMethod] =
        useState<ExtractionMethodDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        const extractionMethodId = Number(id);
        if (!extractionMethodId) {
            setNotFound(true);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);

        getExtractionMethodById(extractionMethodId)
            .then((data) => {
                if (!cancelled) setExtractionMethod(data);
            })
            .catch((err) => {
                console.error("Failed to load extraction method:", err);
                if (cancelled) return;
                if (err?.response?.status === 404) {
                    setNotFound(true);
                } else {
                    setError(t("loaderror"));
                }
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });

        return () => {
            cancelled = true;
        };
    }, [id, t, reloadKey]);

    if (loading) return <LoadingSkeleton variant="block" rows={6} />;
    if (notFound) {
        return (
            <EmptyState
                glyph={glyph}
                title={t("notfound")}
                action={
                    <Link
                        to="/storymap/extraction-methods"
                        className={s.backLink}
                    >
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !extractionMethod) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/extraction-methods">{t("listTitle")}</Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>
                    {extractionMethod.name}
                </span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>
                        {extractionMethod.methodType || t("listTitle")}
                    </div>
                    <h1 className={s.name}>{extractionMethod.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(
                                `/storymap/extraction-methods/${extractionMethod.id}/edit`
                            )
                        }
                    >
                        {t("form.edit")}
                    </Button>
                )}
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={3}>
                    <OrnateDisplayBox
                        label={t("fields.methodType")}
                        value={extractionMethod.methodType || dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.isSustainable")}
                        value={extractionMethod.isSustainable ? "✓" : dash}
                    />
                    <OrnateDisplayBox
                        label={t("form.history")}
                        value={
                            extractionMethod.history ? (
                                <Link
                                    className={s.refLink}
                                    to={`/storymap/histories/${extractionMethod.history.id}`}
                                >
                                    {extractionMethod.history.name}
                                </Link>
                            ) : (
                                dash
                            )
                        }
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {extractionMethod.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>
                    {extractionMethod.description}
                </p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            {/* Read-only: resursi koji koriste ovu metodu */}
            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>
                    {t("resourcesExtracted.label")}
                </span>
                <span className={s.sectionLine} />
            </div>
            {extractionMethod.resourcesExtracted.length === 0 ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                extractionMethod.resourcesExtracted.map((r) => (
                    <div key={r.id} className={s.childRow}>
                        <Link
                            className={s.refLink}
                            to={`/storymap/natural-resources/${r.id}`}
                        >
                            {r.name}
                        </Link>
                    </div>
                ))
            )}
        </div>
    );
}
