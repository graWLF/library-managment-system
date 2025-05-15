import React, { useState } from 'react';
import {
  View,
  Text,
  TouchableOpacity,
  StyleSheet,
  ScrollView,
} from 'react-native';
import BackButton from '../../components/BackButton';
import AddBook from '../book/AddBook';
import DeleteBook from '../book/DeleteBook';

const BookActionsScreen = () => {
  const [selectedAction, setSelectedAction] = useState<'add' | 'delete' | null>(null);

  const renderForm = () => {
    switch (selectedAction) {
      case 'add':
        return <AddBook />;
      case 'delete':
        return <DeleteBook />;
      default:
        return <Text style={styles.info}>Choose an action below</Text>;
    }
  };

  return (
    <ScrollView contentContainerStyle={styles.container}>
      <BackButton />
      <Text style={styles.header}>Edit Book</Text>

      <View style={styles.methodSelector}>
        <TouchableOpacity style={styles.button} onPress={() => setSelectedAction('add')}>
          <Text style={styles.buttonText}>Add Book</Text>
        </TouchableOpacity>
        <TouchableOpacity style={styles.button} onPress={() => setSelectedAction('delete')}>
          <Text style={styles.buttonText}>Delete Book</Text>
        </TouchableOpacity>
      </View>

      <View style={styles.formContainer}>{renderForm()}</View>
    </ScrollView>
  );
};

const styles = StyleSheet.create({
  container: {
    flexGrow: 1,
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
  },
  buttonText: {
    color: '#fff',
    fontSize: 16,
    fontWeight: '700',
  },
  formContainer: {
    marginTop: 20,
  },
  info: {
    color: '#ccc',
    fontSize: 16,
    textAlign: 'center',
    marginTop: 20,
  },
});

export default BookActionsScreen;
