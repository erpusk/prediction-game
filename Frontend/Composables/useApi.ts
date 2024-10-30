import type { NitroFetchOptions, NitroFetchRequest } from "nitropack";
import { ErrorCodes } from "vue";
import { errorMessages } from "vue/compiler-sfc";

export const useApi = () => {
  const runtimeConfig = useRuntimeConfig();

  const customFetch = async <T>(
    url: string,
    options?: NitroFetchOptions<NitroFetchRequest>
  ) => {
    //try {
    const token = localStorage.getItem('token');

    return await $fetch<T>(url, {
      baseURL: runtimeConfig.public.predictionGamesApiUrl,
      headers: {
        ...options?.headers,
        ...(token && { Authorization: `Bearer ${token}` }),
      },
      ...options,
    });
  /*} catch (error: any){
    var status = error.status
    return { error, status};
  }*/
  };
  return { customFetch };
};
