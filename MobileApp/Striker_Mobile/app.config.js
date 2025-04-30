import 'dotenv/config';

export default {
  expo: {
    name: "Striker_Mobile",
    slug: "striker",
    version: "1.0.0",
    orientation: "portrait",
    icon: "./assets/icon.png",
    userInterfaceStyle: "light",
    newArchEnabled: true,
    splash: {
      image: "./assets/splash-icon.png",
      resizeMode: "contain",
      backgroundColor: "#ffffff"
    },
    plugins: [
        'expo-dev-client',
    ],
    assetBundlePatterns: [
      "**/*"
    ],
    ios: {
      supportsTablet: true,
      infoPlist: {
        NSBluetoothAlwaysUsageDescription: "Allow Striker to access Bluetooth to connect with fitness devices.",
        NSBluetoothPeripheralUsageDescription: "Allow Striker to connect with Bluetooth peripherals.",
      }
    },
    android: {
      permissions: [
        'ACCESS_FINE_LOCATION',
        'ACCESS_COARSE_LOCATION',
        'BLUETOOTH',
        'BLUETOOTH_ADMIN',
        'BLUETOOTH_SCAN',
        'BLUETOOTH_CONNECT',
        'BLUETOOTH_ADVERTISE',
      ],
      adaptiveIcon: {
        foregroundImage: "./assets/adaptive-icon.png",
        backgroundColor: "#ffffff"
      },
      package: "com.rudychemik.striker",
    },
    androidStatusBar: {
      barStyle: "light-content",
      backgroundColor: "#FF8303",
      translucent: false
    },
    web: {
      favicon: "./assets/favicon.png"
    },
    extra: {
      eas: {
        projectId: process.env.EAS_PROJ_ID
      }
    }
  }
};