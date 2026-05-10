namespace EvolutionSandbox.NeuralNetwork
{
    internal class NNNode
    {
        public double Value { get; set; }
        public double Bias { get; set; }

        public int Layer { get; set; }
        public int Index { get; private set; }

        public List<NNConnection> OutConns { get; set; } = new List<NNConnection>();

        public NNNode() { }

        public NNNode(double bias, int layer, int index)
        {
            Bias = bias;
            Layer = layer;
            Index = index;
        }
    }
}
