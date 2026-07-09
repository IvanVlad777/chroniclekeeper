import { forwardRef } from 'react';
import type { ComponentPropsWithoutRef, ReactNode } from 'react';
import s from './OrnateCheckbox.module.css';

export interface OrnateCheckboxProps extends Omit<ComponentPropsWithoutRef<'input'>, 'type'> {
  /** label rendered next to the box (whole row is clickable) */
  label?: ReactNode;
}

export const OrnateCheckbox = forwardRef<HTMLInputElement, OrnateCheckboxProps>(
  function OrnateCheckbox({ label, className, ...rest }, ref) {
    return (
      <label className={s.wrap}>
        <input ref={ref} type="checkbox" className={`${s.input} ${className ?? ''}`} {...rest} />
        {label && <span className={s.label}>{label}</span>}
      </label>
    );
  },
);
