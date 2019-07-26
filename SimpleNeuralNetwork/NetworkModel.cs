using ConsoleTableExt;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SimpleNeuralNetwork
{
  internal class NetworkModel
  {
    public IList<NeuralLayer> Layers { get; set; }

    public NetworkModel()
    {
      Layers = new List<NeuralLayer>();
    }

    public void AddLayer(NeuralLayer layer)
    {
      int dendriteCount = 1;

      if (Layers.Count > 0)
      {
        dendriteCount = Layers.Last().Neurons.Count;
      }

      foreach (var element in layer.Neurons)
      {
        for (var i = 0; i < dendriteCount; i++)
        {
          element.Dendrites.Add(new Dendrite());
        }
      }
    }

    public void Build()
    {
      int i = 0;

      foreach (var layer in Layers)
      {
        if (i >= Layers.Count - 1)
        {
          break;
        }

        var nextLayer = Layers[i + 1];

        CreateNetwork(layer, nextLayer);

        i++;
      }
    }

    public IEnumerable<(int epoch, double accuracy)> Train(NeuralData X, NeuralData Y, int iterations)
    {
      List<(int epoch, double accuracy)> trainingResults = new List<(int epoch, double accuracy)>();
      int epoch = 1;

      // Loop till the number of iterations
      while (iterations >= epoch)
      {
        // Get the input layers
        var inputLayer = Layers[0];
        List<double> outputs = new List<double>();

        // Loop through the record
        for (int i = 0; i < X.Data.Length; i++)
        {
          // Set the input data into the first layer
          for (int j = 0; j < X.Data[i].Length; j++)
          {
            inputLayer.Neurons[j].OutputPulse.Value = X.Data[i][j];
          }

          // Fire all the neurons and collect the output
          ComputeOutput();
          outputs.Add(Layers.Last().Neurons.First().OutputPulse.Value);
        }

        // Check the accuracy score against Y with the actual output
        double accuracySum = 0;
        int y_counter = 0;

        outputs.ForEach((x) =>
        {
          if (x == Y.Data[y_counter].First())
          {
            accuracySum++;
          }

          y_counter++;
        });

        var accuracy = (accuracySum / (double)y_counter);

        // Optimize the synaptic weights
        OptimizeWeights(accuracy);

        trainingResults.Add((epoch, accuracy * 100));

        epoch++;
      }

      return trainingResults;
    }

    public void Print()
    {
      DataTable dt = new DataTable();
      dt.Columns.Add("Name");
      dt.Columns.Add("Neurons");
      dt.Columns.Add("Weight");

      foreach (var element in Layers)
      {
        DataRow row = dt.NewRow();
        row[0] = element.Name;
        row[1] = element.Neurons.Count;
        row[2] = element.Weight;

        dt.Rows.Add(row);
      }

      ConsoleTableBuilder builder = ConsoleTableBuilder.From(dt);
      builder.ExportAndWrite();
    }

    private void CreateNetwork(NeuralLayer connectingFrom, NeuralLayer connectingTo)
    {
      foreach (var to in connectingTo.Neurons)
      {
        foreach (var from in connectingFrom.Neurons)
        {
          to.Dendrites.Add(new Dendrite() { InputPulse = from.OutputPulse, SynapticWeight = connectingTo.Weight });
        }
      }
    }

    private void ComputeOutput()
    {
      bool first = true;

      foreach (var layer in Layers)
      {
        // Skip first layer as it is input
        if (first)
        {
          first = false;
          continue;
        }

        layer.Forward();
      }
    }

    private void OptimizeWeights(double accuracy)
    {
      // Skip if the accuracy reached 100%
      if (accuracy == 1)
      {
        return;
      }

      // Update the weights for all the layers
      Layers.ToList().ForEach(x => x.Optimize(1));
    }
  }
}