import {useAuth} from "react-oidc-context";
import {Button} from "@mui/material";

function App() {
  const auth = useAuth();
  switch (auth.activeNavigator) {
    case "signinSilent":
      return <div>Signing you in...</div>;

    case "signoutSilent":
      return <div>Signing you out...</div>
  }

  if (auth.isLoading)
    return <div>Loading...</div>;

  if (auth.error)
    return <div>Oops... {auth.error.message}</div>

  if (auth.isAuthenticated)
    return (
        <div>
          Hello {auth.user?.profile.sub}{' '}
          <Button onClick={() => void auth.removeUser()}>Log out</Button>
        </div>
    );

  return <Button onClick={() => void auth.signinRedirect()}>Log in</Button>
}

export default App
