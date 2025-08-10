import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../../hooks/useAuth";
import { useTranslation } from "react-i18next";
import { ThemeSwitcher } from "../../theme/ThemeSwitcher";
import Button from "../basic/button/Button";
import "./styles.css";

const Navigation = () => {
    const { token, user, logout } = useAuth();
    const navigate = useNavigate();
    const roles = Array.isArray(user?.role)
        ? user.role
        : user?.role
        ? [user.role]
        : [];
    const { t } = useTranslation();

    const { userInfo } = useAuth();
    console.log("roles:", userInfo?.roles);
    //   if (user?.role === 'Admin') { ... }

    const handleLogout = () => {
        logout();
        navigate("/login");
    };

    return (
        <nav className="nav-ornate">
            {token ? (
                <>
                    <Link to="/dashboard" className="nav-link">
                        {t("dashboard")}
                    </Link>
                    {user && (
                        <p className="nav-info">
                            {t("loggedinas")}: <strong>{user.email}</strong>
                            <br />
                            {t("role")}:{" "}
                            <strong>
                                {Array.isArray(user.role)
                                    ? user.role.join(", ")
                                    : user.role}
                            </strong>
                        </p>
                    )}
                    <Button onClick={handleLogout} className="nav-btn">
                        {t("logout")}
                    </Button>
                    <p className="nav-admin-flag">
                        {roles.includes("Admin")
                            ? t("adminaccess")
                            : t("useraccess")}
                    </p>
                </>
            ) : (
                <>
                    <Button>
                        <Link to="/login" className="link-reset">
                            {t("login")}
                        </Link>
                    </Button>
                    <Button>
                        <Link to="/register" className="link-reset">
                            {t("register")}
                        </Link>
                    </Button>
                </>
            )}
            <span className="nav-theme">
                <ThemeSwitcher />
            </span>
        </nav>
    );
};

export default Navigation;
