using System;
namespace ArchiLibrary.Models
{
    public class Params
    {
        public string? Asc { get; set; }
        public string? Desc { get; set; }
        public string? Range { get; set; }
        public string? Type { get; set; }
        public string? Sold { get; set;}
        public string? CreatedAt { get; set;}
        public string? dateBetween { get; set; }
        public string? Fields { get; set; }
        public string? SoldBetween { get; set; }
    }

}
