using FluentValidation;
using onion_architecture.Application.Common.Regex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onion_architecture.Application.Features.Dto.UserDto.Validator
{
    public class CreateUserValidator : AbstractValidator<CreateUser>
    {
        public CreateUserValidator()
        {

            RuleFor(x=>x.FullName).NotEmpty().WithMessage("Vui lòng nhập họ và tên");
            RuleFor(x=>x.Address).NotEmpty().WithMessage("Vui lòng nhập địa chỉ");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Vui lòng nhập email");
                //.Matches(RegexValidator.EMAIL).WithMessage("Vui lòng nhập email");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Vui lòng nhập số điện thoại")
                //.Matches(RegexValidator.PHONE).WithMessage("Số điện thoại không đúng định dạng")
                .MinimumLength(10).WithMessage("Số điện thoại không được dưới 10 số").
                MaximumLength(11).WithMessage("Số điện thoại không được quá 10 số");
        }
    }
}
