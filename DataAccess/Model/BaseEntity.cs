using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Model
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}
