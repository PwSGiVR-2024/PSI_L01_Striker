import React, { useState, useEffect  } from 'react';
import { ActivityIndicator, View } from 'react-native';
import { NavigationContainer } from '@react-navigation/native';
import { createNativeStackNavigator } from '@react-navigation/native-stack';

import HomeScreen from './Screens/HomeScreen';
import BleConnectionScreen from './Screens/BleConnectionScreen';
import UnityConnectionScreen from './Screens/UnityConnectionScreen';

import * as Font from 'expo-font';

const Stack = createNativeStackNavigator();

function App() {
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    const prepareApp = async () => {
      try {
        await Font.loadAsync({
          Helvetica: require('./assets/fonts/Helvetica.ttf'),
          HelveticaBold: require('./assets/fonts/Helvetica-Bold.ttf'),
        });
        
        setIsReady(true);
      } catch (error) {
        console.error('Error during app preparation', error);
      }
    };

    prepareApp();
  }, []);

  if (!isReady) {
    return (
      <View style={{ flex: 1, justifyContent: 'center', alignItems: 'center', backgroundColor: 'black' }}>
        <ActivityIndicator size="large" color="#FFF" />
      </View>
    );
  }

  return (
    <NavigationContainer>
      <Stack.Navigator
        initialRouteName={'Home'}
        screenOptions={{
          headerShown: false,
          animation: 'none',
        }}
      >
        <Stack.Screen name="Home" component={HomeScreen} />
        <Stack.Screen name="BleConnection" component={BleConnectionScreen} />
        <Stack.Screen name="UnityConnectionScreen" component={UnityConnectionScreen}/>
      </Stack.Navigator>
    </NavigationContainer>
  );
}

export default function AppWrapper() {
  return (
        <App />
  );
}