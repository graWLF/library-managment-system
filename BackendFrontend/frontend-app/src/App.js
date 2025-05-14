import { BrowserRouter as Router, Routes, Route, Link, Navigate } from 'react-router-dom';
import React, { useState } from 'react';
import './App.css';
import Login from './pages/Login';
import Search from './pages/Search';
import AdminDashboard from './pages/AdminDashboard';
import BorrowerManagement from './pages/BorrowerManagement';
import Layout from './Layout';
import AddBook from './pages/AddBook';
import EditBook from './pages/EditBook';
import EditBorrower from './pages/EditBorrower';

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);

  return (
    <Router>
      <Routes>
        <Route
          path="/"
          element={
            isLoggedIn ? (
              <Navigate to="/admin" />
            ) : (
              <Login onLogin={() => setIsLoggedIn(true)} />
            )
          }
        />
        <Route path="/admin" element={<AdminDashboard />} />
        <Route path="/admin/borrowers" element={<BorrowerManagement />} />
        <Route path="/admin/add-book" element={<AddBook />} />
        <Route path="/admin/edit-book/:id" element={<EditBook />} />
        <Route path="/admin/edit-borrower/:id" element={<EditBorrower />} />
      </Routes>
    </Router>
  );
}

export default App;
