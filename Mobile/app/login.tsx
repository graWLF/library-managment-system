export const options = {
    headerShown: false,
  };
  
  import React, { useState } from 'react';
  import {
    View,
    Text,
    TextInput,
    TouchableOpacity,
    StyleSheet,
    KeyboardAvoidingView,
    Platform
  } from 'react-native';
  import AsyncStorage from '@react-native-async-storage/async-storage';
  import axios from 'axios';
  import { useRouter } from 'expo-router';
  import BackButton from '../components/BackButton';
  
  const LoginScreen = () => {
    const router = useRouter();
  
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [errors, setErrors] = useState<{ email?: string; password?: string }>({});
  
    const validateInputs = () => {
      const newErrors: { email?: string; password?: string } = {};
  
      if (!email.trim()) {
        newErrors.email = 'Email is required.';
      } else if (!/\S+@\S+\.\S+/.test(email)) {
        newErrors.email = 'Invalid email format.';
      }
  
      if (!password.trim()) {
        newErrors.password = 'Password is required.';
      } else if (password.length < 6) {
        newErrors.password = 'Password must be at least 6 characters.';
      }
  
      setErrors(newErrors);
      return Object.keys(newErrors).length === 0;
    };
  
    const handleLogin = async () => {
      if (!validateInputs()) return;
  
      try {
        const response = await axios.post('http://192.168.X.X:5000/api/Registration/login', {
          email,
          password,
        });
  
        const token = response.data?.token;
        if (token) {
          await AsyncStorage.setItem('authToken', token);
          router.replace('/(tabs)/explore');
        } else {
          setErrors({ email: 'Invalid email or password.' });
        }
      } catch (err: any) {
        console.error(err);
        setErrors({ email: 'Login failed. Please try again later.' });
      }
    };
  
    return (
      <KeyboardAvoidingView
        style={styles.container}
        behavior={Platform.select({ ios: 'padding', android: undefined })}
      >
        <BackButton />
        <Text style={styles.title}>Login to Lib++</Text>
  
        <View style={styles.form}>
          <TextInput
            style={[styles.input, errors.email && styles.inputError]}
            placeholder="Email"
            placeholderTextColor="#888"
            keyboardType="email-address"
            autoCapitalize="none"
            onChangeText={setEmail}
            value={email}
          />
          {errors.email && <Text style={styles.errorText}>{errors.email}</Text>}
  
          <TextInput
            style={[styles.input, errors.password && styles.inputError]}
            placeholder="Password"
            placeholderTextColor="#888"
            secureTextEntry
            onChangeText={setPassword}
            value={password}
          />
          {errors.password && <Text style={styles.errorText}>{errors.password}</Text>}
  
          <TouchableOpacity style={styles.button} onPress={handleLogin}>
            <Text style={styles.buttonText}>Login</Text>
          </TouchableOpacity>
        </View>
      </KeyboardAvoidingView>
    );
  };
  
  const styles = StyleSheet.create({
    container: {
      flex: 1,
      backgroundColor: '#f7f7ff',
      justifyContent: 'center',
      paddingHorizontal: 28,
    },
    title: {
      fontSize: 26,
      fontWeight: '700',
      color: '#6a0dad',
      textAlign: 'center',
      marginBottom: 24,
    },
    form: {
      gap: 10,
    },
    input: {
      backgroundColor: '#fff',
      borderColor: '#ccc',
      borderWidth: 1.3,
      borderRadius: 10,
      paddingHorizontal: 15,
      paddingVertical: 13,
      fontSize: 16,
    },
    inputError: {
      borderColor: 'red',
    },
    errorText: {
      color: 'red',
      fontSize: 13,
      marginBottom: 4,
      marginLeft: 4,
    },
    button: {
      backgroundColor: '#5E239D',
      borderRadius: 10,
      paddingVertical: 14,
      marginTop: 12,
      alignItems: 'center',
      elevation: 2,
    },
    buttonText: {
      color: '#fff',
      fontWeight: '700',
      fontSize: 16,
    },
  });
  
  export default LoginScreen;
  