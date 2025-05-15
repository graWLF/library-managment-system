import React, { useState } from 'react';
import {
  View,
  Text,
  TextInput,
  TouchableOpacity,
  StyleSheet,
  KeyboardAvoidingView,
  Platform,
  Alert,
} from 'react-native';
import { useRouter } from 'expo-router';
import { loginUser } from '@/api/services';
import BackButton from '../components/BackButton';

const LoginScreen = () => {
  const router = useRouter();

  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [errors, setErrors] = useState<{ username?: string; password?: string }>({});

  const validateInputs = () => {
    const newErrors: { username?: string; password?: string } = {};

    if (!username.trim()) {
      newErrors.username = 'Username or email is required.';
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
      const userData = {
        username: username,
        password: password,
      };

      console.log('Login request sent:', userData);
      const result = await loginUser(userData);
      console.log('Login response received:', result);

      if (result === 'Login successful.') {
        console.log('navigating to /search...');
        router.replace('/(tabs)/search');
      } else {
        Alert.alert('Error', 'Invalid username or password.');
      }
    } catch (error: any) {
      if (error.response) {
        console.error('❌ Login Error (Backend):', error.response.data);
        Alert.alert('Error', error.response.data.message || 'Invalid credentials.');
      } else {
        console.error('❌ Login Error (Network):', error.message);
        Alert.alert('Error', 'Network error. Please try again.');
      }
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
          style={[styles.input, errors.username && styles.inputError]}
          placeholder="Username or Email"
          placeholderTextColor="#888"
          autoCapitalize="none"
          onChangeText={setUsername}
          value={username}
        />
        {errors.username && <Text style={styles.errorText}>{errors.username}</Text>}

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
