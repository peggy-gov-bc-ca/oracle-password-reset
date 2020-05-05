import React from "react";
import { create } from "react-test-renderer";
import { Router } from "react-router-dom";
import { createMemoryHistory } from "history";
import { render, fireEvent, getAllByRole, wait } from "@testing-library/react";

import Header, { goHome } from "./Header";

describe("Header Component", () => {
  const header = {
    name: "Oracle Password Reset"
  };

  window.confirm = jest.fn();

  test("Matches the snapshot", () => {
    const history = createMemoryHistory();

    const headerComponent = create(
      <Router history={history}>
        <Header header={header} />
      </Router>
    );
    expect(headerComponent.toJSON()).toMatchSnapshot();
  });
});
