﻿using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace PipelineTest.Models
{
    public class Todo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public bool IsCompleted { get; set; } = false;
    }
}
