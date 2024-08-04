import { defineConfig } from 'vite';

export default defineConfig({
    clearScreen: false,
    root: 'src/ui',
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