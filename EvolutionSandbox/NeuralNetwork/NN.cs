using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionSandbox.NeuralNetwork
{
    internal class NN // TODO: Move as much variables to arrays and limit list and other simulat var types to optimaliaze garbage collenction
    {
        // Input: pozice agenta (2), pozice nejbližšího jídla (2),  energie agenta (1)
        // Output: For each MovementType (13)

        List<NNNode>[] nodes = new List<NNNode>[2];
        List<NNConnection> connections = new List<NNConnection>();

        public NN()
        {
        }

        public MovementType Forward(double[] input)
        {
            
        }
    }
}
