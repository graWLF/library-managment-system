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
import { registerUser } from '@/api/services';

const RegisterScreen = () => {
  const router = useRouter();
  const [username, setUsername] = useState('');
  const [name, setName] = useState('');
  const [lastname, setLastname] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const handleRegister = async () => {
    try {
      const userData = {
        id: 0,
        username,
        name,
        lastname,
        email,
        password,
        authLevel: 0,
      };

      await registerUser(userData);
      Alert.alert('Success', 'Registration successful');
      router.replace('/login');
    } catch (error: any) {
      if (error.response) {
        console.error("Registration Error (Backend):", error.response.data);
        Alert.alert('Error', error.response.data.message || "Registration failed.");
      } else {
        console.error("Registration Error (Network):", error.message);
        Alert.alert('Error', 'Network error. Please try again.');
      }
    }
  };

  return (
    <View style={styles.container}>
      <View style={styles.innerContainer}>
        <Text style={styles.title}>Lib++ Register</Text>
        <TextInput placeholder="Username" style={styles.input} onChangeText={setUsername} value={username} />
        <TextInput placeholder="Name" style={styles.input} onChangeText={setName} value={name} />
        <TextInput placeholder="Lastname" style={styles.input} onChangeText={setLastname} value={lastname} />
        <TextInput placeholder="Email" style={styles.input} onChangeText={setEmail} value={email} />
        <TextInput placeholder="Password" style={styles.input} secureTextEntry onChangeText={setPassword} value={password} />
        <TouchableOpacity style={styles.button} onPress={handleRegister}>
          <Text style={styles.buttonText}>Register</Text>
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

export default RegisterScreen;
