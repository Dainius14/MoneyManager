/* eslint-disable @typescript-eslint/no-var-requires */
const os = require('os');

module.exports = {
    'transpileDependencies': [
        'vuetify'
    ],
    configureWebpack: {
        devtool: 'source-map'
    },
    chainWebpack: config => {
        config
            .plugin('fork-ts-checker')
            .tap(args => {
                const totalmem = Math.floor(os.totalmem()/1024/1024); //get OS mem size
                args[0].memoryLimit = totalmem > 2048 ? 2048 : 1024;
                return args;
            });
    },
};
