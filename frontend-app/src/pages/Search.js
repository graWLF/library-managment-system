import React, { useState } from 'react';
import { fetchBookByName, fetchBookById } from '../api/Services';

import '../styles/Search.css';
import { Link } from 'react-router-dom';

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
                key={book.id}
                className="book-list-item"
                onClick={() => setSelectedBook(book)}
              >
                <strong>{book.title}</strong> (ISBN: {book.id})
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
                <td>{selectedBook.id}</td>
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
              <tr>
                <th>Content</th>
                <td>{selectedBook.content}</td>
              </tr>
              <tr>
                <th>Info URL</th>
                <td>{selectedBook.infoUrl}</td>
              </tr>
              <tr>
                <th>Content Language</th>
                <td>{selectedBook.contentLanguage}</td>
              </tr>
              <tr>
                <th>Content Source</th>
                <td>{selectedBook.contentSource}</td>
              </tr>
              <tr>
                <th>Image</th>
                <td>{selectedBook.image}</td>
              </tr>
              <tr>
                <th>Price</th>
                <td>{selectedBook.price}</td>
              </tr>
              <tr>
                <th>Content Link</th>
                <td>{selectedBook.contentLink}</td>
              </tr>
              <tr>
                <th>Librarian ID</th>
                <td>{selectedBook.librarianId}</td>
              </tr>
              <tr>
                <th>Format</th>
                <td>{selectedBook.format}</td>
              </tr>
              <tr>
                <th>Publishing Status</th>
                <td>{selectedBook.publishingstatus}</td>
              </tr>
              <tr>
                <th>Release Date</th>
                <td>{selectedBook.releaseDate}</td>
              </tr>
              <tr>
                <th>Publisher ID</th>
                <td>{selectedBook.publisherId}</td>
              </tr>
              <tr>
                <th>Weight</th>
                <td>{selectedBook.weight}</td>
              </tr>
              <tr>
                <th>Dimensions</th>
                <td>{selectedBook.dimensions}</td>
              </tr>
              <tr>
                <th>Material</th>
                <td>{selectedBook.material}</td>
              </tr>
              <tr>
                <th>Color</th>
                <td>{selectedBook.color}</td>
              </tr>
            </tbody>
          </table>
          <button onClick={() => setSelectedBook(null)} className="back-button">
            ← Back to list
          </button>
          <Link to={`/admin/edit-book/${selectedBook.id}`}>
          <button title="Edit">
                  <span role="img" aria-label="edit">✏️</span>
          </button>
          </Link>
        </div>
      )}
    </div>
  );
}

export default Search;
