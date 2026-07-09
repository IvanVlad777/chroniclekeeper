import { Outlet } from "react-router-dom";
import { TopBar } from "./TopBar";
import { Sidebar } from "./Sidebar";
import s from "./shell.module.css";

/** Glavni okvir aplikacije: top bar + sidebar + sadržaj (Outlet). */
export function AppShell() {
    return (
        <div className={s.shell}>
            <TopBar />
            <div className={s.body}>
                <Sidebar />
                <main className={s.main}>
                    <Outlet />
                </main>
            </div>
        </div>
    );
}
