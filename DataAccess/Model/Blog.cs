using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Model
{
    public class Blog : BaseEntity
    {
        public string Name { get; set; }

        public virtual List<Post> Posts { get; set; }
    }
}
