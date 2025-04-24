import { RegisterData } from "../types/registerData";
import { backend } from "./api";

export const postRegisterUser = async (formData: RegisterData) => {
  const res = await backend.post("/Accounts/register-new-caseworker", formData);
  return res;
};
