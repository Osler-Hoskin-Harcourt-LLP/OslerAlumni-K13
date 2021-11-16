module.exports = {
    parser: 'postcss-scss',
    plugins: {
        'postcss-easy-import': {},
        'autoprefixer': { browsers: ['last 2 versions', 'ie 10'] }
    }
}
