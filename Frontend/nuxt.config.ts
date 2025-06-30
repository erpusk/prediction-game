export default defineNuxtConfig({
  css: ['@fortawesome/fontawesome-free/css/all.css'],
  compatibilityDate: "2024-04-03",
  devtools: { enabled: true },
  modules: ["@pinia/nuxt", "@nuxt/ui"],
  imports: { dirs: ["types/*.ts"] },
  runtimeConfig: {
    public: { predictionGamesApiUrl: process.env.PREDICTION_GAMES_API_URL || 'http://localhost:5160/api' },
  },
  app: {
    head: {
      link: [
        {
          rel: "stylesheet",
          href: "https://fonts.googleapis.com/css2?family=Roboto:wght@400;700&display=swap",
        },
      ],
    },
  },
});


