import { useTranslation } from "react-i18next";
import { Link } from "react-router-dom";

export default function Overview() {
    const { t } = useTranslation("overview");
    return (
        <div>
            <h1>{t("overview")}</h1>
            <ul>
                <li>
                    <Link to="characters">
                        {t("characters") || "Characters"}
                    </Link>
                </li>
            </ul>
            {/* Ovdje kasnije ubacim “cards”: broj likova, nedavni unosi, brzi linkovi... */}
        </div>
    );
}
