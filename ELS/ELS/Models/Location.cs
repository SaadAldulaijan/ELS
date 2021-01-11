using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELS.Models
{
    public class Location
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string SubName { get; set; }
        public string MapLink { get; set; }
    }
}
