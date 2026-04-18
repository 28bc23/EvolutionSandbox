namespace EvolutionSandbox.NeuralNetwork
{
    internal class NNConnection
    {
        NNNode FromNode;
        NNNode ToNode;
        public double Weight;

        public NNConnection(NNNode fromNode, NNNode toNode, double weight)
        {
            FromNode = fromNode;
            ToNode = toNode;
            Weight = weight;
        }

        /* Getters */
        public NNNode GFromNode
        {
            get { return FromNode; }
        }

        public NNNode GToNode
        {
            get { return ToNode; }
        }
    }
}
