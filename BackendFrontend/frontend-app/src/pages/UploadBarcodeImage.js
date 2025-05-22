import React, { useState } from 'react';
import { API_BASE_URL } from '../api/config';
import { fetchBookByGoogle } from '../api/Services.jsx';
import { GOOGLE_API_KEY } from '../api/config'; 

const UploadBarcodeImage = () => {
  const [file, setFile] = useState(null);
  const [isbn, setIsbn] = useState('');
  const [isbnSearch, setIsbnSearch] = useState('');
  const [error, setError] = useState('');
  const [book, setBook] = useState(null);
  const [message, setMessage] = useState('');

  const handleFileChange = (e) => {
    setFile(e.target.files[0]);
    setIsbn('');
    setIsbnSearch('');
    setError('');
    setBook(null);
    setMessage('');
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!file) {
      setError('Please upload an image file.');
      return;
    }

    const formData = new FormData();
    formData.append('image', file);

    try {
      const isbnRes = await fetch(`${API_BASE_URL}/book/extract-isbn`, {
        method: 'POST',
        body: formData,
      });

      if (!isbnRes.ok) {
        throw new Error(await isbnRes.text());
      }

      const extractedIsbn = await isbnRes.json(); // Assuming response is JSON with { "barcode": "9786057775986" }

      // Extract ISBN from the response object and set it to state
      if (extractedIsbn?.barcode) {
        setIsbn(extractedIsbn.barcode); // Set the raw ISBN value
        setIsbnSearch(extractedIsbn.barcode); // Autofill the ISBN input
        setMessage('ISBN extracted! You can now add by Google.');
      } else {
        setError('ISBN extraction failed.');
      }
    } catch (err) {
      setError(err.message);
    }
  };

  // Add by Google logic
  const handleGoogleAdd = async () => {
    setError('');
    setMessage('');
    setBook(null);
    if (!isbnSearch) {
      setError('Please enter an ISBN.');
      return;
    }
    try {
      const apiKey = GOOGLE_API_KEY; // Replace with your actual API key
      const result = await fetchBookByGoogle(isbnSearch, apiKey);
      setBook(result);
      setMessage('Book added to database!');
    } catch (err) {
      setError(err.message);
    }
  };

  return (
    <div>
      <h2>Upload Barcode Image</h2>
      <form onSubmit={handleSubmit}>
        <input type="file" accept="image/*" onChange={handleFileChange} />
        <button type="submit">Extract ISBN</button>
      </form>

      {isbn && <p><strong>Extracted ISBN:</strong> {isbn}</p>}

      <div style={{ marginBottom: 16 }}>
        <input
          type="text"
          placeholder="Enter ISBN"
          value={isbnSearch}
          onChange={e => setIsbnSearch(e.target.value)}
        />
        <button type="button" onClick={handleGoogleAdd}>
          Add by Google
        </button>
      </div>

      {message && <p style={{ color: 'green' }}>{message}</p>}
      {error && <p style={{ color: 'red' }}>Error: {error}</p>}

      {/* {book && (
        <div style={{ marginTop: 16 }}>
          <h3>Book Data:</h3>
          <pre>{JSON.stringify(book, null, 2)}</pre>
        </div>
      )} */}
    </div>
  );
};

export default UploadBarcodeImage;
