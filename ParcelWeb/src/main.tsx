import 'devextreme/dist/css/dx.light.css'
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import "devextreme/dist/css/dx.common.css";
import "devextreme/dist/css/dx.light.css";

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <App />
  </StrictMode>,
)
