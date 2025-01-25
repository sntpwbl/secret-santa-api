using AutoMapper;
using SecretSanta.Entities;
using SecretSanta.DTO;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Group, GroupDTO>()
            .ForMember(dest => dest.People, opt => opt.MapFrom(src => src.People));

        CreateMap<GroupDTO, Group>();

        CreateMap<GroupCreateDTO, GroupDTO>();
        
        CreateMap<GroupCreateDTO, Group>();

        CreateMap<GroupUpdateDTO, GroupDTO>();

        CreateMap<Person, PersonDTO>();

        CreateMap<PersonCreateDTO, Person>();
    }
}
