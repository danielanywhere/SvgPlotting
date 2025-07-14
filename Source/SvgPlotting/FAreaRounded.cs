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
using System.Text;

using Geometry;

namespace SvgPlotting
{
	//*-------------------------------------------------------------------------*
	//*	FAreaRounded																														*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// A rectangle with rounded corners.
	/// </summary>
	public class FAreaRounded : FArea
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
		/// Create a new instance of the FAreaRounded item.
		/// </summary>
		/// <param name="x">
		/// Horizontal coordinate of the rectangle.
		/// </param>
		/// <param name="y">
		/// Vertical coordinate of the rectangle.
		/// </param>
		/// <param name="width">
		/// Width of the rectangle.
		/// </param>
		/// <param name="height">
		/// Height of the rectangle.
		/// </param>
		/// <param name="roundedX">
		/// Rounded corner X radius.
		/// </param>
		/// <param name="roundedY">
		/// Rounded corner Y radius.
		/// </param>
		public FAreaRounded(float x, float y, float width, float height,
			float roundedX, float roundedY) : base(x, y, width, height)
		{
			mRoundedX = Math.Min(roundedX, width / 2f);
			mRoundedY = Math.Min(roundedY, height / 2f);

			if(mRoundedX != 0f && mRoundedY == 0f)
			{
				mRoundedY = Math.Min(mRoundedX, height / 2f);
			}
			else if(mRoundedY != 0f && mRoundedX == 0f)
			{
				mRoundedX = Math.Min(mRoundedY, width / 2f);
			}

		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetVertices																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a collection of vertices composing the supplied rounded corner
		/// rectangle.
		/// </summary>
		/// <param name="area">
		/// Reference to the rectangle to plot.
		/// </param>
		/// <param name="curveCount">
		/// Count of points in each curve.
		/// </param>
		/// <returns>
		/// Reference to a collection of points composing a rounded rectangle.
		/// </returns>
		public static List<FVector2> GetVertices(FAreaRounded area, int curveCount)
		{
			int cornerCount = 0;
			FEllipse pattern = null;
			List<FVector2> result = new List<FVector2>();

			if(area.mRoundedX != 0f && area.mRoundedY != 0f && curveCount != 0)
			{
				cornerCount = curveCount / 4;
				pattern = new FEllipse(0f, 0f, area.mRoundedX, area.mRoundedY);
				//	Top left center.
				pattern.Center = new FVector2(
					area.Left + area.mRoundedX, area.Top + area.mRoundedY);
				result.AddRange(FEllipse.GetVerticesInArc(pattern, cornerCount,
					Trig.DegToRad(90f), Trig.DegToRad(90f)));
				//	Bottom left center.
				pattern.Center = new FVector2(
					area.Left + area.mRoundedX, area.Bottom - area.mRoundedY);
				result.AddRange(FEllipse.GetVerticesInArc(pattern, cornerCount,
					180f, 90f));
				//	Bottom right center.
				pattern.Center = new FVector2(
					area.Right - area.mRoundedX, area.Bottom - area.mRoundedY);
				result.AddRange(FEllipse.GetVerticesInArc(pattern, cornerCount,
					Trig.DegToRad(270f), Trig.DegToRad(90f)));
				//	Top right center.
				pattern.Center = new FVector2(
					area.Right - area.mRoundedX, area.Top + area.mRoundedY);
				result.AddRange(FEllipse.GetVerticesInArc(pattern, cornerCount,
					Trig.DegToRad(0f), Trig.DegToRad(90f)));
			}
			else
			{
				//	This area doesn't have any rounding.
				result.Add(new FVector2(area.Left, area.Top));
				result.Add(new FVector2(area.Left, area.Bottom));
				result.Add(new FVector2(area.Right, area.Bottom));
				result.Add(new FVector2(area.Right, area.Top));
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	RoundedX																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="RoundedX">RoundedX</see>.
		/// </summary>
		private float mRoundedX = 0f;
		/// <summary>
		/// Get/Set the horizontal corner rounding radius.
		/// </summary>
		public float RoundedX
		{
			get { return mRoundedX; }
			set { mRoundedX = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	RoundedY																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="RoundedY">RoundedY</see>.
		/// </summary>
		private float mRoundedY = 0f;
		/// <summary>
		/// Get/Set the vertical corner rounding radius.
		/// </summary>
		public float RoundedY
		{
			get { return mRoundedY; }
			set { mRoundedY = value; }
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

}
