
import axios from 'axios';
import API_BASE_URL from './apiConfig';
import GOOGLE_BOOKS_API_KEY from './googleConfig';

export const registerUser = async (userData: any) => {
  try {
    console.log("Register İsteği Atılıyor (Axios):", userData);

    const response = await axios.post(`${API_BASE_URL}/Registration`, userData, {
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
      }
    });

    console.log("Register Response (Axios):", response.data);
    return response.data;
  } catch (error: any) {
    if (error.response) {
      console.error("Registration Error (Backend):", error.response.data);
      throw new Error(error.response.data.message || "Registration failed.");
    } else {
      console.error("Registration Error (Network):", error.message);
      throw new Error("Network error. Please try again.");
    }
  }
};




export const loginUser = async (userData: any) => {
  try {
    console.log("➡️ Login İsteği Atılıyor (Axios):", userData);

    const response = await axios.post(`${API_BASE_URL}/Registration/login`, userData, {
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
      }
    });

    console.log("✅ Login Yanıtı Geldi:", response); 

    if (response.data) {
      console.log("✅ Login Response Data:", JSON.stringify(response.data, null, 2)); 
    } else {
      console.log("❌ Yanıt geldi ama data boş!");
    }

    console.log("🧐 Gelen Response: ", JSON.stringify(response.data, null, 2));
    return response.data;
  } catch (error: any) {
    if (error.response) {
      console.error("❌ Login Error (Backend):", error.response.data);
      throw new Error(error.response.data.message || "Invalid credentials.");
    } else {
      console.error("❌ Login Error (Network):", error.message);
      throw new Error("Network error. Please try again.");
    }
  }
};

export const getUserProfile = async () => {
  try {
    const response = await axios.get(`${API_BASE_URL}/Registration`);
    return response.data;
  } catch (error: any) {
    console.error("❌ Kullanıcı Bilgileri Alınamadı:", error.message);
    throw new Error("Profile bilgileri alınamadı.");
  }
};


export const fetchBooks = async () => {
  const response = await axios.get(`${API_BASE_URL}/Book`);
  return response.data;
};


export const fetchBookByISBN = async (isbn: string) => {
  const response = await axios.get(`${API_BASE_URL}/Book/${isbn}`);
  return response.data;
};


export const searchBooks = async (query: string) => {
  console.log("🔎 Sending search request for:", query);
  try {
    const response = await axios.get(`${API_BASE_URL}/Book/search`, {
      params: { name: query }, // ✅ backend'in istediği parametre ismi
    });
    console.log("📚 Search Response Received:", response.data);
    return response.data;
  } catch (error: any) {
    console.error("❌ Search Error:", error.message);
    return [];
  }
};
export const searchBooksByIsbn = async (isbn: string) => {
  console.log("🔎 Sending search request for:", isbn);
  try {
    const response = await axios.get(`${API_BASE_URL}/Book/${isbn}`)
    console.log("📚 Search Response Received:", response.data);
    return response.data;
  } catch (error: any) {
    console.error("❌ Search Error:", error.message);
    return [];
  }
};

export const fetchAndAddBookByISBN = async (isbn: string) => {
  const response = await fetch(`${API_BASE_URL}/Book/${isbn}/${GOOGLE_BOOKS_API_KEY}`);

  if (!response.ok) {
    throw new Error("❌ Failed to fetch book from Google API");
  }

  const data = await response.json();

  if (!data.title) {
    throw new Error("❌ Book data incomplete or not found");
  }

  const newBook = {
    id: Number(isbn), // veya data.id varsa o
    title: data.title,
    category: data.category || 'Unknown',
    type: data.type || 'Book',
    pages: data.pages || 0,
    format: 'Google',
    releaseDate: data.releaseDate || '',
    publisherId: 1,
    content: data.content || 'No description available',
  };

  // POST işlemi: veritabanına kitap ekleme
  await addBook(newBook);

  return newBook;
};


export const addBook = async (bookData: any) => {
  const response = await axios.post(`${API_BASE_URL}/Book`, bookData);
  return response.data;
};

// Kitap güncelleme (PUT - /api/Book/{isbn})
export const updateBook = async (isbn: string, bookData: any) => {
  const response = await axios.put(`${API_BASE_URL}/Book/${isbn}`, bookData);
  return response.data;
};

export const findAuthor = async (isbn: string | number) => {
    const response = await fetch(`${API_BASE_URL}/Isbnauthorid/${isbn}`);
    if (!response.ok) {
        throw new Error("Failed to fetch book");
    }
    return response.json();
};

export const getAuthorById = async (authorId: string | number) => {
  const response = await fetch(`${API_BASE_URL}/Author/${authorId}`);
  if (!response.ok) {
    throw new Error(`Failed to fetch author ${authorId}`);
  }
  return response.json();
};

export const fetchLibrarianById = async (id: string | number) => {
    const response = await fetch(`${API_BASE_URL}/librarian/${id}`);
    if (!response.ok) {
        throw new Error("Failed to fetch librarian");
    }
    return response.json();
}

export const getPublisherById = async (publisherId: string | number) => {
  try {
    const response = await fetch(`${API_BASE_URL}/publisher/${publisherId}`);
    const data = await response.json();
    return data;  // Should return an object with { publisherName: 'Some Publisher' }
  } catch (error) {
    console.error('Error fetching publisher:', error);
    throw error;
  }
};
export const getLoginInfo = async (username: string) => {
  try {
    const response = await fetch(`${API_BASE_URL}/Registration/userInfo/${username}`);
    const data = await response.json();
    return data;  // Should return an object with { username: 'Some User' }
  } catch (error) {
    console.error('Error fetching login info:', error);
    throw error;
  }
};

export const fetchAuthors = async () => {
    const response = await fetch(`${API_BASE_URL}/author`);
    if (!response.ok) {
        throw new Error("Failed to fetch authors");
    }
    return response.json();
}

export const addIsbnAuthorid = async (id : string | number , authorid : string | number) => {
    const response = await fetch(`${API_BASE_URL}/Isbnauthorid`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ id, authorid }),
    });
    if (!response.ok) {
        throw new Error("Failed to add ISBN and Author ID");
    }
    return response.json();
}

export const deleteIsbnAuthorid = async (id : string | number , authorid : string | number) => {
    const response = await fetch(`${API_BASE_URL}/Isbnauthorid/${id}/${authorid}`, {
        method: "DELETE",
    });
    if (!response.ok) {
        throw new Error("Failed to delete ISBN-AuthorID relation");
    }
    return;
};
export const deleteBookCopyByIsbn = async (isbn: string | number) => {
    const response = await fetch(`${API_BASE_URL}/BookCopy/isbn/${isbn}`, {
        method: "DELETE",
    });
    if (!response.ok) {
        throw new Error("Failed to delete book copies by ISBN");
    }
    return;
};

export const deleteBook = async (id: string | number) => {
    const response = await fetch(`${API_BASE_URL}/book/${id}`, {
        method: "DELETE",
    });
    if (!response.ok) {
        throw new Error("Failed to delete book");
    }
    return;
};
