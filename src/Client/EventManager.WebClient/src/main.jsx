import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './components/App.jsx'
import '@fontsource/roboto/300.css'
import '@fontsource/roboto/400.css'
import '@fontsource/roboto/500.css'
import '@fontsource/roboto/700.css'
import {CssBaseline} from "@mui/material";
import {ORIGINAL_DESTINATION} from "./components/App.const.jsx";
import {AuthProvider} from "react-oidc-context";

const oidcConfig = {
  authority: import.meta.env.VITE_IDENTITY_URL,
  client_id: import.meta.env.VITE_CLIENT_ID,
  redirect_uri: import.meta.env.VITE_CLIENT_URL,
  loadUserInfo: true
};

const signinCallback = () => {
  const url = window.sessionStorage.getItem(ORIGINAL_DESTINATION);
  if (url) {
    window.sessionStorage.removeItem(ORIGINAL_DESTINATION);
    window.location.replace(url);
  } else {
    window.history.replaceState({}, document.title, window.location.pathname);
  }
}

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <AuthProvider onSigninCallback={signinCallback} {...oidcConfig}>
      <CssBaseline/>
      <App/>
    </AuthProvider>
  </React.StrictMode>,
)
