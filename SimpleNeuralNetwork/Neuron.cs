using System.Collections.Generic;
using System.Linq;

namespace SimpleNeuralNetwork
{
  internal class Neuron
  {
    public IList<Dendrite> Dendrites { get; set; }

    public Pulse OutputPulse { get; set; }

    public Neuron()
    {
      Dendrites = new List<Dendrite>();
      OutputPulse = new Pulse();
    }

    public void Fire()
    {
      OutputPulse.Value = Sum();
      OutputPulse.Value = Activation(OutputPulse.Value);
    }

    public void UpdateWeights(double new_weights)
    {
      Dendrites.ToList().ForEach(x => x.SynapticWeight = new_weights);
    }

    private double Sum()
    {
      double computeValue = 0.0f;

      Dendrites.ToList().ForEach(x => computeValue += x.InputPulse.Value * x.SynapticWeight);

      return computeValue;
    }

    private double Activation(double input)
    {
      double threshold = 1;

      return input <= threshold ? 0 : threshold;
    }
  }
}