import React from "react";

export type SortType = "auto" | "number" | "string" | "date";
type SortStateStrict = { id: string; desc: boolean };
type SortState = SortStateStrict | undefined;
type SortInit = { id: string; desc?: boolean };
export type Comparator = (a: unknown, b: unknown) => number;

export type ColumnDef<T> = {
    /** Jedinstveni ID kolone (za sortiranje i key) */
    id: string;
    /** Naslov kolone (string ili renderer) */
    header: React.ReactNode | ((col: ColumnDef<T>) => React.ReactNode);
    /**
     * Dohvat vrijednosti iz retka. Možeš vratiti number/string/Date/boolean/... .
     * Ako vratiš nešto kompleksno, za search se fallbacka na JSON.stringify(value).
     */
    accessor: (row: T) => unknown;
    /** Prilagođeni prikaz ćelije */
    cell?: (value: unknown, row: T, rowIndex: number) => React.ReactNode;
    /** Vrsta sortiranja: auto (po tipu vrijednosti) ili prisilno */
    sortType?: SortType | Comparator;
    /** Uključiti ovu kolonu u globalni search? (default: true) */
    searchable?: boolean;
};

export type DataTableProps<T> = {
    data: T[];
    columns: ColumnDef<T>[];
    /** Dohvat ID-a reda (za key). Ako ne daš, koristi se index. */
    getRowId?: (row: T, index: number) => string;
    /** Handler klika na red. */
    onRowClick?: (row: T, index: number) => void;
    /** Početno sortiranje. */
    initialSort?: SortInit;
    /** Prikaži input za traženje? (default: true) */
    enableSearch?: boolean;
    /** Placeholder za search input. */
    searchPlaceholder?: string;
    /** Debounce za search (ms). (default: 250) */
    searchDebounceMs?: number;
    /** Custom globalni filter (ako želiš nadjačati default). */
    globalFilter?: (row: T, query: string, columns: ColumnDef<T>[]) => boolean;
    /** Opcionalna paginacija; ako postaviš npr. 10 – uključena je. (default: off) */
    pageSize?: number;
    /** Prikaži redni broj? (default: true) */
    showIndexColumn?: boolean;
    /** Callback kad se promijeni sortiranje (npr. za spremiti u URL). */
    onSortChange?: (sort: SortState) => void;
};

