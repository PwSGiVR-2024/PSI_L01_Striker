import * as Location from 'expo-location';

export const checkAndRequestLocationPermission = async () => {
  const { status: existingStatus } = await Location.getForegroundPermissionsAsync();
  let finalStatus = existingStatus;

  if (existingStatus !== 'granted') {
    const { status } = await Location.requestForegroundPermissionsAsync();
    finalStatus = status;
  }

  return finalStatus === 'granted';
};