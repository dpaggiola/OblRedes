using System;
using System.Collections.Generic;

namespace Domain
{
    public class Photo
    {
        public virtual List<Comment> Comments { get; set; }

        public Photo()
        {
            CreationDate = DateTime.Now;
            Comments = new List<Comment>();
        }

        public DateTime CreationDate { get; set; }
        public string Name { get; set; }
    }
}