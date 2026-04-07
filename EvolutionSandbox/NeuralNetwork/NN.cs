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

        List<NNNode[]> Layers = new List<NNNode[]>();
        List<NNConnection> Connections = new List<NNConnection>();

        public NN()
        {
            Layers.Add(new NNNode[5]);
            Layers.Add(new NNNode[13]);

            for (int i = 0; i < Layers[1].Length; i++)
            {
                Layers[1][i].Bias = Program.GRND.NextDouble();
                for (int j = 0; j < Layers[0].Length; j++)
                {
                    Connections.Add(new NNConnection(Layers[1][i], Program.GRND.NextDouble()));
                    Layers[0][j].OutConns.Add(Connections.Last());
                }
            }
        }

        public MovementType Forward(double[] input)
        {
            Debug.Assert(input.Length == Layers[0].Length, "Input to forward pass isn't equal to lenght of inpput layer");
            for (int i = 0; i < Layers[0].Length; i++)
            {
                Layers[0][i].Value = input[i];
            }

            for (int l = 0; l < Layers.Count; l++)
            {
                foreach(NNNode node in Layers[l])
                {
                    foreach(NNConnection conn in node.OutConns)
                    {
                        conn.GToNode.Value = conn.Weight * node.Value;
                    }
                }
                //TODO: Activation function for layer l+1
            }

            double maxVal = double.MinValue;
            int idx = 0;
            int lastIdx = Layers.Count - 1;
            for(int i = 0; i < Layers[lastIdx].Length; i++)
            {
                if (Layers[lastIdx][i].Value > maxVal)
                {
                    idx = i;
                    maxVal = Layers[lastIdx][i].Value;
                }
            }
            return (MovementType)idx;
        }
    }
}
