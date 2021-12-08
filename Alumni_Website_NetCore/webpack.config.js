const path = require('path'),
      postCSS = require('./postcss.config.js'),
      MiniCssExtractPlugin = require('mini-css-extract-plugin'),
      CopyWebpackPlugin = require('copy-webpack-plugin'),
      webpack = require('webpack'),
      config = require('./project.config'),
      FixStyleOnlyEntriesPlugin = require("webpack-fix-style-only-entries");


// ------------------------------------
// COMMON PROCESSES TO ALL ENVIRONMENTS
// ------------------------------------

//rules
const rules = [
    { //babel for js
        test: /\.js$/, //files ending with .js
        exclude: [/(node_modules)/, /vendor/],
        use: {
            loader: 'babel-loader'
        }
    },
    {
        test: /\.css$/,
        include: /node_modules/,
        use: [
            MiniCssExtractPlugin.loader,
            'css-loader'
            ]
    },
    {
        test: /\.(eot|svg|ttf|woff(2)?)(\?v=\d+\.\d+\.\d+)?/,
        loader: 'url-loader'
    },
    {
        test: /\.vue$/,
        exclude: [/(node_modules)/, /vendor/],
        use: 'vue-loader'
    }
];

//plugins
const plugins = [
    new FixStyleOnlyEntriesPlugin(),
    new MiniCssExtractPlugin({ // define where to save the compiled css file
        filename: '[name].css',
        chunkFilename: "[id].css"
    }),

    new CopyWebpackPlugin([ // move images from src to build
        { from: config.paths.assets, to: config.paths.assetsBuild }
    ])
];

// PRODUCTION ONLY
if (config.isProd) {

    // rules
    rules.push({
        test: /\.(sass|scss)$/,
        // use : ['MiniCssExtractPlugin.loader', 'css-loader', 'sass-loader'],
        use : [
            MiniCssExtractPlugin.loader,
            {loader: 'css-loader'},
            {loader: 'postcss-loader'},
            {loader: 'sass-loader', options: {outputStyle: 'compressed'}},
        ]
    })

    // plugins
    plugins.push(
        new webpack.optimize.UglifyJsPlugin({
            sourceMap: true,
            compress: {
                warnings: false,
                conditionals: true,
                unused: true,
                comparisons: true,
                sequences: true,
                dead_code: true,
                evaluate: true,
                if_return: true,
                join_vars: true,
            },
            output: {
                comments: false,
            },
            ie8: false,
        }),
    );
// DEVELOPMENT ONLY
} else {
    // rules
    rules.push({
        test: /\.(sass|scss)$/,
        use: [
            MiniCssExtractPlugin.loader,
            {
                loader: 'css-loader',
                options: { sourceMap: true }
            },
            {
                loader: 'postcss-loader',
                options: { sourceMap: true }
            },
            {
                loader: 'sass-loader',
                options: { // dev only b/c it's faster
                    sourceMap: true,
                    outputStyle: 'compact' //necessary to use with sourcemap because of bug https://github.com/sass/libsass/issues/2312
                }
            },
        ]
    })


    // plugins
    plugins.push(
        function () {
            this.plugin('watch-run', function (watching, callback) {
                console.log('Begin compile at ' + new Date());
                callback();
            })
        }
    )
}

//Using MiniCSSExtrXtPlugin to split css files for site and ckeditor https://webpack.js.org/plugins/mini-css-extract-plugin/#extracting-css-based-on-entry
function recursiveIssuer(m) {
    if (m.issuer) {
      return recursiveIssuer(m.issuer);
    } else if (m.name) {
      return m.name;
    } else {
      return false;
    }
  }

const globalConfig = {
    entry: {
        app: config.paths.js,
        picturefill: config.paths.picturefill,
        style: config.paths.scss,
        adminStyles: config.paths.admin_scss

    },
	optimization: {
		splitChunks: {
			cacheGroups: {
				standaloneStyles: {
					name: 'style',
					test: (m,c,entry = 'style') => m.constructor.name === 'CssModule' && recursiveIssuer(m) === entry,
					chunks: 'all',
					enforce: true
				},
				whitelabelStyles: {
					name: 'adminStyles',
					test: (m,c,entry = 'adminStyles') => m.constructor.name === 'CssModule' && recursiveIssuer(m) === entry,
					chunks: 'all',
					enforce: true
				}
			}
		}
	},
    resolve: {
        extensions: ['.js', '.json', '.scss'],
        alias: {
            'vue$': process.env.NODE_ENV === 'production' ? 'vue/dist/vue.min.js' : 'vue/dist/vue.esm.js'
        }
    },
    output: {
        filename: '[name].js',
        path: config.paths.build,
    },
    module: {
        rules
    },
    plugins,
    devtool: process.env.NODE_ENV === 'production' ? "source-map" : "inline-source-map"
};

module.exports = globalConfig;
