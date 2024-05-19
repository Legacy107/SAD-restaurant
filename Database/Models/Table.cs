using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
    public class Table
    {
        public int Id { get; set; }
        public string Traits { get; set; } = "";
    }
}

