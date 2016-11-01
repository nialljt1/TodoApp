﻿using System;

namespace TodoApp.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
    }
}