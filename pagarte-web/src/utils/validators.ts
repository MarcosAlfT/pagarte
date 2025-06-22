/**
 * This file contains pure, reusable validation functions for use throughout the application,
 */

/**
 * Validates an email address format.
 * @param email The email string to validate.
 * @returns `true` if the email format is valid, otherwise `false`.
 */
export const validateEmail = (email: string): boolean => {
  const emailRegex = /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i;
  return emailRegex.test(email);
};

/**
 * Validates a password based on a set of security rules.
 * @param password The password string to validate.
 * @returns `true` if the password meets all criteria, otherwise `false`.
 */
export const validatePassword = (password: string): boolean => {
  // --- Define the Password Rules ---
  const hasMinLength = password.length >= 8;
  const hasMaxLength = password.length <= 20;
  const hasUppercase = /[A-Z]/.test(password); // Checks for at least one uppercase letter
  const hasNumber = /[0-9]/.test(password); // Checks for at least one number

  // Checks for at least one special character.
  const hasSpecialChar = /[^a-zA-Z0-9]/.test(password);

  // --- Check All Rules ---
  if (
    hasMinLength &&
    hasMaxLength &&
    hasUppercase &&
    hasNumber &&
    hasSpecialChar
  ) {
    return true;
  }

  return false;
};

/**
 * Validates that a username is between 3 and 15 characters and contains no special characters.
 * @param username The username string to validate.
 * @returns `true` if the username is valid, otherwise `false`.
 */
export const validateUsername = (username: string): boolean => {
  const usernameRegex = /^[a-zA-Z0-9]{3,15}$/;
  return usernameRegex.test(username);
};
