import React, { useState, useEffect } from 'react';
import { View, Text, TextInput, TouchableOpacity, StyleSheet, Alert, ScrollView } from 'react-native';
import { Picker } from '@react-native-picker/picker';
import { fetchAuthors, addIsbnAuthorid, addBook } from '@/api/services'; // Import your existing services

// Define the type for an Author
type Author = {
  id: number;
  author: string;
};

const AddBookScreen = () => {
  const [id, setId] = useState('');
  const [localIsbn, setLocalIsbn] = useState('');
  const [title, setTitle] = useState('');
  const [type, setType] = useState('');
  const [category, setCategory] = useState('');
  const [content, setContent] = useState('');
  const [additionDate, setAdditionDate] = useState('');
  const [infoUrl, setInfoUrl] = useState('');
  const [image, setImage] = useState('');
  const [format, setFormat] = useState('');
  const [releaseDate, setReleaseDate] = useState('');
  const [publisherId, setPublisherId] = useState('');
  const [pages, setPages] = useState('');
  
  // Type the state as an array of Author objects
  const [authors, setAuthors] = useState<Author[]>([]);
  
  const [selectedAuthors, setSelectedAuthors] = useState<number[]>([]); // Typing the state as an array of numbers

  useEffect(() => {
    const loadAuthors = async () => {
      try {
        const authorList = await fetchAuthors(); // Fetch authors from your API
        setAuthors(authorList); // Ensure the fetched data is of type Author[]
      } catch (error) {
        console.error("Error fetching authors:", error);
        Alert.alert("Error fetching authors");
      }
    };
    loadAuthors();
  }, []);

  const handleSubmit = async () => {
    if (!id || !localIsbn || !title) {
      Alert.alert('Please fill in ID (ISBN), Local ISBN, and Title');
      return;
    }

    const newBook = {
      id: parseInt(id),
      local_isbn: localIsbn,
      title,
      type,
      category,
      content,
      additionDate,
      infoUrl,
      image,
      format,
      releaseDate,
      publisherId: publisherId ? parseInt(publisherId) : undefined,
      pages: pages ? parseInt(pages) : 0,
    };

    try {
      // Add the book to the database (replace with actual API call to add a book)
      await addBook(newBook);

      // Add the relationship between the ISBN and selected authors
      for (let authorId of selectedAuthors) {
        await addIsbnAuthorid(id, authorId); // Add ISBN-author relationship
      }

      Alert.alert('✅ Book added successfully');

      // Reset fields after successful submission
      setId('');
      setLocalIsbn('');
      setTitle('');
      setType('');
      setCategory('');
      setContent('');
      setAdditionDate('');
      setInfoUrl('');
      setImage('');
      setFormat('');
      setReleaseDate('');
      setPublisherId('');
      setPages('');
      setSelectedAuthors([]);
    } catch (err) {
      console.error(err);
      Alert.alert('❌ Error adding book');
    }
  };

  return (
    <ScrollView style={styles.container}>
      <Text style={styles.header}>Add Book</Text>

      {/* Render book fields */}
      {[{ label: 'ID (ISBN)*', value: id, setter: setId },
        { label: 'Local ISBN*', value: localIsbn, setter: setLocalIsbn },
        { label: 'Title*', value: title, setter: setTitle },
        { label: 'Type', value: type, setter: setType },
        { label: 'Category', value: category, setter: setCategory },
        { label: 'Content / Description', value: content, setter: setContent },
        { label: 'Addition Date', value: additionDate, setter: setAdditionDate },
        { label: 'Info URL', value: infoUrl, setter: setInfoUrl },
        { label: 'Image URL', value: image, setter: setImage },
        { label: 'Format', value: format, setter: setFormat },
        { label: 'Release Date', value: releaseDate, setter: setReleaseDate },
        { label: 'Publisher ID', value: publisherId, setter: setPublisherId },
        { label: 'Pages', value: pages, setter: setPages }].map((field, index) => (
        <View key={index}>
          <Text style={styles.label}>{field.label}</Text>
          <TextInput
            style={styles.input}
            value={field.value}
            onChangeText={field.setter}
            placeholder={`Enter ${field.label.replace('*', '')}`}
            placeholderTextColor="#aaa"
          />
        </View>
      ))}

      {/* Authors dropdown */}
      <View>
        <Text style={styles.label}>Select Authors</Text>
        <Picker
          selectedValue={selectedAuthors}
          onValueChange={(itemValue: number[]) => setSelectedAuthors(itemValue)} // Explicitly typing itemValue
          style={styles.input}
          // multiple={true} // Allow multiple selections
        >
          {authors.map((author) => (
            <Picker.Item key={author.id} label={author.author} value={author.id} />
          ))}
        </Picker>
      </View>

      <TouchableOpacity style={styles.button} onPress={handleSubmit}>
        <Text style={styles.buttonText}>Submit</Text>
      </TouchableOpacity>
    </ScrollView>
  );
};

const styles = StyleSheet.create({
  container: {
    backgroundColor: '#181818',
    padding: 20,
    flex: 1,
  },
  header: {
    fontSize: 24,
    fontWeight: '700',
    color: '#B266FF',
    marginBottom: 20,
    textAlign: 'center',
  },
  label: {
    color: '#ccc',
    fontSize: 14,
    marginBottom: 6,
  },
  input: {
    backgroundColor: '#1f1f1f',
    borderColor: '#6a0dad',
    borderWidth: 1.2,
    borderRadius: 10,
    paddingHorizontal: 15,
    paddingVertical: 10,
    fontSize: 16,
    color: '#fff',
    marginBottom: 12,
  },
  button: {
    backgroundColor: '#6a0dad',
    borderRadius: 10,
    paddingVertical: 14,
    alignItems: 'center',
    marginTop: 10,
    marginBottom: 30,
  },
  buttonText: {
    color: '#fff',
    fontSize: 16,
    fontWeight: '700',
  },
});

export default AddBookScreen;
