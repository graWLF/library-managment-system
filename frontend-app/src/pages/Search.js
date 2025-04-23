import React, { useState } from 'react';
import { fetchBookByName } from '../api/Services';
import '../styles/Search.css';

function Search() {
  const [bookName, setBookName] = useState('');
  const [book, setBook] = useState(null);
  const [error, setError] = useState(null);

  const handleSearch = async (e) => {
    e.preventDefault();
    try {
      const result = await fetchBookByName(bookName);
      setBook(result);
      setError(null); // Clear any previous errors
    } catch (err) {
      setBook(null);
      setError('Failed to fetch book. Please check the name and try again.');
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
      {book && (
        <div className="search-results">
          <h2>Book Details</h2>
          <table>
            <tbody>
              <tr>
                <th>ISBN</th>
                <td>{book.isbn}</td>
              </tr>
              <tr>
                <th>Title</th>
                <td>{book.title}</td>
              </tr>
              <tr>
                <th>Author</th>
                <td>{book.author}</td>
              </tr>
              <tr>
                <th>Total Copies</th>
                <td>{book.totalCopies}</td>
              </tr>
              <tr>
                <th>Left</th>
                <td>{book.left}</td>
              </tr>
              <tr>
                <th>Branch Name</th>
                <td>{book.branchName}</td>
              </tr>
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}

export default Search;