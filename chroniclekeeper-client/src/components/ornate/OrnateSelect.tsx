import { forwardRef } from 'react';
import type { ComponentPropsWithoutRef } from 'react';
import s from './OrnateSelect.module.css';

export interface SelectOption {
  value: string;
  label: string;
  disabled?: boolean;
}

export interface OrnateSelectProps extends ComponentPropsWithoutRef<'select'> {
  /** convenience — pass options instead of <option> children */
  options?: (SelectOption | string)[];
  placeholder?: string;
}

const cx = (...c: (string | false | undefined)[]) => c.filter(Boolean).join(' ');
const norm = (o: SelectOption | string): SelectOption =>
  typeof o === 'string' ? { value: o, label: o } : o;

/**
 * Native <select> in the Ornate skin — keyboard-accessible for free.
 * For a fully custom listbox, keep this CSS module and mount a Radix Select
 * with the `.trigger` / `.menu` / `.item` classes (see README).
 */
export const OrnateSelect = forwardRef<HTMLSelectElement, OrnateSelectProps>(
  function OrnateSelect({ options, placeholder, className, children, ...rest }, ref) {
    return (
      <select ref={ref} className={cx(s.select, className)} {...rest}>
        {placeholder && <option value="" disabled>{placeholder}</option>}
        {options
          ? options.map(norm).map((o) => (
              <option key={o.value} value={o.value} disabled={o.disabled}>{o.label}</option>
            ))
          : children}
      </select>
    );
  },
);
