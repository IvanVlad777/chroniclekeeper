import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { ColumnDef, DataTable } from "../../../basic/table/DataTable";
import { CharacterDto } from "../../../../interfaces/loreInterfaces";
import { getCharacters } from "../../../../api/characters";
import { useWorld } from "../../../../hooks/useWorld";

const columns: ColumnDef<CharacterDto>[] = [
    {
        id: "id",
        header: "ID",
        accessor: (r) => r.id,
        sortType: "number",
        searchable: false,
    },
    { id: "name", header: "Name", accessor: (r) => r.name },
    { id: "title", header: "Title", accessor: (r) => r.title ?? "" },
    {
        id: "createdAt",
        header: "Created",
        accessor: (r) => r.createdAt ?? "",
        sortType: "date",
        cell: (value) =>
            value ? new Date(String(value)).toLocaleDateString() : "",
    },
];

export default function CharactersList() {
    const navigate = useNavigate();
    const { t } = useTranslation("character");
    const { selectedWorld, loading: worldLoading } = useWorld();

    const [characters, setCharacters] = useState<CharacterDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        if (worldLoading) return;
        if (!selectedWorld) {
            setCharacters([]);
            setLoading(false);
            return;
        }

        let cancelled = false;
        setLoading(true);
        setError(null);

        getCharacters(selectedWorld.id)
            .then((data) => {
                if (!cancelled) setCharacters(data);
            })
            .catch((err) => {
                console.error("Failed to load characters:", err);
                if (!cancelled) setError(t("loaderror") || "Failed to load characters");
            })
            .finally(() => {
                if (!cancelled) setLoading(false);
            });

        return () => {
            cancelled = true;
        };
    }, [selectedWorld, worldLoading, t]);

    if (worldLoading || loading) return <p>{t("loading") || "Loading…"}</p>;
    if (error) return <p>{error}</p>;
    if (!selectedWorld) return <p>{t("noworld") || "Select a world first."}</p>;

    return (
        <DataTable<CharacterDto>
            data={characters}
            columns={columns}
            getRowId={(r) => String(r.id)}
            onRowClick={(row) => {
                navigate(`/storymap/characters/${row.id}`);
            }}
            initialSort={{ id: "name", desc: false }}
            pageSize={10}
            enableSearch
            showIndexColumn
            searchPlaceholder={t("search") || "Search characters…"}
        />
    );
}
