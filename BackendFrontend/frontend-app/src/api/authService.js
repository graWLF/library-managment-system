// src/api/authService.js
const API_BASE_URL = 'http://localhost:5000/api/Registration';

export const loginUser = async (credentials) => {
  const response = await fetch(`${API_BASE_URL}/login`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(credentials),
  });

  if (!response.ok) {
    throw new Error("Network error during login.");
  }

  const data = await response.json();
  return data;
};
