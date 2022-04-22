
using System.ComponentModel.DataAnnotations;
using ConcordiaLib.Domain;
using Microsoft.EntityFrameworkCore.Storage.Internal;

namespace ConcordiaWebApi.Dtos;

public class PersonDto
{

    [Required]
    public string Name { get; init; }

    [Required] 
    public string Id { get; init; }   

    public PersonDto(Person s)
    {
        Name = s.Name;
        Id = s.Id;
    }

}
    
    

        
    


