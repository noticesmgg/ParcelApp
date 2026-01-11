import { Navigate } from "react-router-dom";
import type { ReactNode } from "react";

const RequireAuth = ({ children }: { children: ReactNode }) => {
    const isAuthenticated = sessionStorage.getItem("isAuthenticated") === "true";

    if (!isAuthenticated) {
        return <Navigate to="/login" replace />;
    }

    return <>{children}</>;
};

export default RequireAuth;
