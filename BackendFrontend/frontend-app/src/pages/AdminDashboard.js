import React from 'react';
import { Link } from 'react-router-dom';
import Search from './Search';
import '../styles/AdminDashboard.css';

function AdminDashboard() {
  const handleLogout = () => {
    window.location.href = '/'; // Redirect to login
  };

  return (
    <div className="admin-dashboard">
      <aside className="sidebar">
        <div className="sidebar-header">
          <h2>📚 Admin</h2>
        </div>

        <nav className="nav-links">
          <Link to="/admin/add-book">
            <button>Add Book</button>
          </Link>
          <Link to="/admin/librarian-managment">
            <button>Librarian</button>
          </Link>
          <button className="sidebar-button">Supervisor</button>
          <Link to="/admin/borrowers">
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
