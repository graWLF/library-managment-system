import axios from "axios";

const BASE_URL = "http://localhost:9001"; 

export const getUsers = async () => {
  try {
    const response = await axios.get(`${BASE_URL}/users`);
    return response.data;
  } catch (error) {
    console.error("Data getting error:", error);
    return [];
  }
};

export const getBooks = async () => {
  try {
    const response = await axios.get(`${BASE_URL}/books`);
    return response.data;
  } catch (error) {
    console.error("Kitapları çekme hatası:", error);
    return [];
  }
};
