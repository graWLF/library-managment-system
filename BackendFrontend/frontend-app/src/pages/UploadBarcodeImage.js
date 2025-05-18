import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { API_BASE_URL } from '../api/config'; // Adjust the import path as necessary

const UploadBarcodeImage = () => {
  const [file, setFile] = useState(null);
  const [isbn, setIsbn] = useState('');
  const [error, setError] = useState('');
  const [book, setBook] = useState(null);

  const handleFileChange = (e) => {
    setFile(e.target.files[0]);
    setIsbn('');
    setError('');
    setBook(null);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!file) {
      setError('Please upload an image file.');
      return;
    }

    const formData = new FormData();
    formData.append('image', file); // ✅ field name must match IFormFile param

    try {
      const isbnRes = await fetch(`${API_BASE_URL}/book/extract-isbn`, {
        method: 'POST',
        body: formData,
      });

      if (!isbnRes.ok) {
        throw new Error(await isbnRes.text());
      }

      const extractedIsbn = await isbnRes.text(); // or .json() if backend wraps it
      setIsbn(extractedIsbn);

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
      {error && <p style={{ color: 'red' }}>Error: {error}</p>}
    </div>
  );
};

export default UploadBarcodeImage;
