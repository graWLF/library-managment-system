import React, { useState } from 'react';
import { postBook } from '../api/Services'; // Ensure this function is defined in your Services.js file

function AddBook() {
  const [title, setTitle] = useState('');
  const [author, setAuthor] = useState('');
  const [isbn, setIsbn] = useState('');
  const [message, setMessage] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const book = { title, author, isbn }; // Create the book object
      await postBook(book); // Call the postBook method
      setMessage('Book added successfully!');
      setTitle('');
      setAuthor('');
      setIsbn('');
    } catch (error) {
      setMessage('Error: ' + error.message);
    }
  };

  return (
    <div>
      <h2>Add Book</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Title:</label>
          <input
            type="text"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            required
          />
        </div>
        <div>
          <label>Author:</label>
          <input
            type="text"
            value={author}
            onChange={(e) => setAuthor(e.target.value)}
            required
          />
        </div>
        <div>
          <label>ISBN:</label>
          <input
            type="text"
            value={isbn}
            onChange={(e) => setIsbn(e.target.value)}
            required
          />
        </div>
        <button type="submit">Add Book</button>
      </form>
      {message && <p>{message}</p>}
    </div>
  );
}

export default AddBook;