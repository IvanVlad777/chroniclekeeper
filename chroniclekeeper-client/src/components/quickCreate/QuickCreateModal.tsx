import { useId, useState, type FormEvent } from 'react';
import { useTranslation } from 'react-i18next';
import { Button, Modal, OrnateField, OrnateTextArea, OrnateTextInput } from '../ornate';
import { apiErrorMessage } from '../../utils/apiError';
import {
  quickCreateRegistry,
  type QuickCreateContext,
  type QuickCreateKind,
  type QuickCreated,
} from './registry';
import s from './QuickCreateModal.module.css';

export interface QuickCreateModalProps {
  kind: QuickCreateKind;
  worldId: number;
  context?: QuickCreateContext;
  onCreated: (created: QuickCreated) => void;
  onClose: () => void;
}

/**
 * Small modal that creates one entity of `kind` (Name + optional Description)
 * without leaving the surrounding form. On success it hands the new entity
 * back so the caller can add it to its options and select it.
 */
export function QuickCreateModal({ kind, worldId, context, onCreated, onClose }: QuickCreateModalProps) {
  const { t } = useTranslation('common');
  const descriptor = quickCreateRegistry[kind];
  const showDescription = descriptor.description !== false;
  const formId = useId();

  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [error, setError] = useState<string | null>(null);
  const [busy, setBusy] = useState(false);

  const kindLabel = t(`quickCreate.kinds.${kind}`);

  async function doSubmit() {
    if (busy) return;
    if (!name.trim()) {
      setError(t('quickCreate.nameRequired'));
      return;
    }
    setError(null);
    setBusy(true);
    try {
      const created = await descriptor.create(
        worldId,
        { name: name.trim(), description: description.trim() },
        context,
      );
      onCreated({ id: created.id, name: created.name });
    } catch (err) {
      console.error('Quick-create failed:', err);
      setError(apiErrorMessage(err, t('quickCreate.saveFailed')));
      setBusy(false);
    }
  }

  // Enter inside the modal submits it — never the surrounding entity form.
  function onFormSubmit(e: FormEvent) {
    e.preventDefault();
    void doSubmit();
  }

  return (
    <Modal
      open
      onClose={() => !busy && onClose()}
      closeLabel={t('quickCreate.close')}
      title={t('quickCreate.newTitle', { kind: kindLabel })}
      footer={
        <>
          <Button variant="ghost" disabled={busy} onClick={onClose}>
            {t('quickCreate.cancel')}
          </Button>
          <Button type="button" disabled={busy} onClick={() => void doSubmit()}>
            {busy ? t('quickCreate.saving') : t('quickCreate.save')}
          </Button>
        </>
      }
    >
      <form id={formId} className={s.form} onSubmit={onFormSubmit} noValidate>
        <OrnateField label={t('quickCreate.nameLabel')} required error={error ?? undefined}>
          <OrnateTextInput
            value={name}
            display
            maxLength={100}
            autoFocus
            onChange={(e) => setName(e.target.value)}
          />
        </OrnateField>
        {showDescription && (
          <OrnateField label={t('quickCreate.descLabel')}>
            <OrnateTextArea
              value={description}
              rows={4}
              maxLength={4000}
              onChange={(e) => setDescription(e.target.value)}
            />
          </OrnateField>
        )}
      </form>
    </Modal>
  );
}
