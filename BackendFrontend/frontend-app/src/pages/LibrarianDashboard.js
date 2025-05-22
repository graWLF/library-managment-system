import React from 'react';
import { Link } from 'react-router-dom';
import Search from './Search';
import '../styles/AdminDashboard.css';

function AdminDashboard() {
  const handleLogout = () => {
    window.location.href = '/'; // Redirect to login or home
  };

  return (
    <div className="admin-dashboard">
      <aside className="sidebar">
        <div className="logo">📚 Admin Panel</div>
        <nav className="nav-links">
          <Link to="/librarian/upload-barcode">
            <button>Add Book by Google</button>
          </Link>
          <Link to="/librarian/add-book">
            <button>Add Book</button>
          </Link>
          <Link to="/librarian/borrowers">
            <button>Borrower Management</button>
          </Link>
        </nav>
        <button className="logout-button" onClick={handleLogout}>
          Logout
        </button>
      </aside>
      <main className="content">
        <Search />
      </main>
    </div>
  );
}

export default AdminDashboard;