export function DataTable<T>({
    data,
    columns,
    getRowId,
    onRowClick,
    initialSort,
    enableSearch = true,
    searchPlaceholder = "Search…",
    searchDebounceMs = 250,
    globalFilter,
    pageSize,
    showIndexColumn = true,
    onSortChange,
}: DataTableProps<T>) {
    // --- state ---
    const [input, setInput] = React.useState("");
    const [query, setQuery] = React.useState("");
    const [sort, setSort] = React.useState<SortState>(() =>
        toStrictSort(initialSort)
    );
    const [page, setPage] = React.useState(0);

    // debounce search
    React.useEffect(() => {
        const id = window.setTimeout(
            () => setQuery(input.trim()),
            searchDebounceMs
        );
        return () => window.clearTimeout(id);
    }, [input, searchDebounceMs]);

    // reset page kad se promijeni query
    React.useEffect(() => {
        setPage(0);
    }, [query]);

    // --- helpers ---
    const findColumn = React.useCallback(
        (id?: string) => columns.find((c) => c.id === id),
        [columns]
    );

    const defaultGlobalFilter = React.useCallback(
        (row: T, q: string, cols: ColumnDef<T>[]) => {
            if (!q) return true;
            const needle = normalize(q);
            for (const col of cols) {
                if (col.searchable === false) continue;
                const v = col.accessor(row);
                const hay = normalize(valueToSearchableString(v));
                if (hay.includes(needle)) return true;
            }
            return false;
        },
        []
    );

    const cmpFactory = React.useCallback((col: ColumnDef<T>): Comparator => {
        // custom comparator
        if (typeof col.sortType === "function") {
            return col.sortType as Comparator;
        }
        const mode: SortType = (col.sortType as SortType) ?? "auto";

        const decide = (a: unknown, b: unknown): number => {
            // nullish na dno
            const aNil = a == null;
            const bNil = b == null;
            if (aNil && bNil) return 0;
            if (aNil) return 1;
            if (bNil) return -1;

            const t = mode !== "auto" ? mode : inferSortType(a); // auto: po 'a' je dosta

            switch (t) {
                case "number":
                    return toNumber(a) - toNumber(b);
                case "date":
                    return toTime(a) - toTime(b);
                case "string":
                default:
                    return toStringLc(a).localeCompare(toStringLc(b));
            }
        };

        return decide;
    }, []);

    // --- derive ---
    const filtered = React.useMemo(() => {
        const f = globalFilter ?? defaultGlobalFilter;
        return query ? data.filter((row) => f(row, query, columns)) : data;
    }, [data, columns, query, globalFilter, defaultGlobalFilter]);

    const sorted = React.useMemo(() => {
        if (!sort) return filtered;
        const col = findColumn(sort.id);
        if (!col) return filtered;
        const cmp = cmpFactory(col);

        // stable sort
        return [...filtered]
            .map((row, idx) => ({ row, idx }))
            .sort((a, b) => {
                const va = col.accessor(a.row);
                const vb = col.accessor(b.row);
                const res = cmp(va, vb);
                if (res !== 0) return sort.desc ? -res : res;
                return a.idx - b.idx;
            })
            .map((x) => x.row);
    }, [filtered, sort, findColumn, cmpFactory]);

    const paginated = React.useMemo(() => {
        if (!pageSize || pageSize <= 0) return { rows: sorted, totalPages: 1 };
        const totalPages = Math.max(1, Math.ceil(sorted.length / pageSize));
        const start = page * pageSize;
        const rows = sorted.slice(start, start + pageSize);
        return { rows, totalPages };
    }, [sorted, page, pageSize]);

    // --- handlers ---
    const toggleSort = React.useCallback(
        (colId: string) => {
            setSort((prev) => {
                let next: SortState;
                if (!prev || prev.id !== colId)
                    next = { id: colId, desc: false }; // asc
                else if (prev && !prev.desc)
                    next = { id: colId, desc: true }; // desc
                else next = undefined; // off
                onSortChange?.(next);
                return next;
            });
        },
        [onSortChange]
    );

    // --- render ---
    return (
        <div>
            {enableSearch && (
                <div>
                    <label
                        htmlFor="datatable-search"
                        style={{ display: "block" }}
                    >
                        Search
                    </label>
                    <input
                        id="datatable-search"
                        type="text"
                        value={input}
                        onChange={(e) => setInput(e.target.value)}
                        placeholder={searchPlaceholder}
                    />
                </div>
            )}

            <div style={{ overflowX: "auto" }}>
                <table style={{ width: "100%", borderCollapse: "collapse" }}>
                    <thead>
                        <tr>
                            {showIndexColumn && <Th>#</Th>}
                            {columns.map((col) => {
                                const isSorted = sort?.id === col.id;
                                const dir = isSorted
                                    ? sort!.desc
                                        ? "↓"
                                        : "↑"
                                    : "";
                                const header =
                                    typeof col.header === "function"
                                        ? col.header(col)
                                        : col.header;

                                return (
                                    <Th
                                        key={col.id}
                                        role="button"
                                        tabIndex={0}
                                        aria-sort={
                                            isSorted
                                                ? sort!.desc
                                                    ? "descending"
                                                    : "ascending"
                                                : "none"
                                        }
                                        onClick={() => toggleSort(col.id)}
                                        onKeyDown={(e) => {
                                            if (
                                                e.key === "Enter" ||
                                                e.key === " "
                                            ) {
                                                e.preventDefault();
                                                toggleSort(col.id);
                                            }
                                        }}
                                        title="Toggle sort"
                                    >
                                        <span>{header}</span>{" "}
                                        <SortIndicator dir={dir} />
                                    </Th>
                                );
                            })}
                        </tr>
                    </thead>
                    <tbody>
                        {paginated.rows.length === 0 ? (
                            <tr>
                                <Td
                                    colSpan={
                                        (showIndexColumn ? 1 : 0) +
                                        columns.length
                                    }
                                >
                                    No results.
                                </Td>
                            </tr>
                        ) : (
                            paginated.rows.map((row, i) => {
                                const absoluteIndex =
                                    (pageSize && pageSize > 0
                                        ? page * pageSize
                                        : 0) + i;

                                const key = getRowId
                                    ? getRowId(row, absoluteIndex)
                                    : String(absoluteIndex);

                                const clickable = !!onRowClick;
                                return (
                                    <tr
                                        key={key}
                                        onClick={
                                            clickable
                                                ? () =>
                                                      onRowClick!(
                                                          row,
                                                          absoluteIndex
                                                      )
                                                : undefined
                                        }
                                        onKeyDown={
                                            clickable
                                                ? (e) => {
                                                      if (
                                                          e.key === "Enter" ||
                                                          e.key === " "
                                                      ) {
                                                          e.preventDefault();
                                                          onRowClick!(
                                                              row,
                                                              absoluteIndex
                                                          );
                                                      }
                                                  }
                                                : undefined
                                        }
                                        tabIndex={clickable ? 0 : -1}
                                        role={clickable ? "button" : undefined}
                                        style={{
                                            cursor: clickable
                                                ? "pointer"
                                                : "default",
                                        }}
                                    >
                                        {showIndexColumn && (
                                            <Td>{absoluteIndex + 1}</Td>
                                        )}
                                        {columns.map((col) => {
                                            const v = col.accessor(row);
                                            return (
                                                <Td key={col.id}>
                                                    {col.cell
                                                        ? col.cell(
                                                              v,
                                                              row,
                                                              absoluteIndex
                                                          )
                                                        : renderValue(v)}
                                                </Td>
                                            );
                                        })}
                                    </tr>
                                );
                            })
                        )}
                    </tbody>
                </table>
            </div>

            {pageSize && pageSize > 0 && (
                <div
                    style={{
                        display: "flex",
                        gap: 8,
                        alignItems: "center",
                        marginTop: 8,
                    }}
                >
                    <button
                        onClick={() => setPage((p) => Math.max(0, p - 1))}
                        disabled={page === 0}
                    >
                        Prev
                    </button>
                    <span>
                        Page {page + 1} / {paginated.totalPages}
                    </span>
                    <button
                        onClick={() =>
                            setPage((p) =>
                                Math.min(paginated.totalPages - 1, p + 1)
                            )
                        }
                        disabled={page + 1 >= paginated.totalPages}
                    >
                        Next
                    </button>
                </div>
            )}
        </div>
    );
}

/* ---------- UI helpers (bez CSS-a) ---------- */

const Th: React.FC<
    React.PropsWithChildren<React.ThHTMLAttributes<HTMLTableCellElement>>
> = ({ children, ...rest }) => (
    <th
        {...rest}
        style={{
            textAlign: "left",
            borderBottom: "1px solid #ddd",
            padding: 8,
            ...rest.style,
        }}
    >
        {children}
    </th>
);

const Td: React.FC<
    React.PropsWithChildren<
        React.TdHTMLAttributes<HTMLTableCellElement> & { colSpan?: number }
    >
> = ({ children, ...rest }) => (
    <td
        {...rest}
        style={{ borderBottom: "1px solid #eee", padding: 8, ...rest.style }}
    >
        {children}
    </td>
);

const SortIndicator: React.FC<{ dir: "" | "↑" | "↓" }> = ({ dir }) => (
    <span aria-hidden="true" style={{ userSelect: "none" }}>
        {dir}
    </span>
);

/* ---------- value utils ---------- */

function inferSortType(v: unknown): SortType {
    if (typeof v === "number") return "number";
    if (v instanceof Date) return "date";
    if (typeof v === "string") {
        const d = Date.parse(v);
        if (!Number.isNaN(d)) return "date";
        return "string";
    }
    if (typeof v === "boolean") return "number"; // false < true
    return "string";
}

function toNumber(v: unknown): number {
    if (typeof v === "number") return v;
    if (typeof v === "boolean") return v ? 1 : 0;
    if (v instanceof Date) return v.getTime();
    if (typeof v === "string") {
        const n = Number(v);
        if (!Number.isNaN(n)) return n;
        const d = Date.parse(v);
        if (!Number.isNaN(d)) return d;
    }
    return 0;
}

function toTime(v: unknown): number {
    if (v instanceof Date) return v.getTime();
    if (typeof v === "string") {
        const d = Date.parse(v);
        if (!Number.isNaN(d)) return d;
    }
    if (typeof v === "number") return v;
    return 0;
}

function toStringLc(v: unknown): string {
    if (v == null) return "";
    if (typeof v === "string") return v.toLocaleLowerCase();
    if (typeof v === "number" || typeof v === "boolean") return String(v);
    if (v instanceof Date) return v.toISOString().toLocaleLowerCase();
    return JSON.stringify(v).toLocaleLowerCase();
}

function normalize(s: string): string {
    return s
        .normalize("NFKD")
        .replace(/\p{Diacritic}/gu, "")
        .toLocaleLowerCase();
}

function valueToSearchableString(v: unknown): string {
    if (v == null) return "";
    if (typeof v === "string") return v;
    if (typeof v === "number" || typeof v === "boolean") return String(v);
    if (v instanceof Date) return v.toISOString();
    return JSON.stringify(v);
}

function renderValue(v: unknown): React.ReactNode {
    if (v == null) return "—";
    if (v instanceof Date) return v.toISOString();
    if (typeof v === "object") return JSON.stringify(v);
    return String(v);
}

function toStrictSort(s?: SortInit): SortState {
    if (!s) return undefined;
    return { id: s.id, desc: !!s.desc }; // default: asc (desc=false)
}
