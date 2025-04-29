import React, { useState, useEffect, useRef, useContext } from 'react';
import { View, Text, TouchableOpacity, StyleSheet, ActivityIndicator, StatusBar, Platform } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useSafeAreaInsets } from 'react-native-safe-area-context';
import { GlobalStyles } from '../Styles/GlobalStyles';

function HomeScreen({ navigation }) {
  const insets = useSafeAreaInsets();

  const beginSetUp = () => {
    navigation.navigate('BleConnection');
  };

  return (
    <SafeAreaView style={[styles.container, GlobalStyles.center]} edges={['left', 'right', 'bottom']}>
      <View style={{ height: insets.top, backgroundColor: "#000" }} />
      <StatusBar style="light" backgroundColor="#000" translucent={false} hidden={false} />

      <View style={[styles.logoContainer, GlobalStyles.center]}>        
        <Text style={styles.logoText}>STRIKER</Text>
      </View>

       <View style={[styles.buttonContainer]}>
            <TouchableOpacity style={styles.elevatedBtn} onPress={() => beginSetUp()} activeOpacity={0.8}>
                <Text style={styles.buttonText}>BEGIN SET UP</Text>
            </TouchableOpacity>
       </View>

    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: 'black',
    flexDirection: 'column',
  },
  logoContainer: {
    flex: 0.85,
  },
  logoText: {
    color: '#FFF',
    fontSize: 56,
    fontWeight: '900',
    letterSpacing: 12,
    textShadowColor: '#FF0000',
    textShadowOffset: { width: 0, height: 8 },
    textShadowRadius: 16,
  },
  buttonContainer: {
    flex: 0.15,
    width: '100%',
    justifyContent: 'center',
    alignItems: 'center',
  },

  elevatedBtn: {
    width: '90%',
    paddingVertical: 16,
    borderRadius: 18,
    backgroundColor: '#000',
    borderColor: '#FFF',
    borderWidth: 2,
    justifyContent: 'center',
    alignItems: 'center',
    elevation: 6,
    shadowColor: '#fff',
    shadowOffset: { width: 2, height: 12 },
    shadowOpacity: 0.3,
    shadowRadius: 6,
  },
  buttonText: {
    color: '#FFF',
    fontSize: 18,
    fontWeight: 'bold',
    letterSpacing: 1,
    textShadowColor: '#FF0000',
    textShadowOffset: { width: 0, height: 2 },
    textShadowRadius: 16,
  },
});

export default HomeScreen;