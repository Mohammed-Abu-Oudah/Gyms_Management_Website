using FluentValidation;
using GymApp.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GymApp.Models
{
    // In the DTO, you can add attributes to contorl the returned data.
    // In shorthand, the idea behind the DTO is that the user dosen't see the modules,
    // and the database doesn't see the DTO, what happens is that the user send the data to the DTO
    // and the DTO sends the data to the modules and the modules send the data to the database
    public class UserDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserFirstName { get; set; }
        public string UserMiddleName { get; set; }
        public string UserLastName { get; set; }

        //[Required, MaxLength(70)]
        public string? DisplayName { get; set; }

        //[Required(ErrorMessage ="Id number should be provided.")]
        public string IdNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public string ResidenceCountry { get; set; }
        public string ResidenceCity { get; set; }
        public string ResidenceStreet { get; set; }

        //[Required, MaxLength(200)]
        public string? FullAddress { get; set; }

        public string Email { get; set; }

        public Guid? GymId { get; set; }

        public ICollection<string> Roles { get; set; } // this line will enable us to add which role or roles does this user have.
    }

    // Now for every DTO we need different DTOs for reading and writing.
    // ForExample, in case of adding new user, we don't want to provide the Id since it's auto generated.

    public class CreateUserDTO
    {
        public string UserFirstName { get; set; }
        public string UserMiddleName { get; set; }
        public string UserLastName { get; set; }

        //[Required(ErrorMessage ="Id number should be provided.")]
        public string IdNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public string ResidenceCountry { get; set; }
        public string ResidenceCity { get; set; }
        public string ResidenceStreet { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "Your Password is limited between {2} to {1} characters", MinimumLength = 5)]
        public string Password { get; set; }

        public Guid GymId { get; set; } // we added this, because when we want to creat new use we will have to specify the Gym.
        // And we did it as a DTO because because we need no correlation between the DTO and the domain objects, we want the DTOs to totally not know about the domain objects.

        public ICollection<string> Roles { get; set; } // this line will enable us to add which role or roles does this user have.
    }

    public class LoginUserDTO // This DTO is going to be used for user login and I don't have to let me mapper know about it since I'm just using it as a type.
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Your Password is limited between {2} to {1} characters", MinimumLength = 5)]
        public string Password { get; set; }
    }

    public class UpdateUserDTO
    {
        public string UserFirstName { get; set; }
        public string UserMiddleName { get; set; }
        public string UserLastName { get; set; }

        //[Required(ErrorMessage ="Id number should be provided.")]
        public string IdNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public string ResidenceCountry { get; set; }
        public string ResidenceCity { get; set; }
        public string ResidenceStreet { get; set; }
    }


    public class CreateUserDTOValidator : AbstractValidator<CreateUserDTO> //Here we defined the user because we need to be able to see the properties of the user and not to make the User class connected to this validator, the step of connecting the user will be conducted inside the program file.
    {
        public CreateUserDTOValidator()
        {
            RuleFor(user => user.GymId)
                .NotEmpty()
                .When(user => !user.Roles.Contains("SuperAdmin"));
        }
    }

}
