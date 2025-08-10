import { BrowserRouter as Router } from "react-router-dom";
import Navigation from "./components/navigation/Navigation";
import AppRoutes from "./routes/AppRoutes";
import { useTranslation } from "react-i18next";
import "./App.css";

function App() {
    const { t } = useTranslation();
    return (
        <div
            className="app-root"
            //className="min-h-screen bg-[var(--color-surface)] text-[var(--color-text)] transition-colors duration-300"
        >
            <Router>
                <header
                    //className="p-4 shadow-md border-b border-[var(--color-accent)]"
                    className="app-banner"
                >
                    <h1
                        className="app-title"
                        //className="text-2xl font-bold"
                    >
                        {t("title")}
                    </h1>
                </header>
                <main className="app-main">
                    <Navigation />
                    <div className="app-content">
                        <AppRoutes />
                    </div>
                </main>
            </Router>
        </div>
    );
}

export default App;
