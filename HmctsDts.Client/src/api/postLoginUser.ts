import { AxiosResponse } from "axios";
import { LoginData } from "../types/loginData";
import { backend } from "./api";

/**
 * Sends a login request to the backend API with the provided login data.
 * 
 * @async
 * @param {LoginData} formData - The login credentials to be sent to the server
 * @returns {Promise<AxiosResponse>} A promise that resolves to the server response
 */
export const postLoginUser = async (formData: LoginData): Promise<AxiosResponse> => {
  const res = await backend.post("/Accounts/login", formData);
  return res;
};
