import { BrowserRouter as Router, Routes, Route, Link, Navigate } from 'react-router-dom';
import React, { useState } from 'react';
import './App.css';
import Login from './pages/Login';
import Search from './pages/Search';
import AdminDashboard from './pages/AdminDashboard';
import BorrowerManagement from './pages/BorrowerManagement';
import Layout from './Layout';

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
      </Routes>
    </Router>
  );
}

export default App;
