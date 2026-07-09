import type { ReactNode } from 'react';
import s from './DataTable.module.css';

/** Character status → token-mapped pill. Extend the map for other entities. */
export type Status = 'living' | 'dead' | 'unknown';

const cx = (...c: (string | false | undefined)[]) => c.filter(Boolean).join(' ');

export function StatusPill({ status, children }: { status: Status; children?: ReactNode }) {
  return <span className={cx(s.status, s[status])}>{children ?? status}</span>;
}
