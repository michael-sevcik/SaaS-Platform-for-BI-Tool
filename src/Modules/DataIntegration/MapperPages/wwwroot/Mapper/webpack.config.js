const path = require('path');

module.exports = {
    resolve: {
        extensions: ['.ts', '.tsx', '.js'],
        fallback: {
            "assert": require.resolve("assert/")
        }
    },
    // target: 'es2020',
    entry: './src/index.ts',
    output: {
        filename: 'bundle.js',
        path: path.resolve(__dirname, 'dist'),
        publicPath: '/dist/'
    },
    mode: 'development',
    module: {
        rules: [
            { test: /\.ts?$/, loader: 'ts-loader' },
            {
                test: /\.css$/,
                use: ['style-loader', 'css-loader']
            }
        ]
    },
    devServer: {
        static: {
            directory: __dirname,
        },
        compress: true
    },
};
