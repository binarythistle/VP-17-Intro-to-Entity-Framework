using System;

namespace TaskMaster.Model
{
    class Task
    {
        public int Id { get; set; }

        
        public string Name { get; set; }

        public Status Status { get; set; }

        public int StatusId { get; set; }

        public DateTime? DueDate { get; set; }
    }
}
