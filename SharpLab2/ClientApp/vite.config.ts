import { defineConfig } from 'vite';

export default defineConfig({
  build: {
    outDir: '../wwwroot',
    emptyOutDir: true,
    rollupOptions: {
      output: {
        entryFileNames: 'app.js',
        assetFileNames: '[name][extname]'
      }
    }
  },
  server: {
    proxy: {
      '/api': 'http://localhost:5080'
    }
  }
});
