import { LoginData } from "../types/loginData";
import { backend } from "./api";

export const postLoginUser = async (formData: LoginData) => {
  const res = await backend.post("/Accounts/login", formData);
  return res;
};
