// src/services/bookService.js
import API_BASE_URL from "./config.js";

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