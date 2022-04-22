using System.ComponentModel.DataAnnotations;

namespace ConcordiaWebApi.Dtos
{
    public record CommentDto(
        [Required]
        string CardId,
        [Required]
        string Text
        )
    {
        //TODO do not hardcode API user id
        public string PersonId => "5f96aaabbc30f60f2e17ba0a";
    }
}