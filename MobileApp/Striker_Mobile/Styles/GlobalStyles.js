import { StyleSheet } from 'react-native';

export const GlobalStyles = StyleSheet.create({
    flex: {
        flex: 1,
    },
    column: {
        flexDirection: 'column',
    },
    row:{
        flexDirection: 'row',
    },
    text12:{
        fontSize: 12,
        fontFamily: 'Helvetica',
    },
    text13: {
        fontSize: 13,
        fontFamily: 'Helvetica',
    },
    text14: {
        fontSize: 14,
        fontFamily: 'Helvetica',
    },
    text16: {
        fontSize: 16,
        fontFamily: 'Helvetica',
    },
    text18: {
        fontFamily: 'Helvetica',
        fontSize: 18,
    },
    text20: {
        fontFamily: 'Helvetica',
        fontSize: 20,
    },
    text22: {
        fontFamily: 'Helvetica',
        fontSize: 22,
    },
    text24: {
        fontFamily: 'Helvetica',
        fontSize: 24,
    },
    textAchievment: {
        fontFamily: 'Helvetica',
        fontSize: 32,
    },
    text32: {
        fontFamily: 'Helvetica',
        fontSize: 32,
    },
    text48: {
        fontFamily: 'Helvetica',
        fontSize: 48,
    },
    text72:{
        fontFamily: 'Helvetica',
        fontSize: 72,
    },
    bold: {
        fontWeight: '700',
    },
    center: {
        justifyContent: 'center',
        alignItems: 'center',
    },   
    centerLeft: {
        justifyContent: 'center',
        alignItems: 'flex-start', 
        paddingLeft: 10,        
    },
    centeredText: {
        textAlign: 'center',
    },
    white: {
        color: 'whitesmoke',
    },
    floatRight: {
        textAlign: 'right',
    },
    textShadow: {
        textShadowColor: 'rgba(0, 0, 0, 0.3)',
        textShadowOffset: { width: 1, height: 1 },
        textShadowRadius: 2,
    }
});