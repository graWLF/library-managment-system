import React, { useState, useEffect } from 'react';
import { fetchBooks, fetchBookByName, fetchBookByIsbn, findAuthor, getAuthorById, fetchLibrarianById, getPublisherById, deleteBook, deleteIsbnAuthorid, deleteBookCopyByIsbn } from '../api/Services';
import '../styles/Search.css';
import { Link } from 'react-router-dom';

function Search() {
  const [bookName, setBookName] = useState('');
  const [books, setBooks] = useState([]);
  const [selectedBook, setSelectedBook] = useState(null);
  const [error, setError] = useState(null);
  const [authors, setAuthors] = useState([]);
  const [librarianName, setLibrarianName] = useState('');
  const [publisherName, setPublisherName] = useState('');

  useEffect(() => {
    const fetchAuthorsAndLibrarian = async () => {
      if (!selectedBook) {
        setAuthors([]);
        setLibrarianName('');
        setPublisherName('');
        return;
      }

      try {
        // Fetch author relations (with authorId)
        const authorRelations = await findAuthor(selectedBook.id); // [{authorId: 1}, ...]
        // Fetch author details and keep authorId
        const authorDetails = await Promise.all(
          authorRelations.map(async (rel) => {
            const author = await getAuthorById(rel.authorId);
            return { ...author, authorId: rel.authorId }; // keep authorId
          })
        );
        setAuthors(authorDetails);

        // Fetch librarian
        if (selectedBook.librarianId) {
          const librarian = await fetchLibrarianById(selectedBook.librarianId);
          setLibrarianName(librarian.librarianName);
        } else {
          setLibrarianName('Unknown');
        }
        // Fetch publisher name
        if (selectedBook.publisherId) {
          const publisher = await getPublisherById(selectedBook.publisherId);
          setPublisherName(publisher.publisher);
        } else {
          setPublisherName('Unknown');
        }
      } catch (error) {
        console.error("Failed to fetch authors, librarian, or publisher", error);
        setAuthors([]);
        setLibrarianName('Unavailable');
        setPublisherName('Unavailable');
      }
    };

    fetchAuthorsAndLibrarian();
  }, [selectedBook]);

  useEffect(() => {
    const loadAllBooks = async () => {
      try {
        const allBooks = await fetchBooks();
        setBooks(allBooks);
      } catch (err) {
        console.error('Failed to load all books:', err);
        setError('Failed to load books.');
      }
    };

    loadAllBooks();
  }, []);

  const handleSearch = async (e) => {
    e.preventDefault();

    try {
      const trimmed = bookName.trim();
      if (trimmed === '') {
        // List all books if search bar is empty
        const allBooks = await fetchBooks();
        setBooks(allBooks);
        setError(null);
        setSelectedBook(null);
        return;
      }

      if (/^\d+$/.test(trimmed)) {
        // If input is all digits, treat as ISBN
        const book = await fetchBookByIsbn(trimmed);
        if (book && book.id) {
          setBooks([book]);
          setError(null);
          setSelectedBook(null);
        } else {
          setBooks([]);
          setSelectedBook(null);
          setError('No book found with this ISBN.');
        }
        return;
      }

      // Otherwise, search by title
      const results = await fetchBookByName(trimmed);
      if (results.length === 0) {
        setError('No books found.');
        setBooks([]);
        setSelectedBook(null);
      } else {
        setBooks(results);
        setError(null);
        setSelectedBook(null);
      }
    } catch (err) {
      setBooks([]);
      setSelectedBook(null);
      setError('Failed to fetch books. Please try again.');
    }
  };

  // Delete all author relations, then delete the book
  const handleDelete = async () => {
    if (window.confirm('Are you sure you want to delete this book?')) {
      try {
        for (const a of authors) {
          try{
          await deleteIsbnAuthorid(selectedBook.id, a.authorId);
          } catch (err) {}
        }
         try {
        await deleteBookCopyByIsbn(selectedBook.id);
      } catch (err) {
        // Ignore if not found or already deleted
      }
        await deleteBook(selectedBook.id);
        setSelectedBook(null);
        setBooks(books.filter(b => b.id !== selectedBook.id));
        alert('Book deleted successfully!');
      } catch (err) {
        alert('Failed to delete book.');
      }
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
                <th>Image</th>
                <td>
                  <img 
                    src={selectedBook.image} 
                    alt={selectedBook.title} 
                    style={{ maxWidth: '150px', height: 'auto' }} 
                  />
                </td>
              </tr>
              <tr>
                <th>ISBN</th>
                <td>{selectedBook.id}</td>
              </tr>
              <tr>
                <th>Title</th>
                <td>{selectedBook.title}</td>
              </tr>
              <tr>
                <th>Author(s)</th>
                <td>
                  {authors.length > 0
                    ? authors.map((a) => a.author).join(', ')
                    : 'Loading authors...'}
                </td>
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
                <th>Price</th>
                <td>{selectedBook.price}</td>
              </tr>
              <tr>
                <th>Content Link</th>
                <td>{selectedBook.contentLink}</td>
              </tr>
              {/* Librarian Name */}
              <tr>
                <th>Librarian</th>
                <td>{librarianName} (ID: {selectedBook.librarianId})</td>
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
                <th>Publisher</th>
                <td>{publisherName} (ID: {selectedBook.publisherId})</td>
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
          <Link to={`/admin/edit-book/${selectedBook.id}`}>
            <button title="Edit">
              <span role="img" aria-label="edit">✏️</span>
            </button>
          </Link>
          <br />
          <button
            onClick={handleDelete}
            className="delete-button"
            style={{ background: '#e74c3c', color: 'white', marginTop: 8 }}
          >
            <span role="img" aria-label="delete">🗑️</span> Delete
          </button>
          <button onClick={() => setSelectedBook(null)} className="back-button" style={{ marginTop: 8 }}>
            ← Back to list
          </button>
        </div>
      )}
    </div>
  );
}

export default Search;
