import { useState } from "react";
import { login } from "../../api/auth";
import { Link, useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";

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
    <form onSubmit={handleSubmit}>
      <h2>{t("title")}</h2>
      <p>
        {t("donthaveacc")} <Link to="/register">{t("registerhere")}</Link>
      </p>
      <div>
        <label htmlFor="">{t("email")}:</label>
        <input
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />
      </div>

      <div>
        <label htmlFor="">{t("password")}:</label>
        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />
      </div>

      <button type="submit">{t("submit")}</button>
      {success && <p style={{ color: "green" }}>{t("successfullogin")}</p>}
      {error && <p style={{ color: "red" }}>{error}</p>}
    </form>
  );
};

export default LoginForm;
