import React, { useState } from 'react';
import { postBook } from '../api/Services'; 
import '../styles/AddBook.css';
//import axios from 'axios';

function AddBook() {
  const [message, setMessage] = useState('');
  const [Id, setId] = useState("");
  const [local_isbn, setLocalIsbn] = useState("");
  const [type, setType] = useState("");
  const [title, setTitle] = useState("");
  const [category, setCategory] = useState("");
  const [additiondate, setAdditiondate] = useState("");
  const [content, setContent] = useState("");
  const [infourl, setInfourl] = useState("");
  const [contentlanguage, setContentlanguage] = useState("");
  const [contentsource, setContentsource] = useState("");
  const [image, setImage] = useState("");
  const [price, setPrice] = useState("");
  const [duration, setDuration] = useState("");
  const [contentlink, setContentlink] = useState("");
  const [librarianid, setLibrarianid] = useState("");
  const [format, setFormat] = useState("");
  const [publishingstatus, setPublishingstatus] = useState("");
  const [releasedate, setReleasedate] = useState("");
  const [publisherid, setPublisherid] = useState("");
  const [pages, setPages] = useState("");
  const [weight, setWeight] = useState("");
  const [dimensions, setDimensions] = useState("");
  const [material, setMaterial] = useState("");
  const [color, setColor] = useState("");
  const [isbnSearch, setIsbnSearch] = useState("");

   const handleGoogleAdd = async () => {
    try {
      const apiKey = "YOURGOOGLEBOOKAPI"; // Replace with your actual API key if needed
      const response = await fetch(`http://localhost:5000/api/Book/${isbnSearch}/${apiKey}`);
      const book = response.data;

      // Fill the form fields with the fetched book data
      setId(book.Id);
      setLocalIsbn(book.local_isbn);
      setType(book.type);
      setTitle(book.title);
      setCategory(book.category);
      setAdditiondate(book.additiondate);
      setContent(book.content);
      setInfourl(book.infourl);
      setContentlanguage(book.contentlanguage);
      setContentsource(book.contentsource);
      setImage(book.image);
      setPrice(book.price);
      setDuration(book.duration);
      setContentlink(book.contentlink);
      setLibrarianid(book.librarianid);

      setFormat(book.format);
      setPublishingstatus(book.publishingstatus);
      setReleasedate(book.releasedate);
      setPublisherid(book.publisherid);
      setPages(book.pages);
      setWeight(book.weight);
      setDimensions(book.dimensions);
      setMaterial(book.material);
      setColor(book.color);
      // Set the message to indicate success
      setMessage("Book data loaded from Google!");
    } catch (error) {
      setMessage("Book not found or error fetching from Google.");
    }
  };


  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const book = {
        Id,
        local_isbn,
        type,
        title,
        category,
        additiondate,
        content,
        infourl,
        contentlanguage,
        contentsource,
        image,
        price,
        duration,
        contentlink,
        librarianid,
        format,
        publishingstatus,
        releasedate,
        publisherid,
        pages,
        weight,
        dimensions,
        material,
        color
      };
       // Create the book object
      await postBook(book); // Call the postBook method
      setId('');
      setLocalIsbn('');
      setType('');
      setTitle('');
      setCategory('');
      setAdditiondate('');
      setContent('');
      setInfourl('');
      setContentlanguage('');
      setContentsource('');
      setImage('');
      setPrice('');
      setDuration('');
      setContentlink('');
      setLibrarianid('');
      setFormat('');
      setPublishingstatus('');
      setReleasedate('');
      setPublisherid('');
      setPages('');
      setWeight('');
      setDimensions('');
      setMaterial('');
      setColor('');
     
    } catch (error) {
      setMessage('Error: ' + error.message);
    }
  };

  return (
    <div>
      <h2>Add Book</h2>
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
      <form onSubmit={handleSubmit}>
        <div className="form-grid">
          <div>
            <label>ID:</label>
            <input
              type="number"
              value={Id}
              onChange={(e) => setId(e.target.value)}
              required
            />
          </div>
          <div>
            <label>Local ISBN:</label>
            <input
              type="text"
              value={local_isbn}
              onChange={(e) => setLocalIsbn(e.target.value)}
            />
          </div>
          <div>
            <label>Type:</label>
            <input
              type="text"
              value={type}
              onChange={(e) => setType(e.target.value)}
            />
          </div>
          <div>
            <label>Title:</label>
            <input
              type="text"
              value={title}
              onChange={(e) => setTitle(e.target.value)}
            />
          </div>
          <div>
            <label>Category:</label>
            <input
              type="text"
              value={category}
              onChange={(e) => setCategory(e.target.value)}
            />
          </div>
          <div>
            <label>Addition Date:</label>
            <input
              type="text"
              value={additiondate}
              onChange={(e) => setAdditiondate(e.target.value)}
            />
          </div>
          <div>
            <label>Content:</label>
            <input
              type="text"
              value={content}
              onChange={(e) => setContent(e.target.value)}
            />
          </div>
          <div>
            <label>Info URL:</label>
            <input
              type="text"
              value={infourl}
              onChange={(e) => setInfourl(e.target.value)}
            />
          </div>
          <div>
            <label>Content Language:</label>
            <input
              type="text"
              value={contentlanguage}
              onChange={(e) => setContentlanguage(e.target.value)}
            />
          </div>
          <div>
            <label>Content Source:</label>
            <input
              type="text"
              value={contentsource}
              onChange={(e) => setContentsource(e.target.value)}
            />
          </div>
          <div>
            <label>Image:</label>
            <input
              type="text"
              value={image}
              onChange={(e) => setImage(e.target.value)}
            />
          </div>
          <div>
            <label>Price:</label>
            <input
              type="text"
              value={price}
              onChange={(e) => setPrice(e.target.value)}
            />
          </div>
          <div>
            <label>Duration:</label>
            <input
              type="text"
              value={duration}
              onChange={(e) => setDuration(e.target.value)}
            />
          </div>
          <div>
            <label>Content Link:</label>
            <input
              type="text"
              value={contentlink}
              onChange={(e) => setContentlink(e.target.value)}
            />
          </div>
          <div>
            <label>Librarian ID:</label>
            <input
              type="number"
              value={librarianid}
              onChange={(e) => setLibrarianid(e.target.value)}
            />
          </div>
          <div>
            <label>Format:</label>
            <input
              type="text"
              value={format}
              onChange={(e) => setFormat(e.target.value)}
            />
          </div>
          <div>
            <label>Publishing Status:</label>
            <input
              type="text"
              value={publishingstatus}
              onChange={(e) => setPublishingstatus(e.target.value)}
            />
          </div>
          <div>
            <label>Release Date:</label>
            <input
              type="text"
              value={releasedate}
              onChange={(e) => setReleasedate(e.target.value)}
            />
          </div>
          <div>
            <label>Publisher ID:</label>
            <input
              type="number"
              value={publisherid}
              onChange={(e) => setPublisherid(e.target.value)}
            />
          </div>
          <div>
            <label>Pages:</label>
            <input
              type="number"
              value={pages}
              onChange={(e) => setPages(e.target.value)}
            />
          </div>
          <div>
            <label>Weight:</label>
            <input
              type="text"
              value={weight}
              onChange={(e) => setWeight(e.target.value)}
            />
          </div>
          <div>
            <label>Dimensions:</label>
            <input
              type="text"
              value={dimensions}
              onChange={(e) => setDimensions(e.target.value)}
            />
          </div>
          <div>
            <label>Material:</label>
            <input
              type="text"
              value={material}
              onChange={(e) => setMaterial(e.target.value)}
            />
          </div>
          <div>
            <label>Color:</label>
            <input
              type="text"
              value={color}
              onChange={(e) => setColor(e.target.value)}
            />
          </div>
        </div>
        <button type="submit">Add Book</button>
      </form>
      {message && <p>{message}</p>}
    </div>
  );
}

export default AddBook;