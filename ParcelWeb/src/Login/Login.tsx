import React, { useState } from "react";
import { TextBox } from "devextreme-react/text-box";
import { Button } from "devextreme-react/button";
import CheckBox from "devextreme-react/check-box";
import { useNavigate } from "react-router-dom";
import './Login.css';

export type LoginRequest = {
    UserName: string,
    Password: string,
}

const API_URL = import.meta.env.VITE_API_URL;

const LoginPage: React.FC = () => {
    const navigate = useNavigate();


    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [rememberMe, setRememberMe] = useState(false);
    const [error, setError] = useState<string | null>(null);


    const handleLogin = async () => {
        setError(null);


        if (!username || !password || username.trim() == "" || password.trim() == "") {
            setError("Username and password are required");
            return;
        }

        try {
            const request: LoginRequest = {
                UserName: username,
                Password: password
            };

            const response = await fetch(`${API_URL}/auth/login`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(request)
            });

            const resp = await response.json();

            if (!resp.Success) {
                setError(resp.Message);
                return;
            }

            sessionStorage.setItem("isAuthenticated", "true");
            if (rememberMe) {
                localStorage.setItem("rememberMe", "true");
            }

            navigate("/");
        } catch {
            setError("Invalid username or password");
        }
    };


    return (
        <div className="login-container">
            <div className="auth-left">
                <h1>Welcome to Parcel Management</h1>
                <p>
                    A centralized platform to manage land parcels, documents,
                    ownership details, and land bank records efficiently.
                </p>

                <ul>
                    <li>✔ Secure parcel records</li>
                    <li>✔ Image & document management</li>
                    <li>✔ Fast search & tracking</li>
                </ul>
            </div>
            <div className="auth-right">
                <div className="login-card">
                    <h1 className="login-title">Parcel Management</h1>
                    <p className="login-subtitle">Sign in to continue</p>


                    {error && <div className="login-error">{error}</div>}


                    <div className="login-field">
                        <TextBox
                            value={username}
                            onValueChanged={(e) => setUsername(e.value)}
                            label="Username"
                            labelMode="floating"
                            stylingMode="outlined"
                        />
                    </div>


                    <div className="login-field">
                        <TextBox
                            value={password}
                            onValueChanged={(e) => setPassword(e.value)}
                            mode="password"
                            label="Password"
                            labelMode="floating"
                            stylingMode="outlined"
                        />
                    </div>


                    <div className="login-actions">
                        <CheckBox
                            text="Remember me"
                            value={rememberMe}
                            onValueChanged={(e) => setRememberMe(e.value)}
                        />
                        <a href="/forgot-password">Forgot password?</a>
                    </div>


                    <Button
                        text="Login"
                        type="default"
                        width="100%"
                        onClick={handleLogin}
                    />

                    <div className="create-account">
                        <span>Don’t have an account?</span>
                        <button
                            type="button"
                            className="create-account-link"
                            onClick={() => navigate("/register")}
                        >
                            Create Account
                        </button>
                    </div>

                    <div className="login-footer">© 2026 Parcel App</div>
                </div>
            </div>
        </div>
    );
};


export default LoginPage;