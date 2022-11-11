using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiLibrary.Models
{
    public abstract class BaseModel
    {
        public int ID { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Active { get; set; } = true;
        public String CarType { get; set; }
        public int  AmountSold { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
