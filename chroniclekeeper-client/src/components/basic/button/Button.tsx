import { ReactNode } from "react";
import styles from "./styles.module.css";

type ButtonProps = {
    children: ReactNode;
    variant?: "primary" | "outline";
    type?: "button" | "submit" | "reset";
    onClick?: () => void;
    className?: string;
};

export default function Button({
    children,
    variant = "primary",
    type = "button",
    onClick,
    className = "",
}: ButtonProps) {
    return (
        <button
            type={type}
            onClick={onClick}
            className={`${styles.btnOrnate} ${
                variant === "outline" ? `${styles.btnOrnateOutline}` : ""
            } ${className}`}
        >
            {children}
        </button>
    );
}
