// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;

namespace fxpa
{
	public delegate void ModifiedDelegate(IManagedTreeNode node);

	/// <summary>
	/// Summary description for ITreeNode.
	/// </summary>
	public interface IManagedTreeNode
	{
		string TreeText
		{
			get;
		}

		IManagedTreeNode[] TreeChildren
		{
			get;
		}

		event ModifiedDelegate TreeModifiedEvent;

		int TreeImageIndex
		{
			get;
		}
	}
}
