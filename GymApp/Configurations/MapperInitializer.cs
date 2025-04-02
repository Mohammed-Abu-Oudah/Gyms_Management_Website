using AutoMapper;
using GymApp.Credentials;
using GymApp.Data;
using GymApp.Models;

namespace GymApp.Configurations
{
    // In this class we are going to initialize the behaviour of the automapper.
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, CreateUserDTO>().ReverseMap();
            CreateMap<User, UpdateUserDTO>().ReverseMap();
            //CreateMap<ApiUser, UserDTO>().ReverseMap();
            //CreateMap<ApiUser, CreateUserDTO>().ReverseMap();
            //CreateMap<UserDTO, CreateUserDTO>().ReverseMap();
            CreateMap<Gym, GymDTO>().ReverseMap();
            CreateMap<Gym, CreateGymDTO>().ReverseMap();
            CreateMap<TrainingPlan, TrainingPlanDTO>().ReverseMap();
            CreateMap<TrainingPlan, CreateTrainingPlanDTO>().ReverseMap();
            CreateMap<TrainingPlan, GetTrainingPlanDTO>().ReverseMap();
        }
    }
}
