import React, { useEffect, useState } from "react";
import { View, Text, FlatList } from "react-native";
import api from "src/services/api.js"; 
import { getBooks } from "../services/api"

const HomeScreen = () => {
  const [data, setData] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await api.get("/books"); 
        setData(response.data);
      } catch (error) {
        console.error("API Error:", error);
      }
    };

    fetchData();
  }, []);

const fetchData = async() => {
    try{
        const books = await getBooks();
        setData(books);
    }catch(error){
        console.error("API Error:",error);
    }
} ;

  return (
    <View>
      <Text>Book List:</Text>
      <FlatList
        data={data}
        keyExtractor={(item) => item.id.toString()}
        renderItem={({ item }) => <Text>{item.title}</Text>}
      />
    </View>
  );
};



export default HomeScreen;
