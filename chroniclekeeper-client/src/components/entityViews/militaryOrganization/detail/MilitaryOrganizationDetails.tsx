import { useCallback, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, DisplayGrid, OrnateDisplayBox } from "../../../ornate";
import { EmptyState, ErrorState, LoadingSkeleton } from "../../../feedback";
import { LinkEditor } from "../../../linking/LinkEditor";
import { HistoryBlock } from "../../../history/HistoryBlock";
import {
    FactionDto,
    MilitaryOrganizationDetailsDto,
} from "../../../../interfaces/loreInterfaces";
import {
    addOrganizationFaction,
    getMilitaryOrganizationById,
    removeOrganizationFaction,
} from "../../../../api/militaryOrganizations";
import { getFactions } from "../../../../api/factions";
import { useAuth } from "../../../../hooks/useAuth";
import { editorRoles } from "../../../shell/roles";
import s from "./styles.module.css";

const glyph = "🛡";

export default function MilitaryOrganizationDetails() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { t } = useTranslation("militaryOrganization");
    const { userInfo } = useAuth();
    const canEdit =
        userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;

    const [organization, setOrganization] =
        useState<MilitaryOrganizationDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [notFound, setNotFound] = useState(false);
    const [reloadKey, setReloadKey] = useState(0);
    const refetch = useCallback(() => setReloadKey((k) => k + 1), []);

    const [factionCandidates, setFactionCandidates] = useState<
        FactionDto[] | null
    >(null);

    useEffect(() => {
        const orgId = Number(id);
        if (!orgId) {
            setNotFound(true);
            setLoading(false);
            return;
        }
        let cancelled = false;
        setLoading(true);
        setError(null);
        setNotFound(false);
        getMilitaryOrganizationById(orgId)
            .then((data) => {
                if (!cancelled) setOrganization(data);
            })
            .catch((err) => {
                console.error("Failed to load organization:", err);
                if (cancelled) return;
                if (err?.response?.status === 404) setNotFound(true);
                else setError(t("loaderror"));
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
                        to="/storymap/military-organizations"
                        className={s.backLink}
                    >
                        ← {t("backtolist")}
                    </Link>
                }
            />
        );
    }
    if (error || !organization) {
        return <ErrorState onRetry={refetch} detail={error} />;
    }

    const dash = "—";
    const doctrineRef = organization.militaryDoctrine ? (
        <Link
            className={s.refLink}
            to={`/storymap/military-doctrines/${organization.militaryDoctrine.id}`}
        >
            {organization.militaryDoctrine.name}
        </Link>
    ) : (
        dash
    );

    return (
        <div className={s.page}>
            <div className={s.breadcrumb}>
                <Link to="/storymap/military-organizations">
                    {t("listTitle")}
                </Link>
                <span className={s.breadcrumbSep}>/</span>
                <span className={s.breadcrumbCurrent}>{organization.name}</span>
            </div>
            <div className={s.headerRow}>
                <div className={s.headerMain}>
                    <div className={s.kicker}>
                        {organization.type || t("listTitle")}
                    </div>
                    <h1 className={s.name}>{organization.name}</h1>
                </div>
                {canEdit && (
                    <Button
                        variant="ghost"
                        onClick={() =>
                            navigate(
                                `/storymap/military-organizations/${organization.id}/edit`
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
                        label={t("fields.type")}
                        value={organization.type || dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.role")}
                        value={organization.role || dash}
                    />
                    <OrnateDisplayBox
                        label={t("fields.militaryDoctrine")}
                        value={doctrineRef}
                    />
                </DisplayGrid>
            </div>

            <div className={s.sectionHead}>
                <span className={s.sectionTitle}>{t("fields.description")}</span>
                <span className={s.sectionLine} />
            </div>
            {organization.description ? (
                <p className={`${s.prose} ${s.dropCap}`}>
                    {organization.description}
                </p>
            ) : (
                <p className={`${s.prose} ${s.muted}`}>{t("none")}</p>
            )}

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("armies.label")}</span>
                <span className={s.sectionLine} />
            </div>
            {organization.armies.length === 0 ? (
                <p className={s.none}>{t("none")}</p>
            ) : (
                organization.armies.map((a) => (
                    <div key={a.id} className={s.childRow}>
                        <Link
                            className={s.refLink}
                            to={`/storymap/armies/${a.id}`}
                        >
                            {a.name}
                        </Link>
                    </div>
                ))
            )}

            <div className={`${s.sectionHead} ${s.sectionSpacer}`}>
                <span className={s.sectionTitle}>{t("links.factions")}</span>
                <span className={s.sectionLine} />
            </div>
            <LinkEditor
                items={organization.factions}
                candidates={factionCandidates}
                onLoadCandidates={() =>
                    getFactions(organization.worldId).then(setFactionCandidates)
                }
                onAdd={(factionId) =>
                    addOrganizationFaction(organization.id, factionId)
                }
                onRemove={(factionId) =>
                    removeOrganizationFaction(organization.id, factionId)
                }
                onChanged={refetch}
                canEdit={canEdit}
                linkTo={(factionId) => `/storymap/factions/${factionId}`}
                addLabel={t("links.add")}
                noneLabel={t("none")}
                pickLabel={t("links.pick")}
                cancelLabel={t("form.cancel")}
                confirmLabel={t("links.confirm")}
                removeLabel={(name) => t("links.remove", { name })}
                addFailedLabel={t("links.addFailed")}
                removeFailedLabel={t("links.removeFailed")}
            />

            <HistoryBlock
                targetType="MilitaryOrganization"
                targetId={organization.id}
                worldId={organization.worldId}
                history={organization.history}
                canEdit={canEdit}
                onChanged={refetch}
            />
        </div>
    );
}
