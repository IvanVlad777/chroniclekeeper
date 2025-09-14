import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../../hooks/useAuth";
import { useTranslation } from "react-i18next";
import { ThemeSwitcher } from "../../theme/ThemeSwitcher";
import Button from "../basic/button/Button";
import styles from "./styles.module.css";

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
        <nav className={styles.navOrnate}>
            {token ? (
                <>
                    <Link to="/storymap" className={styles.navLink}>
                        {t("storymap")}
                    </Link>
                    {user && (
                        <p className={styles.navInfo}>
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
                    <Button onClick={handleLogout} className={styles.navBtn}>
                        {t("logout")}
                    </Button>
                    <p className={styles.navAdminFlag}>
                        {roles.includes("Admin")
                            ? t("adminaccess")
                            : t("useraccess")}
                    </p>
                </>
            ) : (
                <>
                    <Button>
                        <Link to="/login" className={styles.linkReset}>
                            {t("login")}
                        </Link>
                    </Button>
                    <Button>
                        <Link to="/register" className={styles.linkReset}>
                            {t("register")}
                        </Link>
                    </Button>
                </>
            )}
            <span className={styles.navTheme}>
                <ThemeSwitcher />
            </span>
        </nav>
    );
};

export default Navigation;
