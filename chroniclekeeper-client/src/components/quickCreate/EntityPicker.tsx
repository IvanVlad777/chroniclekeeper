import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { OrnateSelect } from '../ornate';
import { QuickCreateModal } from './QuickCreateModal';
import {
  quickCreateRegistry,
  type QuickCreateContext,
  type QuickCreateKind,
  type QuickCreated,
} from './registry';

/** sentinel option value that opens the quick-create modal instead of selecting. */
const ADD_SENTINEL = '__quickCreateNew__';

export interface EntityOption {
  value: number | string;
  label: string;
}

export interface EntityPickerProps {
  value: string;
  onChange: (value: string) => void;
  options: EntityOption[];
  kind: QuickCreateKind;
  worldId: number;
  /** editor gate — when false the "+ add new" row is hidden (plain select). */
  canCreate: boolean;
  /** label for the empty "" option; omit for a required picker with no blank row. */
  noneLabel?: string;
  /** value to hide from the list (e.g. the entity being edited). */
  excludeValue?: number | string;
  disabled?: boolean;
  id?: string;
  /** context handed to the quick-create descriptor (e.g. a race's species). */
  createContext?: QuickCreateContext;
  /** called with the fresh entity so the caller can add it to its own options. */
  onCreated?: (created: QuickCreated) => void;
  'aria-invalid'?: boolean;
  'aria-describedby'?: string;
}

/**
 * OrnateSelect + an inline "+ Add new…" row that opens a quick-create modal
 * for the picked entity `kind`, then selects the new entity. Drop-in for a
 * single-select entity picker; readers (canCreate=false) get a plain select.
 */
export function EntityPicker({
  value,
  onChange,
  options,
  kind,
  worldId,
  canCreate,
  noneLabel,
  excludeValue,
  disabled,
  id,
  createContext,
  onCreated,
  ...aria
}: EntityPickerProps) {
  const { t } = useTranslation('common');
  const [modalOpen, setModalOpen] = useState(false);

  const descriptor = quickCreateRegistry[kind];
  const showAdd = canCreate && !(descriptor.requiresContext?.(createContext) ?? false);

  const visible =
    excludeValue === undefined
      ? options
      : options.filter((o) => String(o.value) !== String(excludeValue));

  return (
    <>
      <OrnateSelect
        id={id}
        value={value}
        disabled={disabled}
        aria-invalid={aria['aria-invalid']}
        aria-describedby={aria['aria-describedby']}
        onChange={(e) => {
          if (e.target.value === ADD_SENTINEL) {
            setModalOpen(true);
            return;
          }
          onChange(e.target.value);
        }}
      >
        {noneLabel !== undefined && <option value="">{noneLabel}</option>}
        {visible.map((o) => (
          <option key={o.value} value={o.value}>
            {o.label}
          </option>
        ))}
        {showAdd && <option value={ADD_SENTINEL}>{t('quickCreate.addNew')}</option>}
      </OrnateSelect>
      {modalOpen && (
        <QuickCreateModal
          kind={kind}
          worldId={worldId}
          context={createContext}
          onClose={() => setModalOpen(false)}
          onCreated={(created) => {
            onCreated?.(created);
            onChange(String(created.id));
            setModalOpen(false);
          }}
        />
      )}
    </>
  );
}
