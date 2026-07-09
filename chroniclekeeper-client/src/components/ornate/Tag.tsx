import type { ComponentPropsWithoutRef, ReactNode } from 'react';
import s from './Tag.module.css';

export interface TagProps extends ComponentPropsWithoutRef<'span'> {
  /** show accent colour (default) or neutral text colour */
  tone?: 'accent' | 'neutral';
  /** hover affordance for clickable filter tags */
  interactive?: boolean;
  selected?: boolean;
  count?: number;
  children: ReactNode;
}

const cx = (...c: (string | false | undefined)[]) => c.filter(Boolean).join(' ');

export function Tag({ tone = 'accent', interactive, selected, count, children, className, ...rest }: TagProps) {
  return (
    <span
      className={cx(s.tag, tone === 'neutral' && s.neutral, interactive && s.interactive, selected && s.selected, className)}
      {...rest}
    >
      {children}
      {count != null && <b className={s.count}>{count}</b>}
    </span>
  );
}
