import { ColumnDef, DataTable } from "../../../basic/table/DataTable";

type Character = {
    id: number;
    name: string;
    role?: string;
    faction?: string;
    createdAt?: string;
};

const DATA: Character[] = [
    {
        id: 1,
        name: "Eli",
        role: "Protagonist",
        faction: "Flora",
        createdAt: "2025-05-01",
    },
    {
        id: 2,
        name: "Mihael",
        role: "Knight",
        faction: "Kanmarski red",
        createdAt: "2025-06-10",
    },
    {
        id: 3,
        name: "Silvia",
        role: "Healer",
        faction: "Erelar",
        createdAt: "2025-04-20",
    },
];

const columns: ColumnDef<Character>[] = [
    {
        id: "id",
        header: "ID",
        accessor: (r) => r.id,
        sortType: "number",
        searchable: false,
    },
    { id: "name", header: "Name", accessor: (r) => r.name },
    { id: "role", header: "Role", accessor: (r) => r.role ?? "" },
    { id: "faction", header: "Faction", accessor: (r) => r.faction ?? "" },
    // auto će prepoznati kao "date" jer je ISO string; možeš i prisiliti sortType: 'date'
    { id: "createdAt", header: "Created", accessor: (r) => r.createdAt ?? "" },
];

export default function CharactersList() {
    return (
        <DataTable<Character>
            data={DATA}
            columns={columns}
            getRowId={(r) => String(r.id)}
            onRowClick={(row) => {
                window.location.href = `/dashboard/characters/${row.id}`;
            }}
            initialSort={{ id: "name", desc: false }}
            pageSize={10}
            enableSearch
            showIndexColumn
            searchPlaceholder="Traži likove…"
        />
    );
}
