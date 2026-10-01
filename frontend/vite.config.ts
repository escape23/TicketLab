import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vueDevTools from 'vite-plugin-vue-devtools'

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    vue(),
    vueDevTools(),
  ],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url)),
    },
  },
  server: {
    // In development, Vite forwards /api/* to the ASP.NET Core API.
    // The browser only talks to one origin (localhost:5173), so no CORS setup is needed.
    // API_URL can point to another API instance, e.g. in automated tests.
    proxy: {
      '/api': process.env.API_URL ?? 'http://localhost:5032',
    },
  },
})
