using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebrisCollector.Models
{
    //SQL table for storing image information
    public class AddPost
    {
        [PrimaryKey, AutoIncrement]
        public string Id { get; set; }
        public string ImageURL { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Material { get; set; }
    }
}
