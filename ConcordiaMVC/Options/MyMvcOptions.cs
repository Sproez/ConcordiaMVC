using System.ComponentModel.DataAnnotations;

namespace ConcordiaMVC.Options
{
    public class MyMvcOptions
    {
        //Cards on this list will be considered completed
        [Required] public string CompletedListId { get; set; } = null!;
    }
}
