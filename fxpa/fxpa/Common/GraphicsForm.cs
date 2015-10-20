// -----
// GNU General Public License
// The Open Forex Platform is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Open Forex Platform is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace ForexPlatformFrontEnd
{
    public partial class GraphicsForm : Form
    {
        public ChartControl Chart
        {
            get { return graphControlZed1; }
        }

        public GraphicsForm(string title)
        {
            InitializeComponent();
            
            this.Text = title;
        }
    }
}
