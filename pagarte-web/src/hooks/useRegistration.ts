import { useState } from "react";
import { type RegisterUserRequest, ApiError } from "../api/generated/";
import { registerUser } from "../services/auth";
import { ApplicationError } from "../utils/errors";
import { isErrorBodyWithMessage } from "../types/typeGuards";

export function useRegistration() {
  // State managed entirely within the hook
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [isSuccess, setIsSuccess] = useState(false);

  // The main function the component will call
  const execute = async (data: RegisterUserRequest) => {
    // Reset state for a new attempt
    setIsLoading(true);
    setError(null);
    setIsSuccess(false);

    try {
      // The service either succeeds or throws.
      await registerUser(data);
      setIsSuccess(true); // Set success state if no error was thrown
    } catch (error: unknown) {
      // This is the single, clean place for all error handling
      if (error instanceof ApplicationError) {
        setError(error.message);
      } else if (error instanceof ApiError) {
        if (isErrorBodyWithMessage(error.body)) {
          const serverMessage = error.body.message;
          setError(serverMessage);
        }
      } else {
        setError("An unexpected error occurred. Please try again.");
      }
    } finally {
      // Ensure loading is always turned off
      setIsLoading(false);
    }
  };

  // Expose the state and the function to the component
  return { execute, isLoading, error, isSuccess };
}
