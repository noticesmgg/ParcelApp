import { licenseKey } from './devextreme-license';
import config from 'devextreme/core/config';
import ParcelGrid from './Parcel/Parcel';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import LoginPage from './Login/Login';
import LoadingSpinner from './Loading/Loading'
import { Suspense } from 'react';
import RequireAuth from './Auth';
import RegisterPage from './Register/Register';

config({ licenseKey });

function App() {
  return (
    <BrowserRouter future={{ v7_startTransition: true, v7_relativeSplatPath: true }}>
      <Suspense fallback={<LoadingSpinner />}>
        <Routes>
          <Route path="/login" element={<LoginPage />} />
          <Route path="/" element={<RequireAuth> <ParcelGrid /></RequireAuth>} />
          <Route path="/register" element={<RegisterPage />} />

        </Routes>
      </Suspense>
    </BrowserRouter>


  );
}

export default App
