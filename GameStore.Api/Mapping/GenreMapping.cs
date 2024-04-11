using GameStore.Api.Dtos;
using GameStore.Api.Entities;

namespace GameStore.Api;

public static class GenreMapping
{
    public static GenreDto ToDto(this Genre genre)
    {
        return new GenreDto(genre.id, genre.Name);
    }

}
