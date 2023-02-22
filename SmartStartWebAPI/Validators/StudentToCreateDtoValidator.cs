using FluentValidation;
using SmartStartWebAPI.DTOs;

namespace SmartStartWebAPI.Validators
{
   
        public class StudentToCreateDtoValidator : AbstractValidator<StudentToCreateDto>
        {
            public StudentToCreateDtoValidator()
            {
                RuleFor(x => x.Email).EmailAddress();
            }
        }

    }

