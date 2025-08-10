import React from "react";
import "../formStyles.css";

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
    placeholder = "�",
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
        <div className={`ornate-field ${className}`}>
            {label ? (
                <label className="ornate-label" id={labelId} htmlFor={boxId}>
                    {label}
                </label>
            ) : null}
            <div
                id={boxId}
                role="textbox"
                aria-readonly="true"
                aria-labelledby={label ? labelId : undefined}
                className="ornate-textarea"
                style={{ whiteSpace: "pre-wrap" }}
            >
                {hasContent ? (
                    value
                ) : (
                    <span style={{ opacity: 0.6 }}>{placeholder}</span>
                )}
            </div>

            {helpText ? <div className="ornate-help">{helpText}</div> : null}
            {error ? <div className="ornate-error">{error}</div> : null}
        </div>
    );
}
