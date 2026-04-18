namespace EvolutionSandbox.NeuralNetwork
{
    internal class NNConnection
    {
        NNNode ToNode;
        public double Weight;

        public NNConnection(NNNode toNode, double weight)
        {
            ToNode = toNode;
            Weight = weight;
        }

        /* Getters */


        public NNNode GToNode
        {
            get { return ToNode; }
        }
    }
}
