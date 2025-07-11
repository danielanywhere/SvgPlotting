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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Geometry;

using static SvgPlotting.SvgPlottingUtil;

namespace SvgPlotting
{
	//*-------------------------------------------------------------------------*
	//*	PathEntryCollection																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of PathEntryItem Items.
	/// </summary>
	public class PathEntryCollection : List<PathEntryItem>
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
		//* ConvertToAbsolute																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a new collection whose path entries are absolute representations
		/// of the members of the supplied entries collection.
		/// </summary>
		/// <param name="entries">
		/// Reference to the collection of entries to inspect.
		/// </param>
		/// <param name="location">
		/// Reference to the absolute starting location to be used, if needed.
		/// </param>
		/// <returns>
		/// Reference to a new collection of entries consisting only of absolute
		/// coordinates and values.
		/// </returns>
		public static PathEntryCollection ConvertToAbsolute(
			PathEntryCollection entries, FPoint location)
		{
			PathEntryItem entry = null;
			int index = 0;
			FPoint point = new FPoint();
			List<float> points = new List<float>();
			PathEntryCollection result = new PathEntryCollection();

			if(entries?.Count > 0)
			{
				//if(IsLowerCase(entries[0].Action) && location != null)
				//{
				//	entry = new PathEntryItem()
				//	{
				//		Action = "M"
				//	};
				//	entry.Points.Add(location.X);
				//	entry.Points.Add(location.Y);
				//}
				foreach(PathEntryItem entryItem in entries)
				{
					entry = PathEntryItem.Clone(entryItem);
					result.Add(entry);
					if(index == 0 && entry.Action == "m")
					{
						//	In this version, the first move to is always interpreted as
						//	absolute.
						entry.Action = "M";
					}
					switch(entry.Action)
					{
						case "A":
							//	Arc: 5, 6.
							//	rx,ry, rotation, arc, sweep, ex, ey
							point.X = entry.Points[5];
							point.Y = entry.Points[6];
							break;
						case "C":
							//	Bezier curve: 4, 5.
							point.X = entry.Points[4];
							point.Y = entry.Points[5];
							break;
						case "H":
							//	Horizontal line: 0.
							point.X = entry.Points[0];
							break;
						case "L":
						case "M":
						case "T":
							//	Line, Move, Quadratic batch: 0, 1.
							point.X = entry.Points[0];
							point.Y = entry.Points[1];
							break;
						case "Q":
							//	Quadratic Bezier curve: 2, 3.
							point.X = entry.Points[2];
							point.Y = entry.Points[3];
							break;
						case "V":
							//	Vertical line: 0.
							point.Y = entry.Points[0];
							break;
						case "Z":
							break;
						case "a":
							//	Arc: 5, 6.
							point.X += entry.Points[5];
							point.Y += entry.Points[6];
							entry.Points[5] = point.X;
							entry.Points[6] = point.Y;
							entry.Action = entry.Action.ToUpper();
							break;
						case "c":
							//	6 coordinates to convert.
							points.Clear();
							points.Add(point.X + entry.Points[0]);
							points.Add(point.Y + entry.Points[1]);
							points.Add(point.X + entry.Points[2]);
							points.Add(point.Y + entry.Points[3]);
							points.Add(point.X + entry.Points[4]);
							points.Add(point.Y + entry.Points[5]);
							entry.Points.Clear();
							entry.Points.AddRange(points);
							point.X = entry.Points[4];
							point.Y = entry.Points[5];
							entry.Action = entry.Action.ToUpper();
							break;
						case "h":
							//	Horizontal line.
							point.X += entry.Points[0];
							entry.Points[0] = point.X;
							entry.Action = entry.Action.ToUpper();
							break;
						case "l":
						case "m":
						case "t":
							//	Line, Move, Batch Quadratic: 0, 1.
							point.X += entry.Points[0];
							point.Y += entry.Points[1];
							entry.Points[0] = point.X;
							entry.Points[1] = point.Y;
							entry.Action = entry.Action.ToUpper();
							break;
						case "q":
						case "s":
							//	Quadratic, Batch cubic: 4 coordinates.
							points.Clear();
							points.Add(point.X + entry.Points[0]);
							points.Add(point.Y + entry.Points[1]);
							points.Add(point.X + entry.Points[2]);
							points.Add(point.Y + entry.Points[3]);
							entry.Points.Clear();
							entry.Points.AddRange(points);
							point.X = entry.Points[2];
							point.Y = entry.Points[3];
							entry.Action = entry.Action.ToUpper();
							break;
						case "v":
							//	Vertical: 0
							point.Y += entry.Points[0];
							entry.Points[0] = point.Y;
							entry.Action = entry.Action.ToUpper();
							break;
						case "z":
							//	Relative actions.
							entry.Action = entry.Action.ToUpper();
							break;
					}
					index++;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetVertices																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a list of distinct vertices rendered from the caller's path
		/// entry collection.
		/// </summary>
		/// <param name="entries">
		/// Reference to the collection of path entries to render.
		/// </param>
		/// <param name="curveVertexCount">
		/// The number of vertices in each curve.
		/// </param>
		/// <returns>
		/// Reference to a newly rendered collection of vertices corresponding
		/// to the shape of the path.
		/// </returns>
		/// <remarks>
		/// This function only works on collections where all path entries
		/// are using absolute values.
		/// </remarks>
		public static PlotPointCollection GetVertices(PathEntryCollection entries,
			int curveVertexCount)
		{
			float angleDiff = 0f;
			float angleEnd = 0f;
			float angleStart = 0f;
			bool bLargeArc = false;
			bool bSweep = false;
			FVector2 center;
			FEllipse ellipse = null;
			FLine line = null;
			FPoint locationPoint = new FPoint();
			PlotPointItem plot = null;
			FPoint point = new FPoint();
			FPoint pointEnd = null;
			List<FPoint> points = null;
			FPoint pointStart = null;
			FPoint previousControlC = null;
			FPoint previousControlQ = null;
			PlotPointCollection result = new PlotPointCollection();
			float rotation = 0f;
			float sweep = 0f;

			if(entries?.Count > 0 && !entries.Exists(x => IsLowerCase(x.Action)))
			{
				foreach(PathEntryItem entryItem in entries)
				{
					if(entryItem.Action != "C" && entryItem.Action != "S")
					{
						previousControlC = null;
					}
					if(entryItem.Action != "Q" && entryItem.Action != "T")
					{
						previousControlQ = null;
					}
					switch(entryItem.Action)
					{
						case "A":
							//	Arc.
							//	Translate ellipse by negative amount.
							if(entryItem.Points.Count > 6)
							{
								pointStart = new FPoint(point.X, point.Y);
								pointEnd =
									new FPoint(entryItem.Points[5], entryItem.Points[6]);
								rotation = entryItem.Points[2];
								line = new FLine(pointStart, pointEnd);
								ellipse = new FEllipse(
									0f,
									0f,
									entryItem.Points[0],
									entryItem.Points[1]);
								ellipse.Rotation = rotation;
								bLargeArc = (entryItem.Points[3] == 1f);
								bSweep = (entryItem.Points[4] == 1f);
								if(FEllipse.TryPlaceEllipseEdgeOnLine(ellipse, line,
									bLargeArc ^ bSweep, out center))
								{
									//	The ellipse can be repositioned to center.
									ellipse.Center = (FPoint)center;
									angleStart = Trig.GetLineAngle(ellipse.Center, pointStart);
									angleEnd = Trig.GetLineAngle(ellipse.Center, pointEnd);
									angleDiff = angleEnd - angleStart;
									if(angleDiff > GeometryUtil.OnePi)
									{
										angleDiff -= GeometryUtil.TwoPi;
									}
									else if(angleDiff < 0f - GeometryUtil.OnePi)
									{
										angleDiff += GeometryUtil.TwoPi;
									}
									if(angleDiff != 0f)
									{
										if(bSweep)
										{
											//	Sweep reverse.
											if(bLargeArc)
											{
												if(angleDiff > 0)
												{
													sweep = angleDiff - GeometryUtil.TwoPi;
												}
												else
												{
													sweep = angleDiff + GeometryUtil.TwoPi;
												}
											}
											else
											{
												sweep = angleDiff;
											}
										}
										else
										{
											//	Sweep normal.
											if(bLargeArc)
											{
												if(angleDiff > 0f)
												{
													sweep = angleDiff - GeometryUtil.TwoPi;
												}
												else
												{
													sweep = angleDiff + GeometryUtil.TwoPi;
												}
											}
											else
											{
												sweep = angleDiff;
											}
										}
										points = FEllipse.GetVerticesInArc(ellipse,
											curveVertexCount, angleStart, sweep);
										foreach(FPoint pointItem in points)
										{
											plot = new PlotPointItem(PlotPointPenStatus.PenDown);
											plot.Point.X = pointItem.X;
											plot.Point.Y = pointItem.Y;
											result.Add(plot);
										}
									}
								}
								plot = new PlotPointItem(PlotPointPenStatus.PenDown);
								plot.Point.X = point.X = entryItem.Points[5];
								plot.Point.X = point.X = entryItem.Points[6];
								result.Add(plot);
							}
							break;
						case "C":
							//	Cubic Bezier curve.
							plot = null;
							if(entryItem.Points.Count > 5)
							{
								points = Bezier.GetCubicCurvePointsEquidistant(
									point,
									new FPoint(entryItem.Points[0], entryItem.Points[1]),
									new FPoint(entryItem.Points[2], entryItem.Points[3]),
									new FPoint(entryItem.Points[4], entryItem.Points[5]),
									curveVertexCount);
								foreach(FPoint pointItem in points)
								{
									plot = new PlotPointItem(PlotPointPenStatus.PenDown);
									plot.Point.X = pointItem.X;
									plot.Point.Y = pointItem.Y;
									result.Add(plot);
								}
								if(plot != null)
								{
									point.X = plot.Point.X;
									point.Y = plot.Point.Y;
									previousControlC = new FPoint(
										entryItem.Points[2], entryItem.Points[3]);
								}
							}
							break;
						case "H":
							//	Horizontal line to.
							if(entryItem.Points.Count > 0)
							{
								plot = new PlotPointItem(PlotPointPenStatus.PenDown);
								plot.Point.X = point.X = entryItem.Points[0];
								plot.Point.Y = point.Y;
								result.Add(plot);
							}
							break;
						case "L":
							//	Line to.
							if(entryItem.Points.Count > 1)
							{
								plot = new PlotPointItem(PlotPointPenStatus.PenDown);
								plot.Point.X = point.X = entryItem.Points[0];
								plot.Point.Y = point.Y = entryItem.Points[1];
								result.Add(plot);
							}
							break;
						case "M":
							//	Move to.
							if(entryItem.Points.Count > 1)
							{
								plot = new PlotPointItem(PlotPointPenStatus.PenUp);
								plot.Point.X = point.X = entryItem.Points[0];
								plot.Point.Y = point.Y = entryItem.Points[1];
								result.Add(plot);
							}
							break;
						case "Q":
							//	Quadratic Bezier.
							plot = null;
							if(entryItem.Points.Count > 3)
							{
								points = Bezier.GetQuadraticCurvePointsEquidistant(
									point,
									new FPoint(entryItem.Points[0], entryItem.Points[1]),
									new FPoint(entryItem.Points[2], entryItem.Points[3]),
									curveVertexCount);
								foreach(FPoint pointItem in points)
								{
									plot = new PlotPointItem(PlotPointPenStatus.PenDown);
									plot.Point.X = pointItem.X;
									plot.Point.Y = pointItem.Y;
									result.Add(plot);
								}
								if(plot != null)
								{
									point.X = plot.Point.X;
									point.Y = plot.Point.Y;
									previousControlQ = new FPoint(
										entryItem.Points[0], entryItem.Points[1]);
								}
							}
							break;
						case "S":
							//	Batch Cubic Bezier.
							//	In this method, the starting point is the previous
							//	ending point and the first control point is the
							//	mirrored offset of the last control point from the
							//	previous ending point. In other words, if
							//	Cubic 10,10 20,20 30,30 40,40
							//	and
							//	S 90,90 100,100
							//	then
							//	S Cubic 40,40 50,50 90,90 100,100
							//	because 40 + (40 - 30) = 50
							plot = null;
							if(previousControlC == null)
							{
								previousControlC = new FPoint(0f, 0f);
							}
							if(entryItem.Points.Count > 3)
							{
								points = Bezier.GetCubicCurvePointsEquidistant(
									point,
									new FPoint(
										point.X + (point.X - previousControlC.X),
										point.Y + (point.Y - previousControlC.Y)),
									new FPoint(entryItem.Points[0], entryItem.Points[1]),
									new FPoint(entryItem.Points[2], entryItem.Points[3]),
									curveVertexCount);
								foreach(FPoint pointItem in points)
								{
									plot = new PlotPointItem(PlotPointPenStatus.PenDown);
									plot.Point.X = pointItem.X;
									plot.Point.Y = pointItem.Y;
									result.Add(plot);
								}
								if(plot != null)
								{
									point.X = plot.Point.X;
									point.Y = plot.Point.Y;
									previousControlC = new FPoint(
										entryItem.Points[0], entryItem.Points[1]);
								}
							}
							break;
						case "T":
							//	Batch Quadratic Bezier.
							plot = null;
							if(previousControlQ == null)
							{
								previousControlQ = new FPoint(0f, 0f);
							}
							if(entryItem.Points.Count > 1)
							{
								previousControlQ.X = point.X + (point.X - previousControlQ.X);
								previousControlQ.Y = point.Y + (point.Y - previousControlQ.Y);
								points = Bezier.GetQuadraticCurvePointsEquidistant(
									point,
									new FPoint(previousControlQ.X, previousControlQ.Y),
									new FPoint(entryItem.Points[0], entryItem.Points[1]),
									curveVertexCount);
								foreach(FPoint pointItem in points)
								{
									plot = new PlotPointItem(PlotPointPenStatus.PenDown);
									plot.Point.X = pointItem.X;
									plot.Point.Y = pointItem.Y;
									result.Add(plot);
								}
								if(plot != null)
								{
									point.X = plot.Point.X;
									point.Y = plot.Point.Y;
								}
							}
							break;
						case "V":
							//	Vertical line to.
							if(entryItem.Points.Count > 0)
							{
								plot = new PlotPointItem(PlotPointPenStatus.PenDown);
								plot.Point.X = point.X;
								plot.Point.Y = point.Y = entryItem.Points[0];
								result.Add(plot);
							}
							break;
						case "Z":
							//	Close path.
							if(result.Count > 0)
							{
								plot = new PlotPointItem(PlotPointPenStatus.PenDown);
								plot.Point.X = point.X = result[0].Point.X;
								plot.Point.Y = point.Y = result[0].Point.Y;
								result.Add(plot);
							}
							break;
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Parse																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Parse the elements in the raw SVG path string into individual plot
		/// point records.
		/// </summary>
		/// <param name="path">
		/// Raw SVG line drawing path command.
		/// </param>
		/// <returns>
		/// Collection of plot points items representing the path being drawn.
		/// </returns>
		public static PathEntryCollection Parse(string path)
		{
			PathEntryItem item = null;
			MatchCollection matches = null;
			string number = "";
			int paramCount = 2;
			int paramIndex = 0;
			PathEntryCollection result = new PathEntryCollection();
			string text = "";

			if(path?.Length > 0)
			{
				matches = Regex.Matches(path, ResourceMain.rxFindSvgTransformParams);
				foreach(Match matchItem in matches)
				{
					text = GetValue(matchItem, "param");
					if(IsNumeric(text))
					{
						//	This is a number.
						number = text;
						paramIndex++;
						if(item == null)
						{
							//	A plot item has not yet been created. By default, we are
							//	using the relative move.
							item = new PathEntryItem()
							{
								Action = "m"
							};
							result.Add(item);
						}
						else if(paramIndex > paramCount)
						{
							//	Create a new related action or ...
							//	Repeat the previous action.
							text = item.Action;
							//if(text.ToLower() == "m")
							//{
							//	//	Coordinates following the MOVETO are relative lineto
							//	//	commands.
							//	text = "l";
							//}
							if(text == "M")
							{
								//	Coordinates following the MOVETO are absolute.
								text = "L";
							}
							if(text == "m")
							{
								//	Coordinates following the moveto are relative.
								text = "l";
							}
							item = new PathEntryItem()
							{
								Action = text
							};
							result.Add(item);
							paramIndex = 1;
						}
						item.Points.Add(ToFloat(number));
					}
					else
					{
						//	This item is a plot command.
						item = new PathEntryItem()
						{
							Action = text
						};
						result.Add(item);
						switch(text)
						{
							case "A":
							case "a":
								paramCount = 7;
								break;
							case "C":
							case "c":
								paramCount = 6;
								break;
							case "H":
							case "h":
								paramCount = 1;
								break;
							case "L":
							case "l":
							case "M":
							case "m":
								paramCount = 2;
								break;
							case "Q":
							case "q":
							case "S":
							case "s":
								paramCount = 4;
								break;
							case "T":
							case "t":
								paramCount = 2;
								break;
							case "V":
							case "v":
								paramCount = 1;
								break;
							case "Z":
							case "z":
								paramCount = 0;
								break;
						}
						paramIndex = 0;
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	PathEntryItem																														*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Information about an individual SVG path entry.
	/// </summary>
	public class PathEntryItem
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
		//*	Action																																*
		//*-----------------------------------------------------------------------*
		private string mAction = "";
		/// <summary>
		/// Get/Set the action to take.
		/// </summary>
		public string Action
		{
			get { return mAction; }
			set { mAction = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Clone																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a deep copy of the provided path entry item.
		/// </summary>
		/// <param name="item">
		/// Reference to the item to clone.
		/// </param>
		/// <returns>
		/// Reference to the newly cloned item.
		/// </returns>
		public static PathEntryItem Clone(PathEntryItem item)
		{
			PathEntryItem result = new PathEntryItem();

			if(item != null)
			{
				result.mAction = item.mAction;
				foreach(float floatItem in item.mPoints)
				{
					result.mPoints.Add(floatItem);
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Points																																*
		//*-----------------------------------------------------------------------*
		private List<float> mPoints = new List<float>();
		/// <summary>
		/// Get a reference to the points assigned to this action.
		/// </summary>
		public List<float> Points
		{
			get { return mPoints; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
