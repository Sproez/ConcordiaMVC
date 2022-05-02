using System.ComponentModel.DataAnnotations;
using ConcordiaLib.Domain;

namespace ConcordiaWebApi.Dtos;

    public class CardListDto
    {
        [Required]
        public string Id { get; init; }
        [Required]
        public string Name { get; init; }

        public CardListDto(CardList c)
        {
            Id = c.Id;
            Name = c.Name;
        }

    }

