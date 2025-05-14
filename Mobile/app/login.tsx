import React, { useState } from 'react';
import {
  View,
  TextInput,
  TouchableOpacity,
  Text,
  StyleSheet,
  Alert,
} from 'react-native';
import { useRouter } from 'expo-router';
import { loginUser } from '@/api/services';

const LoginScreen = () => {
  const router = useRouter();
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');

  const handleLogin = async () => {
    try {
      const userData = {
        username: username,
        password: password
      };

      console.log("➡️ Login İsteği Gönderiliyor:", userData);

      const response = await loginUser(userData);

      console.log("⬅️ Gelen Yanıt:", response);

      if (response === "Login successful.") {
        console.log("✅ Login Başarılı:", response);

        // Başarılı giriş yaptı, profil sayfasına yönlendiriyoruz.
        Alert.alert('Success', 'Login successful');
        router.replace('/profile');
      } else {
        console.log("❌ Beklenen yanıt gelmedi!");
        Alert.alert('Error', 'Login failed: Missing data.');
      }
    } catch (error: any) {
      if (error.response) {
        console.error("❌ Login Error (Backend):", error.response.data);
        Alert.alert('Error', error.response.data.message || "Invalid credentials.");
      } else {
        console.error("❌ Login Error (Network):", error.message);
        Alert.alert('Error', 'Network error. Please try again.');
      }
    }
  };

  return (
    <View style={styles.container}>
      <View style={styles.innerContainer}>
        <Text style={styles.title}>Lib++ Login</Text>

        <TextInput
          placeholder="Username"
          placeholderTextColor="#888"
          style={styles.input}
          onChangeText={setUsername}
          value={username}
        />

        <TextInput
          placeholder="Password"
          placeholderTextColor="#888"
          style={styles.input}
          secureTextEntry
          onChangeText={setPassword}
          value={password}
        />

        <TouchableOpacity style={styles.button} onPress={handleLogin}>
          <Text style={styles.buttonText}>Login</Text>
        </TouchableOpacity>
      </View>
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
  innerContainer: {
    width: '90%',
    maxWidth: 400,
    backgroundColor: '#282828',
    padding: 20,
    borderRadius: 10,
    alignItems: 'center',
  },
  title: {
    fontSize: 26,
    fontWeight: 'bold',
    marginBottom: 20,
    color: '#6a0dad',
  },
  input: {
    width: '100%',
    borderWidth: 1,
    borderColor: '#ccc',
    padding: 12,
    marginBottom: 10,
    borderRadius: 8,
    backgroundColor: '#333',
    color: '#fff',
  },
  button: {
    backgroundColor: '#6a0dad',
    padding: 15,
    borderRadius: 8,
    width: '100%',
    alignItems: 'center',
    marginTop: 10,
  },
  buttonText: {
    color: '#fff',
    fontSize: 16,
    fontWeight: '600',
  },
});

export default LoginScreen;
