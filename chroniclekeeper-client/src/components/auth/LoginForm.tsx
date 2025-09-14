import { useState } from "react";
import { login } from "../../api/auth";
import { Link, useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import styles from "./styles.module.css";
import formStyles from "../basic/forms/formStyles.module.css";
import fieldStyles from "../basic/forms/field/styles.module.css";
import Button from "../basic/button/Button";
import { useAuth } from "../../hooks/useAuth";

const LoginForm = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const [success, setSuccess] = useState(false);
    const navigate = useNavigate();
    const { t } = useTranslation("login");
    const { login: authLogin } = useAuth();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError("");
        setSuccess(false);

        try {
            const response = await login({ email, password });
            sessionStorage.setItem("token", response.token);
            authLogin(response.token);
            setSuccess(true);
            console.log(t("logsucces"));
            navigate("/storymap");
        } catch (err) {
            if (typeof err === "string") {
                setError(err);
            } else {
                console.error(`${t("logunknownerr")}:`, err);
                setError(`${t("logunknownerr")}.`);
            }
        }
    };

    return (
        <form onSubmit={handleSubmit} className={styles.ornateCard}>
            <h2 className={styles.ornateTitle}>{t("title")}</h2>
            <p className={styles.ornateSub}>
                {t("donthaveacc")}{" "}
                <Link to="/register" className={styles.ornateLink}>
                    {t("registerhere")}
                </Link>
            </p>
            <div className={styles.field}>
                <label className={fieldStyles.ornateLabel} htmlFor="">
                    {t("email")}:
                </label>
                <input
                    type="email"
                    className={formStyles.ornateInput}
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required
                />
            </div>

            <div className={styles.field}>
                <label className={fieldStyles.ornateLabel} htmlFor="">
                    {t("password")}:
                </label>
                <input
                    type="password"
                    className={formStyles.ornateInput}
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                />
            </div>

            <Button type="submit" className={`${styles.loginSubmit}`}>
                {t("submit")}{" "}
            </Button>
            {success && (
                <p className={styles.alertSucces}>{t("successfullogin")}</p>
            )}
            {error && <p className={styles.alertError}>{error}</p>}
        </form>
    );
};

export default LoginForm;
