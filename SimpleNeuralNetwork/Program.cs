using System;
using System.Linq;

namespace SimpleNeuralNetwork
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      NetworkModel model = new NetworkModel();
      model.Layers.Add(new NeuralLayer(3, 0.1, 0.1, "INPUT"));
      model.Layers.Add(new NeuralLayer(1, 0.1, 0.1, "OUTPUT"));
      model.Build();

      Console.WriteLine("----Before Training------------");

      model.Print();

      Console.WriteLine();

      NeuralData X = new NeuralData(4);
      X.Add(1, 1, 1);
      X.Add(1, 0, 1);
      X.Add(0, 1, 1);
      X.Add(0, 0, 0);

      NeuralData Y = new NeuralData(4);
      Y.Add(1);
      Y.Add(1);
      Y.Add(1);
      Y.Add(0);

      model.Train(X, Y, iterations: 10).ToList().ForEach(x => Console.WriteLine("Epoch: {0}, Accuracy: {1} %", x.epoch, x.accuracy));

      Console.WriteLine();
      Console.WriteLine("----After Training------------");

      model.Print();
    }
  }
}