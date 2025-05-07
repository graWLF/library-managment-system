import React, { useState } from 'react';
import {
  View,
  Text,
  TextInput,
  TouchableOpacity,
  StyleSheet,
  Alert,
  KeyboardAvoidingView,
} from 'react-native';
import axios from 'axios';
import { useRouter } from 'expo-router';



const RegisterScreen = () => {
  const router = useRouter();

  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [errors, setErrors] = useState<{ name?: string; email?: string; password?: string }>({});

  const validateInputs = () => {
    const newErrors: { name?: string; email?: string; password?: string } = {};

    if (!name.trim()) {
      newErrors.name = 'Name is required.';
    }

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

  const handleRegister = async () => {
    if (!validateInputs()) return;

    try {
      const response = await axios.post('http://YOUR-IP-ADDRESS/registration', {
        name,
        email,
        password,
      });

      if (response.status === 200 || response.status === 201) {
        Alert.alert('Registration Successful', 'You can now log in to your account.');
        router.replace('/login');
      } else {
        Alert.alert('Registration Failed', 'Please try again.');
      }
    } catch (err: any) {
      console.error(err);
      Alert.alert('Registration Error', 'An error occurred. Please try again.');
    }
  };

  return (
    <KeyboardAvoidingView style={styles.container}>
      <Text style={styles.brand}>Create your <Text style={styles.brandHighlight}>Lib++</Text> account</Text>
      <Text style={styles.subtitle}>Start managing your books smarter</Text>

      <View style={styles.form}>
        <TextInput
          style={[styles.input, errors.name && styles.inputError]}
          placeholder="Name"
          placeholderTextColor="#888"
          onChangeText={setName}
          value={name}
        />
        {errors.name && <Text style={styles.errorText}>{errors.name}</Text>}

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
const ACCENT_ORANGE = '#ffa500';

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#f7f7ff',
    justifyContent: 'center',
    paddingHorizontal: 28,
  },
  brand: {
    fontSize: 28,
    fontWeight: '700',
    color: '#333',
    textAlign: 'center',
    marginBottom: 4,
  },
  brandHighlight: {
    color: PRIMARY_PURPLE,
  },
  subtitle: {
    fontSize: 16,
    textAlign: 'center',
    color: '#666',
    marginBottom: 30,
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
