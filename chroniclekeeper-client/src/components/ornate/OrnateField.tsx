import { cloneElement, isValidElement, useId } from 'react';
import type { ReactElement, ReactNode } from 'react';
import s from './OrnateField.module.css';

export interface OrnateFieldProps {
  label: ReactNode;
  /** the control (OrnateTextInput / TextArea / Select / MultiSelect) */
  children: ReactNode;
  required?: boolean;
  hint?: ReactNode;
  /** error message; presence flips the field to the invalid state */
  error?: ReactNode;
  /** override the auto-generated id linking label ⇄ control */
  htmlFor?: string;
  className?: string;
}

const cx = (...c: (string | false | undefined)[]) => c.filter(Boolean).join(' ');

/**
 * Wrapper that owns the label / required marker / hint / error and wires
 * accessibility onto its single control child (id, aria-invalid,
 * aria-describedby) so callers don't have to repeat it.
 */
export function OrnateField({ label, children, required, hint, error, htmlFor, className }: OrnateFieldProps) {
  const autoId = useId();
  const id = htmlFor ?? autoId;
  const hintId = hint ? `${id}-hint` : undefined;
  const errId = error ? `${id}-error` : undefined;
  const describedBy = [hintId, errId].filter(Boolean).join(' ') || undefined;

  const control = isValidElement(children)
    ? cloneElement(children as ReactElement<Record<string, unknown>>, {
        id,
        'aria-invalid': error ? true : undefined,
        'aria-describedby': describedBy,
      })
    : children;

  return (
    <div className={cx(s.field, className)} data-invalid={error ? '' : undefined}>
      <label className={s.label} htmlFor={id}>
        {label}
        {required && <span className={s.required} aria-hidden="true">*</span>}
      </label>
      {control}
      {hint && <span className={s.hint} id={hintId}>{hint}</span>}
      {error && <span className={s.error} id={errId} role="alert">{error}</span>}
    </div>
  );
}
