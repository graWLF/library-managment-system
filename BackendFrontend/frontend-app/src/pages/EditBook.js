import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { fetchBookById, updateBook } from '../api/Services';

function EditBook() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [book, setBook] = useState(null);
  const [message, setMessage] = useState('');

  useEffect(() => {
    fetchBookById(id)
      .then(setBook)
      .catch(() => setMessage('Failed to fetch book info'));
  }, [id]);

  const handleChange = (e) => {
    setBook({ ...book, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await updateBook(id, book);
      setMessage('Book updated successfully!');
      setTimeout(() => navigate('/admin'), 1000); // Redirect after update
    } catch (error) {
      setMessage('Error: ' + error.message);
    }
  };

  if (!book) return <div>Loading...</div>;

  return (
    <div>
      <h2>Edit Book</h2>
      <form onSubmit={handleSubmit}>
        <div className="form-grid">
            <div>
            <label>ISBN:</label>
            <input name="Id" value={book.id || ''} onChange={handleChange} />
          </div>
          <div>
            <label>Title:</label>
            <input name="title" value={book.title || ''} onChange={handleChange} />
          </div>
          <div>
            <label>Category:</label>
            <input name="category" value={book.category || ''} onChange={handleChange} />
          </div>
          <div>
            <label>Pages:</label>
            <input name="pages" value={book.pages || ''} onChange={handleChange} />
          </div>
          <div>
            <label>Type:</label>
            <input name="type" value={book.type || ''} onChange={handleChange} />
          </div>
          
          <div>
            <label>Duration:</label>
            <input name="duration" value={book.duration || ''} onChange={handleChange} />
          </div>
          <div>
            <label>Content:</label>
            <input name="content" value={book.content || ''} onChange={handleChange} />
          </div>
          <div>
            <label>Info URL:</label>
            <input name="cinfoUrl" value={book.infoUrl || ''} onChange={handleChange} />
          </div>
          <div>
            <label>Content Language:</label>
            <input name="contentLanguage" value={book.contentLanguage || ''} onChange={handleChange} />
          </div>
          <div>
            <label>Content Source:</label>
            <input name="contentSource" value={book.contentSource || ''} onChange={handleChange} />
          </div>
          <div>
            <label>Content Language:</label>
            <input name="contentLanguage" value={book.contentLanguage || ''} onChange={handleChange} />
          </div>
          <div>
            <label>Image:</label>
            <input name="image" value={book.image || ''} onChange={handleChange} />
          </div>
          <div>
            <label>Price:</label>
            <input name="price" value={book.price || ''} onChange={handleChange} />
          </div>
          <div>
            <label>Content Link:</label>
            <input name="contentLink" value={book.contentLink || ''} onChange={handleChange} />
          </div>
          <div>
            <label>Librarian Id:</label>
            <input name="librarianId" value={book.librarianId || ''} onChange={handleChange} />
          </div>
          <div>
            <label>Format:</label>
            <input name="format" value={book.format || ''} onChange={handleChange} />
          </div>
          <div>
            <label>Publishing Status:</label>
            <input name="publishingstatus" value={book.publishingstatus || ''} onChange={handleChange} />
          </div>
          <div>
            <label>Release Date:</label>
            <input name="releaseDate" value={book.releaseDate || ''} onChange={handleChange} />
          </div>
          <div>
            <label>Publisher ID:</label>
            <input name="publisherId" value={book.publisherId || ''} onChange={handleChange} />
          </div>
          <div>
            <label>Weight:</label>
            <input name="weight" value={book.weight || ''} onChange={handleChange} />
          </div>
          <div>
            <label>Dimensions:</label>
            <input name="dimensions" value={book.dimensions || ''} onChange={handleChange} />
          </div>
          <div>
            <label>Material:</label>
            <input name="material" value={book.material || ''} onChange={handleChange} />
          </div>
          <div>
            <label>Color:</label>
            <input name="color" value={book.color || ''} onChange={handleChange} />
          </div>
        </div>
        <button type="submit">Edit</button>
      </form>
      {message && <p>{message}</p>}
    </div>
  );
}

export default EditBook;