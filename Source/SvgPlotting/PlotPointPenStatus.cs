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

namespace SvgPlotting
{
	//*-------------------------------------------------------------------------*
	//*	PlotPointPenStatus																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Enumeration of pen states for plot points.
	/// </summary>
	public enum PlotPointPenStatus
	{
		/// <summary>
		/// No pen status specified or unknown.
		/// </summary>
		None = 0,
		/// <summary>
		/// The pen is up (move-to).
		/// </summary>
		PenUp,
		/// <summary>
		/// The pen is down (line-to).
		/// </summary>
		PenDown
	}
	//*-------------------------------------------------------------------------*

}
