﻿﻿const path = require('path');
const ManifestPlugin = require('webpack-manifest-plugin');

module.exports = {
	entry: './PrototypeWithAuth/wwwroot/ReactViews/expose-components.js',
	output: {
		filename: '[name].js',
		globalObject: 'this',
		path: path.resolve(__dirname, 'PrototypeWithAuth/wwwroot/dist'),
		publicPath: '/dist/'
	},
	mode: process.env.NODE_ENV === 'production' ? 'production' : 'development',
	optimization: {
		runtimeChunk: {
			name: 'runtime', // necessary when using multiple entrypoints on the same page
		},
		splitChunks: {
			cacheGroups: {
				commons: {
					test: /[\\/]node_modules[\\/](react|react-dom)[\\/]/,
					name: 'vendor',
					chunks: 'all',
				},
			},
		},
	},
	module: {
		rules: [
			{
				test: /\.jsx?$/,
				exclude: /node_modules/,
				loader: 'babel-loader',
			},
				
			{
  				test: /\.css$/,
				use:[
				{ loader: 'style-loader', options: {  }},
				{
					loader:'css-loader',
					options: {
							url: false,
					}
				}
				]
			},

				 {
					test: /\.scss$/,
					use: ['style-loader',  'sass-loader'],
				 },
		],
	},

	plugins: [

		new ManifestPlugin({
			fileName: 'asset-manifest.json',
			generate: (seed, files) => {
				const manifestFiles = files.reduce((manifest, file) => {
					manifest[file.name] = file.path;
					return manifest;
				}, seed);

				const entrypointFiles = files.filter(x => x.isInitial && !x.name.endsWith('.map')).map(x => x.path);

				return {
					files: manifestFiles,
					entrypoints: entrypointFiles,
				};
			},
		}),
	],
	 resolve: {
		 alias: {
			 react: path.resolve('./node_modules/react'),
			 'react-dom': path.resolve('./node_modules/react-dom'),
		},
	}
};