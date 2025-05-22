import React, { useState, useEffect } from 'react';
import { View, Text, TextInput, TouchableOpacity, StyleSheet, Alert, ScrollView } from 'react-native';
import { Picker } from '@react-native-picker/picker';
import { fetchAuthors, addIsbnAuthorid, addBook, fetchAndAddBookByISBN } from '@/api/services'; // Import your existing services

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
  const [selectedMethod, setSelectedMethod] = useState<'manual' | 'isbn' | 'barcode' | null>(null);
  const [isbnInput, setIsbnInput] = useState('');
  
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

  const handleManualSubmit = async () => {
    if (!id || !title) {
      Alert.alert('Please fill in ID (ISBN), and Title');
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
  const handleIsbnSubmit = async (isbn?: string) => {
    const code = isbn || isbnInput.trim();
    if (!code) {
      Alert.alert('Please enter an ISBN');
      return;
    }

    try {
      const added = await fetchAndAddBookByISBN(code);
      console.log('✅ Book added:', added);
      Alert.alert('✅ Book added successfully');
      setIsbnInput('');
    } catch (error) {
      console.error('❌ ISBN Error:', error);
      Alert.alert('❌ Failed to fetch/add book');
    }
  };

  const renderForm = () => {
    switch (selectedMethod) {
      case 'manual':
        return (
          <ScrollView style={styles.container}>
            <Text style={styles.header}>Add Book</Text>

            {/* Render book fields */}
            {[{ label: 'ISBN*', value: id, setter: setId },
              { label: 'Title*', value: title, setter: setTitle },
              { label: 'Local ISBN', value: localIsbn, setter: setLocalIsbn },
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

            <TouchableOpacity style={styles.button} onPress={handleManualSubmit}>
              <Text style={styles.buttonText}>Submit</Text>
            </TouchableOpacity>
          </ScrollView>
        );
      case 'isbn':
        return (
          <View style={styles.formContainer}>
            <Text style={styles.label}>Enter ISBN</Text>
            <TextInput
              style={styles.input}
              placeholder="ISBN"
              value={isbnInput}
              onChangeText={setIsbnInput}
              keyboardType="numeric"
              placeholderTextColor="#aaa"
            />
            <TouchableOpacity style={styles.button} onPress={() => handleIsbnSubmit()}>
              <Text style={styles.buttonText}>Fetch & Add Book</Text>
            </TouchableOpacity>
          </View>
        );
      // case 'barcode':
      //   return (
      //     <View>
      //       <Text style={styles.label}>Scan Barcode</Text>
      //       {/* Implement barcode scanning logic here */}
      //     </View>
      //   );
      default:
        return <Text style={styles.info}>Choose a method to add a book</Text>;
    }
  };
  return (
    <View style={styles.container}>
      <Text style={styles.header}>Add a Book</Text>

      <View style={styles.methodSelector}>
        <TouchableOpacity style={styles.button} onPress={() => setSelectedMethod('manual')}>
          <Text style={styles.buttonText}>Manual Entry</Text>
        </TouchableOpacity>
        <TouchableOpacity style={styles.button} onPress={() => setSelectedMethod('isbn')}>
          <Text style={styles.buttonText}>Add by ISBN</Text>
        </TouchableOpacity>
      </View>

      {renderForm()}
    </View>
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
  methodSelector: {
    flexDirection: 'column',
    gap: 12,
    marginBottom: 20,
  },
  info: {
    color: '#ccc',
    fontSize: 16,
    textAlign: 'center',
    marginTop: 20,
  },
  formContainer: {
    marginTop: 10,
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
