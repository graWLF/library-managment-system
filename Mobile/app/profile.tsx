import React from 'react';
import { View, Text, TouchableOpacity, StyleSheet } from 'react-native';
import { useRouter } from 'expo-router';
import AsyncStorage from '@react-native-async-storage/async-storage';

const ProfileScreen = () => {
  const router = useRouter();

  const handleLogout = async () => {
    await AsyncStorage.removeItem('authToken');
    router.replace('/login');
  };

  return (
    <View style={styles.container}>
      <Text style={styles.title}>👤 Lib++ User Profile</Text>
      <Text style={styles.subtitle}>You're logged in. Welcome to the app!</Text>

      <TouchableOpacity style={styles.logoutButton} onPress={handleLogout}>
        <Text style={styles.logoutText}>Log Out</Text>
      </TouchableOpacity>
    </View>
  );
};

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#f7f7ff', justifyContent: 'center', alignItems: 'center' },
  title: { fontSize: 24, fontWeight: '700', color: '#6a0dad', marginBottom: 10 },
  subtitle: { fontSize: 16, color: '#333', marginBottom: 30 },
  logoutButton: {
    backgroundColor: '#ff5c5c',
    paddingVertical: 12,
    paddingHorizontal: 24,
    borderRadius: 10,
  },
  logoutText: { color: '#fff', fontSize: 16, fontWeight: '600' },
});

export default ProfileScreen;
