using FluentValidation;
using GymApp.ContextsAndFlunetAPIs.CustomeValidators;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GymApp.Data
{
    public class User : IdentityUser
    {
        public string UserFirstName { get; set; }
        public string UserMiddleName { get; set; }
        public string UserLastName { get; set; }

        //[Required, MaxLength(70)]
        public string DisplayName { get; set; }
        public string IdNumber { get; set; }
        public string ResidenceCountry { get; set; }
        public string ResidenceCity { get; set; }
        public string ResidenceStreet { get; set; }

        //[Required, MaxLength(200)]
        public string FullAddress { get; set; }

        public TrainingPlan? TrainingPlan { get; set; }

        //[RequiredIfAnotherExists(nameof(UserType), ErrorMessage = "Gym id has to be provided")]
        public Guid? GymId { get; set; }
        public Gym? Gym { get; set; }


    }

}
