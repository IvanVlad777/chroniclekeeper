import { useState } from "react";
import OrnateTextArea from "../components/basic/forms/textArea/OrnateTextArea";
import OrnateTextInput from "../components/basic/forms/textInput/OrnateTextInput";
import { useTranslation } from "react-i18next";
import OrnateSelect from "../components/basic/forms/select/OrnateSelect";
import OrnateMultiSelect from "../components/basic/forms/multiselect/OrnateMultiSelect";

const DashboardPage = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [status, setStatus] = useState("");
    const [tags, setTags] = useState<string[]>([]);
    const { t } = useTranslation("login");
    return (
        <div>
            <h2>Welcome to your dashboard!</h2>
            <OrnateTextInput
                label={t("email")}
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                placeholder="ime@primjer.com"
                allowedRoles={["SuperAdmin", "Admin"]}
            />

            <OrnateTextInput
                label={t("password")}
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
            />

            <OrnateTextArea
                label={t("bio")}
                placeholder={t("bioPlaceholder")}
                rows={5}
                allowedRoles={["SuperAdmin", "Admin"]}
            />

            <OrnateSelect
                label="Status"
                value={status}
                onChange={(e) => setStatus(e.target.value)}
                //allowedRoles={["Admin", "Editor"]}
                options={[
                    { value: "draft", label: "Skica" },
                    { value: "pub", label: "Objavljeno" },
                    { value: "arch", label: "Arhivirano" },
                ]}
                placeholderOption="Odaberi status…"
            />

            <OrnateMultiSelect
                label="Tagovi"
                value={tags}
                onChange={setTags}
                allowedRoles={["Admin", "Editor"]}
                options={[
                    { value: "fantasy", label: "Fantasy" },
                    { value: "history", label: "Povijest" },
                    { value: "science", label: "Znanost" },
                ]}
                placeholder="Odaberi tagove..."
            />
        </div>
    );
};

export default DashboardPage;
