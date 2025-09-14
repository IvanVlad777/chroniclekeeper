import React from "react";
import styles from "../formStyles.module.css";

import OrnateField from "../field/OrnateField";
import { useAuth } from "../../../../hooks/useAuth";
import OrnateDisplayBox from "../field/OrnateDisplayBox";

type Props = Omit<
    React.TextareaHTMLAttributes<HTMLTextAreaElement>,
    "className"
> & {
    label?: string;
    helpText?: string;
    error?: string;
    className?: string; // dodatni wrapper class ako treba
    allowedRoles?: string[];
    value?: string;
};

export default function OrnateTextArea({
    id,
    label,
    helpText,
    error,
    className,
    allowedRoles,
    value,
    ...textareaProps
}: Props) {
    const { userInfo } = useAuth();
    const roles = userInfo?.roles ?? [];
    const editable =
        roles.includes("SuperAdmin") ||
        (Array.isArray(allowedRoles) && allowedRoles.length > 0
            ? allowedRoles.some((r) => roles.includes(r))
            : false);
    const areaId =
        id || `ornate-textarea-${Math.random().toString(36).slice(2)}`;

    if (!editable) {
        // read-only prikaz
        const display = (value ?? "").toString();
        return (
            <OrnateDisplayBox
                id={areaId}
                label={label}
                value={display}
                className={className}
                helpText={helpText}
                error={error}
            />
        );
    }

    return (
        <OrnateField
            id={areaId}
            label={label}
            helpText={helpText}
            error={error}
            className={className}
        >
            <textarea
                id={areaId}
                className={styles.ornateTextarea}
                value={value}
                {...textareaProps}
            />
        </OrnateField>
    );
}
