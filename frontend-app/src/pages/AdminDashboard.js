import React from 'react';
import Search from './Search';
import '../styles/AdminDashboard.css';
import { Link } from 'react-router-dom';
import BorrowerManagement from './BorrowerManagement';

function AdminDashboard() {
  const handleLogout = () => {
    // You can also clear any auth tokens here
    window.location.href = '/'; // Redirect to login or home
  };
  return (
    <div className="admin-dashboard">
      <div className="sidebar">
        <button>Add Book</button>
        <button>Librarian</button>
        <button>Supervisor</button>
        <Link to="/admin/borrowers">
          <button>BorrowerManagement</button>
        </Link>
        <button 
          onClick={handleLogout} 
          style={{
            backgroundColor: '#dc2626', // Tailwind's red-600
            color: 'white',
            border: 'none',
            padding: '10px 20px',
            cursor: 'pointer',
            borderRadius: '4px'
          }}
        >
          Logout
        </button>

      </div>
      <div className="content">
        <Search />
      </div>
    </div>
  );
}

export default AdminDashboard;