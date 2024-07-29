import { defineConfig } from 'vite';

export default defineConfig({
    clearScreen: false,
    root: 'src',
    server: {
        watch: {
            ignored: [
                "**/*.fs"
            ]
        },
        port: 1234,
        strictPort: true,
        host: '0.0.0.0'
    }
});