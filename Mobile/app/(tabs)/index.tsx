// app/index.tsx
import { useEffect } from 'react';
import { useRouter } from 'expo-router';

export default function AppIndex() {
  const router = useRouter();

  useEffect(() => {
    const timeout = setTimeout(() => {
      router.replace('/welcome');
    }, 100);

    return () => clearTimeout(timeout);
  }, []);

  return null;
}
