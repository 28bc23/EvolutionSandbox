namespace EvolutionSandbox.NeuralNetwork
{
    internal class NNConnection
    {
        public NNNode FromNode { get; private set; }
        public NNNode ToNode { get; private set; }
        public double Weight { get; set; }

        public NNConnection(NNNode fromNode, NNNode toNode, double weight)
        {
            FromNode = fromNode;
            ToNode = toNode;
            Weight = weight;
        }
    }
}
