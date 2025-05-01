import React, { useState, useEffect } from 'react';
import { View, Text, StyleSheet } from 'react-native';
import BLEService from '../Services/BLEService';
import { GlobalStyles } from '../Styles/GlobalStyles';

function UnityConnectionScreen({ navigation }) {
  const [heartRate, setHrValue] = useState(BLEService.latestHr);

  useEffect(() => {
    const unsubscribe = BLEService.addListener((bpm) => {
      setHrValue(bpm);
    });
    return () => unsubscribe();
  }, []);

  return (
    <View style={[styles.container, GlobalStyles.center]}>
      <Text style={styles.bpm}>{heartRate}</Text>
    </View>
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
  }
});

export default UnityConnectionScreen;