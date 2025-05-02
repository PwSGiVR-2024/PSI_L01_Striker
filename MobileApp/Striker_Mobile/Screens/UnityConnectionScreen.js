import React, { useState, useEffect } from 'react';
import { View, Text, StyleSheet, StatusBar, TouchableOpacity, TextInput } from 'react-native';
import BLEService from '../Services/BLEService';
import { GlobalStyles } from '../Styles/GlobalStyles';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useSafeAreaInsets } from 'react-native-safe-area-context';
import LottieView from 'lottie-react-native';
import ConnectingLottie from '../assets/lotties/connecting.json';


function UnityConnectionScreen({ navigation }) {
  const insets = useSafeAreaInsets();

  const [heartRate, setHrValue] = useState(BLEService.latestHr);
  const [ipAddr, setIpAddr] = useState(null);
  const [ipSetted, setIpSetted] = useState(false);
  const [isConnecting, setIsConnecting] = useState(false);
  const [isConnected, setIsConnected] = useState(false);

  useEffect(() => {
    const unsubscribe = BLEService.addListener((bpm) => {
      setHrValue(bpm);
    });
    return () => unsubscribe();
  }, []);

  const setIpAddress = () => {
    console.log("Ip adr setted");
    //validate
    setIpSetted(true);
  };

  const connectToClient = () => {
    setIsConnecting(true);
    
  };

  return (
    <SafeAreaView style={[styles.container, GlobalStyles.center]} edges={['left', 'right', 'bottom']}>
    <View style={{ height: insets.top, backgroundColor: "#000" }} />
    <StatusBar style="light" backgroundColor="#000" translucent={false} hidden={false} />

        {!isConnected ? (
            <>
                <View style={[styles.logoContainer, GlobalStyles.center]}>
                    {!ipSetted ? (
                        <>
                            <TextInput
                                style={styles.input}
                                value={ipAddr}
                                onChangeText={setIpAddr}
                                autoFocus
                                placeholderTextColor="#FFF"
                            />
                            <Text style={[styles.errorText, {marginTop: 50}]}>provide ip address of device where the game is running.</Text>
                        </>
                    ):(
                        <>
                        {!isConnecting ? (
                            <Text style={[styles.ipText, {marginTop: 50}]}>{ipAddr}</Text>
                        ):(
                            <>
                                <View style={styles.lottieContainer}>
                                  <LottieView
                                    source={ConnectingLottie}
                                    style={styles.lottie}
                                    autoPlay
                                    loop
                                  />
                                  <Text style={styles.connectionText}>Connecting to {ipAddr}...</Text>
                                </View>
                            </>
                        )}
                        </>
                    )}                   
                </View>        
                    
                <View style={[styles.buttonContainer]}>
                    {!ipSetted ? (
                        <TouchableOpacity onPress={() => setIpAddress()} style={styles.elevatedBtn} activeOpacity={0.8}>
                            <Text style={styles.buttonText}>SET</Text>
                        </TouchableOpacity>
                    ):(
                      <>
                        {!isConnecting && (
                          <TouchableOpacity onPress={() => connectToClient()} style={styles.elevatedBtn} activeOpacity={0.8}>
                              <Text style={styles.buttonText}>CONNECT</Text>
                          </TouchableOpacity>
                      ) }
                      </>
                    )}
                </View>
            </>
        ):(
            <>
                <View style={[GlobalStyles.flex, {width: '100%'}]}>
                  <Text style={[styles.ipText, {marginTop: 50}]}>CONNECTED</Text>
                </View>
            </>
        )}
  </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: 
  { 
    flex:1, 
    backgroundColor:'#000' 
  },
  bpm: {
    color: 'whitesmoke',
    fontSize: 72,
  },
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
  ipText: {
    textAlign: 'center',
    color: '#FFF',
    fontSize: 36,
    fontWeight: '900',
    letterSpacing: 6,
    textShadowColor: '#FF0000',
    textShadowOffset: { width: 0, height: 4 },
    textShadowRadius: 36,
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
  input: {
    color: '#FFF',
    backgroundColor: '#000',
    borderBottomWidth: 2,
    borderBottomColor: '#FFF',
    width: '80%',
    alignSelf: 'center',
    paddingVertical: 8,
    fontSize: 42,
  },

  lottieContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    width: '100%',
  },
  lottie: {
    flex: 1,
    width: '80%',
  },
  connectionText: {
    textAlign: 'center',
    color: '#FFF',
    paddingLeft: 12,
    paddingRight: 12,
    fontSize: 20,
    fontWeight: '900',
    letterSpacing: 3,
    textShadowColor: '#FF0000',
    textShadowOffset: { width: 0, height: 4 },
    textShadowRadius: 16,
  },
});

export default UnityConnectionScreen;