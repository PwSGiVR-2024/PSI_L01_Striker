import { PermissionsAndroid, Platform } from 'react-native';

export const checkAndRequestBluetoothPermissionAndroid = async () => {
  if (Platform.OS !== 'android') {
    return false;
  }

  let permissions = [
    PermissionsAndroid.PERMISSIONS.BLUETOOTH,
    PermissionsAndroid.PERMISSIONS.BLUETOOTH_ADMIN,
  ];

  if (Platform.Version >= 31) {
    permissions = permissions.concat([
      PermissionsAndroid.PERMISSIONS.BLUETOOTH_SCAN,
      PermissionsAndroid.PERMISSIONS.BLUETOOTH_CONNECT,
      PermissionsAndroid.PERMISSIONS.BLUETOOTH_ADVERTISE,
    ]);
  }

  const filteredPermissions = permissions.filter((perm) => perm);

  try {
    const res = await PermissionsAndroid.requestMultiple(filteredPermissions);

    const allGranted = filteredPermissions.every(
      (permission) => res[permission] === PermissionsAndroid.RESULTS.GRANTED
    );

    return allGranted;
  } catch (error) {
    return false;
  }
};