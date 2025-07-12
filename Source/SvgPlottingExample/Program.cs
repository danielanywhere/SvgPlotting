/*
 * Copyright (c). 2020 - 2025 Daniel Patterson, MCSD (danielanywhere).
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 * 
 */
using System;
using System.Diagnostics;
using System.IO;

using Html;
using SvgPlotting;

using static SvgPlotting.SvgPlottingUtil;

namespace SvgPlottingExample
{
	//*-------------------------------------------------------------------------*
	//*	Program																																	*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// The main instance of the SVG Plotting Example application.
	/// </summary>
	public class Program
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	_Main																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Configure and run the application.
		/// </summary>
		public static void Main(string[] args)
		{
			bool bShowHelp = false; //	Flag - Explicit Show Help.
			FileInfo file = null;
			string key = "";        //	Current Parameter Key.
			string lowerArg = "";   //	Current Lowercase Argument.
			string message = "";    //	Message to display in Console.
			Program prg = new Program();  //	Initialized instance.

			Console.WriteLine("SvgPlottingExample.exe");
			foreach(string arg in args)
			{
				lowerArg = arg.ToLower();
				key = "/?";
				if(lowerArg == key)
				{
					bShowHelp = true;
					continue;
				}
				key = "/svg:";
				if(lowerArg.StartsWith(key))
				{
					prg.mSvgFilename = arg.Substring(key.Length);
					continue;
				}
				key = "/wait";
				if(lowerArg.StartsWith(key))
				{
					prg.mWaitAfterEnd = true;
					continue;
				}
			}
			if(!bShowHelp)
			{
				if(prg.mSvgFilename.Length > 0)
				{
					file = new FileInfo(prg.mSvgFilename);
					if(file.Exists)
					{
						Console.WriteLine($"SVG File: {file.Name}");
					}
					else
					{
						message += $"Error: SVG file not found: [{file.Name}]";
						bShowHelp = true;
					}
				}
				else
				{
					message += "Error: SVG file was not specified...\r\n";
					bShowHelp = true;
				}
			}
			if(bShowHelp)
			{
				//	Display Syntax.
				Console.WriteLine(message.ToString() + "\r\n" + ResourceMain.Syntax);
			}
			else
			{
				//	Run the configured application.
				prg.Run();
			}
			if(prg.mWaitAfterEnd)
			{
				Console.WriteLine("Press [Enter] to exit...");
				Console.ReadLine();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Run																																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Run the configured application.
		/// </summary>
		public void Run()
		{
			string content = File.ReadAllText(mSvgFilename);
			HtmlDocument doc = new HtmlDocument(content);
			PlotPointPenStatus pen = PlotPointPenStatus.None;
			SvgImageItem svg = new SvgImageItem(doc, 50);

			Trace.Listeners.Add(new ConsoleTraceListener());

			//	Test radial conversions.
			Console.WriteLine(
				$"27.9deg = {HConverter.Convert(27.9d, "deg", "turn")}turn");
			Console.WriteLine(
				$"1.2turns = {HConverter.Convert(1.2d, "turn", "rad")}rad");

			//	Test linear conversions.
			Console.WriteLine($"87vw = {GetPixelValue(svg, "87vw", doc)}px");
			doc.Attributes.SetAttribute("font-size", "8pt");
			Console.WriteLine($"At 8pt, 2rem = {GetPixelValue(svg, "2rem", doc)}px");
			doc.Attributes.SetAttribute("font-size", "24pt");
			Console.WriteLine($"At 24pt, 0.6em = {GetPixelValue(svg, "0.6em", doc)}px");
			doc.Attributes.SetAttribute("font-size", "14pt");
			Console.WriteLine($"At 14pt, 5ch = {GetPixelValue(svg, "5ch", doc)}px");
			doc.Attributes.Remove("font-size");

			foreach(PlotPointItem plotPointItem in svg.PlotPoints)
			{
				if(plotPointItem.PenStatus != pen)
				{
					Console.WriteLine($"Pen Status: {plotPointItem.PenStatus}");
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
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	SvgFilename																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="SvgFilename">SvgFilename</see>.
		/// </summary>
		private string mSvgFilename = "";
		/// <summary>
		/// Get/Set the full path and filename of the SVG file to open.
		/// </summary>
		public string SvgFilename
		{
			get { return mSvgFilename; }
			set { mSvgFilename = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	WaitAfterEnd																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="WaitAfterEnd">WaitAfterEnd</see>.
		/// </summary>
		private bool mWaitAfterEnd = false;
		/// <summary>
		/// Get/Set a value indicating whether to wait for user keypress after
		/// processing has completed.
		/// </summary>
		public bool WaitAfterEnd
		{
			get { return mWaitAfterEnd; }
			set { mWaitAfterEnd = value; }
		}

	}
	//*-------------------------------------------------------------------------*


}
