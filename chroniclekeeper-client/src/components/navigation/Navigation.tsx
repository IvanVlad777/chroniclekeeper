import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../../hooks/useAuth";
import { useTranslation } from "react-i18next";

const Navigation = () => {
  const { token, user, logout } = useAuth();
  const navigate = useNavigate();
  const roles = Array.isArray(user?.role) ? user.role : [user?.role];
  const { t } = useTranslation();

  //   if (user?.role === 'Admin') { ... }

  const handleLogout = () => {
    logout();
    navigate("/login");
  };

  return (
    <nav style={{ padding: "1rem" }}>
      {token ? (
        <>
          <Link to="/dashboard" style={{ marginRight: "1rem" }}>
            {t("dashboard")}
          </Link>
          {user && (
            <p style={{ marginBottom: "1rem" }}>
              {t("loggedinas")}: <strong>{user.email}</strong>
              <br />
              {t("role")}:{" "}
              <strong>
                {Array.isArray(user.role) ? user.role.join(", ") : user.role}
              </strong>
            </p>
          )}
          <button onClick={handleLogout}>{t("logout")}</button>
          <p>{roles.includes("Admin") && t("adminaccess")}</p>
        </>
      ) : (
        <>
          <Link to="/login" style={{ marginRight: "1rem" }}>
            {t("login")}
          </Link>
          <Link to="/register">{t("register")}</Link>
        </>
      )}
    </nav>
  );
};

export default Navigation;
