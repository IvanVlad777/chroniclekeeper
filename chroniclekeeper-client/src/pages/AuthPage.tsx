import { useState, type FormEvent } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button, OrnateField, OrnateTextInput } from "../components/ornate";
import { login, register } from "../api/auth";
import { useAuth } from "../hooks/useAuth";
import { IdentityError } from "../interfaces/authInterfaces";
import s from "./AuthPage.module.css";

type Mode = "signin" | "register";

/** Izvuci čitljivu poruku iz API greške (Identity vraća array [{code, description}]). */
function errorMessage(err: unknown, fallback: string): string {
    if (Array.isArray(err)) {
        return (err as IdentityError[]).map((e) => e.description).join(" ");
    }
    if (err && typeof err === "object" && "$values" in err) {
        return (err as { $values: IdentityError[] }).$values
            .map((e) => e.description)
            .join(" ");
    }
    if (typeof err === "string") return err;
    return fallback;
}

/**
 * Zajednička stranica za /login i /register — jedna kartica s tabovima.
 * Tab mijenja rutu (deep-linkovi rade); nakon registracije preusmjerava
 * na /login sa success porukom kroz location state.
 */
export function AuthPage({ initialMode }: { initialMode: Mode }) {
    const { t } = useTranslation("auth");
    const navigate = useNavigate();
    const location = useLocation();
    const { login: authLogin } = useAuth();

    const mode = initialMode;
    const registered = Boolean(
        (location.state as { registered?: boolean } | null)?.registered
    );

    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState<string | null>(null);
    const [busy, setBusy] = useState(false);

    const switchMode = (next: Mode) => {
        if (next !== mode) {
            navigate(next === "signin" ? "/login" : "/register");
        }
    };

    async function onSubmit(e: FormEvent) {
        e.preventDefault();
        setError(null);
        setBusy(true);
        try {
            if (mode === "signin") {
                const response = await login({ email, password });
                sessionStorage.setItem("token", response.token);
                authLogin(response.token);
                navigate("/storymap");
            } else {
                await register({ email, password });
                navigate("/login", { state: { registered: true } });
            }
        } catch (err) {
            setError(errorMessage(err, t("failed")));
        } finally {
            setBusy(false);
        }
    }

    return (
        <div className={s.page}>
            <div className={s.brand}>
                <span className={s.mark}>❦</span>
                <h1 className={s.title}>
                    Chronicle<span>Keeper</span>
                </h1>
                <p className={s.lede}>{t("lede")}</p>
                <div className={s.flourish}>
                    <span className={s.line} />
                    <span className={s.flor}>✦</span>
                    <em>{t("tagline")}</em>
                </div>
            </div>

            <div className={s.formWrap}>
                <form className={s.card} onSubmit={onSubmit} noValidate>
                    <div className={s.tabs} role="tablist">
                        <button
                            type="button"
                            role="tab"
                            className={s.tab}
                            aria-selected={mode === "signin"}
                            onClick={() => switchMode("signin")}
                        >
                            {t("signin")}
                        </button>
                        <button
                            type="button"
                            role="tab"
                            className={s.tab}
                            aria-selected={mode === "register"}
                            onClick={() => switchMode("register")}
                        >
                            {t("register")}
                        </button>
                    </div>

                    <div className={s.fields}>
                        <OrnateField label={t("email")} required>
                            <OrnateTextInput
                                type="email"
                                value={email}
                                autoComplete="email"
                                placeholder="s.vale@aventhal.ink"
                                onChange={(e) => setEmail(e.target.value)}
                            />
                        </OrnateField>

                        <OrnateField label={t("password")} required>
                            <OrnateTextInput
                                type="password"
                                value={password}
                                autoComplete={
                                    mode === "signin"
                                        ? "current-password"
                                        : "new-password"
                                }
                                placeholder="•••••••••"
                                onChange={(e) => setPassword(e.target.value)}
                            />
                        </OrnateField>

                        {registered && !error && (
                            <p className={s.formSuccess} role="status">
                                {t("registerSuccess")}
                            </p>
                        )}
                        {error && (
                            <p className={s.formError} role="alert">
                                {error}
                            </p>
                        )}

                        <div className={s.actions}>
                            <Button type="submit" disabled={busy}>
                                {busy
                                    ? t("working")
                                    : mode === "signin"
                                    ? t("enter")
                                    : t("create")}
                            </Button>
                        </div>
                    </div>
                </form>
            </div>
        </div>
    );
}

export default AuthPage;
