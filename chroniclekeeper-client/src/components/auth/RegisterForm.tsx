import { useState } from "react";
import { register } from "../../api/auth";
import { IdentityError } from "../../interfaces/authInterfaces";
import { Link } from "react-router-dom";
import { useTranslation } from "react-i18next";
import styles from "./styles.module.css";
import formStyles from "../basic/forms/formStyles.module.css";
import fieldStyles from "../basic/forms/field/styles.module.css";
import Button from "../basic/button/Button";

const RegisterForm = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const [success, setSuccess] = useState(false);
    const { t } = useTranslation("register");

    const handleRegister = async (e: React.FormEvent) => {
        e.preventDefault();
        setError("");
        setSuccess(false);
        // doraditi da izlista greške
        try {
            await register({ email, password });
            setSuccess(true);
            console.log(t("success"));
        } catch (err) {
            if (err && typeof err === "object" && "$values" in err) {
                const errorArray = (err as { $values: IdentityError[] })
                    .$values;
                const messages = errorArray.map((e) => e.description).join(" ");
                setError(messages);
            } else if (typeof err === "string") {
                setError(err);
            } else {
                console.error(`${t("logunknownerr")}:`, err);
                setError(`${t("logunknownerr")}.`);
            }
        }
    };

    return (
        <form onSubmit={handleRegister} className={styles.ornateCard}>
            <h2 className={styles.ornateTitle}>{t("title")}</h2>
            <p className={styles.ornateSub}>
                {t("alreadyhaveaccount")}{" "}
                <Link to="/login" className={styles.ornateLink}>
                    {t("loginhere")}
                </Link>
            </p>
            <div className={styles.field}>
                <label className={fieldStyles.ornateLabel}>{t("email")}:</label>
                <input
                    type="email"
                    className={formStyles.ornateInput}
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required
                />
            </div>

            <div className={styles.field}>
                <label className={fieldStyles.ornateLabel}>
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
                {t("submit")}
            </Button>

            {success && (
                <p className={styles.alertSuccess}>Uspješna registracija!</p>
            )}
            {error && <p className={styles.alertSuccess}>{error}</p>}
        </form>
    );
};

export default RegisterForm;
