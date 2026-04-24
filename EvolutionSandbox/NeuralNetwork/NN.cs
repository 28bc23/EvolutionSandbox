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

        public NN(int inputLayerSize, int outputLayerSize)
        {
            Layers.Add(new NNNode[inputLayerSize]);
            Layers.Add(new NNNode[outputLayerSize]);

            for (int i = 0; i < Layers[0].Length; i++)
            {
                Layers[0][i] = new NNNode(Random.NextDouble(-1, 1));
            }

            for (int i = 0; i < Layers[1].Length; i++)
            {
                Layers[1][i] = new NNNode(Random.NextDouble(-1, 1));
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
            Debug.Assert(input.Length == Layers[0].Length, "Input to forward pass isn't equal to lenght of inpput layer");

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
            for (int l = 0; l < Layers.Count - 1; l++) // Output layer don't have OutConns so we save one itteration
            {
                foreach(NNNode node in Layers[l])
                {
                    foreach(NNConnection conn in node.OutConns)
                    {
                        conn.GToNode.Value += conn.Weight * node.Value;
                    }
                }
                //TODO: Activation function for layer l+1
            }

            // Get result
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


        public void Mutate()
        {
            // First add bias so there is chance that nex connection will be added
            if (Random.Chance(NewNodeMutationChance))
            {
                if(Layers.Count == 2) // if there is only input layer we must create new layer bc we don't want to change input or output size
                {
                    Layers.Insert(1, new NNNode[1]);
                    Layers[1][0] = new NNNode(Random.NextDouble(-1, 1));
                }
                else
                {
                    int randomLayerIdx = Random.Next(1, Layers.Count - 1); // Except input and output layer

                    ResizeLayer(randomLayerIdx, 1);

                    Layers[randomLayerIdx][Layers[randomLayerIdx].Length - 1] = new NNNode(Random.NextDouble(-1, 1));
                }
            }

            // Second so there is chance for it to get splited
            if (Random.Chance(NewConnectionMutationChance))
            {
                int randomLayerIdx1 = Random.Next(Layers.Count - 1); // except output bc there won't be next layer if output l got chousen
                int randomLayerIdx2 = Random.Next(randomLayerIdx1 + 1, Layers.Count); // random layer from randomLayerIdx1

                int randomNodeIdx1 = Random.Next(Layers[randomLayerIdx1].Length);
                int randomNodeIdx2 = Random.Next(Layers[randomLayerIdx2].Length);

                Connections.Add(new NNConnection(Layers[randomLayerIdx1][randomNodeIdx1], Layers[randomLayerIdx2][randomNodeIdx2], Random.NextDouble(-1, 1)));
                Layers[randomLayerIdx1][randomNodeIdx1].OutConns.Add(Connections[Connections.Count - 1]);
            }

            // Third split so there is a chance that new conns and node gets weights and bias mutated later
            if (Random.Chance(SplitMutationChance))
            {
                int randomConnection = Random.Next(Connections.Count);

                Connections[randomConnection].GFromNode.OutConns.Remove(Connections[randomConnection]);

                //Find layer of from node
                int FromLayer = -1;
                int ToLayer = -1;
                for (int l = 0; l < Layers.Count; l++)
                {
                    if (Layers[l].Contains(Connections[randomConnection].GFromNode))
                    {
                        FromLayer = l;
                    }

                    if (Layers[l].Contains(Connections[randomConnection].GToNode))
                    {
                        ToLayer = l;
                        break;
                    }
                }
                if(FromLayer != -1)
                {
                    NNNode newNodeRef;
                    if (ToLayer - FromLayer == 1)
                    {
                        Layers.Insert(FromLayer + 1, new NNNode[1]);
                        Layers[FromLayer + 1][0] = new NNNode(Random.NextDouble(-1, 1));
                        newNodeRef = Layers[FromLayer + 1][0];
                    }
                    else
                    {
                        ResizeLayer(FromLayer + 1, 1);
                        Layers[FromLayer + 1][Layers[FromLayer + 1].Length - 1] = new NNNode(Random.NextDouble(-1, 1));
                        newNodeRef = Layers[FromLayer + 1][Layers[FromLayer + 1].Length - 1];
                    }

                    Connections.Add(new NNConnection(Connections[randomConnection].GFromNode, newNodeRef, Random.NextDouble(-1, 1)));
                    Connections.Add(new NNConnection(newNodeRef, Connections[randomConnection].GToNode, Random.NextDouble(-1, 1)));

                    Connections[randomConnection].GFromNode.OutConns.Add(Connections[Connections.Count - 2]);
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

            // BIases mutation
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

        void ResizeLayer(int layerIdx, uint plusSize)
        {
            NNNode[] targetArray = Layers[layerIdx];
            Array.Resize(ref targetArray, targetArray.Length + (int)plusSize);
            Layers[layerIdx] = targetArray;
        }

        /* getters */
        public int GInputSize
        {
            get { return Layers[0].Length; }
        }

        public int GOutputSize
        {
            get { return Layers[Layers.Count - 1].Length; }
        }
    }
}
