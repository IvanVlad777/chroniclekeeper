import { Navigate } from "react-router-dom";
import { JSX } from "react";
import { useAuth } from "../hooks/useAuth";

const AdminRoute = ({ children }: { children: JSX.Element }) => {
  const { user } = useAuth();

  const roles = Array.isArray(user?.role) ? user?.role : [user?.role];

  if (!roles || !roles.includes("Admin") || !roles.includes("SuperAdmin")) {
    return <Navigate to="/login" replace />;
  }

  return children;
};

export default AdminRoute;
