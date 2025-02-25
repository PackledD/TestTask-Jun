using System;

namespace TestWPF.Models
{
    public class Task
    {
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public Task(string description)
        {
            Description = description;
        }
    }
}