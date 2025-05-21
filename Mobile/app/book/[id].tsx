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
import { fetchBookByISBN, findAuthor, getAuthorById, fetchLibrarianById, getPublisherById } from '@/api/services';

const BookDetailScreen = () => {
  const { id } = useLocalSearchParams();
  const router = useRouter();
  const [book, setBook] = useState<any>(null);
  const [loading, setLoading] = useState(true);
  const [authors, setAuthors] = useState<any[]>([]);
  const [librarianName, setLibrarianName] = useState('');
  const [publisherName, setPublisherName] = useState('');

  useEffect(() => {
    const loadBook = async () => {
      try {
        const data = await fetchBookByISBN(id as string);
        setBook(data);

        // Fetch authors and librarian data
        const authorIds = await findAuthor(data.id); // Fetch author ids for the book
        const authorPromises = authorIds.map((a: any) => getAuthorById(a.authorId));
        const authorDetails = await Promise.all(authorPromises);
        setAuthors(authorDetails);

        if (data.librarianId) {
          const librarian = await fetchLibrarianById(data.librarianId);
          setLibrarianName(librarian.librarianName);
        } else {
          setLibrarianName('Unknown');
        }

        // Fetch publisher data
        if (data.publisherId) {
          const publisher = await getPublisherById(data.publisherId);
          setPublisherName(publisher.publisher);
        } else {
          setPublisherName('Unknown Publisher');
        }

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

          <Text style={styles.label}>Published:</Text>
          <Text style={styles.value}>{book.releaseDate}</Text>

          <Text style={styles.label}>Description:</Text>
          <Text style={styles.value}>{book.content}</Text>

          {/* Author, Librarian, and Publisher Info */}
          <View style={styles.infoSection}>
            <Text style={styles.label}>Author(s):</Text>
            <Text style={styles.value}>
              {authors.length > 0
                ? authors.map((a: any) => a.author).join(', ')
                : 'Loading authors...'}
            </Text>

              <Text style={styles.label}>Librarian:</Text>
              <Text style={styles.value}>
                {librarianName || 'Loading librarian...'} (ID: {book.librarianId})
              </Text>

            <Text style={styles.label}>Publisher:</Text>
            <Text style={styles.value}>
              {publisherName || 'Loading publisher...'}  (ID: {book.publisherId})
              </Text>

          </View>
        </View>
      </View>
    </ScrollView>
  );
};

const styles = StyleSheet.create({
  scrollContainer: {
    flex: 1,
    backgroundColor: '#181818',
  },
  contentContainer: {
    padding: 20,
    flexGrow: 1,
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
    width: '100%',
    height: 250,
    borderRadius: 8,
    marginBottom: 16,
  },
  infoSection: {
    marginTop: 20,
  },
});

export default BookDetailScreen;
