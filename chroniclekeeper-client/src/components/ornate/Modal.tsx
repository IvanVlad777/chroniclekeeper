import { useEffect, useId, useRef } from 'react';
import { createPortal } from 'react-dom';
import type { ReactNode } from 'react';
import s from './Modal.module.css';

export interface ModalProps {
  open: boolean;
  onClose: () => void;
  title: ReactNode;
  children: ReactNode;
  /** buttons rendered in the footer row (right-aligned) */
  footer?: ReactNode;
  /** accessible label for the × button */
  closeLabel?: string;
}

/**
 * Generic ornate-skinned dialog. Portals to <body>, closes on Escape /
 * backdrop click, traps initial focus to the first focusable control and
 * restores focus to the opener on close. Content-agnostic — compose it.
 */
export function Modal({ open, onClose, title, children, footer, closeLabel = 'Close' }: ModalProps) {
  const panelRef = useRef<HTMLDivElement>(null);
  const restoreRef = useRef<HTMLElement | null>(null);
  const titleId = useId();

  useEffect(() => {
    if (!open) return;
    restoreRef.current = document.activeElement as HTMLElement | null;
    const onKey = (e: KeyboardEvent) => {
      if (e.key === 'Escape') {
        e.stopPropagation();
        onClose();
      }
    };
    document.addEventListener('keydown', onKey);
    panelRef.current
      ?.querySelector<HTMLElement>('input, textarea, select, button')
      ?.focus();
    return () => {
      document.removeEventListener('keydown', onKey);
      restoreRef.current?.focus?.();
    };
  }, [open, onClose]);

  if (!open) return null;

  return createPortal(
    <div className={s.overlay} onMouseDown={onClose}>
      <div
        ref={panelRef}
        className={s.panel}
        role="dialog"
        aria-modal="true"
        aria-labelledby={titleId}
        onMouseDown={(e) => e.stopPropagation()}
      >
        <div className={s.header}>
          <h2 id={titleId} className={s.title}>{title}</h2>
          <button type="button" className={s.close} aria-label={closeLabel} onClick={onClose}>
            ×
          </button>
        </div>
        <div className={s.body}>{children}</div>
        {footer && <div className={s.footer}>{footer}</div>}
      </div>
    </div>,
    document.body,
  );
}
