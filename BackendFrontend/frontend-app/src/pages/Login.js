import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import '../styles/Login.css';

function Login() {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({ username: '', password: '' });
  const [error, setError] = useState('');

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleLogin = async (e) => {
    e.preventDefault();

    try {
      const response = await fetch('http://localhost:5000/api/Registration/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(formData),
      });

      if (response.status === 200) {
        // post to check supervisor role
        const roleResponse = await fetch('http://localhost:5000/api/Registration/checkSupervisor', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(formData),
        });
        if (roleResponse.status === 200) {
          navigate('/adminSupervisor'); // ✅ Success: redirect to admin page
        } else {
          navigate('/adminLibrarian'); // ✅ Success: redirect to adminLibrarian page
        }

        // navigate('/admin'); // ✅ Success: redirect to search page
      } else if (response.status === 401) {
        setError('Invalid username or password'); // ❌ Wrong credentials
      } else {
        setError('Unexpected error. Please try again.'); // 🔄 Other issues
      }
    } catch (err) {
      console.error('Login failed:', err);
      setError('Network error. Please try again.');
    }
  };

return (
  <div className="login-container">
    <h2>Login Page</h2>
    <form onSubmit={handleLogin}>
        <div>
          <label htmlFor="username">Username or Email:</label>
          <input
            type="text"
            id="username"
            name="username"
            value={formData.username}
            onChange={handleChange}
            required
          />
        </div>
        <div>
          <label htmlFor="password">Password:</label>
          <input
            type="password"
            id="password"
            name="password"
            value={formData.password}
            onChange={handleChange}
            required
          />
        </div>
        {error && <p style={{ color: 'red' }}>{error}</p>}
        <button type="submit">Login</button>
      </form>
    </div>
  );
}

export default Login;
