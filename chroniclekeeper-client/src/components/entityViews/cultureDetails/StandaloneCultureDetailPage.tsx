import { useTranslation } from "react-i18next";
import { EmptyState, LoadingSkeleton } from "../../feedback";
import { useWorld } from "../../../hooks/useWorld";
import { useAuth } from "../../../hooks/useAuth";
import CultureDetailSection from "./CultureDetailSection";
import { descriptorByKey } from "./descriptors";
import s from "./styles.module.css";

const editorRoles = ["Editor", "Admin", "SuperAdmin"];

/** World-scoped standalone page for a culture-detail type that can exist
 * without a culture (Custom, CulturalInstitution). Reuses the inline section. */
export default function StandaloneCultureDetailPage({
    descriptorKey,
    glyph,
}: {
    descriptorKey: string;
    glyph: string;
}) {
    const { t } = useTranslation("cultureDetails");
    const { selectedWorld, loading: worldLoading } = useWorld();
    const { userInfo } = useAuth();
    const canEdit = userInfo?.roles.some((r) => editorRoles.includes(r)) ?? false;
    const descriptor = descriptorByKey(descriptorKey);

    if (worldLoading) return <LoadingSkeleton variant="block" rows={4} />;
    if (!selectedWorld)
        return <EmptyState glyph={glyph} title={t("standalone.noWorld")} />;
    if (!descriptor) return null;

    return (
        <div className={s.page}>
            <div className={s.kicker}>{selectedWorld.name}</div>
            <h1 className={s.name}>{t(`sections.${descriptorKey}`)}</h1>
            <p className={s.prose}>{t(`standalone.${descriptorKey}Intro`)}</p>
            <CultureDetailSection
                descriptor={descriptor}
                worldId={selectedWorld.id}
                canEdit={canEdit}
            />
        </div>
    );
}
