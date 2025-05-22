import { Tabs, router } from 'expo-router';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { useEffect, useState } from 'react';
import { Ionicons } from '@expo/vector-icons';

export default function ProtectedTabsLayout() {
  const [isLoading, setIsLoading] = useState(true);
  const [authenticated, setAuthenticated] = useState(false);

  useEffect(() => {
    const checkLogin = async () => {
      const user = await AsyncStorage.getItem('user');
      if (user) {
        setAuthenticated(true);
      } else {
        setAuthenticated(false);
        router.replace('/welcome'); // login yoksa welcome'a gönder
      }
      setIsLoading(false);
    };

    checkLogin();
  }, []);

  if (isLoading || !authenticated) return null;

  return (
    <Tabs
      screenOptions={{
        headerShown: false,
        tabBarActiveTintColor: '#6a0dad',
        tabBarStyle: {
          backgroundColor: '#121212',
          borderTopColor: '#333',
        },
        tabBarLabelStyle: {
          fontSize: 12,
        },
      }}
    >
      {/* Görünecek sekmeler */}
      <Tabs.Screen
        name="BookEdit"
        options={{
          title: 'Edit Book',
          tabBarIcon: ({ color, size }) => (
            <Ionicons name="book-outline" size={size} color={color} />
          ),
        }}
      />
      <Tabs.Screen
        name="search"
        options={{
          title: 'Search',
          tabBarIcon: ({ color, size }) => (
            <Ionicons name="search-outline" size={size} color={color} />
          ),
        }}
      />
      <Tabs.Screen
        name="profile"
        options={{
          title: 'Profile',
          tabBarIcon: ({ color, size }) => (
            <Ionicons name="person-outline" size={size} color={color} />
          ),
        }}
      />

      {/* TabBar'da görünmeyecek ekranlar */}
      <Tabs.Screen
        name="index"
        options={{
          href: null,
        }}
      />
      <Tabs.Screen
        name="BarcodeScanner"
        options={{
          href: null,
        }}
      />
    </Tabs>
  );
}
