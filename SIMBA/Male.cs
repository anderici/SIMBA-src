using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMBA
{
    class Male : Lion
    {
        public Male(List<string> sequences, double fitness, char sex) : base(sequences, fitness, sex)
        { }
    }
}
