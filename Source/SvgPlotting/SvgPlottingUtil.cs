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

using ConversionCalc;
using Geometry;
using Html;

namespace SvgPlotting
{
	//*-------------------------------------------------------------------------*
	//*	SvgPlottingUtil																													*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Global functionality for the SVG Plotting library.
	/// </summary>
	public class SvgPlottingUtil
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//	TODO: Write that the only containers currently supported are a and g.
		/// <summary>
		/// The currently accepted container types.
		/// </summary>
		private static string[] mContainerTypes = new string[]
		{
			"a", "g"
		};

		//	TODO: Write that the only shapes currently supported are circle, ellipse, line, path, polygon, polyline, and rect.
		/// <summary>
		/// The currently recognized shape types.
		/// </summary>
		private static string[] mShapeTypes = new string[]
		{
			"circle", "ellipse", "line", "path", "polygon", "polyline", "rect"
		};

		//	TODO: Write that 3D transforms are not yet supported.
		//	TODO: Write that text plotting is not yet supported.
		/// <summary>
		/// Dynamic conversion unit groups.
		/// </summary>
		private static UnitGroupCollection mUnitGroups = new UnitGroupCollection();

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* ApplyMatrix																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Apply the contents 2x2 matrix to the provided collection of points.
		/// </summary>
		/// <param name="points">
		/// Reference to a collection of points to which the matrix will be appled.
		/// </param>
		/// <param name="anchor">
		/// Reference to the anchor for the point collection.
		/// </param>
		/// <param name="matrix">
		/// Reference to the 2x2 matrix to apply to each of the points in the
		/// collection.
		/// </param>
		public static void ApplyMatrix(PlotPointCollection points, FPoint anchor,
			FMatrix2 matrix)
		{
			int count = 0;
			FPoint home = null;
			int index = 0;
			FPoint point = null;

			if(points?.Count > 0 && anchor != null && matrix != null)
			{

				count = points.Count;
				home = FPoint.Negate(anchor);
				for(index = 0; index < count; index++)
				{
					point = points[index].Point;
					FPoint.Translate(point, home);
					point = (FPoint)FMatrix2.Multiply(matrix, point);
					FPoint.Translate(point, anchor);
					points[index].Point = point;
				}
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Apply the contents 4x4 matrix to the provided collection of points.
		/// </summary>
		/// <param name="points">
		/// Reference to a collection of points to which the matrix will be appled.
		/// </param>
		/// <param name="anchor">
		/// Reference to the anchor for the point collection.
		/// </param>
		/// <param name="matrix">
		/// Reference to the 4x4 matrix to apply to each of the points in the
		/// collection.
		/// </param>
		public static void ApplyMatrix(PlotPointCollection points, FPoint anchor,
			FMatrix4 matrix)
		{
			int count = 0;
			FPoint home = null;
			int index = 0;
			FPoint point = null;

			if(points?.Count > 0 && anchor != null && matrix != null)
			{

				count = points.Count;
				home = FPoint.Negate(anchor);
				for(index = 0; index < count; index++)
				{
					point = points[index].Point;
					FPoint.Translate(point, home);
					point = FMatrix4.Multiply(matrix, (FVector4)point);
					FPoint.Translate(point, anchor);
					points[index].Point = point;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ApplyRotation																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Apply the specified rotation to the provided collection of points.
		/// </summary>
		/// <param name="points">
		/// Reference to a collection of points to which the rotation will be
		/// appled.
		/// </param>
		/// <param name="anchor">
		/// Reference to the anchor for the point collection.
		/// </param>
		/// <param name="rotation">
		/// The rotation to apply, in radians.
		/// </param>
		public static void ApplyRotation(PlotPointCollection points, FPoint anchor,
			float rotation)
		{
			int count = 0;
			FPoint home = null;
			int index = 0;
			FPoint point = null;

			if(points?.Count > 0 && anchor != null && rotation != 0f)
			{

				count = points.Count;
				home = FPoint.Negate(anchor);
				for(index = 0; index < count; index++)
				{
					point = points[index].Point;
					FPoint.Translate(point, home);
					point = FPoint.Rotate(point, rotation);
					FPoint.Translate(point, anchor);
					points[index].Point = point;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ApplyScale																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Apply the scale to the collection of points.
		/// </summary>
		/// <param name="points">
		/// Reference to a collection of points to be scaled.
		/// </param>
		/// <param name="scaleX">
		/// The X-axis scale to apply.
		/// </param>
		/// <param name="scaleY">
		/// The Y-axis scale to apply.
		/// </param>
		/// <remarks>
		/// In this version, it is assumed that all points are already converted to
		/// absolute values.
		/// </remarks>
		public static void ApplyScale(PlotPointCollection points,
			float scaleX, float scaleY)
		{
			int count = 0;
			int index = 0;
			FPoint point = null;

			if(points?.Count > 0)
			{
				count = points.Count;
				for(index = 0; index < count; index++)
				{
					point = points[index].Point;
					point.X *= scaleX;
					point.Y *= scaleY;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ApplyTranslation																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Apply the translation to the collection of points.
		/// </summary>
		/// <param name="points">
		/// Reference to a collection of points to be translated.
		/// </param>
		/// <param name="translationX">
		/// The X-axis translation to apply.
		/// </param>
		/// <param name="translationY">
		/// The Y-axis translation to apply.
		/// </param>
		/// <remarks>
		/// In this version, it is assumed that all points are already converted to
		/// absolute values.
		/// </remarks>
		public static void ApplyTranslation(PlotPointCollection points,
			float translationX, float translationY)
		{
			int count = 0;
			int index = 0;
			FPoint point = null;

			if(points?.Count > 0)
			{
				count = points.Count;
				for(index = 0; index < count; index++)
				{
					point = points[index].Point;
					point.X += translationX;
					point.Y += translationY;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* FillTransformations																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fill the provided string collection with the transform values found in
		/// the specified node and its ancestors.
		/// </summary>
		/// <param name="node">
		/// Reference to the node at which to begin the search for transform
		/// operations.
		/// </param>
		/// <param name="transformations">
		/// List of transform values to be filled with the findings.
		/// </param>
		public static void FillTransformations(HtmlNodeItem node,
			List<string> transformations)
		{
			string attributeValue = "";
			int count = 0;
			int index = 0;
			MatchCollection matches = null;

			if(node != null && transformations != null)
			{
				attributeValue = GetPropertyValue(node, "transform");
				if(attributeValue.Length > 0)
				{
					matches = Regex.Matches(attributeValue,
						ResourceMain.rxCssTransformFunction);
					count = matches.Count;
					for(index = count - 1; index > -1; index--)
					{
						transformations.Insert(0, GetValue(matches[index], "function"));
					}
				}
				FillTransformations(node.ParentNode, transformations);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetCoordinatePairs																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a collection of coordinates from a list of coordinate pairs,
		/// formatted in CSS style.
		/// </summary>
		/// <param name="coordinates">
		/// The list of value pairs to parse as coordinates.
		/// </param>
		/// <returns>
		/// Reference to a list of FPoint coordinates.
		/// </returns>
		public static List<FPoint> GetCoordinatePairs(string coordinates)
		{
			MatchCollection matches = null;
			List<FPoint> result = new List<FPoint>();

			if(coordinates?.Length > 0)
			{
				matches = Regex.Matches(coordinates, ResourceMain.rxXYCoordinatePair);
				foreach(Match matchItem in matches)
				{
					result.Add(new FPoint(
						ToFloat(GetValue(matchItem, "x")),
						ToFloat(GetValue(matchItem, "y"))));
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetCurrentFontSize																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the current font size, in pixels.
		/// </summary>
		/// <param name="svg">
		/// Reference to the SVG object in focus.
		/// </param>
		/// <param name="node">
		/// Reference to the node for which the active font size will be found.
		/// </param>
		/// <returns>
		/// The current font size of the specified node, if legitimate. Otherwise,
		/// 15.9996 (12pt).
		/// </returns>
		/// <remarks>
		/// GetCurrentFontSize is similar to GetDocumentFontSize because they
		/// both recurse backward up the chain through their parent predecessors.
		/// However, GetDocumentFontSize will update each time it finds a
		/// source closer to the root, while GetCurrentFontSize updates only
		/// the first time backward it finds a source.
		/// </remarks>
		public static float GetCurrentFontSize(SvgImageItem svg, HtmlNodeItem node)
		{
			float result = 15.9996f;
			string text = "";

			if(node != null)
			{
				text = GetPropertyValue(node, "font-size");
				if(text.Length > 0)
				{
					//	Font size was found at this level.
					result = GetPixelValue(svg, text, node.ParentNode);
				}
				else if(node.ParentNode != null)
				{
					result = GetCurrentFontSize(svg, node.ParentNode);
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetDocumentFontSize																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the font size of the root document, in pixels.
		/// </summary>
		/// <param name="svg">
		/// Reference to the SVG object in focus.
		/// </param>
		/// <param name="node">
		/// Reference to the node for which the root document font size will be
		/// found.
		/// </param>
		/// <param name="defaultValue">
		/// The default value to be assigned if a specific font size is not found
		/// on the way to the root.
		/// </param>
		/// <returns>
		/// The font size in the root document element, if legitimate. Otherwise,
		/// 15.9996 (12pt).
		/// </returns>
		/// <remarks>
		/// GetDocumentFontSize is similar to GetCurrentFontSize because they
		/// both recurse backward up the chain through their parent predecessors.
		/// However, GetDocumentFontSize will update each time it finds a
		/// source closer to the root, while GetCurrentFontSize updates only
		/// the first time backward it finds a source.
		/// </remarks>
		public static float GetDocumentFontSize(SvgImageItem svg, HtmlNodeItem node,
			float defaultValue = 15.9996f)
		{
			float result = defaultValue;
			string text = "";

			if(node != null)
			{
				text = GetPropertyValue(node, "font-size");
				if(text.Length > 0)
				{
					//	Font size was found at this level.
					result = GetPixelValue(svg, text, node.ParentNode);
				}
				if(node.ParentNode != null)
				{
					result = GetDocumentFontSize(svg, node.ParentNode, result);
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetFloatValue																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the floating point representation of the caller's unit value.
		/// </summary>
		/// <param name="svg">
		/// Reference to the SVG object being processed.
		/// </param>
		/// <param name="number">
		/// A text numeric value, in HTML units.
		/// </param>
		/// <param name="node">
		/// Reference the node to inspect.
		/// </param>
		/// <returns>
		/// A floating point base value corresponding to the caller's unit value,
		/// aligned to the base type for the conversion category.
		/// </returns>
		public static float GetFloatValue(SvgImageItem svg, string number,
			HtmlNodeItem node)
		{
			ConversionDefinitionItem definition = null;
			ConversionDomainItem domain = null;
			Match match = null;
			string numeric = "";
			float result = 0f;
			string suffix = "";

			if(number?.Length > 0 && node != null)
			{
				match = Regex.Match(number, ResourceMain.rxNumericWithSuffix);
				if(match.Success)
				{
					numeric = GetValue(match, "numeric");
					suffix = GetValue(match, "suffix");
					if(numeric.Length > 0)
					{
						result = ToFloat(numeric);
						domain = mHConverter.FindDomain(suffix);
						if(domain != null)
						{
							if(domain.DomainName == "Length")
							{
								result = GetPixelValue(svg, number, node);
							}
							else
							{
								//	Once the domain is known, let's find the base unit.
								definition = domain.Conversions.FirstOrDefault(x =>
									x.EntryType == ConversionDefinitionEntryType.Base);
								if(definition != null)
								{
									result = (float)mHConverter.Convert((double)result,
										suffix, definition.Name);
								}
							}
						}
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetPixelValue																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the pixel value of the caller's number string.
		/// </summary>
		/// <param name="svg">
		/// Reference to the SVG image being processed.
		/// </param>
		/// <param name="number">
		/// The string number value for which the pixel value will be found.
		/// </param>
		/// <param name="node">
		/// Reference to the current HTML node in context to be used if relative
		/// units need to be converted.
		/// </param>
		/// <returns>
		/// The single floating point value representing the pixel value of the
		/// caller's numeric string.
		/// </returns>
		public static float GetPixelValue(SvgImageItem svg, string number,
			HtmlNodeItem node)
		{
			Match match = null;
			string numeric = "";
			float result = 0f;
			float size = 0f;
			string suffix = "";
			UnitGroupItem unitGroup = null;

			if(number?.Length > 0 && svg != null)
			{
				match = Regex.Match(number, ResourceMain.rxNumericWithSuffix);
				if(match.Success)
				{
					numeric = GetValue(match, "numeric");
					suffix = GetValue(match, "suffix");
					if(numeric.Length > 0)
					{
						result = ToFloat(numeric);
						unitGroup = mUnitGroups.FirstOrDefault(x =>
							x.UnitName == suffix.ToLower());
						if(unitGroup != null)
						{
							//	This item has a configurable value.
							switch(unitGroup.GroupName)
							{
								case "FontSize":
									//	The font size is retrieved in pixels.
									if(unitGroup.UnitName == "rem")
									{
										size = GetDocumentFontSize(svg, node);
									}
									else
									{
										size = GetCurrentFontSize(svg, node);
									}
									if(size != 0d)
									{
										switch(unitGroup.UnitName)
										{
											case "ch":
											//	From current font size.
											//	Width of the '0' character.
											case "ex":
												//	From current font size.
												unitGroup.Conversion.Value = size / 2d;
												break;
											case "em":
											//	From current font size.
											case "rem":
												//	From document's base font size.
												unitGroup.Conversion.Value = size;
												break;
										}
									}
									break;
								case "ViewSize":
									switch(unitGroup.UnitName)
									{
										case "vmax":
											//	From current viewport size.
											unitGroup.Conversion.Value = Math.Max(
												svg.GlobalImageSize.Width,
												svg.GlobalImageSize.Height) / 100d;
											break;
										case "vmin":
											//	From current viewport size.
											unitGroup.Conversion.Value = Math.Min(
												svg.GlobalImageSize.Width,
												svg.GlobalImageSize.Height) / 100d;
											break;
										case "vh":
											//	From current viewport size.
											unitGroup.Conversion.Value =
												(svg.GlobalImageSize.Height / 100d);
											break;
										case "vw":
											//	From current viewport size.
											unitGroup.Conversion.Value =
												(svg.GlobalImageSize.Width / 100d);
											break;
									}
									break;
							}
						}
						result = (float)mHConverter.Convert((double)result, suffix, "px");
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetPropertyValue																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the value of the specified property - whether it happens to
		/// reside within the element or as a property value within the style.
		/// </summary>
		/// <param name="node">
		/// Reference to the node from which to read the property.
		/// </param>
		/// <param name="propertyName">
		/// Name of the property to read.
		/// </param>
		/// <returns>
		/// Value of the specified property, either in the attributes collection,
		/// or as a property within the style attribute, if found. Otherwise,
		/// an empty string.
		/// </returns>
		public static string GetPropertyValue(HtmlNodeItem node,
			string propertyName)
		{
			string result = "";

			if(node != null && propertyName?.Length > 0)
			{
				result = node.Attributes.GetStyle(propertyName);
				if(result.Length == 0)
				{
					result = node.Attributes.GetValue(propertyName);
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetSystemValue																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the real-world system value of the specified property, in mm.
		/// </summary>
		/// <param name="svg">
		/// Reference to the SVG image being worked on.
		/// </param>
		/// <param name="node">
		/// Reference to the node to be inspected.
		/// </param>
		/// <param name="propertyName">
		/// Name of the property to read.
		/// </param>
		/// <returns>
		/// Value of the specified property, in mm, if found. Otherwise, 0.
		/// </returns>
		public static float GetSystemValue(SvgImageItem svg, HtmlNodeItem node,
			string propertyName)
		{
			float result = 0f;

			if(node != null && propertyName?.Length > 0)
			{
				result =
					GetPixelValue(svg, GetPropertyValue(node, propertyName), node);
				result = (float)mHConverter.Convert((double)result, "px", "mm");
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetTransformations																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a collection of transformation values found in the specified
		/// node and its ancestors.
		/// </summary>
		/// <param name="node">
		/// Reference to the node which may directly contain or inherit
		/// transformations.
		/// </param>
		/// <returns>
		/// Reference to the collection of transformation values present in the
		/// specified node and its ancestors, if found. Otherwise, an empty list.
		/// </returns>
		public static List<string> GetTransformations(HtmlNodeItem node)
		{
			List<string> result = new List<string>();

			if(node != null)
			{
				FillTransformations(node, result);
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetValue																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the value of the specified group member in the provided match.
		/// </summary>
		/// <param name="match">
		/// Reference to the match to be inspected.
		/// </param>
		/// <param name="groupName">
		/// Name of the group for which the value will be found.
		/// </param>
		/// <returns>
		/// The value found in the specified group, if found. Otherwise, empty
		/// string.
		/// </returns>
		public static string GetValue(Match match, string groupName)
		{
			string result = "";

			if(match != null && match.Groups[groupName] != null &&
				match.Groups[groupName].Value != null)
			{
				result = match.Groups[groupName].Value;
			}
			return result;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the value of the specified group member in a match found with
		/// the provided source and pattern.
		/// </summary>
		/// <param name="source">
		/// Source string to search.
		/// </param>
		/// <param name="pattern">
		/// Regular expression pattern to apply.
		/// </param>
		/// <param name="groupName">
		/// Name of the group for which the value will be found.
		/// </param>
		/// <returns>
		/// The value found in the specified group, if found. Otherwise, empty
		/// string.
		/// </returns>
		public static string GetValue(string source, string pattern,
			string groupName)
		{
			Match match = null;
			string result = "";

			if(source?.Length > 0 && pattern?.Length > 0 && groupName?.Length > 0)
			{
				match = Regex.Match(source, pattern);
				if(match.Success)
				{
					result = GetValue(match, groupName);
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* HasTransformations																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the specified node has
		/// transformations.
		/// </summary>
		/// <param name="node">
		/// Reference to the node which may directly contain or inherit
		/// transformations.
		/// </param>
		/// <returns>
		/// True if the node or its parent structure contains transformations.
		/// Otherwise, false.
		/// </returns>
		public static bool HasTransformations(HtmlNodeItem node)
		{
			bool result = false;

			if(node != null)
			{
				if(GetPropertyValue(node, "transform").Length > 0)
				{
					result = true;
				}
				else
				{
					result = HasTransformations(node.ParentNode);
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	HConverter																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="HConverter">HConverter</see>.
		/// </summary>
		private static ConversionCalc.Converter mHConverter = new Converter("");
		/// <summary>
		/// Get a reference to the session-wide converter for this library.
		/// </summary>
		public static ConversionCalc.Converter HConverter
		{
			get { return mHConverter; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* InitializeConverter																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Initialize the value converter for this specialized use.
		/// </summary>
		internal static void InitializeConverter()
		{
			ConversionDefinitionItem cnv = null;
			ConversionDomainItem domain = null;

			//	Global scale:
			//	Horizontal:
			//	svg.viewBox.width / svg.width
			//	Vertical:
			//	svg.viewBox.height / svg.height
			//	Global translation:
			//	Horizontal: 0 - svg.viewBox.x
			//	Vertical: 0 - svg.viewBox.y
			mHConverter.Data.Domains.Clear();
			domain = new ConversionDomainItem()
			{
				DomainName = "Angles"
			};
			mHConverter.Data.Domains.Add(domain);
			cnv = new ConversionDefinitionItem()
			{
				EntryType = ConversionDefinitionEntryType.Base,
				Name = "turn",
				Value = 1d
			};
			cnv.Aliases.Add("turns");
			domain.Conversions.Add(cnv);
			cnv = new ConversionDefinitionItem()
			{
				EntryType = ConversionDefinitionEntryType.Conversion,
				Name = "deg",
				Value = 0.00277777777777777777777777777778d
			};
			cnv.Aliases.Add("degrees");
			domain.Conversions.Add(cnv);
			cnv = new ConversionDefinitionItem()
			{
				EntryType = ConversionDefinitionEntryType.Conversion,
				Name = "rad",
				Value = 0.15915494309189533576888376337251d
			};
			cnv.Aliases.Add("radians");
			domain.Conversions.Add(cnv);
			cnv = new ConversionDefinitionItem()
			{
				EntryType = ConversionDefinitionEntryType.Conversion,
				Name = "grad",
				Value = 0.0025d
			};
			cnv.Aliases.Add("grads");
			domain.Conversions.Add(cnv);
			domain = new ConversionDomainItem()
			{
				DomainName = "Length"
			};
			domain.Remarks.AddRange(new string[]
			{
				"HTML length units, in this instance, except percentage, ",
				"preserve px as the common base.",
				"Wherever '%' is expressed, it must be handled separately with the ",
				"ancestor container size.",
				"",
				"Before using the 'ch' unit, it must be set to the size of the ",
				"'0' character in the active font.",
				"",
				"'em' must be set to the size of the currently active font.",
				"",
				"'ex' must be set to the height of a lower case letter in the ",
				"active font.",
				"",
				"'rem' must be set to the document's root element font size.",
				"",
				"'vh' must be set to the document's view height.",
				"",
				"'vmax' must be set to the document's view height or width, ",
				"whichever is greater.",
				"'vmin' must be set to the document's view height or width, ",
				"whichever is smaller.",
				"'vw' must be set to the document's view width."
			});
			mHConverter.Data.Domains.Add(domain);

			cnv = new ConversionDefinitionItem()
			{
				EntryType = ConversionDefinitionEntryType.Base,
				Name = "px",
				Value = 1d
			};
			domain.Conversions.Add(cnv);

			cnv = new ConversionDefinitionItem()
			{
				EntryType = ConversionDefinitionEntryType.Conversion,
				Name = "ch",
				Value = 7.9998d
			};
			domain.Conversions.Add(cnv);
			mUnitGroups.Add(new UnitGroupItem(cnv, "FontSize"));

			cnv = new ConversionDefinitionItem()
			{
				EntryType = ConversionDefinitionEntryType.Conversion,
				Name = "cm",
				Value = 37.795d
			};
			domain.Conversions.Add(cnv);

			cnv = new ConversionDefinitionItem()
			{
				EntryType = ConversionDefinitionEntryType.Conversion,
				Name = "em",
				Value = 15.9996d
			};
			domain.Conversions.Add(cnv);
			mUnitGroups.Add(new UnitGroupItem(cnv, "FontSize"));

			cnv = new ConversionDefinitionItem()
			{
				EntryType = ConversionDefinitionEntryType.Conversion,
				Name = "ex",
				Value = 7.9998d
			};
			domain.Conversions.Add(cnv);
			mUnitGroups.Add(new UnitGroupItem(cnv, "FontSize"));

			cnv = new ConversionDefinitionItem()
			{
				EntryType = ConversionDefinitionEntryType.Conversion,
				Name = "in",
				Value = 96d
			};
			domain.Conversions.Add(cnv);

			cnv = new ConversionDefinitionItem()
			{
				EntryType = ConversionDefinitionEntryType.Conversion,
				Name = "mm",
				Value = 3.7795d
			};
			domain.Conversions.Add(cnv);

			cnv = new ConversionDefinitionItem()
			{
				EntryType = ConversionDefinitionEntryType.Conversion,
				Name = "pc",
				Value = 16d
			};
			domain.Conversions.Add(cnv);

			cnv = new ConversionDefinitionItem()
			{
				EntryType = ConversionDefinitionEntryType.Conversion,
				Name = "pt",
				Value = 1.3333d
			};
			domain.Conversions.Add(cnv);

			cnv = new ConversionDefinitionItem()
			{
				EntryType = ConversionDefinitionEntryType.Conversion,
				Name = "rem",
				Value = 15.9996d
			};
			domain.Conversions.Add(cnv);
			mUnitGroups.Add(new UnitGroupItem(cnv, "FontSize"));

			cnv = new ConversionDefinitionItem()
			{
				EntryType = ConversionDefinitionEntryType.Conversion,
				Name = "vh",
				Value = 1d
			};
			domain.Conversions.Add(cnv);
			mUnitGroups.Add(new UnitGroupItem(cnv, "ViewSize"));

			cnv = new ConversionDefinitionItem()
			{
				EntryType = ConversionDefinitionEntryType.Conversion,
				Name = "vmax",
				Value = 1d
			};
			domain.Conversions.Add(cnv);
			mUnitGroups.Add(new UnitGroupItem(cnv, "ViewSize"));

			cnv = new ConversionDefinitionItem()
			{
				EntryType = ConversionDefinitionEntryType.Conversion,
				Name = "vmin",
				Value = 1d
			};
			domain.Conversions.Add(cnv);
			mUnitGroups.Add(new UnitGroupItem(cnv, "ViewSize"));

			cnv = new ConversionDefinitionItem()
			{
				EntryType = ConversionDefinitionEntryType.Conversion,
				Name = "vw",
				Value = 1d
			};
			domain.Conversions.Add(cnv);
			mUnitGroups.Add(new UnitGroupItem(cnv, "ViewSize"));

			mInitialized = true;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Initialized																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Initialized">Initialized</see>.
		/// </summary>
		private static bool mInitialized = false;
		/// <summary>
		/// Get a value indicating whether the converter has been initialized.
		/// </summary>
		internal static bool Initialized
		{
			get { return mInitialized; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* IsContainer																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the specified node type is currently
		/// recognized as a container type.
		/// </summary>
		/// <param name="nodeType">
		/// Name of the node type to check.
		/// </param>
		/// <returns>
		/// True if the specified node type is a container. Otherwise, false.
		/// </returns>
		public static bool IsContainer(string nodeType)
		{
			bool result = false;

			if(nodeType?.Length > 0)
			{
				result = mContainerTypes.Contains(nodeType.ToLower());
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* IsLowerCase																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the specified string is lower case.
		/// </summary>
		/// <param name="value">
		/// The value to inspect.
		/// </param>
		/// <returns>
		/// True if the entire string is lower case. Otherwise, false.
		/// </returns>
		public static bool IsLowerCase(string value)
		{
			char[] chars = null;
			bool result = true;

			if(value?.Length > 0)
			{
				chars = value.ToCharArray();
				foreach(char charItem in chars)
				{
					if(Char.IsUpper(charItem))
					{
						result = false;
						break;
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* IsNumeric																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the specified string is totally
		/// numeric.
		/// </summary>
		/// <param name="value">
		/// The value to inspect.
		/// </param>
		/// <param name="floatingPoint">
		/// Value indicating whether to allow floating point values.
		/// </param>
		/// <returns>
		/// True if the caller's value is a valid number. Otherwise, false.
		/// </returns>
		public static bool IsNumeric(string value, bool floatingPoint = true)
		{
			Match match = null;
			bool result = false;

			if(value?.Length > 0)
			{
				if(floatingPoint)
				{
					match = Regex.Match(value, ResourceMain.rxNumericalFloat);
				}
				else
				{
					match = Regex.Match(value, ResourceMain.rxNumericalInt);
				}
				if(match.Success && match.Length == value.Length)
				{
					result = true;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* IsShape																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the specified node type is currently
		/// recognized as a shape type.
		/// </summary>
		/// <param name="nodeType">
		/// Name of the node type to check.
		/// </param>
		/// <returns>
		/// True if the specified node type is a shape. Otherwise, false.
		/// </returns>
		public static bool IsShape(string nodeType)
		{
			bool result = false;

			if(nodeType?.Length > 0)
			{
				result = mShapeTypes.Contains(nodeType.ToLower());
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* IsVisible																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the specified node is visible, either
		/// at this level or a parent level.
		/// </summary>
		/// <param name="node">
		/// Reference to the node to inspect.
		/// </param>
		/// <returns>
		/// True if the node is visible. Otherwise, false.
		/// </returns>
		public static bool IsVisible(HtmlNodeItem node)
		{
			string display = "";
			bool result = true;

			if(node != null)
			{
				display = node.Attributes.GetStyle("display").ToLower();
				if(display.Length > 0)
				{
					result = (display != "none");
				}
				else
				{
					result = IsVisible(node.ParentNode);
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ProcessNode																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Recursively process the specified node and its children.
		/// </summary>
		/// <param name="svg">
		/// Reference to the SVG image object in context.
		/// </param>
		/// <param name="node">
		/// Reference to the node currently in focus.
		/// </param>
		/// <remarks>
		/// In this version a container will have children and a shape won't.
		/// </remarks>
		public static void ProcessNode(SvgImageItem svg, HtmlNodeItem node)
		{
			int circleVertexCount = 200;
			string content = "";
			FEllipse ellipse = null;
			int index = 0;
			FLine line = null;
			FPoint location = null;
			PathEntryCollection pathEntries = null;
			FPoint point = null;
			int pointCount = 0;
			int pointIndex = 0;
			PlotPointCollection points = new PlotPointCollection();
			float radius = 0f;
			FAreaRounded rect = null;

			if(svg != null && node != null)
			{
				circleVertexCount = svg.CurveVertexCount * 4;
				if(IsContainer(node.NodeType))
				{
					if(IsVisible(node))
					{
						foreach(HtmlNodeItem nodeItem in node.Nodes)
						{
							ProcessNode(svg, nodeItem);
						}
					}
				}
				else if(IsShape(node.NodeType))
				{
					if(svg.PlotPoints.Count > 0)
					{
						location = svg.PlotPoints[svg.PlotPoints.Count - 1].Point;
					}
					else
					{
						location = new FPoint();
					}
					switch(node.NodeType.ToLower())
					{
						case "circle":
							radius = GetSystemValue(svg, node, "r");
							ellipse = new FEllipse(
								GetSystemValue(svg, node, "cx"),
								GetSystemValue(svg, node, "cy"),
								radius, radius);
							points = FEllipse.GetVertices(ellipse, circleVertexCount, 0f);
							Transform(svg, points, ellipse.Center, node);
							break;
						case "ellipse":
							ellipse = new FEllipse(
								GetSystemValue(svg, node, "cx"),
								GetSystemValue(svg, node, "cy"),
								GetSystemValue(svg, node, "rx"),
								GetSystemValue(svg, node, "ry"));
							points = FEllipse.GetVertices(ellipse, circleVertexCount, 0f);
							Transform(svg, points, ellipse.Center, node);
							break;
						case "line":
							line = new FLine(
								GetSystemValue(svg, node, "x1"),
								GetSystemValue(svg, node, "y1"),
								GetSystemValue(svg, node, "x2"),
								GetSystemValue(svg, node, "y2"));
							points = new List<FPoint>();
							points.Add(PlotPointPenStatus.PenUp, line.PointA);
							points.Add(PlotPointPenStatus.PenDown, line.PointB);
							//Transform(points, FLine.GetCenter(line), node);
							//	In this version, lines will be anchored on the first point.
							Transform(svg, points, line.PointA, node);
							break;
						case "path":
							pathEntries =
								PathEntryCollection.Parse(GetPropertyValue(node, "d"));
							pathEntries =
								PathEntryCollection.ConvertToAbsolute(pathEntries, location);
							points = PathEntryCollection.GetVertices(pathEntries,
								svg.CurveVertexCount);
							point = PlotPointCollection.GetCenter(points);
							Transform(svg, points, point, node);
							break;
						case "polygon":
						case "polyline":
							content = GetPropertyValue(node, "points");
							points = GetCoordinatePairs(content);
							point = PlotPointCollection.GetCenter(points);
							Transform(svg, points, point, node);
							break;
						case "rect":
							rect = new FAreaRounded(
								GetSystemValue(svg, node, "x"),
								GetSystemValue(svg, node, "y"),
								GetSystemValue(svg, node, "width"),
								GetSystemValue(svg, node, "height"),
								GetSystemValue(svg, node, "rx"),
								GetSystemValue(svg, node, "ry")
								);
							points = FAreaRounded.GetVertices(rect, svg.CurveVertexCount);
							Transform(svg, points, FArea.GetCenter(rect), node);
							break;
					}
					if(points.Count > 0)
					{
						//	Convert to real-world coordinates.
						ApplyScale(points, svg.GlobalScale.ScaleX, svg.GlobalScale.ScaleY);
						//	In this version, we aren't going to cross the edge of a shape
						//	directly. Instead, we'll find the closest point on the
						//	transformed edge to the active location and make that the
						//	starting point. After crossing the last point in the collection,
						//	wrap back around to the first item and fill in all of the
						//	segments that haven't yet been covered, stopping at the initial
						//	starting point.
						if(points[0].PenStatus == PlotPointPenStatus.PenUp)
						{
							//	We are going to be moving to our own starting place.
							points.RemoveAt(0);
						}
						pointIndex = PlotPointCollection.ClosestPointIndex(location, points);
						if(pointIndex > -1)
						{
							point = points[pointIndex].Point;
							if(!point.Equals(location))
							{
								svg.PlotPoints.Add(PlotPointPenStatus.PenUp, point);
								location = point;
							}
						}
						pointCount = points.Count;
						for(index = pointIndex + 1; index < pointCount; index++)
						{
							svg.PlotPoints.Add(points[index]);
						}
						for(index = 0; index <= pointIndex; index++)
						{
							svg.PlotPoints.Add(points[index]);
						}
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToDouble																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Provide fail-safe conversion of string to numeric value.
		/// </summary>
		/// <param name="value">
		/// Value to convert.
		/// </param>
		/// <returns>
		/// Double-precision floating point value. 0 if not convertible.
		/// </returns>
		public static double ToDouble(object value)
		{
			double result = 0d;
			if(value != null)
			{
				result = ToDouble(value.ToString());
			}
			return result;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Provide fail-safe conversion of string to numeric value.
		/// </summary>
		/// <param name="value">
		/// Value to convert.
		/// </param>
		/// <returns>
		/// Double-precision floating point value. 0 if not convertible.
		/// </returns>
		public static double ToDouble(string value)
		{
			double result = 0d;
			try
			{
				result = double.Parse(value);
			}
			catch { }
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToFloat																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Provide fail-safe conversion of string to numeric value.
		/// </summary>
		/// <param name="value">
		/// Value to convert.
		/// </param>
		/// <returns>
		/// Floating point value. 0 if not convertible.
		/// </returns>
		public static float ToFloat(object value)
		{
			float result = 0f;
			if(value != null)
			{
				result = ToFloat(value.ToString());
			}
			return result;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Provide fail-safe conversion of string to numeric value.
		/// </summary>
		/// <param name="value">
		/// Value to convert.
		/// </param>
		/// <returns>
		/// Floating point value. 0 if not convertible.
		/// </returns>
		public static float ToFloat(string value)
		{
			float result = 0f;
			try
			{
				result = float.Parse(value);
			}
			catch { }
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Transform																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Perform any transformations needed on the provided point list.
		/// </summary>
		/// <param name="svg">
		/// Reference to the SVG image object currently in focus.
		/// </param>
		/// <param name="points">
		/// Reference to the list of points to transform.
		/// </param>
		/// <param name="anchor">
		/// Reference to the anchor reference of the point set.
		/// </param>
		/// <param name="node">
		/// Reference to the node which may contain transformations, either at
		/// this level, or toward the base.
		/// </param>
		public static void Transform(SvgImageItem svg, PlotPointCollection points,
			FPoint anchor, HtmlNodeItem node)
		{
			int count = 0;
			string functionName = "";
			List<float> functionParameters = null;
			string functionParams = "";
			int index = 0;
			Match match = null;
			MatchCollection matches = null;
			FMatrix2 matrix2 = null;
			FMatrix4 matrix4 = null;
			List<string> transformations = null;
			float x = 0f;
			float y = 0f;

			if(points?.Count > 0 && anchor != null &&
				node != null && HasTransformations(node))
			{
				functionParameters = new List<float>();
				transformations = GetTransformations(node);
				foreach(string transformationItem in transformations)
				{
					match = Regex.Match(transformationItem,
						ResourceMain.rxCssTransformFunctionParts);
					if(match.Success)
					{
						functionName = GetValue(match, "functionName").ToLower();
						functionParams = GetValue(match, "functionParams").ToLower();
						if(functionParams.Length > 0)
						{
							matches = Regex.Matches(functionParams,
								ResourceMain.rxCssTransformFunctionParams);
							foreach(Match matchItem in matches)
							{
								functionParameters.Add(
									GetFloatValue(svg, GetValue(matchItem, "paramValue"), node));
							}
							//	All of the parameter values are now either in
							//	raw, radians or pixels, depending upon the kind of function.
							count = functionParameters.Count;
							switch(functionName)
							{
								case "matrix":
									matrix4 = new FMatrix4();
									for(index = 0; index < count; index++)
									{
										switch(index)
										{
											case 0:
												//	a
												matrix4.Values[0, 0] = functionParameters[index];
												break;
											case 1:
												//	b
												matrix4.Values[1, 0] = functionParameters[index];
												break;
											case 2:
												//	c
												matrix4.Values[0, 1] = functionParameters[index];
												break;
											case 3:
												//	d
												matrix4.Values[1, 1] = functionParameters[index];
												break;
											case 4:
												//	tx
												matrix4.Values[0, 3] = functionParameters[index];
												break;
											case 5:
												//	ty
												matrix4.Values[1, 3] = functionParameters[index];
												break;
										}
									}
									ApplyMatrix(points, anchor, matrix4);
									break;
								case "rotate":
									if(count > 0)
									{
										ApplyRotation(points, anchor, functionParameters[0]);
									}
									break;
								case "scale":
									x = 0f;
									y = 0f;
									if(count > 0)
									{
										x = functionParameters[0];
									}
									if(count > 1)
									{
										y = functionParameters[1];
									}
									else
									{
										y = x;
									}
									ApplyScale(points, x, y);
									break;
								case "skew":
									x = 0f;
									y = 0f;
									if(count > 0)
									{
										x = functionParameters[0];
									}
									if(count > 1)
									{
										y = functionParameters[1];
									}
									else
									{
										y = x;
									}
									matrix2 = new FMatrix2();
									matrix2.Values[0, 0] = 1f;
									matrix2.Values[0, 1] = y;
									matrix2.Values[1, 0] = x;
									matrix2.Values[1, 1] = 1f;
									ApplyMatrix(points, anchor, matrix2);
									break;
								case "translate":
									x = 0f;
									y = 0f;
									if(count > 0)
									{
										x = functionParameters[0];
									}
									if(count > 1)
									{
										y = functionParameters[1];
									}
									else
									{
										y = x;
									}
									ApplyTranslation(points, x, y);
									break;
							}
						}
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

}
