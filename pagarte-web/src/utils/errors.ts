/**
 * This file defines custom error classes for the application.
 * Using custom errors allows us to handle different failure scenarios
 * more gracefully and explicitly in our try/catch blocks.
 */

/**
 * Represents a business logic error that occurs even when the API request
 * itself was successful (e.g., HTTP 200 OK, but { success: false }).
 *
 * This is used to differentiate application-level failures (like "Email already in use")
 * from network or server failures (like a 404 or 500 error, which are handled by ApiError).
 */
export class ApplicationError extends Error {
  /**
   * Optional field to hold structured validation errors from the API,
   * though your current API does not provide this. It's here for future flexibility.
   */
  public readonly errors?: Record<string, unknown> | null;

  /**
   * Creates an instance of ApplicationError.
   * @param message A human-readable message describing the error.
   * @param errors An optional object containing more detailed, structured errors.
   */
  constructor(message: string, errors?: Record<string, unknown> | null) {
    // Pass the message to the base Error class constructor
    super(message);

    // Set the name of the error, which is a best practice for custom errors.
    // This helps in debugging and allows for identification with `error.name`.
    this.name = "ApplicationError";

    // Store the optional structured errors.
    this.errors = errors;
  }
}
