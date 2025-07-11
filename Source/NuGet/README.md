# Dan's SVG Plotting Library

An easy-to-use library that brings physical plotting and shape-cutting capabilities to normal SVG image files, without any dependencies on Windows or other similar vendors.

Basic Example:

```cs
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
   SvgImageItem svg = new SvgImageItem();

   // Optionally add a trace listener to output library activities to console.
   Trace.Listeners.Add(new ConsoleTraceListener());

   svg.Initialize(doc);
   foreach(PlotPointItem plotItem in svg.PlotCommands)
   {
    if(plotItem.PenStatus != pen)
    {
     Console.WriteLine($"Pen Status: {plotItem.PenStatus}");
    }
    switch(plotItem.PenStatus)
    {
     case PlotPointPenStatus.PenDown:
      Console.WriteLine($" Line To: {plotItem.Point}");
      break;
     case PlotPointPenStatus.PenUp:
      Console.WriteLine($" Move To: {plotItem.Point}");
      break;
    }
   }
  }
 }
}
```

## Updates

| Version | Description |
|---------|-------------|
| 25.2711.4233 | Initial public release. |

## More Information

For more information, please see the GitHub project:
[danielanywhere/SvgPlotting](https://github.com/danielanywhere/SvgPlotting)

Full API documentation is available at this library's [GitHub User Page](https://danielanywhere.github.io/SvgPlotting).

