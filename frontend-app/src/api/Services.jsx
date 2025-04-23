// src/services/bookService.js
import API_BASE_URL from "./config";

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
    const response = await fetch(`${API_BASE_URL}/book/search?name=${encodeURIComponent(name)}`); 
    if (!response.ok) {
        throw new Error("Failed to fetch book by name");
    }
    return response.json();
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