export function isErrorBodyWithMessage(
  body: unknown
): body is { message: string } {
  if (typeof body !== "object" || body === null) {
    return false;
  }
  if (!("message" in body)) {
    return false;
  }
  if (typeof (body as { message: unknown }).message !== "string") {
    return false;
  }
  return true;
}
