import React, { useRef } from 'react';
import { View, Text, TouchableOpacity } from 'react-native';
import Tesseract from 'tesseract.js';

const WebOCRScanner = ({ onDetected }: { onDetected: (isbn: string) => void }) => {
  const fileInputRef = useRef<HTMLInputElement>(null);

  const handleImageUpload = async (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (!file) return;

    const imageUrl = URL.createObjectURL(file);

    const result = await Tesseract.recognize(imageUrl, 'eng', {
      logger: m => console.log(m),
    });

    const text = result.data.text.replace(/[^0-9]/g, ''); // sadece rakamları al

    if (/^\d{10,13}$/.test(text)) {
      onDetected(text);
    } else {
      alert('Valid ISBN not found');
    }
  };

  return (
    <View>
      <Text style={{ color: 'white', marginBottom: 10 }}>Upload barcode image (number part)</Text>
      <input
        type="file"
        accept="image/*"
        ref={fileInputRef}
        onChange={handleImageUpload}
        style={{ marginBottom: 10 }}
      />
      <TouchableOpacity
        style={{
          backgroundColor: '#6a0dad',
          padding: 10,
          borderRadius: 6,
          alignItems: 'center',
        }}
        onPress={() => fileInputRef.current?.click()}
      >
        <Text style={{ color: 'white' }}>Select Image</Text>
      </TouchableOpacity>
    </View>
  );
};

export default WebOCRScanner;
