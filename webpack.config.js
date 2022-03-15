﻿const path = require('path');
const ManifestPlugin = require('webpack-manifest-plugin');

module.exports = {
	entry:  './PrototypeWithAuth/wwwroot/ReactViews/expose-components.js',
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
				test: /\.(ts|js)x?$/,
				exclude: /node_modules/,
				loader: 'babel-loader',
				options: {
					presets: [
						'@babel/preset-env',
						['@babel/preset-react', { runtime: 'automatic' }],
						'@babel/preset-typescript',
					],
				},
			},
			{
  				test: /\.css$/,
				use:[
					{ loader: 'style-loader', options: {} },

				{
					loader:'css-loader',
					options: {
						url: false,
					}
					},

				]
			},

				 {
					test: /\.scss$/,
					use: ['style-loader',  'sass-loader'],
			},
			//{
			//	test: /.(png|woff(2)?|eot|ttf|gif)/,
			//	loader: "url-loader",
			//},
			
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
		modules: ['node_modules'],
		 alias: {
			 react: path.resolve('./node_modules/react'),
			'react-dom': path.resolve('./node_modules/react-dom'),
			"@mui/styles": path.resolve('./node_modules/@mui/styles'),
			"@mui/material": path.resolve('./node_modules/@mui/material'),
		},
	}
};