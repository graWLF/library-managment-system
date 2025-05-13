import React, { useEffect, useState } from 'react';
import { View, Text, StyleSheet } from 'react-native';
import axios from 'axios';
import API_BASE_URL from '@/api/apiConfig';

const ProfileScreen = () => {
  const [userData, setUserData] = useState<any>(null);

  useEffect(() => {
    const fetchUserData = async () => {
      try {
        console.log("➡️ Kullanıcı Bilgileri İsteniyor...");
        const response = await axios.get(`${API_BASE_URL}/Registration`);
        console.log("✅ Kullanıcı Bilgileri Geldi:", response.data);

        // Gelen yanıtı kaydedelim:
        if (response.data) {
          setUserData(response.data);
        }
      } catch (error: any) {  // <-- Burada any olarak belirttim
        console.error("❌ Kullanıcı Bilgileri Alınamadı:", error.message);
      }
    };

    fetchUserData();
  }, []);

  return (
    <View style={styles.container}>
      {userData ? (
        <>
          <Text style={styles.title}>Merhaba, {userData.username}!</Text>
          <Text style={styles.info}>Email: {userData.email}</Text>
          <Text style={styles.info}>Name: {userData.name}</Text>
          <Text style={styles.info}>Lastname: {userData.lastname}</Text>
        </>
      ) : (
        <Text style={styles.info}>Yükleniyor...</Text>
      )}
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#181818',
  },
  title: {
    fontSize: 24,
    fontWeight: 'bold',
    color: '#6a0dad',
  },
  info: {
    fontSize: 18,
    color: '#ccc',
    marginVertical: 5,
  },
});

export default ProfileScreen;
