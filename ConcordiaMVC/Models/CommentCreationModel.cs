using System.ComponentModel.DataAnnotations;

namespace ConcordiaMVC.Models;

public record CommentCreationModel(
        [Required]
        string CardId,
        [Required]
        string Text
        )

{
    //TODO do not hardcode API user id
    public string PersonId => "5f96aaabbc30f60f2e17ba0a";
    //Trello Ids are hexadecimal, so we won't have collisions
    public string Id => $"local-{CardId}";
}


