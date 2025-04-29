module.exports = {
  assets: ['./assets/fonts'],
  transformer: {
    babelTransformerPath: require.resolve('react-native-svg-transformer'),
  },
  resolver: {
    assetExts: require('metro-config/src/defaults/defaults').assetExts.filter(
      ext => ext !== 'svg'
    ),
    sourceExts: [...require('metro-config/src/defaults/defaults').sourceExts, 'svg'],
  },
};
