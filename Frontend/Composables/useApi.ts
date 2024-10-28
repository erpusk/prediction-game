import type { NitroFetchOptions, NitroFetchRequest } from "nitropack";

export const useApi = () => {
  const runtimeConfig = useRuntimeConfig();

  const customFetch = async <T>(
    url: string,
    options?: NitroFetchOptions<NitroFetchRequest>
  ) => {
    const token = localStorage.getItem('token');

    return await $fetch<T>(url, {
      baseURL: runtimeConfig.public.predictionGamesApiUrl,
      headers: {
        ...options?.headers,
        ...(token && { Authorization: `Bearer ${token}` }),
      },
      ...options,
    });
  };
  return { customFetch };
};
