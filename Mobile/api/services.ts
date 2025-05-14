
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

export const fetchAndAddBookByISBN = async (isbn: string) => {
  const response = await fetch(`${API_BASE_URL}/Book/${isbn}/${GOOGLE_BOOKS_API_KEY}`);
  if (!response.ok) {
    throw new Error("Failed to fetch and add book from Google API");
  }
  return response.json(); 
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


export const deleteBook = async (isbn: number) => {
  const response = await axios.delete(`${API_BASE_URL}/Book/${isbn}`);
  return response.data;
};
