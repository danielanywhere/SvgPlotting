using System;
using System.IO;

using Html;
using SvgPlotting;

namespace SvgPlottingDemo
{
 public class Program
 {
  public static void Main(string[] args)
  {
   string content = File.ReadAllText(@"C:\Temp\MySvg.svg");
   HtmlDocument doc = new HtmlDocument(content);
   PlotPointPenStatus pen = PlotPointPenStatus.None;
   SvgImageItem svg = new SvgImageItem(doc, 50);

   // Optionally add a trace listener to output library activities to console.
   Trace.Listeners.Add(new ConsoleTraceListener());

   foreach (PlotPointItem plotPointItem in svg.PlotPoints)
   {
    if (plotPointItem.PenStatus != pen)
    {
     Console.WriteLine($"Pen Status: {plotPointItem.PenStatus}");
     pen = plotPointItem.PenStatus;
    }
    switch (plotPointItem.PenStatus)
    {
     case PlotPointPenStatus.PenDown:
      Console.WriteLine($" Line To: {plotPointItem.Point}");
      break;
     case PlotPointPenStatus.PenUp:
      Console.WriteLine($" Move To: {plotPointItem.Point}");
      break;
    }
   }
  }
 }
}
