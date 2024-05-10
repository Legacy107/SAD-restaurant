using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }

        public MenuItem(string name, string description, float price)
        {
            Name = name;
            Description = description;
            Price = price;
        }
    }
}

