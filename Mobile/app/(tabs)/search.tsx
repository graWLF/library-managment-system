import React, { useState } from 'react';
import {
  View,
  Text,
  TextInput,
  TouchableOpacity,
  StyleSheet,
  FlatList,
  ActivityIndicator,
  KeyboardAvoidingView,
  Platform,
} from 'react-native';
import { searchBooks } from '@/api/services';
import BackButton from '../../components/BackButton';

const SearchScreen = () => {
  const [query, setQuery] = useState('');
  const [results, setResults] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const handleSearch = async () => {
    if (!query.trim()) return;

    setLoading(true);
    try {
      const books = await searchBooks(query.trim());
      setResults(books);
    } catch (error) {
      console.error('❌ Search Error:', error);
      setResults([]);
    } finally {
      setLoading(false);
    }
  };

  const renderItem = ({ item }: { item: any }) => (
    <View style={styles.card}>
      <Text style={styles.bookTitle}>{item.title}</Text>
      <Text style={styles.bookInfo}>ISBN: {item.isbn}</Text>
      <Text style={styles.bookInfo}>Category: {item.category}</Text>
    </View>
  );

  return (
    <KeyboardAvoidingView
      style={styles.container}
      behavior={Platform.select({ ios: 'padding', android: undefined })}
    >
      <View style={styles.backButton}>
        <BackButton />
      </View>

      <View style={styles.centerContent}>
        <Text style={styles.title}>Search for Books</Text>

        <TextInput
          style={styles.input}
          placeholder="Enter a book title"
          placeholderTextColor="#aaa"
          value={query}
          onChangeText={setQuery}
          onSubmitEditing={handleSearch}
        />

        <TouchableOpacity style={styles.button} onPress={handleSearch}>
          <Text style={styles.buttonText}>Search</Text>
        </TouchableOpacity>
      </View>

      {loading ? (
        <ActivityIndicator size="large" color="#6a0dad" style={{ marginTop: 30 }} />
      ) : (
        <FlatList
          data={results}
          renderItem={renderItem}
          keyExtractor={(item) => item.id?.toString() || item.isbn}
          contentContainerStyle={{ paddingTop: 20, paddingBottom: 80 }}
        />
      )}
    </KeyboardAvoidingView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#181818',
    paddingHorizontal: 24,
  },
  backButton: {
    position: 'absolute',
    top: 50,
    left: 20,
    zIndex: 10,
  },
  centerContent: {
    flex: 1,
    justifyContent: 'center',
    gap: 18,
  },
  title: {
    fontSize: 26,
    fontWeight: '700',
    color: '#B266FF',
    textAlign: 'center',
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
  },
  button: {
    backgroundColor: '#6a0dad',
    borderRadius: 10,
    paddingVertical: 14,
    alignItems: 'center',
  },
  buttonText: {
    color: '#fff',
    fontSize: 16,
    fontWeight: '700',
  },
  card: {
    backgroundColor: '#2c2c2c',
    borderRadius: 10,
    padding: 15,
    marginBottom: 12,
  },
  bookTitle: {
    fontSize: 18,
    fontWeight: '700',
    color: '#fff',
    marginBottom: 6,
  },
  bookInfo: {
    fontSize: 14,
    color: '#ccc',
  },
});

export default SearchScreen;
