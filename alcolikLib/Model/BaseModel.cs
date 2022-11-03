using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alcolikLib.Model
{
    public abstract class BaseModel
    {
        public int Id { get; set; }

        public string? Name { get; set; } 

        public DateTime CreatedAt { get; set; }

        public bool Active { get; set; } = true;

        public DateTime? UpdatedAt{ get; set; }

        public DateTime? DeletedAt { get; set; }

    }
}
