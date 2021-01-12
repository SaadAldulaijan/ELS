using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELS.Models
{
    public class District
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
