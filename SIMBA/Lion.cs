using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMBA
{
    class Lion
    {
        private List<string> sequences;
        private double[] fitness;

        public Lion() { }

        public Lion(List<string> sequences, double[] fitness)
        {
            this.sequences = sequences;
            this.fitness = fitness;
        }

        public List<string> getSequences()
        { return sequences; }

        public void setSequences(List<string> sequences)
        { this.sequences = sequences; }

        public double[] getFitness()
        { return fitness; }

        public void setFitness(double[] fitness)
        { this.fitness = fitness; }
                
    }
}
