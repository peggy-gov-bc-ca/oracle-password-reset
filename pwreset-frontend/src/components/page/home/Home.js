import React from "react";
import "../page.css";
import Header from "../../base/header/Header";
import Footer from "../../base/footer/Footer";

export default function Home({ page: { header } }) {
  return (
    <main>
      <Header header={header} />
      <div className="page">
        <div className="content col-md-10">
          <p>Welcome to the Oracle Password Reset App!</p>
        </div>
      </div>
      <Footer />
    </main>
  );
}
