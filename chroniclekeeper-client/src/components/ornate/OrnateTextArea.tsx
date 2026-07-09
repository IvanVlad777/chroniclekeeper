import { forwardRef } from 'react';
import type { ComponentPropsWithoutRef } from 'react';
import s from './OrnateTextArea.module.css';

export type OrnateTextAreaProps = ComponentPropsWithoutRef<'textarea'>;

const cx = (...c: (string | false | undefined)[]) => c.filter(Boolean).join(' ');

export const OrnateTextArea = forwardRef<HTMLTextAreaElement, OrnateTextAreaProps>(
  function OrnateTextArea({ className, rows = 4, ...rest }, ref) {
    return <textarea ref={ref} rows={rows} className={cx(s.textarea, className)} {...rest} />;
  },
);
