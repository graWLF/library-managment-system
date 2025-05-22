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
    if (response.status !== 204) {
        return response.json();
    }
    return;
};
export const deleteLibrarian = async (id) => {
    const response = await fetch(`${API_BASE_URL}/librarian/${id}`, {
        method: "DELETE",
    });
    if (!response.ok) {
        throw new Error("Failed to delete librarian");
    }
    return;
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
    const contentLength = response.headers.get("Content-Length");
    if (contentLength && contentLength !== "0") {
        return response.json();
    }
    return;
};

export default function ProtectedRoute({ children }) {
  const isAuthenticated = localStorage.getItem('isAuthenticated') === 'true';
  if (!isAuthenticated) {
    return <Navigate to="/" />;
  } return children;
}

export const findAuthor = async (isbn) => {
    const response = await fetch(`${API_BASE_URL}/Isbnauthorid/${isbn}`);
    if (!response.ok) {
        throw new Error("Failed to fetch book");
    }
    return response.json();
};

export const getAuthorById = async (authorId) => {
  const response = await fetch(`${API_BASE_URL}/Author/${authorId}`);
  if (!response.ok) {
    throw new Error(`Failed to fetch author ${authorId}`);
  }
  return response.json();
};

export const getPublisherById = async (publisherId) => {
  try {
    const response = await fetch(`${API_BASE_URL}/publisher/${publisherId}`);
    const data = await response.json();
    return data;  // Should return an object with { publisherName: 'Some Publisher' }
  } catch (error) {
    console.error('Error fetching publisher:', error);
    throw error;
  }
};

export const fetchBookByGoogle = async (isbn, apiKey) => {
    // 1. Fetch book data from Google via your backend
    const response = await fetch(`${API_BASE_URL}/Book/${isbn}/${apiKey}`);
    if (!response.ok) {
        throw new Error("Failed to fetch book from Google");
    }
    const bookData = await response.json();

    // 2. Post the fetched book data to your database
    //    (Assumes bookData is in the correct format for your postBook method)
    const postResult = await postBook(bookData);

    // 3. Return the result of the post operation
    return postResult;
};

export const fetchAuthors = async () => {
    const response = await fetch(`${API_BASE_URL}/author`);
    if (!response.ok) {
        throw new Error("Failed to fetch authors");
    }
    return response.json();
}

export const addIsbnAuthorid = async (id, authorid) => {
    const response = await fetch(`${API_BASE_URL}/Isbnauthorid`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ id, authorid }),
    });
    if (!response.ok) {
        throw new Error("Failed to add ISBN and Author ID");
    }
    return response.json();
}