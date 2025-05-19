import React, { useState } from 'react';
import {
  View,
  Text,
  TextInput,
  TouchableOpacity,
  StyleSheet,
  Modal,
} from 'react-native';
import { fetchBookByISBN, deleteBook } from '@/api/services';

const DeleteBookScreen = () => {
  const [isbn, setIsbn] = useState('');
  const [bookToDelete, setBookToDelete] = useState<any>(null);
  const [showConfirm, setShowConfirm] = useState(false);

  const handleDelete = async () => {
    const cleanedIsbn = isbn.trim().replace(/[^0-9X]/gi, '');

    if (!cleanedIsbn) {
      alert('Please enter a valid ISBN.');
      return;
    }

    console.log('🔍 Trying to fetch book with ISBN:', cleanedIsbn);

    try {
      const book = await fetchBookByISBN(cleanedIsbn);
      console.log('📕 Book fetched:', book);

      if (!book || !book.id) {
        console.warn('⚠️ Book found but missing ID (ISBN-as-ID expected):', book);
        alert('Book not found or missing ID');
        return;
      }

      setBookToDelete(book);
      setShowConfirm(true);
    } catch (error: any) {
      console.error('❌ FETCH ERROR:', error.response?.data || error.message);
      alert('❌ Error fetching book');
    }
  };

  const confirmDelete = async () => {
    if (!bookToDelete?.id) return;

    console.log('🗑 Sending DELETE request for ID (used as ISBN):', bookToDelete.id);

    try {
      const result = await deleteBook(bookToDelete.id); // id kullanılıyor
      console.log('✅ DELETE SUCCESS:', result);
      setShowConfirm(false);
      setIsbn('');
      setBookToDelete(null);
      alert('✅ Book deleted successfully');
    } catch (error: any) {
      console.error('❌ DELETE ERROR:', error.response?.data || error.message);
      alert('❌ Failed to delete the book');
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

      {/* Modal */}
      <Modal visible={showConfirm} transparent animationType="fade">
        <View style={styles.modalOverlay}>
          <View style={styles.modalBox}>
            <Text style={styles.modalText}>
              Are you sure you want to delete "{bookToDelete?.title}"?
            </Text>
            <View style={styles.modalButtons}>
              <TouchableOpacity onPress={() => setShowConfirm(false)}>
                <Text style={styles.cancelButton}>Cancel</Text>
              </TouchableOpacity>
              <TouchableOpacity onPress={confirmDelete}>
                <Text style={styles.confirmButton}>Delete</Text>
              </TouchableOpacity>
            </View>
          </View>
        </View>
      </Modal>
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
  modalOverlay: {
    flex: 1,
    backgroundColor: 'rgba(0,0,0,0.5)',
    justifyContent: 'center',
    alignItems: 'center',
  },
  modalBox: {
    backgroundColor: '#fff',
    padding: 24,
    borderRadius: 14,
    width: '85%',
  },
  modalText: {
    fontSize: 16,
    fontWeight: '500',
    color: '#111',
    marginBottom: 20,
    textAlign: 'center',
  },
  modalButtons: {
    flexDirection: 'row',
    justifyContent: 'space-evenly',
  },
  cancelButton: {
    fontSize: 16,
    color: '#666',
    backgroundColor: '#eee',
    paddingVertical: 10,
    paddingHorizontal: 20,
    borderRadius: 8,
  },
  confirmButton: {
    fontSize: 16,
    color: '#fff',
    backgroundColor: '#B266FF',
    paddingVertical: 10,
    paddingHorizontal: 20,
    borderRadius: 8,
    fontWeight: '600',
  },
});
