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
import { useRouter } from 'expo-router';
import { searchBooks, searchBooksByIsbn } from '@/api/services';
import BackButton from '../../components/BackButton';

const SearchScreen = () => {
  const [query, setQuery] = useState('');
  const [results, setResults] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const router = useRouter();

  const isIsbn = (text: string) => {
    // Remove dashes and spaces, and check if it's all digits and 10 or 13 characters
    const cleaned = text.replace(/[-\s]/g, '');
    return /^\d{10}(\d{3})?$/.test(cleaned);
  };

  const handleSearch = async () => {
    if (!query.trim()) return;

    setLoading(true);
    try {
      const trimmedQuery = query.trim();
      const books = isIsbn(trimmedQuery)
        ? await searchBooksByIsbn(trimmedQuery)
        : await searchBooks(trimmedQuery);

      // Normalize result to array for FlatList
      const resultsArray = Array.isArray(books) ? books : [books];
      setResults(resultsArray);
    } catch (error) {
      console.error('❌ Search Error:', error);
      setResults([]);
    } finally {
      setLoading(false);
    }
  };

  const renderItem = ({ item }: { item: any }) => (
    <TouchableOpacity
      style={styles.card}
      onPress={() => router.push(`../book/${item.id}`)}
    >
      <Text style={styles.bookTitle}>{item.title}</Text>
      <Text style={styles.bookInfo}>ISBN: {item.id}</Text>
      <Text style={styles.bookInfo}>Category: {item.category}</Text>
    </TouchableOpacity>
  );

  return (
    <KeyboardAvoidingView
      style={styles.container}
      behavior={Platform.select({ ios: 'padding', android: undefined })}
    >
      <BackButton />

      <View style={styles.centerContent}>
        <Text style={styles.title}>Search for Books</Text>

        <TextInput
          style={styles.input}
          placeholder="Enter book title or ISBN"
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
          contentContainerStyle={styles.listContent}
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
    paddingTop: 40,
  },
  centerContent: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    gap: 18,
  },
  title: {
    fontSize: 26,
    fontWeight: '700',
    color: '#B266FF',
    textAlign: 'center',
    marginBottom: 10,
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
    width: '100%',
  },
  button: {
    backgroundColor: '#6a0dad',
    borderRadius: 10,
    paddingVertical: 14,
    alignItems: 'center',
    width: '100%',
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
  listContent: {
    paddingBottom: 80,
  },
});

export default SearchScreen;
