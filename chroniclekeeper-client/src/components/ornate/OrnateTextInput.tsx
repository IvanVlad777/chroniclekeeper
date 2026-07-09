import { forwardRef } from 'react';
import type { ComponentPropsWithoutRef } from 'react';
import s from './OrnateTextInput.module.css';

export interface OrnateTextInputProps extends ComponentPropsWithoutRef<'input'> {
  /** render the value in the display serif (for names / titles) */
  display?: boolean;
}

const cx = (...c: (string | false | undefined)[]) => c.filter(Boolean).join(' ');

export const OrnateTextInput = forwardRef<HTMLInputElement, OrnateTextInputProps>(
  function OrnateTextInput({ display, className, type = 'text', ...rest }, ref) {
    return <input ref={ref} type={type} className={cx(s.input, display && s.display, className)} {...rest} />;
  },
);
