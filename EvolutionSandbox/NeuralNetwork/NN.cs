using System.Diagnostics;
using System.Reflection.Emit;

namespace EvolutionSandbox.NeuralNetwork
{
    internal class NN
    {
        // Input: pozice agenta (2), pozice nejbližšího jídla (2),  energie agenta (1)
        // Output: For each MovementType (13)

        List<NNNode[]> Layers = new List<NNNode[]>();
        List<NNConnection> Connections = new List<NNConnection>();

        //Mutation chances - percentages in float form
        float WeightMutationChance = Configuration.Config.WeightMutationChance;
        float BiasMutationChance = Configuration.Config.BiasMutationChance;
        float SplitMutationChance = Configuration.Config.SplitMutationChance;
        float NewConnectionMutationChance = Configuration.Config.NewConnectionMutationChance;
        float NewNodeMutationChance = Configuration.Config.NewNodeMutationChance;

        // Mutation size
        float WeightMutationSizeMin = Configuration.Config.WeightMutationSizeMin;
        float WeightMutationSizeMax = Configuration.Config.WeightMutationSizeMax;
        float BiasMutationSizeMin = Configuration.Config.BiasMutationSizeMin;
        float BiasMutationSizeMax = Configuration.Config.BiasMutationSizeMax;

        public NN(int inputLayerSize, int outputLayerSize, bool inicialize = true)
        {
            if (!inicialize)
                return;

            Layers.Add(new NNNode[inputLayerSize]);
            Layers.Add(new NNNode[outputLayerSize]);

            for (int i = 0; i < Layers[0].Length; i++)
            {
                Layers[0][i] = new NNNode(Random.NextDouble(-1, 1), 0, i);
            }

            for (int i = 0; i < Layers[1].Length; i++)
            {
                Layers[1][i] = new NNNode(Random.NextDouble(-1, 1), 1, i);
                for (int j = 0; j < Layers[0].Length; j++)
                {
                    Connections.Add(new NNConnection(Layers[0][j], Layers[1][i], Random.NextDouble(-1, 1)));
                    Layers[0][j].OutConns.Add(Connections[Connections.Count - 1]);
                }
            }

            Mutate();
        }

        public MovementType Forward(double[] input)
        {
            Debug.Assert(input.Length == Layers[0].Length, "Input to forward pass isn't equal to length of inpput layer");

            // Reset NN and apply bias
            for (int l = 1; l < Layers.Count; l++)
            {
                for (int i = 0; i < Layers[l].Length; i++)
                {
                    Layers[l][i].Value = Layers[l][i].Bias;
                }
            }

            // Assign input
            for (int i = 0; i < Layers[0].Length; i++)
            {
                Layers[0][i].Value = input[i];
            }

            // Signal propagation
            for (int l = 0; l < Layers.Count - 1; l++) // Output layer don't have OutConns so we save one iteration
            {
                foreach (NNNode node in Layers[l])
                {
                    foreach (NNConnection conn in node.OutConns)
                    {
                        conn.ToNode.Value += conn.Weight * node.Value;
                    }
                }
                //TODO: Activation function for layer l+1
            }

            // Get result
            double maxVal = double.MinValue;
            int idx = 0;
            int lastIdx = Layers.Count - 1;
            for (int i = 0; i < Layers[lastIdx].Length; i++)
            {
                if (Layers[lastIdx][i].Value > maxVal)
                {
                    idx = i;
                    maxVal = Layers[lastIdx][i].Value;
                }
            }
            return (MovementType)idx;
        }


