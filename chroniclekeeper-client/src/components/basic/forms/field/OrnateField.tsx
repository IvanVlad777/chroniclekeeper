import React from "react";
import "../styles.css"; // [DODANO] zajednički ornate stilovi

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
        <div className={`ornate-field ${className}`}>
            {label ? (
                <label htmlFor={id} className="ornate-label">
                    {label}
                </label>
            ) : null}

            {/* [NAPOMENA] očekujemo da child (input/textarea) koristi isti id */}
            {children}

            {helpText ? <div className="ornate-help">{helpText}</div> : null}
            {error ? <div className="ornate-error">{error}</div> : null}
        </div>
    );
}
