using System.ComponentModel.DataAnnotations;

namespace ConcordiaWebApi.Options
{
    public class WebApiOptions
    {
        //Cards on this list will be considered completed
        [Required] public string CompletedListId { get; set; } = null!;
    }
}
