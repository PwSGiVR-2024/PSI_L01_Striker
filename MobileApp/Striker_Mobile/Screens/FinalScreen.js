import React, { useState, useEffect, useRef } from 'react';
import { View, Text, StyleSheet, StatusBar } from 'react-native';
import BLEService from '../Services/BLEService';
import { GlobalStyles } from '../Styles/GlobalStyles';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useSafeAreaInsets } from 'react-native-safe-area-context';
import WebSocketService from '../Services/WebSocketService';


function FinalScreen({ navigation }) {
  const insets = useSafeAreaInsets();

  const [heartRate, setHrValue] = useState(BLEService.latestHr);
  const [isTcpConnected, setIsTcpConnected] = useState(false);
  const hrRef = useRef(heartRate);

  useEffect(() => { hrRef.current = heartRate; }, [heartRate]);

  useEffect(() => {
    const unsubscribe = BLEService.addListener(setHrValue);
    return () => unsubscribe();
  }, []);

  useEffect(() => {
    WebSocketService.on('open',   () => setIsTcpConnected(true));
    WebSocketService.on('close',  () => setIsTcpConnected(false));
    WebSocketService.on('error',  err => {
      setIsTcpConnected(false);
    });

    WebSocketService.connect();

    return () => {
      WebSocketService.disconnect();
      WebSocketService.removeAllListeners();
    };
  }, []);

  useEffect(() => {
    if (!isTcpConnected) return;
    const timer = setInterval(() => {
      WebSocketService.send({
        heartRate: hrRef.current,
        timestamp: Date.now(),
      });
    }, 1000);
    return () => clearInterval(timer);
  }, [isTcpConnected]);

  return (
    <SafeAreaView style={[styles.container, GlobalStyles.center]} edges={['left', 'right', 'bottom']}>
    <View style={{ height: insets.top, backgroundColor: "#000" }} />
    <StatusBar style="light" backgroundColor="#000" translucent={false} hidden={false} />

        <View style={[GlobalStyles.flex, GlobalStyles.center]}>
            <Text style={[GlobalStyles.text32, GlobalStyles.white]}>{heartRate}</Text>
        </View>

    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: 
  { 
    flex:1, 
    backgroundColor:'#000' 
  },
  container: {
    flex: 1,
    backgroundColor: 'black',
    flexDirection: 'column',
  },

});

export default FinalScreen;