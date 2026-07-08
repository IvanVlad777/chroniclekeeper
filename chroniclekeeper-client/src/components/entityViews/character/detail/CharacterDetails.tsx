import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import OrnateDisplayBox from "../../../basic/forms/field/OrnateDisplayBox";
import { CharacterDetailsDto } from "../../../../interfaces/loreInterfaces";
import { getCharacter } from "../../../../api/characters";

export default function CharacterDetail() {
    const { id } = useParams<{ id: string }>();
    const { t } = useTranslation("character");

    const [character, setCharacter] = useState<CharacterDetailsDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const characterId = Number(id);
        if (!characterId) {
            setError(t("notfound") || "Character not found");
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);

        getCharacter(characterId)
            .then((data) => {
                if (!cancelled) setCharacter(data);
            })
            .catch((err) => {
                console.error("Failed to load character:", err);
                if (!cancelled) {
                    setError(
                        err?.response?.status === 404
                            ? t("notfound") || "Character not found"
                            : t("loaderror") || "Failed to load character"
                    );
                }
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });

        return () => {
            cancelled = true;
        };
    }, [id, t]);

    if (loading) return <p>{t("loading") || "Loading…"}</p>;
    if (error || !character) return <p>{error}</p>;

    const lifespan = [character.birthDate, character.deathDate]
        .map((d) => (d ? new Date(d).toLocaleDateString() : null))
        .filter(Boolean)
        .join(" — ");

    return (
        <div>
            <p>
                <Link to="/storymap/characters">
                    ← {t("backtolist") || "Back to characters"}
                </Link>
            </p>

            <h1>
                {character.name}
                {character.title ? <small> · {character.title}</small> : null}
            </h1>

            <div
                style={{
                    display: "grid",
                    gridTemplateColumns: "repeat(auto-fill, minmax(220px, 1fr))",
                    gap: 12,
                }}
            >
                <OrnateDisplayBox
                    label={t("firstname") || "First name"}
                    value={character.firstName}
                />
                <OrnateDisplayBox
                    label={t("lastname") || "Last name"}
                    value={character.lastName}
                />
                <OrnateDisplayBox
                    label={t("nickname") || "Nickname"}
                    value={character.nickname}
                />
                <OrnateDisplayBox
                    label={t("lifespan") || "Born — died"}
                    value={lifespan}
                />
                <OrnateDisplayBox
                    label={t("species") || "Species"}
                    value={character.species?.name}
                />
                <OrnateDisplayBox
                    label={t("race") || "Race"}
                    value={character.race?.name}
                />
                <OrnateDisplayBox
                    label={t("father") || "Father"}
                    value={character.father?.name}
                />
                <OrnateDisplayBox
                    label={t("mother") || "Mother"}
                    value={character.mother?.name}
                />
                <OrnateDisplayBox
                    label={t("haircolor") || "Hair color"}
                    value={character.hairColor}
                />
                <OrnateDisplayBox
                    label={t("eyecolor") || "Eye color"}
                    value={character.eyeColor}
                />
                <OrnateDisplayBox
                    label={t("height") || "Height (cm)"}
                    value={character.height ?? undefined}
                />
                <OrnateDisplayBox
                    label={t("weight") || "Weight (kg)"}
                    value={character.weight ?? undefined}
                />
            </div>

            <OrnateDisplayBox
                label={t("description") || "Description"}
                value={character.description}
            />
            <OrnateDisplayBox
                label={t("features") || "Special physical features"}
                value={character.specialPhysicalFeatures}
            />

            <h2>{t("factions") || "Factions"}</h2>
            {character.factions.length === 0 ? (
                <p>{t("none") || "None"}</p>
            ) : (
                <ul>
                    {character.factions.map((f) => (
                        <li key={f.id}>{f.name}</li>
                    ))}
                </ul>
            )}

            <h2>{t("relationships") || "Relationships"}</h2>
            {character.relationships.length === 0 ? (
                <p>{t("none") || "None"}</p>
            ) : (
                <ul>
                    {character.relationships.map((r) => (
                        <li key={r.id}>
                            <Link to={`/storymap/characters/${r.relatedCharacterId}`}>
                                {r.relatedCharacterName}
                            </Link>{" "}
                            — {r.type}
                            {r.description ? ` (${r.description})` : ""}
                        </li>
                    ))}
                </ul>
            )}

            <h2>{t("tags") || "Tags"}</h2>
            {character.tags.length === 0 ? (
                <p>{t("none") || "None"}</p>
            ) : (
                <ul style={{ display: "flex", gap: 8, listStyle: "none", padding: 0 }}>
                    {character.tags.map((tag) => (
                        <li key={tag.id}>#{tag.name}</li>
                    ))}
                </ul>
            )}
        </div>
    );
}
