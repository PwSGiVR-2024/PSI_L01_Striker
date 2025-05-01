import { Buffer } from 'buffer';

export function decodeBase64(base64Value) {
  const buffer = Buffer.from(base64Value, 'base64');
  return new Uint8Array(buffer);
}

export function parseHeartRate(base64Value) {
  const bytes = decodeBase64(base64Value);
  if (bytes.length < 2) {
    heartRate = 0;
  }

  const flags = bytes[0];
  const hrFormatUint16 = (flags & 0x01) === 0x01;

  let heartRate;
  if (hrFormatUint16) {
    heartRate = bytes[1] | (bytes[2] << 8);
  } else {
    heartRate = bytes[1];
  }

  return heartRate;
}