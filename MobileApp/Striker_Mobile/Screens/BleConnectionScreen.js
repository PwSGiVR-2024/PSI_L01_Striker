import React, { useState, useEffect, useRef, useContext } from 'react';
import { View, Linking, Text, TouchableOpacity, StyleSheet, ActivityIndicator, StatusBar, Platform, ScrollView } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useSafeAreaInsets } from 'react-native-safe-area-context';
import { GlobalStyles } from '../Styles/GlobalStyles';
import BluetoothDevice from '../Components/BluetoothDevice';
import { checkAndRequestBluetoothPermissionAndroid } from '../Helpers/BluetoothPermissionHelper.js';
import { checkAndRequestLocationPermission } from '../Helpers/LocationPermissionHelper.js';


function BleConnectionScreen({ navigation }) {
  const insets = useSafeAreaInsets();

  const [blePermissionsGranted, setBlePermissionsGranted] = useState(false);
  const [searchingForDevices, setSearchingForDevices] = useState(false);
  const [selectedDevice, setSelectedDevice] = useState(null);
  const [avbDevicesList, setAvbDevicesList] = useState([{name: "device 1", id: 1}, {name: "device 2", id: 2}, {name: "device 3", id: 3}])

  useEffect(() => {
    checkAndRequestPermissions();
  }, []);

  const checkAndRequestPermissions = async () => {
    if (Platform.OS === 'android') {
      const bleGranted = await checkAndRequestBluetoothPermissionAndroid();
      const locationGranted = await checkAndRequestLocationPermission();
      if(bleGranted && locationGranted){
        setBlePermissionsGranted(true);
        return;
      }
      setBlePermissionsGranted(false);
      return;
    }

    setBlePermissionsGranted(false);
  }

  const redirectToSettings = () => {
    if (Platform.OS === 'android') {
      Linking.openSettings();
    }
  }

  const searchForDevices = () => {
    setSearchingForDevices(true);


  };

  const selectDevice = (device) => {
    setSelectedDevice(device);
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
                    <TouchableOpacity onPress={redirectToSettings} style={styles.elevatedBtn} activeOpacity={0.8}>
                        <Text style={styles.buttonText}>I UNDERSTAND</Text>
                    </TouchableOpacity>
                </View>
            </>
        ):(
            <>
                <ScrollView style={[styles.logoContainer, {marginTop: 10}]} contentContainerStyle={GlobalStyles.center}>
                    {avbDevicesList.map((device) => (
                        <BluetoothDevice key={device.id} deviceData={device} isSelected={selectedDevice?.id === device.id} onSelect={() => selectDevice(device)}/>
                    ))}

                    {searchingForDevices && (
                        <ActivityIndicator size="large" color="#FFF" />
                    )}
                </ScrollView>
                
                <View style={[styles.buttonContainer]}>
                    {selectedDevice ? (
                        <TouchableOpacity style={styles.elevatedBtn} onPress={() => searchForDevices()} activeOpacity={0.8}>
                            <Text style={styles.buttonText}>CONNECT</Text>
                        </TouchableOpacity>
                    ):(
                        <TouchableOpacity style={styles.elevatedBtn} onPress={() => searchForDevices()} activeOpacity={0.8}>
                            <Text style={styles.buttonText}>SEARCH FOR DEVICES</Text>
                        </TouchableOpacity>
                    )}
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
    width: '100%', 
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