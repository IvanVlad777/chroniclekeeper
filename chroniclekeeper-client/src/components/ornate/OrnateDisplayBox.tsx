import type { CSSProperties, ReactNode } from 'react';
import s from './OrnateDisplayBox.module.css';

const cx = (...c: (string | false | undefined)[]) => c.filter(Boolean).join(' ');

export function OrnateDisplayBox({ label, value, ticked = true, className }: {
  label: ReactNode; value: ReactNode; ticked?: boolean; className?: string;
}) {
  return (
    <div className={cx(s.box, ticked && s.ticked, className)}>
      <div className={s.label}>{label}</div>
      <div className={s.value}>{value}</div>
    </div>
  );
}

/** Row of display boxes; `cols` sets the column count. */
export function DisplayGrid({ cols = 6, children }: { cols?: number; children: ReactNode }) {
  return <div className={s.grid} style={{ '--o-cols': cols } as CSSProperties}>{children}</div>;
}
