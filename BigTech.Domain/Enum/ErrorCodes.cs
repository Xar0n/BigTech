namespace BigTech.Domain.Enum;
public enum ErrorCodes
{
    // 0 - 10 - Report
    ReportsNotFound = 0,
    ReportNotFound = 1,
    ReportAlreadyExists = 2,
    // 11 - 20 - User
    UserNotFound = 11,
    UserAlreadyExists = 12,
    UnauthorizedAccessException = 13,

    InternalServerError = 10,

    PaswordNotEqulsPasswordConfirm = 21,
    PasswordIsWrong = 22,
}
