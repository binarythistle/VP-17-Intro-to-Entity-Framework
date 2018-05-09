using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMaster.Model
{
    class Status
    {
        public int Id { get; set; }
        public string Name { get; set; }

        
        public override string ToString()
        {
            return Name;
        }
        
    }
}