        public void Mutate()
        {
            // First add bias so there is chance that nex connection will be added
            if (Random.Chance(NewNodeMutationChance))
            {
                if (Layers.Count == 2) // if there is only input layer we must create new layer bc we don't want to change input or output size
                {
                    InsertLayer(1);
                }
                else
                {
                    int randomLayerIdx = Random.Next(1, Layers.Count - 1); // Except input and output layer

                    ResizeLayer(randomLayerIdx, 1);

                    Layers[randomLayerIdx][Layers[randomLayerIdx].Length - 1] = new NNNode(Random.NextDouble(-1, 1), randomLayerIdx, Layers[randomLayerIdx].Length - 1);
                }
            }

            // Second so there is chance for it to get splited
            if (Random.Chance(NewConnectionMutationChance))
            {
                int randomSourceLayerIdx = Random.Next(Layers.Count - 1); // except output bc there won't be next layer if output l got chousen
                int randomTargetLayerIdx = Random.Next(randomSourceLayerIdx + 1, Layers.Count); // random layer from randomSourceLayerIdx to output layer

                int randomSourceNodeIdx = Random.Next(Layers[randomSourceLayerIdx].Length);
                int randomTargetNodeIdx = Random.Next(Layers[randomTargetLayerIdx].Length);

                Connections.Add(new NNConnection(Layers[randomSourceLayerIdx][randomSourceNodeIdx], Layers[randomTargetLayerIdx][randomTargetNodeIdx], Random.NextDouble(-1, 1)));
                Layers[randomSourceLayerIdx][randomSourceNodeIdx].OutConns.Add(Connections[Connections.Count - 1]);
            }

            // Third split so there is a chance that new conns and node gets weights and bias mutated later
            if (Random.Chance(SplitMutationChance))
            {
                int randomConnection = Random.Next(Connections.Count);

                Connections[randomConnection].FromNode.OutConns.Remove(Connections[randomConnection]);

                //Find layer of from node
                int FromLayer = -1;
                int ToLayer = -1;
                for (int l = 0; l < Layers.Count; l++)
                {
                    if (Layers[l].Contains(Connections[randomConnection].FromNode))
                    {
                        FromLayer = l;
                    }

                    if (Layers[l].Contains(Connections[randomConnection].ToNode))
                    {
                        ToLayer = l;
                        break;
                    }
                }
                if (FromLayer != -1)
                {
                    NNNode newNodeRef;
                    if (ToLayer - FromLayer == 1)
                    {
                        InsertLayer(FromLayer + 1);
                        newNodeRef = Layers[FromLayer + 1][0];
                    }
                    else
                    {
                        ResizeLayer(FromLayer + 1, 1);
                        Layers[FromLayer + 1][Layers[FromLayer + 1].Length - 1] = new NNNode(Random.NextDouble(-1, 1), FromLayer + 1, Layers[FromLayer + 1].Length - 1);
                        newNodeRef = Layers[FromLayer + 1][Layers[FromLayer + 1].Length - 1];
                    }

                    Connections.Add(new NNConnection(Connections[randomConnection].FromNode, newNodeRef, Random.NextDouble(-1, 1)));
                    Connections.Add(new NNConnection(newNodeRef, Connections[randomConnection].ToNode, Random.NextDouble(-1, 1)));

                    Connections[randomConnection].FromNode.OutConns.Add(Connections[Connections.Count - 2]);
                    newNodeRef.OutConns.Add(Connections[Connections.Count - 1]);

                    Connections.Remove(Connections[randomConnection]);
                }
            }

            // Weights mutation
            for (int i = 0; i < Connections.Count; i++)
            {
                if (Random.Chance(WeightMutationChance))
                {
                    Connections[i].Weight += Random.NextDouble(WeightMutationSizeMin, WeightMutationSizeMax);
                }
            }

            // Biases mutation
            for (int l = 0; l < Layers.Count; l++)
            {
                for (int i = 0; i < Layers[l].Length; i++)
                {
                    if (Random.Chance(BiasMutationChance))
                    {
                        Layers[l][i].Bias += Random.NextDouble(BiasMutationSizeMin, BiasMutationSizeMax);
                    }
                }
            }
        }

        void ResizeLayer(int layerIdx, uint nodesToAddCount)
        {
            NNNode[] targetArray = Layers[layerIdx];
            Array.Resize(ref targetArray, targetArray.Length + (int)nodesToAddCount);
            Layers[layerIdx] = targetArray;
        }

        void InsertLayer(int layerIndex)
        {
            Layers.Insert(layerIndex, new NNNode[1]);
            Layers[layerIndex][0] = new NNNode(Random.NextDouble(-1, 1), layerIndex, 0);
            for (int l = layerIndex + 1; l < Layers.Count; l++)
            {
                for (int i = 0; i < Layers[l].Length; i++)
                {
                    Layers[l][i].Layer++;
                }
            }
        }

        /* getters */
        public int InputSize
        {
            get { return Layers[0].Length; }
        }

        public int OutputSize
        {
            get { return Layers[Layers.Count - 1].Length; }
        }

        public NN Copy(bool mutate)
        {
            NN nn = new NN(0, 0, false);
            nn.BiasMutationChance = BiasMutationChance;
            nn.SplitMutationChance = SplitMutationChance;
            nn.WeightMutationChance = WeightMutationChance;
            nn.NewNodeMutationChance = NewNodeMutationChance;
            nn.NewConnectionMutationChance = NewConnectionMutationChance;

            nn.BiasMutationSizeMin = BiasMutationSizeMin;
            nn.BiasMutationSizeMax = BiasMutationSizeMax;

            nn.WeightMutationSizeMin = WeightMutationSizeMin;
            nn.WeightMutationSizeMax = WeightMutationSizeMax;

            for (int l = 0; l < Layers.Count; l++)
            {
                nn.Layers.Add(new NNNode[Layers[l].Length]);
                for (int i = 0; i < nn.Layers[l].Length; i++)
                {
                    nn.Layers[l][i] = new NNNode(Layers[l][i].Bias, Layers[l][i].Layer, Layers[l][i].Index);
                }
            }

            foreach (NNConnection c in Connections)
            {
                nn.Connections.Add(new NNConnection(nn.Layers[c.FromNode.Layer][c.FromNode.Index], nn.Layers[c.ToNode.Layer][c.ToNode.Index], c.Weight));
                nn.Layers[c.FromNode.Layer][c.FromNode.Index].OutConns.Add(nn.Connections[nn.Connections.Count - 1]);
            }

            if (mutate)
                nn.Mutate();

            return nn;
        }
    }
}
