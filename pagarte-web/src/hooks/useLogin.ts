import { useState } from "react";
import { type LoginRequest, ApiError } from "../api/generated/";
import { loginUser } from "../services/auth";
import { ApplicationError } from "../utils/errors";
import { isErrorBodyWithMessage } from "../types/typeGuards";

export function useLogin() {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [isSuccess, setIsSuccess] = useState(false);

  // The main function the component will call
  const execute = async (data: LoginRequest) => {
    setIsLoading(true);
    setError(null);
    setIsSuccess(false);

    try {
      // Call the service
      const response = await loginUser(data);

      // On success, extract the token from the data payload.
      const token = response.data;

      if (!token) {
        throw new ApplicationError(
          "Login was successful, but no authentication token was provided."
        );
      }

      // Save the token.
      localStorage.setItem("token", token);

      // Signal success to the component.
      setIsSuccess(true);
    } catch (err: unknown) {
      // 5. Handle all possible errors in one place.
      if (err instanceof ApplicationError) {
        setError(err.message);
      } else if (err instanceof ApiError) {
        if (isErrorBodyWithMessage(err.body)) {
          setError(err.body.message);
        } else {
          setError("An unexpected server error occurred.");
        }
      } else {
        setError("An unexpected error occurred. Please try again.");
      }
    } finally {
      setIsLoading(false);
    }
  };

  return { execute, isLoading, error, isSuccess };
}
