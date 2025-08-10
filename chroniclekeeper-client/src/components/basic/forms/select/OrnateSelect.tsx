import React from "react";
import "../formStyles.css";
import OrnateField from "../field/OrnateField";
import OrnateDisplayBox from "../field/OrnateDisplayBox";
import { useAuth } from "../../../../hooks/useAuth";
// Ovdje ne radi dobro READ ONLY
type Option = {
    value: string;
    label: string;
    disabled?: boolean;
};

type BaseProps = React.SelectHTMLAttributes<HTMLSelectElement>;

type Props = Omit<
    BaseProps,
    "className" | "value" | "children" | "multiple"
> & {
    label?: string;
    helpText?: string;
    error?: string;
    className?: string; // wrapper klasa
    allowedRoles?: string[]; // tko smije uređivati (SuperAdmin uvijek može)
    value?: string; // SINGLE select vrijednost
    options?: Option[]; // lista opcija
    placeholderOption?: string; // npr. "Odaberi…"
};

export default function OrnateSelect({
    id,
    label,
    helpText,
    error,
    className = "",
    allowedRoles,
    value = "",
    options = [],
    placeholderOption,
    ...selectProps
}: Props) {
    const { userInfo } = useAuth();
    const roles = userInfo?.roles ?? [];

    // SuperAdmin uvijek može; inače provjeri allowedRoles (ako nisu zadane -> read-only)
    const editable =
        roles.includes("SuperAdmin") ||
        (Array.isArray(allowedRoles) && allowedRoles.length > 0
            ? allowedRoles.some((r) => roles.includes(r))
            : false);

    const selectId =
        id || `ornate-select-${Math.random().toString(36).slice(2)}`;

    if (!editable) {
        // READ-ONLY: prikaži labelu odabrane opcije (ako postoji), inače raw value
        const display =
            options.find((o) => o.value === value)?.label ?? (value || "");
        return (
            <OrnateDisplayBox
                id={selectId}
                label={label}
                value={display}
                className={className}
                helpText={helpText}
                error={error}
            />
        );
    }

    // EDIT mod (SINGLE select)
    return (
        <OrnateField
            id={selectId}
            label={label}
            helpText={helpText}
            error={error}
            className={className}
        >
            <select
                id={selectId}
                className="ornate-input"
                value={value}
                {...selectProps}
            >
                {placeholderOption ? (
                    <option value="" disabled hidden>
                        {placeholderOption}
                    </option>
                ) : null}

                {options.map((opt) => (
                    <option
                        key={opt.value}
                        value={opt.value}
                        disabled={opt.disabled}
                    >
                        {opt.label}
                    </option>
                ))}
            </select>
        </OrnateField>
    );
}
