import React, { useState } from 'react';
import {
  View,
  Text,
  TextInput,
  TouchableOpacity,
  Alert,
  StyleSheet,
} from 'react-native';
import { fetchBookByISBN, deleteBook } from '@/api/services';

const DeleteBookScreen = () => {
  const [isbn, setIsbn] = useState('');

const handleDelete = async () => {
  if (!isbn.trim()) {
    Alert.alert('Please enter an ISBN.');
    return;
  }

  console.log('⬇️ Trying to fetch book with ISBN:', isbn);

  try {
    const book = await fetchBookByISBN(isbn.trim());

    console.log('📕 Book fetched:', book);

    if (!book) {
      Alert.alert('Book not found');
      return;
    }

    Alert.alert(
      'Confirm Deletion',
      `Are you sure you want to delete "${book.title}"?`,
      [
        { text: 'Cancel', style: 'cancel' },
        {
          text: 'Yes',
          style: 'destructive',
          onPress: async () => {
            console.log('🗑 Attempting to delete book with ID:', book.id);

            try {
              await deleteBook(Number(book.id));
              Alert.alert('Book deleted successfully');
              setIsbn('');
            } catch (err) {
              console.error('❌ Delete error:', err);
              Alert.alert('Failed to delete the book');
            }
          },
        },
      ]
    );
  } catch (error) {
    console.error('❌ Fetch error:', error);
    Alert.alert('Error fetching book');
  }
};


  return (
    <View style={styles.container}>
      <Text style={styles.title}>Delete a Book</Text>
      <TextInput
        style={styles.input}
        placeholder="Enter ISBN"
        placeholderTextColor="#999"
        value={isbn}
        onChangeText={setIsbn}
        keyboardType="numeric"
      />
      <TouchableOpacity style={styles.button} onPress={handleDelete}>
        <Text style={styles.buttonText}>Delete</Text>
      </TouchableOpacity>
    </View>
  );
};

export default DeleteBookScreen;

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#181818',
    paddingHorizontal: 24,
    paddingTop: 60,
  },
  title: {
    fontSize: 24,
    fontWeight: '700',
    color: '#B266FF',
    textAlign: 'center',
    marginBottom: 30,
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
    marginBottom: 20,
  },
  button: {
    backgroundColor: '#6a0dad',
    borderRadius: 10,
    paddingVertical: 14,
    alignItems: 'center',
  },
  buttonText: {
    color: '#fff',
    fontWeight: '700',
    fontSize: 16,
  },
});
