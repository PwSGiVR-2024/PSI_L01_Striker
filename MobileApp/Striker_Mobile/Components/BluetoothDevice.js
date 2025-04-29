import React, { useState } from 'react';
import { View, StyleSheet, Text, TouchableOpacity, Image } from 'react-native';
import { GlobalStyles } from '../Styles/GlobalStyles';


function BluetoothDevice({ deviceData, isSelected, onSelect }) {
  return (
    <TouchableOpacity onPress={onSelect} style={[ styles.container, isSelected && styles.selectedContainer, GlobalStyles.center, ]}>
        <View style={styles.imgContainer}>
            {isSelected ? (
                <Image
                    source={require('../assets/images/bleImgRed.png')}
                    style={styles.image}
                    resizeMode="contain"
                />
            ):(
                <Image
                    source={require('../assets/images/bleImgWhite.png')}
                    style={styles.image}
                    resizeMode="contain"
                />
            )}
        </View>
        <View style={styles.dataContainer}>
            <Text style={[ GlobalStyles.bold, GlobalStyles.text18, isSelected ? styles.selectedText : GlobalStyles.white,]}>{deviceData.name}</Text>
        </View>
    </TouchableOpacity>
  );
}

const styles = StyleSheet.create({
    container: {
        width: '90%',
        height: 50,
        marginBottom: 10,
        flexDirection: 'row',
        borderColor: '#FFF',
        borderWidth: 2,
        borderRadius: 15,
        overflow: 'hidden',
    },
    selectedContainer: {
        borderColor: 'red',
    },
    selectedText:{
        color: 'red',
    },
    imgContainer: {
        flex: 0.20,
    },
    dataContainer: {
        flex: 0.80,
    },
    image: {
        width: '90%',
        height: '90%',
    },
});

export default BluetoothDevice;