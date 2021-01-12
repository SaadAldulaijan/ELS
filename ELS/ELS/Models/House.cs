using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELS.Models
{
    public class House
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int HouseNumber { get; set; }
        public string Location { get; set; }
        public int SubDistrictId { get; set; }
    }
}
