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
    },
    build: {
        rollupOptions: {
            onwarn(warning, warn) {
                if (warning.code === "MODULE_LEVEL_DIRECTIVE")
                    return;

                warn(warning);
            }
        }
    }
});