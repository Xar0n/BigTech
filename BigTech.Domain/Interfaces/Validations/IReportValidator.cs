using BigTech.Domain.Entity;
using BigTech.Domain.Result;

namespace BigTech.Domain.Interfaces.Validations;
public interface IReportValidator : IBaseValidator<Report>
{
    /// <summary>
    /// Проверяется наличие отчета, если отчет с переданным названием есть в БД, то создать точно такой же нельзя
    /// Проверяется пользователь, если с UserId пользователь не найден, то его нет
    /// </summary>
    /// <param name="report"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    BaseResult CreateValidator(Report report, User user);
}
