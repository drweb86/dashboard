using AutoMapper;
using Dashboard.Dtos.Auth;
using Dashboard.Entitites;
using Dashboard.ViewModels.Auth;

namespace Dashboard.Api.Mappings
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<AuthRegisterDto, User>()
                .ForMember(d => d.FirstName, opts => opts.MapFrom(s => s.FirstName))
                .ForMember(d => d.LastName, opts => opts.MapFrom(s => s.LastName))
                .ForMember(d => d.Username, opts => opts.MapFrom(s => s.Username))
                .ForMember(d => d.Password, opts => opts.MapFrom(s => s.Password));

            CreateMap<AuthRegisterInputModel, AuthRegisterDto>()
                .ForMember(d => d.FirstName, opts => opts.MapFrom(s => s.FirstName))
                .ForMember(d => d.LastName, opts => opts.MapFrom(s => s.LastName))
                .ForMember(d => d.Username, opts => opts.MapFrom(s => s.Username))
                .ForMember(d => d.Password, opts => opts.MapFrom(s => s.Password));

            CreateMap<AuthLoginInputModel, AuthLoginDto>()
                .ForMember(d => d.Username, opts => opts.MapFrom(s => s.Username))
                .ForMember(d => d.Password, opts => opts.MapFrom(s => s.Password));

            CreateMap<AuthDto, AuthResultModel>()
                .ForMember(d => d.Message, opts => opts.MapFrom(s => s.Message))
                .ForMember(d => d.Token, opts => opts.MapFrom(s => s.Token));
        }
    }
}
