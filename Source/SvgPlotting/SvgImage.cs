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
using System.Diagnostics;
using System.Text;

using Geometry;
using Html;

using static SvgPlotting.SvgPlottingUtil;

namespace SvgPlotting
{
	//*-------------------------------------------------------------------------*
	//*	SvgImageCollection																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of SvgImageItem Items.
	/// </summary>
	public class SvgImageCollection : List<SvgImageItem>
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


	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	SvgImageItem																														*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Individual SVG image file object.
	/// </summary>
	public class SvgImageItem
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
		//*	_Constructor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new instance of the SvgImageItem item.
		/// </summary>
		public SvgImageItem()
		{
			if(!Initialized)
			{
				InitializeConverter();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Document																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Document">Document</see>.
		/// </summary>
		private HtmlDocument mDocument = null;
		/// <summary>
		/// Get a reference to the HTML document object model backing this image.
		/// </summary>
		public HtmlDocument Document
		{
			get { return mDocument; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GlobalImageSize																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for
		/// <see cref="GlobalImageSize">GlobalImageSize</see>.
		/// </summary>
		private FSize mGlobalImageSize = new FSize(1920f, 1080f);
		/// <summary>
		/// Get/Set a reference to the global image size.
		/// </summary>
		public FSize GlobalImageSize
		{
			get { return mGlobalImageSize; }
			set { mGlobalImageSize = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GlobalScale																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="GlobalScale">GlobalScale</see>.
		/// </summary>
		private FScale mGlobalScale = new FScale();
		/// <summary>
		/// Get/Set a reference to the global scale for the current file.
		/// </summary>
		/// <remarks>
		/// This value is calculated using the viewbox dimensions divided by
		/// the svg dimensions.
		/// </remarks>
		public FScale GlobalScale
		{
			get { return mGlobalScale; }
			set { mGlobalScale = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GlobalTranslation																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for
		/// <see cref="GlobalTranslation">GlobalTranslation</see>.
		/// </summary>
		private FVector2 mGlobalTranslation = new FVector2();
		/// <summary>
		/// Get/Set a reference to the global translation for this image.
		/// </summary>
		/// <remarks>
		/// This value represents the crop value defined in the viewbox. In most
		/// cases, vectors outside the viewbox window won't be accessible, but
		/// there are some cases in which this translation could represent
		/// a portion of the image 'hanging in space', for example.
		/// </remarks>
		public FVector2 GlobalTranslation
		{
			get { return mGlobalTranslation; }
			set { mGlobalTranslation = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Initialize																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Initialize the image information from an open HTML document.
		/// </summary>
		/// <param name="document">
		/// Reference to the HTML document used to back this image.
		/// </param>
		public void Initialize(HtmlDocument document)
		{
			HtmlNodeItem svg = null;
			string[] viewBox = null;
			float width = 0f;
			float height = 0f;

			if(document != null)
			{
				//	Calculate the global scale.
				svg = document.Nodes.FindMatch(x => x.NodeType.ToLower() == "svg");
				if(svg != null)
				{
					viewBox = svg.Attributes.GetValue("viewBox").Split(' ');
					if(viewBox.Length == 1 && viewBox[0] == "")
					{
						viewBox = new string[] { "0", "0", "0", "0" };
					}
					if(viewBox.Length > 0)
					{
						mGlobalTranslation.X = ToFloat(viewBox[0]);
					}
					if(viewBox.Length > 1)
					{
						mGlobalTranslation.Y = ToFloat(viewBox[1]);
					}
					if(viewBox.Length > 2)
					{
						width = ToFloat(viewBox[2]);
					}
					if(viewBox.Length > 3)
					{
						height = ToFloat(viewBox[3]);
					}
					mGlobalImageSize.Width =
						GetPixelValue(this, svg.Attributes.GetValue("width"), svg);
					mGlobalImageSize.Height =
						GetPixelValue(this, svg.Attributes.GetValue("height"), svg);
					if(width != 0f)
					{
						mGlobalScale.ScaleX = mGlobalImageSize.Width / width;
					}
					if(height != 0f)
					{
						mGlobalScale.ScaleY = mGlobalImageSize.Height / height;
					}
					Trace.WriteLine($"Image: {mGlobalImageSize}");
					Trace.WriteLine($"Scale: {mGlobalScale}");
					foreach(HtmlNodeItem nodeItem in svg.Nodes)
					{
						ProcessNode(this, nodeItem);
					}
				}
			}
			mDocument = document;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	PlotCommands																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="PlotCommands">PlotCommands</see>.
		/// </summary>
		private PlotPointCollection mPlotCommands = new PlotPointCollection();
		/// <summary>
		/// Get a reference to the collection of physical plot commands.
		/// </summary>
		public PlotPointCollection PlotCommands
		{
			get { return mPlotCommands; }
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

}
