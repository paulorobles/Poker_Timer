using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class Configuration
    {
        public TimeSpan RoundTime { get; set; }
        public List<Blind> Blinds { get; set; }
        public Configuration()
        {
            Blinds = new List<Blind>();
        }


    }
}


