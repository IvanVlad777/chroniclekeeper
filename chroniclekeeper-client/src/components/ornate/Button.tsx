import { forwardRef } from 'react';
import type { ComponentPropsWithoutRef } from 'react';
import s from './Button.module.css';

type Variant = 'primary' | 'ghost' | 'danger';
type Size = 'sm' | 'md' | 'lg';

export interface ButtonProps extends ComponentPropsWithoutRef<'button'> {
  variant?: Variant;
  size?: Size;
  /** square icon-only button; pass an aria-label */
  icon?: boolean;
}

const cx = (...c: (string | false | undefined)[]) => c.filter(Boolean).join(' ');

export const Button = forwardRef<HTMLButtonElement, ButtonProps>(function Button(
  { variant = 'primary', size = 'md', icon = false, type = 'button', className, ...rest },
  ref,
) {
  return (
    <button
      ref={ref}
      type={type}
      className={cx(s.button, s[variant], size !== 'md' && s[size], icon && s.icon, className)}
      {...rest}
    />
  );
});
