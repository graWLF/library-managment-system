import React, { useState, useEffect } from 'react';
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
  Image,
  ScrollView,
} from 'react-native';
import { useRouter } from 'expo-router';
import { searchBooks, searchBooksByIsbn, fetchBooks } from '@/api/services'; // Assuming there's an API to get all books
import BackButton from '../../components/BackButton';

const SearchScreen = () => {
  const [query, setQuery] = useState('');
  const [results, setResults] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const router = useRouter();

  // Function to fetch all books
  const fetchAllBooks = async () => {
    setLoading(true);
    try {
      const books = await fetchBooks(); // Assuming there's an API to fetch all books
      setResults(books);
    } catch (error) {
      console.error('❌ Fetch All Books Error:', error);
      setResults([]);
    } finally {
      setLoading(false);
    }
  };

  // Fetch all books when the component is mounted
  useEffect(() => {
    fetchAllBooks();
  }, []);

  const isIsbn = (text: string) => {
    const cleaned = text.replace(/[-\s]/g, '');
    return /^\d{10}(\d{3})?$/.test(cleaned);
  };

  const handleSearch = async () => {
    if (!query.trim()) {
      // If query is empty, fetch all books
      fetchAllBooks();
      return;
    }

    setLoading(true);
    try {
      const trimmedQuery = query.trim();
      const books = isIsbn(trimmedQuery)
        ? await searchBooksByIsbn(trimmedQuery)
        : await searchBooks(trimmedQuery);

      const resultsArray = Array.isArray(books) ? books : [books];
      setResults(resultsArray);
    } catch (error) {
      console.error('❌ Search Error:', error);
      setResults([]);
    } finally {
      setLoading(false);
    }
  };


  const renderItem = ({ item }: { item: any }) => {
    const imageUrl =
      item.image && item.image.startsWith('http')
        ? item.image
        : 'https://via.placeholder.com/100x150.png?text=No+Image';

    return (
      <TouchableOpacity
        style={styles.card}
        onPress={() => router.push(`../book/${item.id}`)}
      >
        <View style={styles.row}>
          <Image
            source={{ uri: imageUrl }}
            style={styles.bookImage}
            resizeMode="cover"
            onError={() => console.log('❌ Failed to load image:', imageUrl)}
          />
          <View style={{ flex: 1, marginLeft: 12 }}>
            <Text style={styles.bookTitle}>{item.title}</Text>
            <Text style={styles.bookInfo}>ISBN: {item.id}</Text>
            <Text style={styles.bookInfo}>Category: {item.category}</Text>
          </View>
        </View>
      </TouchableOpacity>
    );
  };

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
        <ScrollView
          style={styles.resultsContainer}
          contentContainerStyle={styles.listContent}
          keyboardShouldPersistTaps="handled"
        >
          <FlatList
            data={results}
            renderItem={renderItem}
            keyExtractor={(item) => item.id?.toString() || item.isbn}
          />
        </ScrollView>
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
  row: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  bookImage: {
    width: 60,
    height: 90,
    borderRadius: 4,
    backgroundColor: '#444',
  },
  resultsContainer: {
    flex: 1,
    marginTop: 20, // Ensure the results start below the input box
  },
});

export default SearchScreen;
