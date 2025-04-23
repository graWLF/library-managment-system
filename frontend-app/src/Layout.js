import React from 'react';
import { Link } from 'react-router-dom';

function Layout({ children }) {
  return (
    <div>
      <header>
        <h1>lib++ Digital Library</h1>
      </header>
      <nav>
        <Link to="/">Login</Link> | <Link to="/home">Home</Link> | <Link to="/admin">Admin Dashboard</Link>
      </nav>
      <main>{children}</main>
      <footer>
        <p>© 2025 lib++ Digital Library</p>
      </footer>
    </div>
  );
}

export default Layout;