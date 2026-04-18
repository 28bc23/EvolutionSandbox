namespace EvolutionSandbox.NeuralNetwork
{
    internal class NNNode
    {
        public double Value;
        public double Bias;

        public List<NNConnection> OutConns = new List<NNConnection>();

        public NNNode(double bias)
        {
            Bias = bias;
        }
    }
}
