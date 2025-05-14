
import React, { useEffect } from 'react';

const WebBarcodeScanner = ({ onDetected }: { onDetected: (isbn: string) => void }) => {
  useEffect(() => {
    const script = document.createElement('script');
    script.src = 'https://unpkg.com/html5-qrcode';
    script.async = true;
    script.onload = () => {
      const { Html5QrcodeScanner } = (window as any);

      const scanner = new Html5QrcodeScanner(
        "barcode-reader",
        {
          fps: 10,
          qrbox: { width: 250, height: 100 }
        },
        false
      );

      scanner.render(
        (decodedText: string) => {
          if (/^\d{10,13}$/.test(decodedText)) {
            console.log("📦 Scanned ISBN:", decodedText);
            onDetected(decodedText);
            scanner.clear(); // Stop scanning after a successful scan
          } else {
            console.warn("Scanned data is not a valid ISBN:", decodedText);
          }
        },
        (error: string) => {
          console.warn("Scanning error:", error);
        }
      );
    };

    document.body.appendChild(script);

    return () => {
      const el = document.getElementById('barcode-reader');
      if (el) el.innerHTML = '';
    };
  }, [onDetected]);

  return (
    <div>
      <p style={{ color: '#fff', marginBottom: 10 }}>Web QR Scan</p>
      <div id="barcode-reader" style={{ width: '100%' }} />
    </div>
  );
};

export default WebBarcodeScanner;
