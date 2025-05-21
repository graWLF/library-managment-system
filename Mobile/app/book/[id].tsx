import React, { useEffect, useState } from 'react';
import { useLocalSearchParams, useRouter } from 'expo-router';
import {
  View,
  Text,
  StyleSheet,
  ScrollView,
  ActivityIndicator,
  TouchableOpacity,
  Image,  // Import Image component
} from 'react-native';
import { fetchBookByISBN } from '@/api/services';

const BookDetailScreen = () => {
  const { id } = useLocalSearchParams();
  const router = useRouter();
  const [book, setBook] = useState<any>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const loadBook = async () => {
      try {
        const data = await fetchBookByISBN(id as string);
        setBook(data);
      } catch (err) {
        console.error('❌ Book Fetch Error:', err);
      } finally {
        setLoading(false);
      }
    };

    loadBook();
  }, [id]);

  if (loading) {
    return (
      <View style={styles.loadingContainer}>
        <ActivityIndicator size="large" color="#6a0dad" />
      </View>
    );
  }

  if (!book) {
    return (
      <View style={styles.loadingContainer}>
        <Text style={{ color: '#fff' }}>Book not found.</Text>
      </View>
    );
  }

  // Fallback for book image
  const imageUrl =
    book.image && book.image.startsWith('http')
      ? book.image
      : 'https://via.placeholder.com/100x150.png?text=No+Image';

  return (
    <ScrollView style={styles.scrollContainer}>
      <View style={styles.contentContainer}>
        <TouchableOpacity style={styles.back} onPress={() => router.back()}>
          <Text style={styles.backText}>← Back</Text>
        </TouchableOpacity>

        <View style={styles.card}>
          {/* Display Image */}
          <Image
            source={{ uri: imageUrl }}
            style={styles.bookImage}
            resizeMode="contain"  // Makes sure the entire image is visible
            onError={() => console.log('❌ Failed to load image:', imageUrl)}
          />

          <Text style={styles.title}>{book.title}</Text>

          <Text style={styles.label}>ISBN:</Text>
          <Text style={styles.value}>{book.id}</Text>

          <Text style={styles.label}>Category:</Text>
          <Text style={styles.value}>{book.category}</Text>

          <Text style={styles.label}>Type:</Text>
          <Text style={styles.value}>{book.type}</Text>

          <Text style={styles.label}>Pages:</Text>
          <Text style={styles.value}>{book.pages}</Text>

          <Text style={styles.label}>Format:</Text>
          <Text style={styles.value}>{book.format}</Text>

          <Text style={styles.label}>Publisher ID:</Text>
          <Text style={styles.value}>{book.publisherId}</Text>

          <Text style={styles.label}>Published:</Text>
          <Text style={styles.value}>{book.releaseDate}</Text>

          <Text style={styles.label}>Description:</Text>
          <Text style={styles.value}>{book.content}</Text>
        </View>
      </View>
    </ScrollView>
  );
};

const styles = StyleSheet.create({
  scrollContainer: {
    flex: 1,  // Ensures the ScrollView takes up the full screen height
    backgroundColor: '#181818',  // This makes sure the background is applied to the entire screen
  },
  contentContainer: {
    padding: 20,
    flexGrow: 1,  // Ensures content stretches when necessary
  },
  loadingContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#181818',
  },
  back: {
    backgroundColor: '#6a0dad',
    borderRadius: 6,
    paddingVertical: 8,
    paddingHorizontal: 16,
    alignSelf: 'flex-start',
    marginBottom: 16,
  },
  backText: {
    color: '#fff',
    fontSize: 16,
  },
  card: {
    backgroundColor: '#222',
    borderRadius: 12,
    padding: 20,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.3,
    shadowRadius: 4,
  },
  title: {
    fontSize: 22,
    fontWeight: '700',
    color: '#B266FF',
    marginBottom: 16,
  },
  label: {
    color: '#aaa',
    fontSize: 14,
    fontWeight: '600',
    marginTop: 12,
  },
  value: {
    color: '#fff',
    fontSize: 16,
    fontWeight: '400',
  },
  bookImage: {
    width: '100%',  // Make the image take up the full width of its container
    height: 250,  // Set a fixed height (you can adjust this value)
    borderRadius: 8,
    marginBottom: 16,
  },
});

export default BookDetailScreen;
