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
            //Usuário
            CreateMap<User, UserModel>();
            CreateMap<UserModel, User>();

            CreateMap<List<User>, List<UserModel>>();
            CreateMap<List<UserModel>, List<User>>();

            //Projeto
            CreateMap<Project, ProjectModel>();
            CreateMap<ProjectModel, Project>();

            CreateMap<List<Project>, List<ProjectModel>>();
            CreateMap<List<ProjectModel>, List<Project>>();

            //Tarefa
            CreateMap<Task, TaskModel>();
            CreateMap<TaskModel, Task>();

            CreateMap<List<Task>, List<TaskModel>>();
            CreateMap<List<TaskModel>, List<Task>>();

            //Mensagem
            CreateMap<Message, MessageModel>();
            CreateMap<MessageModel, Message>();

            CreateMap<List<Message>, List<MessageModel>>();
            CreateMap<List<MessageModel>, List<Message>>();
        }
    }
}
