﻿using BigTech.Domain.Dto.Report;
using FluentValidation;

namespace BigTech.Application.Validations.FluentValidations.Report;
public class CreateReportValidator : AbstractValidator<CreateReportDto>
{
    public CreateReportValidator()
    {
        RuleFor(x => x.Name).NotEmpty()
            .MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty()
            .MaximumLength(1000);
    }
}
