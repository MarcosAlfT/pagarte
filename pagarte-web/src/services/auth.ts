import {
  AuthService,
  type LoginRequest,
  type RegisterUserRequest,
  type ApiResponseOfstring,
} from "../api/generated/";
import { ApplicationError } from "../utils/errors";

export async function registerUser(data: RegisterUserRequest): Promise<void> {
  const response = await AuthService.postApiAuthRegister(data);
  if (!response.success) {
    throw new ApplicationError(response.message || "Registration failed.");
  }
}

export async function loginUser(
  data: LoginRequest
): Promise<ApiResponseOfstring> {
  const response = await AuthService.postApiAuthLogin(data);
  if (!response.success) {
    throw new ApplicationError(response.message || "Login failed.");
  }
  return response;
}
