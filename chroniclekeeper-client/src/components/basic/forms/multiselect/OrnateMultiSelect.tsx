import { useState, useRef, useEffect } from "react";
import formStyles from "../formStyles.module.css";
import styles from "./styles.module.css";
import OrnateField from "../field/OrnateField";
import OrnateDisplayBox from "../field/OrnateDisplayBox";
import { useAuth } from "../../../../hooks/useAuth";
// Ovdje ne radi dobro READ ONLY
type Option = {
    value: string;
    label: string;
    disabled?: boolean;
};

type Props = {
    id?: string;
    label?: string;
    helpText?: string;
    error?: string;
    className?: string;
    allowedRoles?: string[];
    value: string[]; // uvijek array
    onChange: (newValues: string[]) => void;
    options: Option[];
    placeholder?: string;
};

export default function OrnateMultiSelect({
    id,
    label,
    helpText,
    error,
    className = "",
    allowedRoles,
    value,
    onChange,
    options,
    placeholder = "Odaberi...",
}: Props) {
    const { userInfo } = useAuth();
    const roles = userInfo?.roles ?? [];

    const editable =
        roles.includes("SuperAdmin") ||
        (Array.isArray(allowedRoles) && allowedRoles.length > 0
            ? allowedRoles.some((r) => roles.includes(r))
            : false);

    const selectId =
        id || `ornateMultiselect-${Math.random().toString(36).slice(2)}`;

    const [open, setOpen] = useState(false);
    const wrapperRef = useRef<HTMLDivElement>(null);

    // Close on outside click
    useEffect(() => {
        const handler = (e: MouseEvent) => {
            if (
                wrapperRef.current &&
                !wrapperRef.current.contains(e.target as Node)
            ) {
                setOpen(false);
            }
        };
        document.addEventListener("mousedown", handler);
        return () => {
            document.removeEventListener("mousedown", handler);
        };
    }, []);

    // READ-ONLY prikaz
    if (!editable) {
        const display = value
            .map((v) => options.find((o) => o.value === v)?.label ?? v)
            .join(", ");
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

    const toggleValue = (val: string) => {
        if (value.includes(val)) {
            onChange(value.filter((v) => v !== val));
        } else {
            onChange([...value, val]);
        }
    };

    return (
        <OrnateField
            id={selectId}
            label={label}
            helpText={helpText}
            error={error}
            className={className}
        >
            <div ref={wrapperRef} className={styles.ornateMultiselectWrapper}>
                <button
                    type="button"
                    className={`${formStyles.ornateInput} ${styles.ornateMultiselectTrigger}`}
                    onClick={() => setOpen((prev) => !prev)}
                >
                    {value.length > 0
                        ? value
                              .map(
                                  (v) =>
                                      options.find((o) => o.value === v)
                                          ?.label ?? v
                              )
                              .join(", ")
                        : placeholder}
                    <span className={styles.ornateMultiselectArrow}>
                        {open ? "▲" : "▼"}
                    </span>
                </button>

                {open && (
                    <div
                        className={`${styles.ornateMultiselectDropdown} animate`}
                    >
                        {options.map((opt) => (
                            <label
                                key={opt.value}
                                className={`${styles.ornateMultiselectOption} ${
                                    opt.disabled ? "disabled" : ""
                                }`}
                            >
                                <input
                                    type="checkbox"
                                    checked={value.includes(opt.value)}
                                    onChange={() => toggleValue(opt.value)}
                                    disabled={opt.disabled}
                                />
                                {opt.label}
                            </label>
                        ))}
                    </div>
                )}
            </div>
        </OrnateField>
    );
}
