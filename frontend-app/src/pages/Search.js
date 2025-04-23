import React, { useState } from 'react';
import '../styles/Search.css';

function Search() {
  const [searchTerm, setSearchTerm] = useState('');
  const [results, setResults] = useState([]);

  const handleSearch = (e) => {
    e.preventDefault();
    // Simulate fetching data from the backend
    const mockData = [
      { isbn: '9789750802942', title: 'Harry Potter ve Felsefe Taşı', author: 'J. K. Rowling', totalCopies: 1, left: 0, branchName: 'Lara' },
      { isbn: '9789750802942', title: 'Harry Potter ve Sırlar Odası', author: 'J. K. Rowling', totalCopies: 1, left: 1, branchName: 'Uncalı' },
      // Add more mock data here
    ];
    const filteredResults = mockData.filter((book) =>
      book.title.toLowerCase().includes(searchTerm.toLowerCase())
    );
    setResults(filteredResults);
  };

  return (
    <div className="search-page">
      <header className="search-header">
        <h1>lib++ Digital Library</h1>
        <form onSubmit={handleSearch} className="search-form">
          <input
            type="text"
            placeholder="Search for books..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
          />
          <button type="submit">Search</button>
        </form>
      </header>
      <div className="search-results">
        <table>
          <thead>
            <tr>
              <th>ISBN</th>
              <th>Title</th>
              <th>Author</th>
              <th>Total Copies</th>
              <th>Left</th>
              <th>Branch Name</th>
            </tr>
          </thead>
          <tbody>
            {results.map((book, index) => (
              <tr key={index}>
                <td>{book.isbn}</td>
                <td>{book.title}</td>
                <td>{book.author}</td>
                <td>{book.totalCopies}</td>
                <td>{book.left}</td>
                <td>{book.branchName}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default Search;