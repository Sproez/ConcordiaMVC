using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ConcordiaTrelloClient.Options
{
    public class ApiOptions
    {
        [Required]
        public string BaseURL { get; set; } = "https://api.trello.com/1";
        [Required]
        public string ConcordiaBoardID { get; set; } = "623346a8d1da132a04deb1fd";
        [Required]
        public string ApiKey { get; set; } = "3717507a5e28c9d3d98b02d3d2fb94b7";
        [Required]
        public string ApiToken { get; set; } = "22e318ffff021abf2f4e3ad1bc212b47182a8a6e2544d08f0c4b76f2393ef914";
    }
}