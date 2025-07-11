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

using ConversionCalc;

namespace SvgPlotting
{
	//*-------------------------------------------------------------------------*
	//*	UnitGroupCollection																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of UnitGroupItem Items.
	/// </summary>
	public class UnitGroupCollection : List<UnitGroupItem>
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
	//*	UnitGroupItem																														*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Information identifying a unit of measurement and an associated group.
	/// </summary>
	public class UnitGroupItem
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
		/// Create a new Instance of the UnitGroupItem Item.
		/// </summary>
		public UnitGroupItem()
		{
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new Instance of the UnitGroupItem Item.
		/// </summary>
		/// <param name="conversion">
		/// Reference to the conversion being associated.
		/// </param>
		/// <param name="groupName">
		/// Name of the group being associated.
		/// </param>
		public UnitGroupItem(ConversionDefinitionItem conversion, string groupName)
		{
			mConversion = conversion;
			if(mConversion != null)
			{
				mUnitName = mConversion.Name;
			}
			if(groupName?.Length > 0)
			{
				mGroupName = groupName;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Conversion																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Conversion">Conversion</see>.
		/// </summary>
		private ConversionDefinitionItem mConversion = null;
		/// <summary>
		/// Get/Set a reference to the conversion unit associated with this
		/// relation.
		/// </summary>
		public ConversionDefinitionItem Conversion
		{
			get { return mConversion; }
			set { mConversion = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GroupName																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="GroupName">GroupName</see>.
		/// </summary>
		private string mGroupName = "";
		/// <summary>
		/// Get/Set the name of the group with which this unit is associated.
		/// </summary>
		public string GroupName
		{
			get { return mGroupName; }
			set { mGroupName = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	UnitName																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="UnitName">UnitName</see>.
		/// </summary>
		private string mUnitName = "";
		/// <summary>
		/// Get/Set the name of the unit.
		/// </summary>
		public string UnitName
		{
			get { return mUnitName; }
			set { mUnitName = value; }
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

}
