using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionSandbox.NeuralNetwork
{
    internal class NNNode
    {
        public double Value;
        public double Bias;

        public NNConnection[] OutConns;
    }
}
