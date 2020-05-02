module.exports = {
    root: true,
    env: {
        node: true
    },
    'extends': [
        'plugin:vue/essential',
        'eslint:recommended',
        '@vue/typescript/recommended',
        '@vue/typescript'
    ],
    parserOptions: {
        ecmaVersion: 2020,
        parser: '@typescript-eslint/parser'
    },
    rules: {
        'no-console': process.env.NODE_ENV === 'production' ? 'warn' : 'off',
        'no-debugger': process.env.NODE_ENV === 'production' ? 'error' : 'off',
        'no-unreachable': process.env.NODE_ENV === 'production' ? 'error' : 'warn',
        'curly': ['warn', 'all'],
        'semi': ['warn', 'always'],
        'quotes': ['warn', 'single', { avoidEscape: true }],
        '@typescript-eslint/no-inferrable-types': 'off',
        '@typescript-eslint/no-non-null-assertion': 'off',
        '@typescript-eslint/no-explicit-any': 'off',
        'no-unused-vars': ['error', { 'varsIgnorePattern': '_\.*' }]
    }
};
