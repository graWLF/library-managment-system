import React, { useState } from 'react';
import {
  View,
  Text,
  TextInput,
  TouchableOpacity,
  StyleSheet,
  Alert,
  ScrollView,
  Platform,
} from 'react-native';
import { addBook, fetchAndAddBookByISBN } from '@/api/services';
import WebBarcodeScanner from '@/components/WebBarcodeScanner'; 


const AddBookScreen = () => {
  const [selectedMethod, setSelectedMethod] = useState<'manual' | 'isbn' | 'barcode' | null>(null);
  const [title, setTitle] = useState('');
  const [category, setCategory] = useState('');
  const [type, setType] = useState('');
  const [pages, setPages] = useState('');
  const [content, setContent] = useState('');
  const [isbnInput, setIsbnInput] = useState('');

  const handleManualSubmit = async () => {
    if (!title || !category || !type || !pages || !content) {
      Alert.alert('Please fill all fields');
      return;
    }

    const newBook = {
      id: Math.floor(Math.random() * 10000000000),
      title,
      category,
      type,
      pages: parseInt(pages),
      format: 'Paperback',
      releaseDate: new Date().toISOString().split('T')[0],
      publisherId: 1,
      content,
    };

    try {
      await addBook(newBook);
      Alert.alert('✅ Book added successfully');
      setTitle('');
      setCategory('');
      setType('');
      setPages('');
      setContent('');
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
          <ScrollView style={styles.formContainer}>
            <Text style={styles.label}>Title</Text>
            <TextInput style={styles.input} value={title} onChangeText={setTitle} placeholder="Enter title" placeholderTextColor="#aaa" />

            <Text style={styles.label}>Category</Text>
            <TextInput style={styles.input} value={category} onChangeText={setCategory} placeholder="Enter category" placeholderTextColor="#aaa" />

            <Text style={styles.label}>Type</Text>
            <TextInput style={styles.input} value={type} onChangeText={setType} placeholder="Enter type" placeholderTextColor="#aaa" />

            <Text style={styles.label}>Pages</Text>
            <TextInput style={styles.input} value={pages} onChangeText={setPages} placeholder="Enter number of pages" placeholderTextColor="#aaa" keyboardType="numeric" />

            <Text style={styles.label}>Description</Text>
            <TextInput style={[styles.input, { height: 100, textAlignVertical: 'top' }]} multiline value={content} onChangeText={setContent} placeholder="Enter description" placeholderTextColor="#aaa" />

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

      case 'barcode':
        return (
          <View style={styles.formContainer}>
            <WebBarcodeScanner onDetected={(code) => handleIsbnSubmit(code)} />
          </View>
        );

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
        <TouchableOpacity style={styles.button} onPress={() => setSelectedMethod('barcode')}>
          <Text style={styles.buttonText}>Scan Barcode</Text>
        </TouchableOpacity>
      </View>

      {renderForm()}
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#181818',
    paddingHorizontal: 24,
    paddingTop: 60,
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
  button: {
    backgroundColor: '#6a0dad',
    borderRadius: 10,
    paddingVertical: 14,
    alignItems: 'center',
    marginBottom: 10,
  },
  buttonText: {
    color: '#fff',
    fontSize: 16,
    fontWeight: '700',
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
    paddingVertical: 12,
    fontSize: 16,
    color: '#fff',
    marginBottom: 12,
  },
  info: {
    color: '#ccc',
    fontSize: 16,
    textAlign: 'center',
    marginTop: 20,
  },
});

export default AddBookScreen;
