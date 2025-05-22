import React, { useState, useEffect } from 'react';
import { postBook } from '../api/Services'; 
import { addIsbnAuthorid } from '../api/Services';  // Assuming the function is correctly imported
import '../styles/AddBook.css';
import { API_BASE_URL } from '../api/config';
import { GOOGLE_API_KEY } from '../api/config'; 

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

  const [authors, setAuthors] = useState([]);
  const [selectedAuthors, setSelectedAuthors] = useState([]); // State for multiple authors
   
  useEffect(() => {
    const fetchAuthors = async () => {
      const response = await fetch(`${API_BASE_URL}/author`);
      if (!response.ok) {
        throw new Error("Failed to fetch authors");
      }
      const data = await response.json();
      setAuthors(data); // Store authors in state
    };
    fetchAuthors().catch(error => setMessage(error.message));
  }, []);

  const handleSubmit = async (e) => {
  e.preventDefault();
  
  // Check if ISBN and Title are provided, if not show an error
  if (!Id || !title) {
    setMessage('ISBN and Title are required.');
    return;
  }

  try {
    // Set default values for the other fields if they are empty
    const book = {
      Id,
      local_isbn: local_isbn || '0',  // Make sure ISBN is not empty
      type: type || '0',
      title: title,
      category: category || '0',
      additiondate: additiondate || '0',
      content: content || '0',
      infourl: infourl || '0',
      contentlanguage: contentlanguage || '0',
      contentsource: contentsource || '0',
      image: image || '0',
      price: price || '0',
      duration: duration || '0',
      contentlink: contentlink || '0',
      librarianid: librarianid || '0',
      format: format || '0',
      publishingstatus: publishingstatus || '0',
      releasedate: releasedate || '0',
      publisherid: publisherid || '0',
      pages: pages || '0',
      weight: weight || '0',
      dimensions: dimensions || '0',
      material: material || '0',
      color: color || '0',
    };

    // Post the book data
    const bookResponse = await postBook(book);
    setMessage('Book added successfully!');

    // Now add the ISBN and author IDs to the database
    const authorPromises = selectedAuthors.map(authorId => {
      return addIsbnAuthorid(Id, authorId); // Assuming this function is defined in Services.jsx
    });
    await Promise.all(authorPromises);

    // Reset form fields after success
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
    setSelectedAuthors([]); // Reset selected authors

  } catch (error) {
    // setMessage('Error: ' + error.message);
  }
};


  const handleGoogleAdd = async () => {
    try {
      const response = await fetch(`${API_BASE_URL}/Book/${isbnSearch}/${GOOGLE_API_KEY}`);
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
      
      setMessage("Book data loaded from Google!");
    } catch (error) {
      setMessage("Book not found or error fetching from Google.");
    }
  };

  return (
    <div>
      <h2>Add Book</h2>
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

          <div>
            <label>Authors:</label>
            <select
              multiple
              value={selectedAuthors}
              onChange={(e) => {
                const selectedOptions = Array.from(e.target.selectedOptions, option => option.value);
                setSelectedAuthors(selectedOptions);
              }}
            >
              {authors.map((author) => (
                <option key={author.id} value={author.id}>
                  {author.author}
                </option>
              ))}
            </select>
          </div>
        </div>
        <button type="submit">Add Book</button>
      </form>
      {message && <p>{message}</p>}
    </div>
  );
}

export default AddBook;
