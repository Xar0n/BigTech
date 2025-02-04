using BigTech.Domain.Result;

namespace BigTech.Domain.Interfaces.Validations;
public interface IBaseValidator<in T> 
    where T : class
{
    BaseResult ValidateOnNull(T model);
}
