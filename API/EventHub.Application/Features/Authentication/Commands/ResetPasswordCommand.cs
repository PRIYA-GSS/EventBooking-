public record ResetPasswordCommand(
    string Email,
    string Token,
    string NewPassword);