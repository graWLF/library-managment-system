import React, { useState } from 'react';
import {
  View,
  Text,
  TextInput,
  TouchableOpacity,
  StyleSheet,
  Alert,
  ScrollView,
} from 'react-native';
import { addBook } from '@/api/services';

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
      await addBook(newBook);
      Alert.alert('✅ Book added successfully');

      // Reset fields
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
    } catch (err) {
      console.error(err);
      Alert.alert('❌ Error adding book');
    }
  };

  return (
    <ScrollView style={styles.container}>
      <Text style={styles.header}>Add Book</Text>

      {[
        { label: 'ID (ISBN)*', value: id, setter: setId },
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
        { label: 'Pages', value: pages, setter: setPages },
      ].map((field, index) => (
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
