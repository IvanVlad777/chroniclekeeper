import React from "react";
import styles from "./styles.module.css";

type Props = {
    id?: string;
    label?: string;
    helpText?: string;
    error?: string;
    children: React.ReactNode;
    className?: string;
};

export default function OrnateField({
    id,
    label,
    helpText,
    error,
    children,
    className = "",
}: Props) {
    return (
        <div className={`${styles.ornateField} ${className}`}>
            {label ? (
                <label htmlFor={id} className={styles.ornateLabel}>
                    {label}
                </label>
            ) : null}

            {/* [NAPOMENA] očekujemo da child (input/textarea) koristi isti id */}
            {children}

            {helpText ? (
                <div className={styles.ornateHelp}>{helpText}</div>
            ) : null}
            {error ? <div className={styles.ornateError}>{error}</div> : null}
        </div>
    );
}
