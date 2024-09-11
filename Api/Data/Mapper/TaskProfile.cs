using Api.Data.DTO;
using Api.Data.Model;
using AutoMapper;

namespace Api.Data.Mapper
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<Tasks, TaskDto>();
            CreateMap<TaskDto, Tasks>();
            CreateMap<CreateTaskDto, Tasks>();
        }
    }
}
