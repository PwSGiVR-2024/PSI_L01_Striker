import React, { useState, useEffect, useRef, useContext } from 'react';
import { View, Text, TouchableOpacity, StyleSheet, ActivityIndicator, StatusBar, Platform, ScrollView } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useSafeAreaInsets } from 'react-native-safe-area-context';
import { GlobalStyles } from '../Styles/GlobalStyles';


function BleConnectionScreen({ navigation }) {
  const insets = useSafeAreaInsets();

  const [blePermissionsGranted, setBlePermissionsGranted] = useState(true);
  const [searchingForDevices, setSearchingForDevices] = useState(false);

  const searchForDevices = () => {
    setSearchingForDevices(true);


  };

  return (
    <SafeAreaView style={[styles.container, GlobalStyles.center]} edges={['left', 'right', 'bottom']}>
      <View style={{ height: insets.top, backgroundColor: "#000" }} />
      <StatusBar style="light" backgroundColor="#000" translucent={false} hidden={false} />

        {!blePermissionsGranted ? (
            <>
                <View style={[styles.logoContainer, GlobalStyles.center]}>        
                    <Text style={styles.errorText}>You need to give striker permisssion to acesss ble to connect.</Text>
                </View>
                
                <View style={[styles.buttonContainer]}>
                    <TouchableOpacity style={styles.elevatedBtn} activeOpacity={0.8}>
                        <Text style={styles.buttonText}>I UNDERSTAND</Text>
                    </TouchableOpacity>
                </View>
            </>
        ):(
            <>
                <View style={[styles.logoContainer, GlobalStyles.center]}>    
                    {searchingForDevices && (
                        <ActivityIndicator size="large" color="#FFF" />
                    )}
                </View>
                
                <View style={[styles.buttonContainer]}>
                    <TouchableOpacity style={styles.elevatedBtn} onPress={() => searchForDevices()} activeOpacity={0.8}>
                        <Text style={styles.buttonText}>SEARCH FOR DEVICES</Text>
                    </TouchableOpacity>
                </View>
            </>
        )}

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
    textAlign: 'center',
  },
  errorText: {
    textAlign: 'center',
    color: '#FFF',
    fontSize: 16,
    fontWeight: '900',
    letterSpacing: 6,
    textShadowColor: '#FF0000',
    textShadowOffset: { width: 0, height: 4 },
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

export default BleConnectionScreen;