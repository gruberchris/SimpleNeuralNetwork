﻿namespace SimpleNeuralNetwork
{
  internal class NeuralData
  {
    public double[][] Data { get; set; }

    private int counter = 0;

    public NeuralData(int rows)
    {
      Data = new double[rows][];
    }

    public void Add(params double[] rec)
    {
      Data[counter] = rec;
      counter++;
    }
  }
}