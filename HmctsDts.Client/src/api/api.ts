import axios, { AxiosInstance } from "axios";

export const backend: AxiosInstance = axios.create({
  baseURL: import.meta.env.VITE_APP_API_URL,
});
