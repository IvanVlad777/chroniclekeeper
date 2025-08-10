import { useState } from "react";
import { register } from "../../api/auth";
import { IdentityError } from "../../interfaces/authInterfaces";
import { Link } from "react-router-dom";
import { useTranslation } from "react-i18next";
import "./styles.css";
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
        <form onSubmit={handleRegister} className="ornate-card">
            <h2 className="ornate-title">{t("title")}</h2>
            <p className="ornate-sub">
                {t("alreadyhaveaccount")}{" "}
                <Link to="/login" className="ornate-link">
                    {t("loginhere")}
                </Link>
            </p>
            <div className="field">
                <label className="ornate-label">{t("email")}:</label>
                <input
                    type="email"
                    className="ornate-input"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required
                />
            </div>

            <div className="field">
                <label className="ornate-label">{t("password")}:</label>
                <input
                    type="password"
                    className="ornate-input"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                />
            </div>

            <Button type="submit" className="btn-ornate login-submit">
                {t("submit")}
            </Button>

            {success && <p className="alert-success">Uspješna registracija!</p>}
            {error && <p className="alert-error">{error}</p>}
        </form>
    );
};

export default RegisterForm;
