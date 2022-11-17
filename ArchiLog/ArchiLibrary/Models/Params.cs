using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiLibrary.Models
{
    public class Params
    {
        public string? Asc { get; set; }
        public string? Desc { get; set; }
        public string? Range { get; set; }
        public string? Type { get; set; }
        public int? Sold { get; set; }
        public string? CreatedAt { get; set; }
        public string? Fields { get; set; }
    }
}
