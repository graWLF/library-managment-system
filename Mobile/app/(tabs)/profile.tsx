import React, { useEffect, useState } from 'react';
import {
  View,
  Text,
  StyleSheet,
  TouchableOpacity,
  Modal,
  ScrollView, // Import ScrollView
} from 'react-native';
import { useRouter } from 'expo-router';
import { getLoginInfo } from '@/api/services';
import AsyncStorage from '@react-native-async-storage/async-storage';

const ProfileScreen = () => {
  const router = useRouter();
  const [user, setUser] = useState<{ username: string; email?: string; name?: string; lastname?: string; authLevel: number } | null>(null);
  const [showConfirm, setShowConfirm] = useState(false);

  useEffect(() => {
    const fetchUser = async () => {
      const stored = await AsyncStorage.getItem('user');
      if (stored) {
        const parsedUser = JSON.parse(stored);
        setUser(parsedUser); // Set the user from AsyncStorage
        const userInfo = await getLoginInfo(parsedUser.username); // Fetch additional user data
        setUser((prevUser) => ({ ...prevUser, ...userInfo })); // Update user state with full info
      }
    };
    fetchUser();
  }, []);

  const handleLogout = () => {
    setShowConfirm(true); // Show modal
  };

  const logoutAndRedirect = async () => {
    setShowConfirm(false);
    await AsyncStorage.removeItem('user');
    router.replace('/welcome');
  };

  // Function to get the authorization level description
  const getAuthLevelDescription = (authLevel: number) => {
    if (authLevel === 0) {
      return 'General User';
    } else if (authLevel === 1) {
      return 'Librarian';
    } else {
      return 'Supervisor';
    }
  };

  return (
    <ScrollView contentContainerStyle={styles.scrollContainer}> {/* Wrap content in ScrollView */}
      <View style={styles.container}>
        <Text style={styles.header}>Profile</Text>

        <View style={styles.card}>
          <Text style={styles.label}>Username:</Text>
          <Text style={styles.value}>{user?.username || 'Unknown'}</Text>

          <Text style={styles.label}>Name:</Text>
          <Text style={styles.value}>{user?.name || 'Unknown'}</Text>

          <Text style={styles.label}>Last Name:</Text>
          <Text style={styles.value}>{user?.lastname || 'Unknown'}</Text>

          <Text style={styles.label}>Email:</Text>
          <Text style={styles.value}>{user?.email || 'Unknown'}</Text>

          <Text style={styles.label}>Authorization Level:</Text>
          <Text style={styles.value}>
            {user?.authLevel !== undefined ? getAuthLevelDescription(user.authLevel) : 'Unknown'}
          </Text>
        </View>

        <TouchableOpacity style={styles.button} onPress={handleLogout}>
          <Text style={styles.buttonText}>Log Out</Text>
        </TouchableOpacity>

        <View style={styles.infoSection}>
          <Text style={styles.infoTitle}>About Us</Text>
          <Text style={styles.infoText}>
            Lib++ is a simple digital library system built for modern book
            management.
          </Text>

          <Text style={styles.infoTitle}>Privacy Policy</Text>
          <Text style={styles.infoText}>
            We do not store any personal data. This app is for educational
            purposes only.
          </Text>
        </View>

        {/* Modal */}
        <Modal
          visible={showConfirm}
          transparent
          animationType="fade"
          onRequestClose={() => setShowConfirm(false)}
        >
          <View style={styles.modalOverlay}>
            <View style={styles.modalBox}>
              <Text style={styles.modalText}>Are you sure you want to log out?</Text>
              <View style={styles.modalButtons}>
                <TouchableOpacity onPress={() => setShowConfirm(false)}>
                  <Text style={styles.cancelButton}>Cancel</Text>
                </TouchableOpacity>
                <TouchableOpacity onPress={logoutAndRedirect}>
                  <Text style={styles.confirmButton}>Log Out</Text>
                </TouchableOpacity>
              </View>
            </View>
          </View>
        </Modal>
      </View>
    </ScrollView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#181818',
    paddingHorizontal: 24,
    paddingTop: 60,
  },
  scrollContainer: {
    flexGrow: 1, // Ensures the content scrolls properly
  },
  header: {
    fontSize: 26,
    fontWeight: '700',
    color: '#B266FF',
    marginBottom: 20,
    textAlign: 'center',
  },
  card: {
    backgroundColor: '#2c2c2c',
    padding: 20,
    borderRadius: 10,
    marginBottom: 30,
  },
  label: {
    color: '#888',
    fontSize: 14,
    marginBottom: 4,
  },
  value: {
    color: '#fff',
    fontSize: 16,
    marginBottom: 12,
  },
  button: {
    backgroundColor: '#6a0dad',
    paddingVertical: 14,
    borderRadius: 10,
    alignItems: 'center',
    marginBottom: 30,
  },
  buttonText: {
    color: '#fff',
    fontSize: 16,
    fontWeight: '700',
  },
  infoSection: {
    marginTop: 10,
  },
  infoTitle: {
    color: '#B266FF',
    fontSize: 18,
    fontWeight: '600',
    marginBottom: 6,
  },
  infoText: {
    color: '#ccc',
    fontSize: 14,
    marginBottom: 20,
  },
  modalOverlay: {
    flex: 1,
    backgroundColor: 'rgba(0,0,0,0.5)',
    justifyContent: 'center',
    alignItems: 'center',
  },
  modalBox: {
    backgroundColor: '#fff',
    padding: 28,
    borderRadius: 16,
    width: '85%',
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.3,
    shadowRadius: 6,
    elevation: 10,
  },
  modalText: {
    fontSize: 18,
    fontWeight: '600',
    color: '#222',
    textAlign: 'center',
    marginBottom: 24,
  },
  modalButtons: {
    flexDirection: 'row',
    justifyContent: 'space-evenly',
  },
  cancelButton: {
    fontSize: 16,
    color: '#666',
    backgroundColor: '#eee',
    paddingVertical: 10,
    paddingHorizontal: 20,
    borderRadius: 8,
  },
  confirmButton: {
    fontSize: 16,
    color: '#fff',
    backgroundColor: '#B266FF',
    paddingVertical: 10,
    paddingHorizontal: 20,
    borderRadius: 8,
    fontWeight: '600',
  },
});

export default ProfileScreen;
