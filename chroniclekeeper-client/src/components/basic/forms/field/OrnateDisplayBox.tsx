import React from "react";
import styles from "../formStyles.module.css";

type Props = {
    id?: string;
    label?: string;
    value?: React.ReactNode;
    placeholder?: string;
    helpText?: string;
    error?: string;
    className?: string;
};

export default function OrnateDisplayBox({
    id,
    label,
    value,
    placeholder = "????",
    helpText,
    error,
    className = "",
}: Props) {
    const boxId = id || `ornate-display-${Math.random().toString(36).slice(2)}`;
    const labelId = `${boxId}-label`;

    const hasContent =
        value !== null &&
        value !== undefined &&
        String(value).trim().length > 0;

    return (
        <div className={`${styles.ornateField} ${className}`}>
            {label ? (
                <label
                    className={styles.ornateLabel}
                    id={labelId}
                    htmlFor={boxId}
                >
                    {label}
                </label>
            ) : null}
            <div
                id={boxId}
                role="textbox"
                aria-readonly="true"
                aria-labelledby={label ? labelId : undefined}
                className={styles.ornateTextarea}
                style={{ whiteSpace: "pre-wrap" }}
            >
                {hasContent ? (
                    value
                ) : (
                    <span style={{ opacity: 0.6 }}>{placeholder}</span>
                )}
            </div>

            {helpText ? (
                <div className={styles.ornateHelp}>{helpText}</div>
            ) : null}
            {error ? <div className={styles.ornateError}>{error}</div> : null}
        </div>
    );
}
