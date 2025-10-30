import axios from "axios";
import type {
  AxiosError,
  AxiosInstance,
  InternalAxiosRequestConfig,
} from "axios";
import type { ApiError } from "../types/property.types";

const API_URL = import.meta.env.VITE_API_URL || "http://localhost:5000";
const API_BASE_PATH = import.meta.env.VITE_API_BASE_PATH || "/api";

const apiClient: AxiosInstance = axios.create({
  baseURL: `${API_URL}${API_BASE_PATH}`,
  headers: {
    "Content-Type": "application/json",
  },
  timeout: 30000,
});

apiClient.interceptors.request.use(
  (config: InternalAxiosRequestConfig) => {
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

apiClient.interceptors.response.use(
  (response) => {
    if (import.meta.env.DEV) {
      console.log("✅ API Response:", {
        status: response.status,
        url: response.config.url,
        data: response.data,
      });
    }
    return response;
  },
  (error: AxiosError<ApiError>) => {
    if (!error.response) {
      return Promise.reject({
        title: "Network Error",
        status: 0,
        detail:
          "Unable to connect to the server. Please check your internet connection.",
      } as ApiError);
    }

    const apiError: ApiError = error.response.data || {
      title: "API Error",
      status: error.response.status,
      detail: error.message,
    };

    return Promise.reject(apiError);
  }
);

export default apiClient;

export function buildQueryString(params: Record<string, unknown>): string {
  const queryParams = new URLSearchParams();

  Object.entries(params).forEach(([key, value]) => {
    if (value !== undefined && value !== null && value !== "") {
      queryParams.append(key, String(value));
    }
  });

  const queryString = queryParams.toString();
  return queryString ? `?${queryString}` : "";
}
