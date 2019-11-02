using AutoMapper;
using Dashboard.Dtos.Auth;
using Dashboard.Dtos.Stickers;
using Dashboard.Entitites;
using Dashboard.ViewModels;
using Dashboard.ViewModels.Auth;
using Dashboard.ViewModels.Stickers;

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
                .ForMember(d => d.Password, opts => opts.MapFrom(s => s.Password))
                .ForAllOtherMembers(opts => opts.Ignore());

            CreateMap<AuthRegisterInputModel, AuthRegisterDto>()
                .ForMember(d => d.FirstName, opts => opts.MapFrom(s => s.FirstName))
                .ForMember(d => d.LastName, opts => opts.MapFrom(s => s.LastName))
                .ForMember(d => d.Username, opts => opts.MapFrom(s => s.Username))
                .ForMember(d => d.Password, opts => opts.MapFrom(s => s.Password))
                .ForAllOtherMembers(opts => opts.Ignore());

            CreateMap<AuthLoginInputModel, AuthLoginDto>()
                .ForMember(d => d.Username, opts => opts.MapFrom(s => s.Username))
                .ForMember(d => d.Password, opts => opts.MapFrom(s => s.Password))
                .ForAllOtherMembers(opts => opts.Ignore());

            CreateMap<AuthDto, AuthResultModel>()
                .ForMember(d => d.Message, opts => opts.MapFrom(s => s.Message))
                .ForMember(d => d.Token, opts => opts.MapFrom(s => s.Token))
                .ForAllOtherMembers(opts => opts.Ignore());

            CreateMap<StickerAddInputModel, StickerAddDto>()
                .ForMember(d => d.Text, opts => opts.MapFrom(s => s.Text))
                .ForMember(d => d.HtmlColor, opts => opts.MapFrom(s => s.HtmlColor))
                .ForMember(d => d.X, opts => opts.MapFrom(s => s.X))
                .ForMember(d => d.Y, opts => opts.MapFrom(s => s.Y))
                .ForAllOtherMembers(opts => opts.Ignore());

            CreateMap<StickerAddDto, Sticker>()
                .ForMember(d => d.Text, opts => opts.MapFrom(s => s.Text))
                .ForMember(d => d.HtmlColor, opts => opts.MapFrom(s => s.HtmlColor))
                .ForMember(d => d.X, opts => opts.MapFrom(s => s.X))
                .ForMember(d => d.Y, opts => opts.MapFrom(s => s.Y))
                .ForAllOtherMembers(opts => opts.Ignore());

            CreateMap<Sticker, StickerDto>()
                .ForMember(d => d.Text, opts => opts.MapFrom(s => s.Text))
                .ForMember(d => d.HtmlColor, opts => opts.MapFrom(s => s.HtmlColor))
                .ForMember(d => d.X, opts => opts.MapFrom(s => s.X))
                .ForMember(d => d.Y, opts => opts.MapFrom(s => s.Y))
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
                .ForAllOtherMembers(opts => opts.Ignore());

            CreateMap<StickerDto, StickerResultModel>()
                .ForMember(d => d.Text, opts => opts.MapFrom(s => s.Text))
                .ForMember(d => d.HtmlColor, opts => opts.MapFrom(s => s.HtmlColor))
                .ForMember(d => d.X, opts => opts.MapFrom(s => s.X))
                .ForMember(d => d.Y, opts => opts.MapFrom(s => s.Y))
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
                .ForAllOtherMembers(opts => opts.Ignore());

            CreateMap<StickerUpdateInputModel, StickerUpdateDto>()
                //.ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id)) automapper bug with props starting with 'ID' mapping.
                .ForMember(d => d.Text, opts => opts.MapFrom(s => s.Text))
                .ForMember(d => d.HtmlColor, opts => opts.MapFrom(s => s.HtmlColor))
                .ForMember(d => d.X, opts => opts.MapFrom(s => s.X))
                .ForMember(d => d.Y, opts => opts.MapFrom(s => s.Y))
                .ForMember(d => d.ItemId, opts => opts.MapFrom(s => s.ItemId))
                .ForAllOtherMembers(opts => opts.Ignore());

            CreateMap<StickerUpdateDto, Sticker>()
                .ForMember(d => d.Text, opts => opts.MapFrom(s => s.Text))
                .ForMember(d => d.HtmlColor, opts => opts.MapFrom(s => s.HtmlColor))
                .ForMember(d => d.X, opts => opts.MapFrom(s => s.X))
                .ForMember(d => d.Y, opts => opts.MapFrom(s => s.Y))
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.ItemId))
                .ForAllOtherMembers(opts => opts.Ignore());
        }
    }
}
