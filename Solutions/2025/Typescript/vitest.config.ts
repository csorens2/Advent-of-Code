import { defineConfig } from 'vitest/config'

export default defineConfig({
    test: {
        environment: 'node',
        coverage: {
            provider: 'v8', // or 'istanbul'
            reporter: ['text', 'json', 'html'],
        },
    },
})