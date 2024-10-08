// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: "2024-04-03",
  devtools: { enabled: true },
  modules: ["@pinia/nuxt", "@nuxt/ui"],
  imports: { dirs: ["types/*.ts"] },
  runtimeConfig: {
    public: { predictionGamesApiUrl: "http://localhost:5160/api/" },
  },
});

