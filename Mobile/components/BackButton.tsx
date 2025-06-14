import { useRouter } from 'expo-router';
import { TouchableOpacity, Text, StyleSheet } from 'react-native';

export default function BackButton() {
  const router = useRouter();

  return (
    <TouchableOpacity onPress={() => router.back()} style={styles.back}>
      <Text style={styles.backText}>← Back</Text>
    </TouchableOpacity>
  );
}

const styles = StyleSheet.create({
  back: {
    position: 'absolute',
    top: 50,
    left: 20,
    zIndex: 10,
    backgroundColor: '#6a0dad',
    borderRadius: 8,
    paddingVertical: 4,   
    paddingHorizontal: 16, 
  },
  backText: {
    color: '#fff',
    fontSize: 16,
    fontWeight: '600',
  },
});
