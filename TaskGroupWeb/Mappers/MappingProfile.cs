using AutoMapper;
using Objetos;
using System.Collections.Generic;
using TaskGroupWeb.Models;

namespace TaskGroupWeb.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<User, UserModel>();
            CreateMap<UserModel, User>();

            CreateMap<List<User>, List<UserModel>>();
            CreateMap<List<UserModel>, List<User>>();

            CreateMap<Project, ProjectModel>();
            CreateMap<ProjectModel, Project>();

            CreateMap<List<Project>, List<ProjectModel>>();
            CreateMap<List<ProjectModel>, List<Project>>();

            CreateMap<Task, TaskModel>();
            CreateMap<TaskModel, Task>();

            CreateMap<List<Task>, List<TaskModel>>();
            CreateMap<List<TaskModel>, List<Task>>();
        }
    }
}
