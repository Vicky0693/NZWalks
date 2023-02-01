using FluentValidation;

namespace NZWalks.API.Validators
{
    public class LoginRequestValidators : AbstractValidator<Model.DTO.LoginRequest>
    {
        public LoginRequestValidators() 
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();


        }
    }
   
}
