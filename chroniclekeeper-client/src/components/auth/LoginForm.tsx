import { useState } from "react";
import { login } from "../../api/auth";
import { Link, useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import "./styles.css";
import Button from "../basic/button/Button";

const LoginForm = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const [success, setSuccess] = useState(false);
    const navigate = useNavigate();
    const { t } = useTranslation("login");

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError("");
        setSuccess(false);

        try {
            const response = await login({ email, password });
            sessionStorage.setItem("token", response.token);
            setSuccess(true);
            console.log(t("logsucces"));
            navigate("/dashboard");
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
        <form onSubmit={handleSubmit} className="ornate-card">
            <h2 className="ornate-title">{t("title")}</h2>
            <p className="ornate-sub">
                {t("donthaveacc")}{" "}
                <Link to="/register" className="ornate-link">
                    {t("registerhere")}
                </Link>
            </p>
            <div className="field">
                <label className="ornate-label" htmlFor="">
                    {t("email")}:
                </label>
                <input
                    type="email"
                    className="ornate-input"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required
                />
            </div>

            <div className="field">
                <label className="ornate-label" htmlFor="">
                    {t("password")}:
                </label>
                <input
                    type="password"
                    className="ornate-input"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                />
            </div>

            <Button type="submit" className="btn-ornate login-submit">
                {t("submit")}{" "}
            </Button>
            {success && <p className="alert-success">{t("successfullogin")}</p>}
            {error && <p className="alert-error">{error}</p>}
        </form>
    );
};

export default LoginForm;
