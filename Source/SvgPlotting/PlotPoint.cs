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

using Geometry;

namespace SvgPlotting
{
	//*-------------------------------------------------------------------------*
	//*	PlotPointCollection																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of PlotPointItem Items.
	/// </summary>
	public class PlotPointCollection : List<PlotPointItem>
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
		//*	_Implicit PlotPointCollection = List<FVector2>													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Cast the List&lt;FVector2&gt; instance to a PlotPointCollection.
		/// </summary>
		/// <param name="points">
		/// Reference to the list of general points to convert.
		/// </param>
		/// <returns>
		/// Reference to the newly constructed plot point collection.
		/// </returns>
		public static implicit operator PlotPointCollection(List<FVector2> points)
		{
			int index = 0;
			PlotPointItem plotPoint = null;
			PlotPointCollection result = new PlotPointCollection();

			if(points?.Count > 0)
			{
				foreach(FVector2 pointItem in points)
				{
					plotPoint = new PlotPointItem()
					{
						PenStatus = (index == 0 ?
							PlotPointPenStatus.PenUp : PlotPointPenStatus.PenDown)
					};
					FVector2.TransferValues(pointItem, plotPoint.Point);
					index++;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Add																																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Add a new item to the collection by member values.
		/// </summary>
		/// <param name="penStatus">
		/// The status of the pen.
		/// </param>
		/// <param name="point">
		/// Reference to the point to be recorded.
		/// </param>
		/// <returns>
		/// Reference to the newly created and added plot point item.
		/// </returns>
		public PlotPointItem Add(PlotPointPenStatus penStatus, FVector2 point)
		{
			PlotPointItem result = new PlotPointItem();

			result.PenStatus = penStatus;
			if(point != null)
			{
				FVector2.TransferValues(point, result.Point);
			}
			this.Add(result);
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ClosestPoint																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the closest point to the provided location from the supplied
		/// collection of points.
		/// </summary>
		/// <param name="location">
		/// Reference to the location to match or approximate.
		/// </param>
		/// <param name="points">
		/// Reference to the collection of points to test.
		/// </param>
		/// <returns>
		/// Reference to the member of the points location closest to the
		/// specified location, if found. Otherwise, null.
		/// </returns>
		public static FVector2 ClosestPoint(FVector2 location,
			PlotPointCollection points)
		{
			List<float> distances = new List<float>();
			int minIndex = -1;
			float minValue = 0;
			FVector2 result = null;

			if(location != null && points?.Count > 0)
			{
				foreach(PlotPointItem pointItem in points)
				{
					distances.Add(
						Math.Abs(Trig.GetLineDistance(
							location.X, location.Y,
							pointItem.Point.X, pointItem.Point.Y)));
				}
				minValue = distances.Min();
				minIndex = distances.IndexOf(minValue);
				result = points[minIndex].Point;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ClosestPointIndex																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the index of the closest point to the provided location from the
		/// supplied collection of points.
		/// </summary>
		/// <param name="location">
		/// Reference to the location to match or approximate.
		/// </param>
		/// <param name="points">
		/// Reference to the collection of points to test.
		/// </param>
		/// <returns>
		/// Index of the member of the points location closest to the
		/// specified location, if found. Otherwise, -1.
		/// </returns>
		public static int ClosestPointIndex(FVector2 location,
			PlotPointCollection points)
		{
			List<float> distances = new List<float>();
			int minIndex = -1;
			float minValue = 0;
			int result = -1;

			if(location != null && points?.Count > 0)
			{
				foreach(PlotPointItem pointItem in points)
				{
					distances.Add(
						Math.Abs(Trig.GetLineDistance(
							location.X, location.Y,
							pointItem.Point.X, pointItem.Point.Y)));
				}
				minValue = distances.Min();
				minIndex = distances.IndexOf(minValue);
				result = minIndex;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetCenter																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the center coordinate of all of the points in the path.
		/// </summary>
		/// <param name="points">
		/// Reference to the collection of plot points to check.
		/// </param>
		/// <returns>
		/// Reference to the centroid coordinate of the specified path.
		/// </returns>
		public static FVector2 GetCenter(PlotPointCollection points)
		{
			FVector2 result = new FVector2();

			if(points?.Count > 0)
			{
				result.X = points.Average(x => x.Point.X);
				result.Y = points.Average(y => y.Point.Y);
			}
			return result;
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	PlotPointItem																														*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Information about an individual plot point.
	/// </summary>
	public class PlotPointItem
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
		/// Create a new instance of the PlotPointItem item.
		/// </summary>
		public PlotPointItem()
		{
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the PlotPointItem item.
		/// </summary>
		/// <param name="penStatus">
		/// Initial status of the pen for this point.
		/// </param>
		public PlotPointItem(PlotPointPenStatus penStatus)
		{
			mPenStatus = penStatus;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	PenStatus																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="PenStatus">PenStatus</see>.
		/// </summary>
		private PlotPointPenStatus mPenStatus = PlotPointPenStatus.PenUp;
		/// <summary>
		/// Get/Set the pen status for this point.
		/// </summary>
		public PlotPointPenStatus PenStatus
		{
			get { return mPenStatus; }
			set { mPenStatus = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Point																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Point">Point</see>.
		/// </summary>
		private FVector2 mPoint = new FVector2();
		/// <summary>
		/// Get/Set a reference to the literal point.
		/// </summary>
		public FVector2 Point
		{
			get { return mPoint; }
			set { mPoint = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
