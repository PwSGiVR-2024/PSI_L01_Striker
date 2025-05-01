import { BleManager } from 'react-native-ble-plx';
import { parseHeartRate } from '../Helpers/Base64Decoder';

class BLEService {
  constructor() {
    this.manager = new BleManager();
    this.device = null;
    this.hrSubscription = null;
    this.hrListeners = new Set();
    this.latestHr = null;
  }

  addListener(fn) {
    this.hrListeners.add(fn);
    if (this.latestHr != null) fn(this.latestHr);
    return () => this.hrListeners.delete(fn);
  }

  async connectAndSubscribe(deviceId) {
    this.device = await this.manager.connectToDevice(deviceId);
    await this.device.discoverAllServicesAndCharacteristics();

    const services = await this.device.services();
    const hrService = services.find(s => s.uuid.toLowerCase().includes('180d'));
    if (!hrService) throw new Error('NO HR');

    const chars = await this.device.characteristicsForService(hrService.uuid);
    const hrChar = chars.find(c => c.uuid.toLowerCase().includes('2a37'));
    if (!hrChar) throw new Error('NO HR');

    this.hrSubscription = this.manager.monitorCharacteristicForDevice(
      deviceId,
      hrService.uuid,
      hrChar.uuid,
      (err, characteristic) => {
        if (err) {
          return;
        }
        let bpm;
        try {
          bpm = parseHeartRate(characteristic.value);
        } catch {
          return;
        }
        this.latestHr = bpm;
        this.hrListeners.forEach(fn => fn(bpm));
      }
    );
  }

  disconnect() {
    if (this.hrSubscription) {
      this.hrSubscription.remove();
      this.hrSubscription = null;
    }
    if (this.device) {
      this.manager.cancelDeviceConnection(this.device.id);
      this.device = null;
    }
    this.hrListeners.clear();
    this.latestHr = null;
  }
}

export default new BLEService();