import React from 'react';
import Search from './Search';
import '../styles/AdminDashboard.css';
import { Link } from 'react-router-dom';
import BorrowerManagement from './BorrowerManagement';

function AdminDashboard() {
  return (
    <div className="admin-dashboard">
      <div className="sidebar">
        <button>Add Book</button>
        <button>Librarian</button>
        <button>Supervisor</button>
        <Link to="/admin/borrowers">
          <button>BorrowerManagement</button>
        </Link>
      </div>
      <div className="content">
        <Search />
      </div>
    </div>
  );
}

export default AdminDashboard;