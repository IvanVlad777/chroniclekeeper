/* Ornate component library — barrel export.
   Import the palette + shared vars once at your app root:
     import './themes.css';
     import './ornate/ornate.vars.css';
   Then use components from here:
     import { Button, DataTable, OrnateField, OrnateTextInput } from './ornate';
*/
export { Button } from './Button';
export type { ButtonProps } from './Button';

export { OrnateField } from './OrnateField';
export type { OrnateFieldProps } from './OrnateField';

export { OrnateTextInput } from './OrnateTextInput';
export type { OrnateTextInputProps } from './OrnateTextInput';

export { OrnateTextArea } from './OrnateTextArea';
export type { OrnateTextAreaProps } from './OrnateTextArea';

export { OrnateSelect } from './OrnateSelect';
export type { OrnateSelectProps, SelectOption } from './OrnateSelect';

export { OrnateCheckbox } from './OrnateCheckbox';
export type { OrnateCheckboxProps } from './OrnateCheckbox';

export { OrnateMultiSelect } from './OrnateMultiSelect';
export type { OrnateMultiSelectProps } from './OrnateMultiSelect';

export { OrnateDisplayBox, DisplayGrid } from './OrnateDisplayBox';

export { DataTable } from './DataTable';
export type { Column, DataTableProps } from './DataTable';

export { Tag } from './Tag';
export type { TagProps } from './Tag';

export { StatusPill } from './StatusPill';
export type { Status } from './StatusPill';
