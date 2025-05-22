
import React, { useState } from 'react';
import { API_BASE_URL } from '../api/config';
import { useNavigate } from 'react-router-dom';
import '../styles/Login.css';

function Login() {
  localStorage.setItem('isAuthenticated', 'false');
  const navigate = useNavigate();
  const [formData, setFormData] = useState({ username: '', password: '' });
  const [error, setError] = useState('');

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleLogin = async (e) => {
    e.preventDefault();

    try {
      const response = await fetch(`${API_BASE_URL}/Registration/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(formData),
      });

      if (response.status === 200) {
        const roleResponse = await fetch(`${API_BASE_URL}/Registration/checkSupervisor`, {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(formData),
        });

        localStorage.setItem('isAuthenticated', 'true');
        navigate(roleResponse.status === 200 ? '/admin' : '/librarian');
      } else if (response.status === 401) {
        setError('Invalid username or password');
      } else {
        setError('Unexpected error. Please try again.');
      }
    } catch (err) {
      console.error('Login failed:', err);
      setError('Network error. Please try again.');
    }
  };

  return (
    <div className="login-background">
      <div className="login-container">
        <h2>Welcome Back</h2>
        <p className="subtext">Please log in to your account</p>
        <form onSubmit={handleLogin}>
          <div className="form-group">
            <label htmlFor="username">Username or Email</label>
            <input
              type="text"
              id="username"
              name="username"
              value={formData.username}
              onChange={handleChange}
              required
              placeholder="Enter your username or email"
            />
          </div>
          <div className="form-group">
            <label htmlFor="password">Password</label>
            <input
              type="password"
              id="password"
              name="password"
              value={formData.password}
              onChange={handleChange}
              required
              placeholder="Enter your password"
            />
          </div>
          {error && <p className="error">{error}</p>}
          <button type="submit">Login</button>
        </form>
      </div>
    </div>
  );
}

export default Login;