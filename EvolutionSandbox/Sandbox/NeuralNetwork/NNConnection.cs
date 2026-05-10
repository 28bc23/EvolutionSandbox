using System.Text.Json.Serialization;

namespace EvolutionSandbox.NeuralNetwork
{
    internal class NNConnection
    {
        public NNNode FromNode { get; set; }
        public NNNode ToNode { get; set; }
        public double Weight { get; set; }

        public NNConnection() { }
        public NNConnection(NNNode fromNode, NNNode toNode, double weight)
        {
            FromNode = fromNode;
            ToNode = toNode;
            Weight = weight;
        }
    }
}
