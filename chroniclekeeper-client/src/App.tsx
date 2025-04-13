import { BrowserRouter as Router } from "react-router-dom";
import Navigation from "./components/navigation/Navigation";
import AppRoutes from "./routes/AppRoutes";
import { useTranslation } from "react-i18next";

function App() {
  const { t } = useTranslation();
  return (
    <div>
      <Router>
        <h1>{t("title")}</h1>
        <Navigation />
        <AppRoutes />
      </Router>
    </div>
  );
}

export default App;
