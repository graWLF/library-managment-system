import React, { useState } from 'react';
import {
  View,
  Text,
  TextInput,
  TouchableOpacity,
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
    if (!validateInputs()) return;

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

        <TouchableOpacity onPress={() => router.push('/login')}>
          <Text style={styles.loginLink}>Already have an account? Log in</Text>
        </TouchableOpacity>
      </View>
    </KeyboardAvoidingView>
  );
};

const PRIMARY_PURPLE = '#6a0dad';
const SECONDARY_GREEN = '#228B22';

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
    backgroundColor: SECONDARY_GREEN,
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
  loginLink: {
    color: PRIMARY_PURPLE,
    textAlign: 'center',
    marginTop: 16,
    fontSize: 14,
    textDecorationLine: 'underline',
  },
});

export default RegisterScreen;
