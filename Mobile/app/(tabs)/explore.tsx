import React, { useState } from 'react';
import {
  View,
  TextInput,
  TouchableOpacity,
  Text,
  FlatList,
  StyleSheet,
  Image,
  Alert,
} from 'react-native';
import { searchBooks } from '@/api/services';

const Explore = () => {
  const [query, setQuery] = useState('');
  const [results, setResults] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const handleSearch = async () => {
    if (!query.trim()) {
      Alert.alert('Error', 'Please enter a search term.');
      return;
    }
  
    console.log("🔎 Sending Search Request:", query);
    setLoading(true);
    try {
      const response = await searchBooks(query);
      console.log("📚 Search Result Received:", response);
  
      if (response.length > 0) {
        setResults(response);
      } else {
        Alert.alert('No Results Found', 'No books match your search criteria.');
        setResults([]);
      }
    } catch (error: any) {
      console.error('❌ Search Error:', error.message);
      Alert.alert('Error', 'An error occurred, please try again.');
    } finally {
      setLoading(false);
    }
  };
  const renderItem = ({ item }: { item: any }) => (
    <View style={styles.card}>
      <Image source={{ uri: item.image }} style={styles.image} />
      <View style={styles.cardContent}>
        <Text style={styles.title}>{item.title}</Text>
        <Text style={styles.author}>{item.category}</Text>
        <Text style={styles.description}>{item.content}</Text>
        <Text style={styles.price}>{item.price}</Text>
      </View>
    </View>
  );

  return (
    <View style={styles.container}>
      <Text style={styles.header}>Search for Books</Text>
      <TextInput
        style={styles.input}
        placeholder="Enter book title"
        placeholderTextColor="#888"
        value={query}
        onChangeText={setQuery}
        onSubmitEditing={handleSearch}
      />
      <TouchableOpacity style={styles.button} onPress={handleSearch}>
        <Text style={styles.buttonText}>Search</Text>
      </TouchableOpacity>

      <FlatList
        data={results}
        renderItem={renderItem}
        keyExtractor={(item) => item.id.toString()}
        style={styles.list}
      />
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#181818',
    padding: 20,
  },
  header: {
    fontSize: 24,
    fontWeight: '700',
    color: '#6a0dad',
    marginBottom: 15,
    textAlign: 'center',
  },
  input: {
    backgroundColor: '#333',
    borderColor: '#555',
    borderWidth: 1.2,
    borderRadius: 10,
    padding: 12,
    fontSize: 16,
    color: '#ffffff',
    marginBottom: 10,
  },
  button: {
    backgroundColor: '#6a0dad',
    padding: 12,
    borderRadius: 10,
    alignItems: 'center',
    marginBottom: 16,
  },
  buttonText: {
    color: '#fff',
    fontSize: 16,
    fontWeight: '600',
  },
  list: {
    marginTop: 10,
  },
  card: {
    backgroundColor: '#282828',
    borderRadius: 8,
    marginBottom: 15,
    overflow: 'hidden',
  },
  image: {
    height: 200,
    width: '100%',
  },
  cardContent: {
    padding: 10,
  },
  title: {
    fontSize: 18,
    fontWeight: '700',
    color: '#fff',
  },
  author: {
    fontSize: 14,
    color: '#999',
    marginBottom: 5,
  },
  description: {
    fontSize: 14,
    color: '#ccc',
  },
  price: {
    marginTop: 5,
    fontSize: 16,
    fontWeight: '600',
    color: '#6a0dad',
  },
});

export default Explore;
