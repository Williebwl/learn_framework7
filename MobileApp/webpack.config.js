var webpack = require('webpack');
module.exports = {

  entry: [
      "./app/index.jsx"
  ],

  output: {
    path: __dirname + "/build/",
    filename: "bundle.js"
  },

  plugins: [/*
      new webpack.DefinePlugin({
          'process.env.NODE_ENV': '"production"'
      }),
      new webpack.optimize.UglifyJsPlugin({
        compress: {
          warnings: false
        }
      })*/
  ],

  module: {
    loaders: [
        {
          test: /\.jsx?$/,
          exclude: /(node_modules|bower_components)/,
          loader: 'babel',
          query: {
              presets: ['react', 'es2015', 'stage-1', 'stage-3']
          }
        }
    ]
  }
}