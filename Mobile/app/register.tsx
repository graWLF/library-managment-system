import React, { useState } from 'react';
import {
  View,
  TextInput,
  TouchableOpacity,
  Text,
  StyleSheet,
  Alert,
  KeyboardAvoidingView,
  Platform,
} from 'react-native';
import { useRouter } from 'expo-router';
import { registerUser } from '@/api/services';
import BackButton from '../components/BackButton';

const RegisterScreen = () => {
  const router = useRouter();

  const [username, setUsername] = useState('');
  const [name, setName] = useState('');
  const [lastname, setLastname] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const [errors, setErrors] = useState<{
    username?: string;
    name?: string;
    lastname?: string;
    email?: string;
    password?: string;
  }>({});

  const validateInputs = () => {
    const newErrors: typeof errors = {};

    if (!username.trim()) newErrors.username = 'Username is required.';
    if (!name.trim()) newErrors.name = 'Name is required.';
    if (!lastname.trim()) newErrors.lastname = 'Lastname is required.';
    if (!email.trim()) newErrors.email = 'Email is required.';
    else if (!/\S+@\S+\.\S+/.test(email)) newErrors.email = 'Invalid email format.';
    if (!password.trim()) newErrors.password = 'Password is required.';
    else if (password.length < 6) newErrors.password = 'Password must be at least 6 characters.';

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

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

      console.log('🔍 Giden userData:', JSON.stringify(userData, null, 2));
      console.log('Registering with:', userData);
      await registerUser(userData);
      Alert.alert('Success', 'You can now log in.');
      router.replace('/login');
    } catch (error: any) {
      if (error.response) {
        console.error('❌ Registration Error (Backend):', error.response.data);
        Alert.alert('Error', error.response.data.message || 'Registration failed.');
      } else {
        console.error('❌ Registration Error (Network):', error.message);
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
      <Text style={styles.title}>Create your Lib++ account</Text>
      <Text style={styles.subtitle}>Start managing your books smarter</Text>

      <View style={styles.form}>
        <TextInput
          style={[styles.input, errors.username && styles.inputError]}
          placeholder="Username"
          placeholderTextColor="#888"
          autoCapitalize="none"
          onChangeText={setUsername}
          value={username}
        />
        {errors.username && <Text style={styles.errorText}>{errors.username}</Text>}

        <TextInput
          style={[styles.input, errors.name && styles.inputError]}
          placeholder="Name"
          placeholderTextColor="#888"
          onChangeText={setName}
          value={name}
        />
        {errors.name && <Text style={styles.errorText}>{errors.name}</Text>}

        <TextInput
          style={[styles.input, errors.lastname && styles.inputError]}
          placeholder="Lastname"
          placeholderTextColor="#888"
          onChangeText={setLastname}
          value={lastname}
        />
        {errors.lastname && <Text style={styles.errorText}>{errors.lastname}</Text>}

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

        <TouchableOpacity style={styles.button} onPress={handleRegister}>
          <Text style={styles.buttonText}>Register</Text>
        </TouchableOpacity>
      </View>
    </View>
  );
};

const PRIMARY_PURPLE = '#6a0dad';
const SECONDARY_GREEN = '#228B22';

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#181818',
  },
  title: {
    fontSize: 26,
    fontWeight: '700',
    color: PRIMARY_PURPLE,
    textAlign: 'center',
    marginBottom: 8,
  },
  subtitle: {
    fontSize: 16,
    textAlign: 'center',
    color: '#666',
    marginBottom: 20,
  },
  form: {
    gap: 10,
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
