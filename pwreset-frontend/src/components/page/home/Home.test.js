import React from "react";
import { create } from "react-test-renderer";
import { MemoryRouter } from "react-router-dom";

import Home from "./Home";

describe("Home Component", () => {
  const header = {
    name: "Oracle Password Reset"
  };

  const page = {
    header
  };

  test("Matches the snapshot", () => {
    const homepage = create(
      <MemoryRouter>
        <Home page={page} />
      </MemoryRouter>
    );
    expect(homepage.toJSON()).toMatchSnapshot();
  });
});
