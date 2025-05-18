// src/api/authService.js
import { API_BASE_URL } from './config'; // Adjust the import path as necessary

export const loginUser = async (credentials) => {
  const response = await fetch(`${API_BASE_URL}/Registration/login`, {
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
