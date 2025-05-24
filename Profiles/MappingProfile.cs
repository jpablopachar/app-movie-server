using app_movie_server.Models;
using app_movie_server.Models.Dtos;
using AutoMapper;

namespace app_movie_server.Profiles
{
    /// <summary>
    /// Clase de perfil de mapeo utilizada para configurar las conversiones automáticas entre entidades y objetos de transferencia de datos (DTO) mediante AutoMapper.
    /// </summary>
    /// <remarks>
    /// Esta clase define las reglas de mapeo entre las entidades del dominio y sus correspondientes DTOs, facilitando la conversión bidireccional entre ambos tipos.
    /// Los mapeos configurados incluyen entidades como <see cref="Category"/>, <see cref="Movie"/> y <see cref="AppUser"/>, así como sus respectivos DTOs.
    /// </remarks>
    public class MappingProfile : Profile
    {
        protected MappingProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Movie, MovieDto>().ReverseMap();
            CreateMap<Movie, CreateMovieDto>().ReverseMap();
            CreateMap<Movie, UpdateMovieDto>().ReverseMap();
            CreateMap<AppUser, UserDataDto>().ReverseMap();
            CreateMap<AppUser, UserDto>().ReverseMap();
        }
    }
}