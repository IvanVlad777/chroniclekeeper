import { useMemo, useState } from 'react';
import type { ReactNode } from 'react';
import s from './DataTable.module.css';

export interface Column<T> {
  key: string;
  header: ReactNode;
  render?: (row: T) => ReactNode;
  /** value used for sorting / searching this column (defaults to row[key]) */
  value?: (row: T) => string | number;
  sortable?: boolean;
  align?: 'left' | 'right';
}

export interface DataTableProps<T> {
  columns: Column<T>[];
  data: T[];
  getRowId: (row: T) => string;
  onRowClick?: (row: T) => void;
  title?: ReactNode;
  /** enable the search box; searches every column's `value` */
  searchable?: boolean;
  searchPlaceholder?: string;
  /** action shown at the toolbar's right edge (e.g. a "+ New" Button) */
  action?: ReactNode;
  pageSize?: number;
  empty?: ReactNode;
  initialSort?: { key: string; dir: 'asc' | 'desc' };
}

const cx = (...c: (string | false | undefined)[]) => c.filter(Boolean).join(' ');
const cellValue = <T,>(col: Column<T>, row: T): string | number =>
  col.value ? col.value(row) : ((row as Record<string, unknown>)[col.key] as string | number) ?? '';

export function DataTable<T>({
  columns, data, getRowId, onRowClick, title,
  searchable = true, searchPlaceholder = 'Search…', action,
  pageSize = 10, empty, initialSort,
}: DataTableProps<T>) {
  const [query, setQuery] = useState('');
  const [sort, setSort] = useState<{ key: string; dir: 'asc' | 'desc' } | null>(initialSort ?? null);
  const [page, setPage] = useState(0);

  const filtered = useMemo(() => {
    if (!query.trim()) return data;
    const q = query.toLowerCase();
    return data.filter((row) =>
      columns.some((c) => String(cellValue(c, row)).toLowerCase().includes(q)),
    );
  }, [data, columns, query]);

  const sorted = useMemo(() => {
    if (!sort) return filtered;
    const col = columns.find((c) => c.key === sort.key);
    if (!col) return filtered;
    const dir = sort.dir === 'asc' ? 1 : -1;
    return [...filtered].sort((a, b) => {
      const av = cellValue(col, a), bv = cellValue(col, b);
      return (av < bv ? -1 : av > bv ? 1 : 0) * dir;
    });
  }, [filtered, sort, columns]);

  const pageCount = Math.max(1, Math.ceil(sorted.length / pageSize));
  const clamped = Math.min(page, pageCount - 1);
  const rows = sorted.slice(clamped * pageSize, clamped * pageSize + pageSize);

  const toggleSort = (key: string) =>
    setSort((prev) =>
      prev?.key === key
        ? { key, dir: prev.dir === 'asc' ? 'desc' : 'asc' }
        : { key, dir: 'asc' },
    );

  return (
    <div>
      {(title || searchable || action) && (
        <div className={s.toolbar}>
          {title && <h1 className={s.title}>{title} <span className={s.count}>{sorted.length}</span></h1>}
          <div className={s.spacer} />
          {searchable && (
            <div className={s.search}>
              <span style={{ color: 'var(--color-muted)' }}>⌕</span>
              <input
                value={query}
                placeholder={searchPlaceholder}
                onChange={(e) => { setQuery(e.target.value); setPage(0); }}
              />
            </div>
          )}
          {action}
        </div>
      )}

      <div className={s.wrap}>
        <table className={s.table}>
          <thead>
            <tr>
              {columns.map((c) => (
                <th
                  key={c.key}
                  className={cx(s.th, c.sortable && s.sortable, sort?.key === c.key && s.sorted)}
                  style={c.align === 'right' ? { textAlign: 'right' } : undefined}
                  onClick={c.sortable ? () => toggleSort(c.key) : undefined}
                  aria-sort={sort?.key === c.key ? (sort.dir === 'asc' ? 'ascending' : 'descending') : undefined}
                >
                  {c.header}
                  {c.sortable && (
                    <span className={s.caret}>{sort?.key === c.key ? (sort.dir === 'asc' ? ' ▴' : ' ▾') : ' ▾'}</span>
                  )}
                </th>
              ))}
            </tr>
          </thead>
          <tbody>
            {rows.length === 0 ? (
              <tr>
                <td className={s.td} colSpan={columns.length}>
                  {empty ?? <div className={s.empty}><p className={s.emptyText}>Nothing to show.</p></div>}
                </td>
              </tr>
            ) : (
              rows.map((row) => (
                <tr
                  key={getRowId(row)}
                  className={s.row}
                  onClick={onRowClick ? () => onRowClick(row) : undefined}
                >
                  {columns.map((c) => (
                    <td
                      key={c.key}
                      className={cx(s.td, c.align === 'right' && s.muted)}
                      style={c.align === 'right' ? { textAlign: 'right' } : undefined}
                    >
                      {c.render ? c.render(row) : String(cellValue(c, row))}
                    </td>
                  ))}
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {pageCount > 1 && (
        <div className={s.pagination}>
          <span>{clamped * pageSize + 1}–{Math.min((clamped + 1) * pageSize, sorted.length)} of {sorted.length}</span>
          <span className={s.pages}>
            <button className={s.page} disabled={clamped === 0} onClick={() => setPage(clamped - 1)}>‹</button>
            {Array.from({ length: pageCount }, (_, i) => (
              <button key={i} className={cx(s.page, i === clamped && s.pageActive)} onClick={() => setPage(i)}>{i + 1}</button>
            ))}
            <button className={s.page} disabled={clamped === pageCount - 1} onClick={() => setPage(clamped + 1)}>›</button>
          </span>
        </div>
      )}
    </div>
  );
}
