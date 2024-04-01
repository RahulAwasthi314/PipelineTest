using AutoMapper;
using PipelineTest.Models;
using PipelineTest.Models.Dto;

namespace PipelineTest.MappingProfiles
{
    public class TodoProfile : Profile
    {
        public TodoProfile()
        {
            CreateMap<Todo, TodoDto>().ReverseMap();
        }
    }
}
