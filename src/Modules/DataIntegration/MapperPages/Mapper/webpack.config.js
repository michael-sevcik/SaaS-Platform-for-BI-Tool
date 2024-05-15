const path = require('path');
const CopyPlugin = require("copy-webpack-plugin");

const distPath = process.env.NODE_ENV_PATH
    ? path.resolve(__dirname, process.env.NODE_ENV_PATH)
    : path.resolve(__dirname, 'dist');

module.exports = {
    plugins: [
        new CopyPlugin({
            patterns: [
                { from: "./src/style.css", to: distPath },
            ],
        }),

     ],
    resolve: {
        extensions: ['.ts', '.tsx', '.js'],
        fallback: {
            "assert": require.resolve("assert/")
        }
    },
    // target: 'es2020',
    entry: './src/index.ts',
    experiments: {
        outputModule: true,
    },
    output: {
        filename: 'bundle.js',
        path: distPath,
        publicPath: './dist/',
        library: {
            type: 'module'
        },
    },
    devtool: 'eval-source-map', 
    mode: 'development',
    module: {
        rules: [
            { test: /\.ts?$/, loader: 'ts-loader' },
        ]
    },
    devServer: {
        static: {
            directory: __dirname,
        },
        compress: true
    },
};
