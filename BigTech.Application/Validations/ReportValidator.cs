using BigTech.Application.Resources;
using BigTech.Domain.Entity;
using BigTech.Domain.Enum;
using BigTech.Domain.Interfaces.Validations;
using BigTech.Domain.Result;

namespace BigTech.Application.Validations;
public class ReportValidator : IReportValidator
{
    public BaseResult CreateValidator(Report report, User user)
    {
        if (report != null)
        {
            return new BaseResult()
            {
                ErrorMessage = ErrorMessage.ReportAlreadyExists,
                ErrorCode = (int)ErrorCodes.ReportAlreadyExists,
            };
        }

        if (user == null)
        {
            return new BaseResult()
            {
                ErrorMessage = ErrorMessage.UserNotFound,
                ErrorCode = (int)ErrorCodes.UserNotFound,
            };
        }
        return new BaseResult();
    }

    public BaseResult ValidateOnNull(Report model)
    {
        if (model == null)
        {
            return new BaseResult()
            {
                ErrorMessage = ErrorMessage.ReportNotFound,
                ErrorCode = (int)ErrorCodes.ReportNotFound
            };
        }
        return new BaseResult();
    }
}
