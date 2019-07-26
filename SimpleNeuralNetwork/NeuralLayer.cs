using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SimpleNeuralNetwork
{
  internal class NeuralLayer
  {
    public IList<Neuron> Neurons { get; set; }

    public string Name { get; set; }

    public double Weight { get; set; }

    public double LearningRate { get; set; }

    public NeuralLayer(int count, double initialWeight, double learningRate = 0.1, string name = "")
    {
      Neurons = new List<Neuron>();

      for (var i = 0; i < count; i++)
      {
        Neurons.Add(new Neuron());
      }

      Weight = initialWeight;
      LearningRate = learningRate;
      Name = name;
    }

    public void Optimize(double delta)
    {
      Weight += LearningRate * delta;

      Debug.WriteLine($"LR = {LearningRate} Delta = {delta} Weight = {Weight}");

      Neurons.ToList().ForEach(x => x.UpdateWeights(Weight));
    }

    public void Log()
    {
      Console.WriteLine("{0}, Weight: {1}", Name, Weight);
    }

    public void Forward()
    {
      Neurons.ToList().ForEach(x => x.Fire());
    }
  }
}