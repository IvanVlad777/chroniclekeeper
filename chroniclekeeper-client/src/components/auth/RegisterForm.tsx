import { useState } from "react";
import { register } from "../../api/auth";
import { IdentityError } from "../../interfaces/authInterfaces";
import { Link } from "react-router-dom";
import { useTranslation } from "react-i18next";

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
        const errorArray = (err as { $values: IdentityError[] }).$values;
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
    <form onSubmit={handleRegister}>
      <h2>{t("title")}</h2>
      <p>
        {t("alreadyhaveaccount")} <Link to="/login">{t("loginhere")}</Link>
      </p>
      <div>
        <label>{t("email")}:</label>
        <input
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />
      </div>

      <div>
        <label>{t("password")}:</label>
        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />
      </div>

      <button type="submit">{t("submit")}</button>

      {success && <p style={{ color: "green" }}>Uspješna registracija!</p>}
      {error && <p style={{ color: "red" }}>{error}</p>}
    </form>
  );
};

export default RegisterForm;
