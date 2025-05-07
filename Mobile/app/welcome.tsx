export const options = {
    headerShown: false,
  };
  
  import React, { useEffect, useState } from 'react';
  import {
    View,
    Text,
    TouchableOpacity,
    StyleSheet,
    StatusBar,
  } from 'react-native';
  import { useRouter } from 'expo-router';
  import * as Animatable from 'react-native-animatable';
  import { LinearGradient } from 'expo-linear-gradient';
  
  const WelcomeScreen = () => {
    const router = useRouter();
    const slogan = '"Organize your mind, one book at a time."';
    const [displayedText, setDisplayedText] = useState('');
  
    useEffect(() => {
      let index = 0;
      const interval = setInterval(() => {
        setDisplayedText(slogan.slice(0, index + 1));
        index++;
        if (index === slogan.length) clearInterval(interval);
      }, 50);
      return () => clearInterval(interval);
    }, []);
  
    return (
      <LinearGradient colors={['#4B0082', '#6a0dad', '#8A2BE2']} style={styles.container}>
        <StatusBar barStyle="light-content" />
  
        <Animatable.Text animation="fadeInDown" duration={1200} style={styles.logoText}>
          Lib<Text style={styles.plus}>++</Text>
        </Animatable.Text>
  
        <Animatable.Text animation="fadeIn" delay={600} style={styles.title}>
          Welcome to <Text style={styles.brand}>Lib++</Text>
        </Animatable.Text>
  
        <Text style={styles.slogan}>{displayedText}</Text>
  
        <Animatable.View animation="fadeInUp" delay={1000} style={styles.buttons}>
          <TouchableOpacity style={styles.button} onPress={() => router.push('/login')}>
            <Text style={styles.buttonText}>Login</Text>
          </TouchableOpacity>
          <TouchableOpacity style={[styles.button, styles.registerButton]} onPress={() => router.push('/register')}>
            <Text style={styles.buttonText}>Register</Text>
          </TouchableOpacity>
        </Animatable.View>
      </LinearGradient>
    );
  };
  
  const styles = StyleSheet.create({
    container: { flex: 1, justifyContent: 'center', paddingHorizontal: 30 },
    logoText: {
      fontSize: 72,
      fontWeight: 'bold',
      textAlign: 'center',
      color: '#fff',
      letterSpacing: 1.5,
      textShadowColor: 'rgba(0, 0, 0, 0.3)',
      textShadowOffset: { width: 1, height: 1 },
      textShadowRadius: 2,
      marginBottom: 60,
    },
    plus: { color: '#ffa500' },
    title: { fontSize: 22, textAlign: 'center', color: '#fff', fontWeight: '500' },
    brand: { color: '#00ffcc', fontWeight: 'bold' },
    slogan: {
      fontSize: 16, color: '#fff', marginTop: 15,
      textAlign: 'center', fontStyle: 'italic', minHeight: 30,
    },
    buttons: { marginTop: 50 },
    button: {
      backgroundColor: '#5E239D',
      paddingVertical: 14,
      borderRadius: 10,
      marginBottom: 15,
      alignItems: 'center',
      borderWidth: 1,
      borderColor: '#fff',
    },
    registerButton: {
      backgroundColor: '#F48FB1',
    },
    buttonText: {
      color: '#fff',
      fontSize: 16,
      fontWeight: '600',
    },
  });
  
  export default WelcomeScreen;
  