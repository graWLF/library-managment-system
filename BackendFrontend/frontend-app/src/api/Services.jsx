// src/services/bookService.js
import {API_BASE_URL} from "./config.js";
import React from 'react';
import { Navigate } from 'react-router-dom';

export const fetchBooks = async () => {
    const response = await fetch(`${API_BASE_URL}/book`);
    if (!response.ok) {
        throw new Error("Failed to fetch books");
    }
    return response.json();
};

export const fetchBookById = async (id) => {
    const response = await fetch(`${API_BASE_URL}/book/${id}`);
    if (!response.ok) {
        throw new Error("Failed to fetch book");
    }
    return response.json();
};

export const fetchBookByName = async (name) => {
    const url = `${API_BASE_URL}/book/search?name=${encodeURIComponent(name)}`;
    console.log('Request URL:', url); // Log the request URL
    const response = await fetch(url);
    console.log('API Response:', response); // Log the raw response
    if (!response.ok) {
        throw new Error("Failed to fetch book by name");
    }
    const data = await response.json();
    console.log('Book Data:', data); // Log the parsed data
    return data;
};

export const createBook = async (book) => {
    const response = await fetch(`${API_BASE_URL}/book`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(book),
    });
    if (!response.ok) {
        throw new Error("Failed to create book");
    }
    return response.json();
};

export const updateBook = async (id, book) => {
    const response = await fetch(`${API_BASE_URL}/book/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(book),
    });
    if (!response.ok) {
        throw new Error("Failed to update book");
    }
    return response.json();
};

export const deleteBook = async (id) => {
    const response = await fetch(`${API_BASE_URL}/book/${id}`, {
        method: "DELETE",
    });
    if (!response.ok) {
        throw new Error("Failed to delete book");
    }
};

export const fetchBorrowers = async () => {
    const response = await fetch(`${API_BASE_URL}/borrower`);
    if (!response.ok) {
        throw new Error("Failed to fetch borrowers");
    }
    return response.json();
}

export const postBook = async (book) => {
    const response = await fetch(`${API_BASE_URL}/book`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(book),
    });
    if (!response.ok) {
        throw new Error("Failed to add book");
    }
    return response.json();
}

export async function createBorrower(data) {
    const response = await fetch(`${API_BASE_URL}/borrower`, {
      method: 'POST',
      headers: {
        'Accept': '*/*',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        id: 0, // or omit if the backend sets ID automatically
        borrowerName: data.borrowerName,
        borrowerPhone: data.borrowerPhone
      })
    });
  
    return response.ok;
  }
  
  
  export async function createBorrowing(data) {
    const response = await fetch(`${API_BASE_URL}/borrowing`, {
      method: 'POST',
      headers: {
        'Accept': '*/*',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        id: data.id,  
        borrowerId: data.borrowerId,
        branchId: data.branchId ?? 0,  // Optional, default to 0
        borrowDate: data.borrowDate,
        dueDate: data.dueDate,
        returnStatus: data.returnStatus ?? false // Optional, default to false
      })
    });
  
    return response.ok;
  }
  
  export const fetchBorrowings = async () => {
    const response = await fetch(`${API_BASE_URL}/borrowing`); // Adjust this endpoint
    if (!response.ok) throw new Error('Failed to fetch borrowings');
    return await response.json();
  };

  export const fetchLibrarians = async () => {
    const response = await fetch(`${API_BASE_URL}/librarian`);
    if (!response.ok) {
        throw new Error("Failed to fetch librarians");
    }
    return response.json();
}
export const fetchLibrarianById = async (id) => {
    const response = await fetch(`${API_BASE_URL}/librarian/${id}`);
    if (!response.ok) {
        throw new Error("Failed to fetch librarian");
    }
    return response.json();
}
export const updateLibrarian = async (id, librarian) => {
    const response = await fetch(`${API_BASE_URL}/librarian/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(librarian),
    });
    if (!response.ok) {
        throw new Error("Failed to update librarian");
    }
    return response.json();
};
export const deleteLibrarian = async (id) => {
    const response = await fetch(`${API_BASE_URL}/librarian/${id}`, {
        method: "DELETE",
    });
    if (!response.ok) {
        throw new Error("Failed to delete librarian");
    }
};
export const createLibrarian = async (librarian) => {
    const response = await fetch(`${API_BASE_URL}/librarian`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(librarian),
    });
    if (!response.ok) {
        throw new Error("Failed to create librarian");
    }
    return response.json();
};
function ProtectedRoute({ children }) {
  const isAuthenticated = localStorage.getItem('isAuthenticated') === 'true';
  if (!isAuthenticated) {
    return <Navigate to="/" />;
  } return children;
}

export default ProtectedRoute;
  