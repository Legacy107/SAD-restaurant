using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
    public class MenuItemCategory
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
