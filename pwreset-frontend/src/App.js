import React from "react";
import { Switch, Route, Redirect } from "react-router-dom";
import Home from "./components/page/home/Home";

export default function App() {
  const header = {
    name: "Oracle Password Reset"
  };

  return (
    <div>
      <Switch>
        <Redirect exact from="/" to="/oraclepwreset" />
        <Route exact path="/oraclepwreset">
          <Home page={{ header }} />
        </Route>
      </Switch>
    </div>
  );
}
