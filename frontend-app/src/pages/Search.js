import React, { useState } from 'react';
import { fetchBookByName, fetchBookById } from '../api/Services';

import '../styles/Search.css';

function Search() {
  const [bookName, setBookName] = useState('');
  const [books, setBooks] = useState([]);
  const [selectedBook, setSelectedBook] = useState(null);
  const [error, setError] = useState(null);

  const handleSearch = async (e) => {
    e.preventDefault();
    console.log('Searching for book:', bookName);

    try {
      const results = await fetchBookByName(bookName);
      console.log('Search Result:', results);
      
      if (results.length === 0) {
        setError('No books found.');
        setBooks([]);
        setSelectedBook(null);
      } else {
        setBooks(results);
        setError(null);
        setSelectedBook(null); // Reset previously selected
      }
    } catch (err) {
      console.error('Error fetching book:', err);
      setBooks([]);
      setSelectedBook(null);
      setError('Failed to fetch books. Please try again.');
    }
  };

  return (
    <div className="search-page">
      <header className="search-header">
        <h1>Search Book by Name</h1>
        <form onSubmit={handleSearch} className="search-form">
          <input
            type="text"
            placeholder="Enter Book Name"
            value={bookName}
            onChange={(e) => setBookName(e.target.value)}
          />
          <button type="submit">Search</button>
        </form>
      </header>

      {error && <p className="error">{error}</p>}

      {books.length > 0 && !selectedBook && (
        <div className="search-results">
          <h2>Select a Book</h2>
          <ul className="book-list">
            {books.map((book) => (
              <li
                key={book.isbn}
                className="book-list-item"
                onClick={() => setSelectedBook(book)}
              >
                <strong>{book.title}</strong> (ISBN: {book.isbn})
              </li>
            ))}
          </ul>
        </div>
      )}

      {selectedBook && (
        <div className="search-results">
          <h2>Book Details</h2>
          <table>
            <tbody>
              <tr>
                <th>ISBN</th>
                <td>{selectedBook.isbn}</td>
              </tr>
              <tr>
                <th>Title</th>
                <td>{selectedBook.title}</td>
              </tr>
              <tr>
                <th>Category</th>
                <td>{selectedBook.category}</td>
              </tr>
              <tr>
                <th>Pages</th>
                <td>{selectedBook.pages}</td>
              </tr>
              <tr>
                <th>Type</th>
                <td>{selectedBook.type}</td>
              </tr>
              <tr>
                <th>Duration</th>
                <td>{selectedBook.duration}</td>
              </tr>
            </tbody>
          </table>
          <button onClick={() => setSelectedBook(null)} className="back-button">
            ← Back to list
          </button>
        </div>
      )}
    </div>
  );
}

export default Search;
