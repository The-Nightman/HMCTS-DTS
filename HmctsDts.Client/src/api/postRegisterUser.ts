import { AxiosResponse } from "axios";
import { RegisterData } from "../types/registerData";
import { backend } from "./api";

/**
 * Registers a new caseworker user in the backend.
 * 
 * @async
 * @param {RegisterData} formData - The form data containing the new user's registration information
 * @returns {Promise<AxiosResponse>} A promise that resolves to the server response
 */
export const postRegisterUser = async (formData: RegisterData): Promise<AxiosResponse> => {
  const res = await backend.post("/Accounts/register-new-caseworker", formData);
  return res;
};
