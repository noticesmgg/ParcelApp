import React, { useState } from "react";
import { TextBox } from "devextreme-react/text-box";
import { Button } from "devextreme-react/button";
import CheckBox from "devextreme-react/check-box";
import { useNavigate } from "react-router-dom";
import "./Register.css";
import { Validator, RequiredRule, EmailRule } from "devextreme-react/validator";
import validationEngine from "devextreme/ui/validation_engine";

const RegisterPage: React.FC = () => {
    const navigate = useNavigate();

    const [fullName, setFullName] = useState("");
    const [username, setUsername] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [acceptTerms, setAcceptTerms] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const handleRegister = () => {
        setError(null);

        const result = validationEngine.validateGroup("registerForm");
        if (!result.isValid) {
            return;
        }

        if (!fullName || !username || !email || !password || !confirmPassword || fullName.trim() == "" || username.trim() == "" || email.trim() == "" || password.trim() == "" || confirmPassword.trim() == "") {
            setError("All fields are required");
            return;
        }

        if (password !== confirmPassword) {
            setError("Passwords do not match");
            return;
        }

        if (!acceptTerms) {
            setError("Please accept the terms and conditions");
            return;
        }

        // Call backend register API here

        navigate("/login");
    };

    return (
        <div className="register-container">
            <div className="auth-left">
                <h1>Create your Parcel Account</h1>
                <p>
                    Join Parcel Management to securely manage land parcels,
                    ownership records, documents, and land bank data.
                </p>

                <ul>
                    <li>✔ Secure access</li>
                    <li>✔ Centralized records</li>
                    <li>✔ Faster parcel workflows</li>
                </ul>
            </div>

            <div className="auth-right">
                <div className="register-card">
                    <h1 className="register-title">Create Account</h1>
                    <p className="register-subtitle">
                        Fill in the details to get started
                    </p>

                    {error && <div className="register-error">{error}</div>}

                    <div className="register-field">
                        <TextBox
                            value={fullName}
                            onValueChanged={(e) => setFullName(e.value)}
                            label="Full Name"
                            labelMode="floating"
                            stylingMode="outlined"
                        />
                    </div>

                    <div className="register-field">
                        <TextBox
                            value={username}
                            onValueChanged={(e) => setUsername(e.value)}
                            label="Username"
                            labelMode="floating"
                            stylingMode="outlined"
                        />
                    </div>

                    <div className="register-field">
                        <TextBox
                            value={email}
                            onValueChanged={(e) => setEmail(e.value)}
                            label="Email"
                            labelMode="floating"
                            stylingMode="outlined"
                        >
                            <Validator validationGroup="registerForm">
                                <RequiredRule message="Email is required" />
                                <EmailRule message="Please enter a valid email address" />
                            </Validator>
                        </TextBox>
                    </div>

                    <div className="register-field">
                        <TextBox
                            value={password}
                            onValueChanged={(e) => setPassword(e.value)}
                            mode="password"
                            label="Password"
                            labelMode="floating"
                            stylingMode="outlined"
                        >
                            <Validator>
                                <RequiredRule message="Password is required" />
                            </Validator>
                        </TextBox>
                    </div>

                    <div className="register-field">
                        <TextBox
                            value={confirmPassword}
                            onValueChanged={(e) => setConfirmPassword(e.value)}
                            mode="password"
                            label="Confirm Password"
                            labelMode="floating"
                            stylingMode="outlined"
                        />
                    </div>

                    <div className="register-actions">
                        <CheckBox
                            value={acceptTerms}
                            onValueChanged={(e) => setAcceptTerms(e.value)}
                            text="I agree to the Terms & Conditions"
                        />
                    </div>

                    <Button
                        text="Create Account"
                        type="default"
                        width="100%"
                        onClick={handleRegister}
                    />

                    <div className="register-footer">
                        Already have an account?
                        <button
                            type="button"
                            onClick={() => navigate("/login")}
                        >
                            Sign in
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default RegisterPage;
