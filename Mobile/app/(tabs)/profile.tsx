// app/(tabs)/profile.tsx
import React from 'react';
import { View, Text, StyleSheet, TouchableOpacity, Alert } from 'react-native';
import { useRouter } from 'expo-router';

const ProfileScreen = () => {
  const router = useRouter();

  const handleLogout = () => {
    Alert.alert(
      'Confirm Logout',
      'Are you sure you want to log out?',
      [
        { text: 'Cancel', style: 'cancel' },
        {
          text: 'Log Out',
          style: 'destructive',
          onPress: () => {
            // Oturum kapatma işlemleri gerekiyorsa burada yapılabilir
            router.replace('../../welcome');
          },
        },
      ],
      { cancelable: true }
    );
  };

  return (
    <View style={styles.container}>
      <Text style={styles.header}>Profile</Text>

      <View style={styles.card}>
        <Text style={styles.label}>Username:</Text>
        <Text style={styles.value}>demo_user</Text>

        <Text style={styles.label}>Email:</Text>
        <Text style={styles.value}>demo@example.com</Text>
      </View>

      <TouchableOpacity style={styles.button} onPress={handleLogout}>
        <Text style={styles.buttonText}>Log Out</Text>
      </TouchableOpacity>

      <View style={styles.infoSection}>
        <Text style={styles.infoTitle}>About Us</Text>
        <Text style={styles.infoText}>
          Lib++ is a simple digital library system built for modern book
          management.
        </Text>

        <Text style={styles.infoTitle}>Privacy Policy</Text>
        <Text style={styles.infoText}>
          We do not store any personal data. This app is for educational
          purposes only.
        </Text>
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#181818',
    paddingHorizontal: 24,
    paddingTop: 60,
  },
  header: {
    fontSize: 26,
    fontWeight: '700',
    color: '#B266FF',
    marginBottom: 20,
    textAlign: 'center',
  },
  card: {
    backgroundColor: '#2c2c2c',
    padding: 20,
    borderRadius: 10,
    marginBottom: 30,
  },
  label: {
    color: '#888',
    fontSize: 14,
    marginBottom: 4,
  },
  value: {
    color: '#fff',
    fontSize: 16,
    marginBottom: 12,
  },
  button: {
    backgroundColor: '#6a0dad',
    paddingVertical: 14,
    borderRadius: 10,
    alignItems: 'center',
    marginBottom: 30,
  },
  buttonText: {
    color: '#fff',
    fontSize: 16,
    fontWeight: '700',
  },
  infoSection: {
    marginTop: 10,
  },
  infoTitle: {
    color: '#B266FF',
    fontSize: 18,
    fontWeight: '600',
    marginBottom: 6,
  },
  infoText: {
    color: '#ccc',
    fontSize: 14,
    marginBottom: 20,
  },
});

export default ProfileScreen;
