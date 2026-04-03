using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionSandbox.NeuralNetwork
{
    internal class NN
    {
        NNNode[] inputNodes = { new NNNode(), new NNNode(), new NNNode(), new NNNode(), new NNNode() }; // Input: pozice agenta (2), pozice nejbližšího jídla (2),  energie agenta (1)
        List<NNNode> hiddenNodes = new List<NNNode>();
        NNNode[] outputNodes = new NNNode[12]; // Output: For each MovementType (13)
        List<NNConnection> connections = new List<NNConnection>();

        public NN()
        {
            //Init NN
            for (int i = 0; i < outputNodes.Length; i++)
            {
                int j = 0;
                outputNodes[i] = new NNNode();
                outputNodes[i].bias = Program.GRND.NextDouble();
                connections.Add(new NNConnection(inputNodes[j++], outputNodes[i], Program.GRND.NextDouble()));
                connections.Add(new NNConnection(inputNodes[j++], outputNodes[i], Program.GRND.NextDouble()));
                connections.Add(new NNConnection(inputNodes[j++], outputNodes[i], Program.GRND.NextDouble()));
                connections.Add(new NNConnection(inputNodes[j++], outputNodes[i], Program.GRND.NextDouble()));
                connections.Add(new NNConnection(inputNodes[j++], outputNodes[i], Program.GRND.NextDouble()));
            }
        }
    }
}
