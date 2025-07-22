# Dan's SVG Plotting Library

An easy-to-use library that brings physical plotting and shape-cutting capabilities to normal SVG image files, without any dependencies on Windows or other similar vendors.

NOTE: In this version, all output is in mm. Please create an issue on the GitHub repository if you would like to be able to choose any other type of measurement unit.

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
   SvgImageItem svg = new SvgImageItem(doc, 50);

   // Optionally add a trace listener to output library activities to console.
   Trace.Listeners.Add(new ConsoleTraceListener());

   foreach(PlotPointItem plotPointItem in svg.PlotPoints)
   {
    if(plotPointItem.PenStatus != pen)
    {
     Console.WriteLine($"Pen Status: {plotPointItem.PenStatus}");
     pen = plotPointItem.PenStatus;
    }
    switch(plotPointItem.PenStatus)
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
```

## Updates

| Version | Description |
|---------|-------------|
| 25.2722.4017 | No breaking changes. Sequential duplicate values are now removed while parsing the plot points from the SVG. |
| 25.2714.4126 | Updated to support **Geometry** library version **25.2714.4042**. |
| 25.2714.3945 | Converted all uses of **Geometry.FPoint** to **Geometry.FVector2**. |
| 25.2711.4233 | Initial public release. |

## More Information

For more information, please see the GitHub project:
[danielanywhere/SvgPlotting](https://github.com/danielanywhere/SvgPlotting)

Full API documentation is available at this library's [GitHub User Page](https://danielanywhere.github.io/SvgPlotting).

