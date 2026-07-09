import { useEffect, useRef, useState } from 'react';
import s from './OrnateMultiSelect.module.css';
import menu from './OrnateSelect.module.css';
import type { SelectOption } from './OrnateSelect';

export interface OrnateMultiSelectProps {
  value: string[];
  onChange: (next: string[]) => void;
  options: (SelectOption | string)[];
  placeholder?: string;
  id?: string;
  'aria-invalid'?: boolean;
  'aria-describedby'?: string;
}

const norm = (o: SelectOption | string): SelectOption =>
  typeof o === 'string' ? { value: o, label: o } : o;

/**
 * Controlled multi-select: chips for chosen values + a popover of the rest.
 * Self-contained (no dependency); closes on outside click / Escape.
 */
export function OrnateMultiSelect({
  value, onChange, options, placeholder = 'Select…', id, ...aria
}: OrnateMultiSelectProps) {
  const [open, setOpen] = useState(false);
  const ref = useRef<HTMLDivElement>(null);
  const opts = options.map(norm);
  const byValue = (v: string) => opts.find((o) => o.value === v);
  const remaining = opts.filter((o) => !value.includes(o.value));

  useEffect(() => {
    if (!open) return;
    const onDoc = (e: PointerEvent) => {
      if (ref.current && !ref.current.contains(e.target as Node)) setOpen(false);
    };
    const onKey = (e: KeyboardEvent) => e.key === 'Escape' && setOpen(false);
    document.addEventListener('pointerdown', onDoc);
    document.addEventListener('keydown', onKey);
    return () => {
      document.removeEventListener('pointerdown', onDoc);
      document.removeEventListener('keydown', onKey);
    };
  }, [open]);

  const add = (v: string) => onChange([...value, v]);
  const remove = (v: string) => onChange(value.filter((x) => x !== v));

  return (
    <div ref={ref} style={{ position: 'relative' }}>
      <div
        className={s.control}
        data-state={open ? 'open' : undefined}
        role="button"
        tabIndex={0}
        id={id}
        aria-haspopup="listbox"
        aria-expanded={open}
        aria-invalid={aria['aria-invalid']}
        aria-describedby={aria['aria-describedby']}
        onClick={() => setOpen((o) => !o)}
        onKeyDown={(e) => (e.key === 'Enter' || e.key === ' ') && (e.preventDefault(), setOpen((o) => !o))}
      >
        {value.length === 0 && <span className={s.placeholder}>{placeholder}</span>}
        {value.map((v) => (
          <span key={v} className={s.chip}>
            {byValue(v)?.label ?? v}
            <button
              type="button"
              className={s.remove}
              aria-label={`Remove ${byValue(v)?.label ?? v}`}
              onClick={(e) => { e.stopPropagation(); remove(v); }}
            >
              ×
            </button>
          </span>
        ))}
        {remaining.length > 0 && <button type="button" className={s.add} tabIndex={-1}>+ add</button>}
      </div>

      {open && remaining.length > 0 && (
        <div className={menu.menu} role="listbox" style={{ position: 'absolute', top: 'calc(100% + 6px)', left: 0, right: 0 }}>
          {remaining.map((o) => (
            <div
              key={o.value}
              role="option"
              aria-selected={false}
              className={menu.item}
              onClick={() => { add(o.value); if (remaining.length === 1) setOpen(false); }}
            >
              {o.label}
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
