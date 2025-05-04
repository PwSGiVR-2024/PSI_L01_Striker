import React, { useState, useEffect, useRef } from 'react';
import { View, Text, StyleSheet, StatusBar, Animated, Easing } from 'react-native';
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
  const pulseAnim = useRef(new Animated.Value(1)).current;

  const pulseLoop = useRef(
    Animated.loop(
      Animated.sequence([
        Animated.timing(pulseAnim, {
          toValue: 0.3,
          duration: 600,
          easing: Easing.inOut(Easing.ease),
          useNativeDriver: true,
        }),
        Animated.timing(pulseAnim, {
          toValue: 1,
          duration: 600,
          easing: Easing.inOut(Easing.ease),
          useNativeDriver: true,
        }),
      ])
    )
  ).current;

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
    setIsTcpConnected(true);

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

  useEffect(() => {
    if (isTcpConnected) {
      pulseLoop.start();
    } else {
      pulseLoop.stop();
      pulseAnim.setValue(1);
    }
  }, [isTcpConnected, pulseLoop, pulseAnim]);

  return (
    <SafeAreaView style={styles.container} edges={['left', 'right', 'bottom']}>
    <View style={{ height: insets.top, backgroundColor: '#000' }} />
    <StatusBar
      barStyle="light-content"
      backgroundColor="#000"
    />

    <View style={styles.inner}>
        <View style={styles.topContainer}>
          <Animated.View
            style={[
              styles.dot,
              {
                backgroundColor: isTcpConnected ? 'limegreen' : 'red',
                opacity: pulseAnim,
              },
            ]}
          />
          <Text style={styles.statusText}>
            {isTcpConnected ? 'Connected' : 'Disconnected'}
          </Text>
        </View>


      <View style={[styles.contentContainer, GlobalStyles.center]}>
        {heartRate < 90 ? (
            <Text style={styles.normalText}>{heartRate} bpm</Text>
        ) : heartRate < 110 ? (
            <Text style={styles.mediumText}>{heartRate} bpm</Text>
        ) : (
            <Text style={styles.highText}>{heartRate} bpm</Text>
        )}
      </View>

    </View>
  </SafeAreaView>
  );
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: 'black',
      },
      inner: {
        flex: 1,
        flexDirection: 'column',
        alignItems: 'stretch',
        justifyContent: 'center',
      },
      topContainer: {
        flex: 0.05,
        width: '100%',
        padding: 20,
        flexDirection: 'row',
      },
      dot: {
        width: 14,
        height: 14,
        marginTop: 3,
        borderRadius: 7,
        marginRight: 8,
      },
      statusText: {
        color: '#FFF',
        fontSize: 16,
        fontWeight: '600',
      },
      contentContainer: {
        flex: 0.90,
        width: '100%',
      },
      white: {
        color: 'white',
      },
      normalText: {
        textAlign: 'center',
        color: '#FFF',
        fontSize: 42,
        fontWeight: '900',
        letterSpacing: 6,
        textShadowColor: '#00FFFF',
        textShadowOffset: { width: 0, height: 4 },
        textShadowRadius: 16,
      },
      mediumText: {
        textAlign: 'center',
        color: '#FFF',
        fontSize: 42,
        fontWeight: '900',
        letterSpacing: 6,
        textShadowColor: '#FF6600',
        textShadowOffset: { width: 0, height: 4 },
        textShadowRadius: 16,
      },
      highText: {
        textAlign: 'center',
        color: '#FFF',
        fontSize: 42,
        fontWeight: '900',
        letterSpacing: 6,
        textShadowColor: '#FF0000',
        textShadowOffset: { width: 0, height: 4 },
        textShadowRadius: 16,
      },
});

export default FinalScreen;