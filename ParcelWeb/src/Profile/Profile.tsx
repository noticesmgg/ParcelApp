import { useState, useRef, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import './Profile.css';

export default function HeaderProfile() {
    const [open, setOpen] = useState(false);
    const menuRef = useRef<HTMLDivElement>(null);
    const navigate = useNavigate();

    // Close on outside click
    useEffect(() => {
        const handleClickOutside = (e: MouseEvent) => {
            if (menuRef.current && !menuRef.current.contains(e.target as Node)) {
                setOpen(false);
            }
        };
        document.addEventListener("mousedown", handleClickOutside);
        return () => document.removeEventListener("mousedown", handleClickOutside);
    }, []);

    const logout = () => {
        sessionStorage.removeItem("isAuthenticated");
        navigate("/login", { replace: true });
    };

    return (
        <div className="profile-wrapper" ref={menuRef}>
            <div className="profile-trigger" onClick={() => setOpen(!open)}>
                <div className="avatar">AK</div>
                {/* <span className="profile-name">Ajaz</span> */}
            </div>

            {open && (
                <div className="profile-menu">
                    <button onClick={() => navigate("/profile")}>Edit Profile</button>
                    <button onClick={() => navigate("/settings")}>Settings</button>
                    <div className="menu-divider" />
                    <button className="logout" onClick={logout}>
                        Logout
                    </button>
                </div>
            )}
        </div>
    );
};
