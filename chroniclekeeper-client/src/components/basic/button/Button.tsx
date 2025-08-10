import { ReactNode } from "react";
import "./styles.css";

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
            className={`btn-ornate ${
                variant === "outline" ? "btn-ornate-outline" : ""
            } ${className}`}
        >
            {children}
        </button>
    );
}
