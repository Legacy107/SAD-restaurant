using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; } = "";
        public bool Available { get; set; } = true;
        public int MenuItemCategoryId { get; set; }
        public MenuItemCategory Category { get; set; } = null!; 
        public ICollection<MenuItemOption> Options { get; set; } = new List<MenuItemOption>();
        public ICollection<MenuItemVariation> Variations { get; set; } = new List<MenuItemVariation>();
    }
}