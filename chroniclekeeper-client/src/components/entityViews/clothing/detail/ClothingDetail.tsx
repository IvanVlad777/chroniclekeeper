import { useCallback, useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { DisplayGrid, OrnateDisplayBox } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { ReadRefList } from "../../../linking/ReadRefList";
import { ClothingDetailsDto } from "../../../../interfaces/loreInterfaces";
import { getClothingById } from "../../../../api/clothing";
import s from "../../detailShared.module.css";

export default function ClothingDetail() {
    const { id } = useParams<{ id: string }>();
    const { t } = useTranslation("cultureDetails");
    const [item, setItem] = useState<ClothingDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    useEffect(() => {
        const cid = Number(id);
        if (!cid) {
            setNotFound(true);
            setLoading(false);
            return;
        }
        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);
        getClothingById(cid)
            .then((data) => {
                if (!cancelled) setItem(data);
            })
            .catch((err) => {
                if (cancelled) return;
                if (err?.response?.status === 404) setNotFound(true);
                else setError(t("detail.loadError"));
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });
        return () => {
            cancelled = true;
        };
    }, [id, t, reloadKey]);

    if (loading) return <LoadingSkeleton variant="block" rows={5} />;
    if (notFound) {
        return (
            <EmptyState glyph="⚑" title={t("detail.notFound")} />
        );
    }
    if (error || !item) return <ErrorState onRetry={refetch} detail={error} />;

    const dash = "—";
    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                {item.culture && (
                    <>
                        <Link to={`/storymap/cultures/${item.culture.id}`}>
                            {item.culture.name}
                        </Link>
                        <span className={s.breadcrumbSep}>/</span>
                    </>
                )}
                <span className={s.breadcrumbCurrent}>{item.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>🧥 {t("clothing.single")}</div>
                    <h1 className={s.name}>{item.name}</h1>
                </div>
            </div>

            <div className={s.facts}>
                <DisplayGrid cols={3}>
                    <OrnateDisplayBox label={t("clothing.fields.clothingType")} value={item.clothingType || dash} />
                    <OrnateDisplayBox label={t("clothing.fields.materials")} value={item.materials || dash} />
                    <OrnateDisplayBox
                        label={t("clothing.fields.culture")}
                        value={
                            item.culture ? (
                                <Link className={s.memberLink} to={`/storymap/cultures/${item.culture.id}`}>
                                    {item.culture.name}
                                </Link>
                            ) : (
                                dash
                            )
                        }
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("detail.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {item.description ? (
                <p className={s.prose}>{item.description}</p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("detail.none")}</p>
            )}

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("clothing.wearers")}</span>
                <span className={s.sectionLine} />
            </div>
            <ReadRefList
                items={item.wearers}
                linkTo={(cid) => `/storymap/characters/${cid}`}
                noneLabel={t("detail.none")}
            />
        </div>
    );
}
