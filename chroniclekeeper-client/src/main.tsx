import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { QueryClientProvider } from "@tanstack/react-query";
import "./themes.css";
import "./ornate.vars.css";
import "./index.css";
import App from "./App.tsx";
import { AuthProvider } from "./context/auth/AuthProvider.tsx";
import { queryClient } from "./lib/queryClient.ts";
import "./i18n";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <QueryClientProvider client={queryClient}>
      <AuthProvider>
        <App />
      </AuthProvider>
    </QueryClientProvider>
  </StrictMode>
);
