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
        NNNode[] InputNodes = { new NNNode(), new NNNode(), new NNNode(), new NNNode(), new NNNode() }; // Input: pozice agenta (2), pozice nejbližšího jídla (2),  energie agenta (1)
        List<NNNode> HiddenNodes = new List<NNNode>();
        NNNode[] OutputNodes = new NNNode[13]; // Output: For each MovementType (13)
        List<NNConnection> Connections = new List<NNConnection>();

        public NN()
        {
            //Init NN
            for (int i = 0; i < OutputNodes.Length; i++)
            {
                OutputNodes[i] = new NNNode();
                OutputNodes[i].bias = Program.GRND.NextDouble();
                for (int j = 0; j < InputNodes.Length; j++)
                {
                    Connections.Add(new NNConnection(InputNodes[j], OutputNodes[i], Program.GRND.NextDouble()));
                }
            }
        }

        public MovementType Forward(double[] input)
        {
            Debug.Assert(input.Length == InputNodes.Length, "NN Forward: input size doesn't match");
            for (int i = 0; i < InputNodes.Length; i++)
            {
                InputNodes[i].value = input[i];
            }

            //Set node valuse do bias so we don't need to care about adding it later
            foreach (NNNode node in HiddenNodes)
            {
                node.value = node.bias;
            }
            foreach (NNNode node in OutputNodes)
            {
                node.value = node.bias;
            }

            List<NNConnection> connections = Connections.FindAll(x => InputNodes.Contains(x.GFromNode));
            Debug.Assert(connections.Count == (InputNodes.Length * OutputNodes.Length), "There is incorrect num of connections from intput nodes");
            int maxItters = 100; // That should be enough to detect the cycle in our small NN
            bool bIsCycleInNN = true; // if the forward pass needs all maxItters itterations there is propably infinite cycle in the NN, so we will then penalize (destroy) agent with this NN.
            for (int i = 0;i <= maxItters; i++)
            {
                if(connections.Count == 0)
                {
                    bIsCycleInNN=false;
                    break;
                }

                foreach (NNConnection connection in connections.ToArray())
                {
                    connection.GToNode.value += connection.GFromNode.value * connection.Weight;
                    connections.Remove(connection);
                    connections.AddRange(Connections.FindAll(x => x.GFromNode == connection.GToNode));
                }

                connections = connections.Distinct().ToList(); // Remove duplicates
            }
            if (bIsCycleInNN)
            {
                //TODO: Agent penalization
                return MovementType.NoMove;
            }
            else
            {
                double maxValue = double.MinValue;
                int maxIndex = 0;
                for (int i = 0; i < OutputNodes.Length; i++)
                {
                    if(maxValue < OutputNodes[i].value)
                    {
                        maxValue = OutputNodes[i].value;
                        maxIndex = i;
                    }
                }
                return (MovementType)maxIndex;
            }
        }
    }
}
