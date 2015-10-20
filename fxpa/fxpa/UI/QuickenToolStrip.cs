using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace System.Windows.Forms
{
	public class QuickenToolStrip : ToolStrip
	{
		private	QuickenRenderer		_renderer = new QuickenRenderer();

		public QuickenToolStrip() : base()
		{
			// Set renderer
			this.Renderer = _renderer;
		}

		private bool _drawBorder=false;

		[DefaultValue(false)]
		public bool DrawBorder
		{
			get { return _drawBorder; }
			set
			{
				_drawBorder = value;
				_renderer.DrawBorder = value;
			}
		}
	
		protected override void OnRendererChanged(EventArgs e)
		{
			if (this.Renderer != _renderer)
			{
				// Force to our renderer
				this.Renderer = _renderer;
			}
			else
			{
				base.OnRendererChanged(e);
			}
		}
	}

	public class QuickenProfessionalToolStrip : ToolStrip
	{
		private QuickenProfessionalRenderer _renderer = new QuickenProfessionalRenderer();
		private QuickenColorTable			_colorTable;

		public QuickenProfessionalToolStrip() : base()
		{
			// Set renderer
			_renderer.RoundedEdges = false;
			this.Renderer = _renderer;
			this.GripStyle = ToolStripGripStyle.Hidden;
			this._colorTable = _renderer.QuickenColorTable;
		}

		protected override void OnBackColorChanged(EventArgs e)
		{
			// Call base
			base.OnBackColorChanged(e);

			// Set backcolor
			_colorTable.BackColor = this.BackColor;
		}

		protected override void OnRendererChanged(EventArgs e)
		{
			if (this.Renderer != _renderer)
			{
				// Force to our renderer
				this.Renderer = _renderer;
			}
			else
			{
				base.OnRendererChanged(e);
			}
		}
	}

	public class QuickenMenuStrip : MenuStrip
	{
		private ToolStripRenderer _renderer = new QuickenRenderer();

		public QuickenMenuStrip() : base()
		{
			this.Renderer = _renderer;
		}

		protected override void OnRendererChanged(EventArgs e)
		{
			if (this.Renderer != _renderer)
			{
				// Force to our renderer
				this.Renderer = _renderer;
			}
			else
			{
				base.OnRendererChanged(e);
			}
		}
	}

	public class QuickenRenderer : ToolStripSystemRenderer
	{
		private bool _drawBorder;

		public bool DrawBorder
		{
			get { return _drawBorder; }
			set { _drawBorder = value; }
		}
	
		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			if (this.DrawBorder)
			{
				Color			color = Color.FromArgb(102, 102, 102);
				Pen				pen;
				ToolStripItem	item=null;
				
				if (e.ToolStrip.Items.Count > 0)
				{
					item = e.ToolStrip.Items[0];

					using (pen = new Pen(color))
					{
						e.Graphics.DrawLine(pen, 0, e.ToolStrip.Height - 1, item.Margin.Left, e.ToolStrip.Height - 1);
						e.Graphics.DrawLine(pen, item.Width + item.Margin.Left, e.ToolStrip.Height - 1, e.ToolStrip.Width, e.ToolStrip.Height - 1);
					}
				}
			}
		}
	}

	public class QuickenHeaderToolStrip : ToolStrip
	{
		private QuickenHeaderRenderer _renderer = new QuickenHeaderRenderer();

		public QuickenHeaderToolStrip() : base()
		{
			// Set renderer
			_renderer.RoundedEdges = false;
			this.Renderer = _renderer;
			this.GripStyle = ToolStripGripStyle.Hidden;
		}

		protected override void OnRendererChanged(EventArgs e)
		{
			if (this.Renderer != _renderer)
			{
				// Force to our renderer
				this.Renderer = _renderer;
			}
			else
			{
				base.OnRendererChanged(e);
			}
		}
	}

	public class QuickenHeaderRenderer : ToolStripProfessionalRenderer
	{
		protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
		{
			// Call base
			base.OnRenderToolStripBackground(e);

			// Do green
			ToolStrip	toolStrip = e.ToolStrip;
			Graphics	g = e.Graphics;
			Rectangle	bounds = new Rectangle(Point.Empty, toolStrip.Size);

			if (bounds.Width > 0 && bounds.Height > 0)
			{
				using (Brush b = new LinearGradientBrush(bounds, Color.FromArgb(69, 104, 139), Color.FromArgb(159, 176, 195), LinearGradientMode.Horizontal))
				{
					g.FillRectangle(b, bounds);
				}
			}
		}

		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
		}
	}

	public class QuickenProfessionalRenderer : ToolStripProfessionalRenderer
	{
		public QuickenProfessionalRenderer() : base(new QuickenColorTable())
		{
		}

		public QuickenColorTable QuickenColorTable
		{
			get { return this.ColorTable as QuickenColorTable; }
		}

		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
		}

		protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
		{
			// Call base
			//base.OnRenderButtonBackground(e);

			int X1 = 0;
			int X2 = e.Item.Width - 2;
			int Y1 = 0;
			int Y2 = e.Item.Height - 2;

			Color C1;
			Color C2;
			Color C3;
			Color C4;
			Color C5;


			if (e.Item.Selected)
			{
				C1 = Color.FromArgb(102, 102, 102);
				C2 = Color.FromArgb(51, 51, 51);
				C3 = Color.White;
				C4 = Color.White;
				C5 = Color.FromArgb(229, 229, 162);
			}
			else
			{
				C1 = Color.FromArgb(153, 153, 153);
				C2 = Color.FromArgb(102, 102, 102);
				C3 = Color.White;
				C4 = Color.FromArgb(250, 250, 250);
				C5 = Color.FromArgb(196, 191, 173);
			}

			Brush		brush;
			Rectangle	rect = new Rectangle(e.Item.ContentRectangle.X+1, e.Item.ContentRectangle.Y+1, e.Item.ContentRectangle.Width - 3, e.Item.ContentRectangle.Height - 3);
			
			using (brush = new LinearGradientBrush(rect, C4, C5, LinearGradientMode.Vertical))
			{
				e.Graphics.FillRectangle(brush, rect);
			}

			Pen		pen;

			using (pen = new Pen(C1))
			{
				// Draw Border
				e.Graphics.DrawLine(pen, X1, Y1 + 1, X1, Y2 - 1);
				e.Graphics.DrawLine(pen, X1 + 1, Y1, X2 - 1, Y1);
			}

			using (pen = new Pen(C3))
			{
				e.Graphics.DrawLine(pen, X1+1, Y1 + 1, X1+1, Y2 - 1);
				e.Graphics.DrawLine(pen, X1 + 1, Y1+1, X2 - 1, Y1+1);
			}

			using (pen = new Pen(C2))
			{
				e.Graphics.DrawLine(pen, X2, Y1 + 1, X2, Y2 - 1);
				e.Graphics.DrawLine(pen, X1 + 1, Y2, X2 - 1, Y2);
			}
		}
	}

	public class QuickenColorTable : ProfessionalColorTable
	{
		public QuickenColorTable()
		{
			this.UseSystemColors = true;
			_backColor = base.ToolStripGradientEnd;
		}

		private Color _backColor;

		public Color BackColor
		{
			get { return _backColor; }
			set
			{
				_backColor = value;
			}
		}

		public override Color ButtonCheckedGradientBegin
		{
			get { return base.ButtonCheckedGradientBegin; }
		}

		public override Color ButtonCheckedGradientMiddle
		{
			get { return base.ButtonCheckedGradientMiddle; }
		}

		public override Color ButtonCheckedGradientEnd
		{
			get { return base.ButtonCheckedGradientEnd; }
		}

		public override Color ButtonCheckedHighlight
		{
			get { return base.ButtonCheckedHighlight; }
		}

		public override Color ButtonCheckedHighlightBorder
		{
			get { return base.ButtonCheckedHighlightBorder; }
		}

		public override Color ButtonPressedBorder
		{
			get { return base.ButtonPressedBorder; }
		}

		public override Color ButtonPressedGradientBegin
		{
			get { return base.ButtonPressedGradientBegin; }
		}

		public override Color ButtonPressedGradientEnd
		{
			get { return base.ButtonPressedGradientEnd; }
		}

		public override Color ButtonPressedGradientMiddle
		{
			get { return base.ButtonPressedGradientMiddle; }
		}

		public override Color ButtonPressedHighlight
		{
			get { return base.ButtonPressedHighlight; }
		}

		public override Color ButtonPressedHighlightBorder
		{
			get { return base.ButtonPressedHighlightBorder; }
		}

		public override Color ButtonSelectedBorder
		{
			get { return base.ButtonSelectedBorder; }
		}

		public override Color ButtonSelectedGradientBegin
		{
			get { return base.ButtonSelectedGradientBegin; }
		}

		public override Color ButtonSelectedGradientEnd
		{
			get { return base.ButtonSelectedGradientEnd; }
		}

		public override Color ButtonSelectedGradientMiddle
		{
			get { return base.ButtonSelectedGradientMiddle; }
		}

		public override Color ButtonSelectedHighlight
		{
			get { return base.ButtonSelectedHighlight; }
		}

		public override Color ButtonSelectedHighlightBorder
		{
			get { return base.ButtonSelectedHighlightBorder; }
		}

		public override Color CheckBackground
		{
			get { return base.CheckBackground; }
		}

		public override Color CheckPressedBackground
		{
			get { return base.CheckPressedBackground; }
		}

		public override Color CheckSelectedBackground
		{
			get { return base.CheckSelectedBackground; }
		}

		public override Color GripDark
		{
			get { return base.GripDark; }
		}

		public override Color GripLight
		{
			get { return base.GripLight; }
		}

		public override Color ImageMarginGradientBegin
		{
			get { return base.ImageMarginGradientBegin; }
		}

		public override Color ImageMarginGradientEnd
		{
			get { return base.ImageMarginGradientEnd; }
		}

		public override Color ImageMarginGradientMiddle
		{
			get { return base.ImageMarginGradientMiddle; }
		}

		public override Color ImageMarginRevealedGradientBegin
		{
			get { return base.ImageMarginRevealedGradientBegin; }
		}

		public override Color ImageMarginRevealedGradientEnd
		{
			get { return base.ImageMarginRevealedGradientEnd; }
		}

		public override Color ImageMarginRevealedGradientMiddle
		{
			get { return base.ImageMarginRevealedGradientMiddle; }
		}

		public override Color MenuBorder
		{
			get { return base.MenuBorder; }
		}

		public override Color MenuItemBorder
		{
			get { return base.MenuItemBorder; }
		}

		public override Color MenuItemPressedGradientBegin
		{
			get { return base.MenuItemPressedGradientBegin; }
		}

		public override Color MenuItemPressedGradientEnd
		{
			get { return base.MenuItemPressedGradientEnd; }
		}

		public override Color MenuItemPressedGradientMiddle
		{
			get { return base.MenuItemPressedGradientMiddle; }
		}

		public override Color MenuItemSelected
		{
			get { return base.MenuItemSelected; }
		}

		public override Color MenuItemSelectedGradientBegin
		{
			get { return base.MenuItemSelectedGradientBegin; }
		}

		public override Color MenuItemSelectedGradientEnd
		{
			get { return base.MenuItemSelectedGradientEnd; }
		}

		public override Color MenuStripGradientBegin
		{
			get { return base.MenuStripGradientBegin; }
		}

		public override Color MenuStripGradientEnd
		{
			get { return base.MenuStripGradientEnd; }
		}

		public override Color OverflowButtonGradientBegin
		{
			get { return base.OverflowButtonGradientBegin; }
		}

		public override Color OverflowButtonGradientEnd
		{
			get { return base.OverflowButtonGradientEnd; }
		}

		public override Color OverflowButtonGradientMiddle
		{
			get { return base.OverflowButtonGradientMiddle; }
		}

		public override Color RaftingContainerGradientBegin
		{
			get { return base.RaftingContainerGradientBegin; }
		}

		public override Color RaftingContainerGradientEnd
		{
			get { return base.RaftingContainerGradientEnd; }
		}

		public override Color SeparatorDark
		{
			get { return base.SeparatorDark; }
		}

		public override Color SeparatorLight
		{
			get { return base.SeparatorLight; }
		}

		public override Color StatusStripGradientBegin
		{
			get { return base.StatusStripGradientBegin; }
		}

		public override Color StatusStripGradientEnd
		{
			get { return base.StatusStripGradientEnd; }
		}

		public override Color ToolStripBorder
		{
			get { return base.ToolStripBorder; }
		}

		public override Color ToolStripContentPanelGradientBegin
		{
			get { return base.ToolStripContentPanelGradientBegin; }
		}

		public override Color ToolStripContentPanelGradientEnd
		{
			get { return base.ToolStripContentPanelGradientEnd; }
		}

		public override Color ToolStripDropDownBackground
		{
			get { return base.ToolStripDropDownBackground; }
		}

		public override Color ToolStripGradientBegin
		{
			get { return _backColor; }
		}

		public override Color ToolStripGradientEnd
		{
			get { return _backColor; }
		}

		public override Color ToolStripGradientMiddle
		{
			get { return _backColor; }
		}

		public override Color ToolStripPanelGradientBegin
		{
			get { return base.ToolStripPanelGradientBegin; }
		}

		public override Color ToolStripPanelGradientEnd
		{
			get { return base.ToolStripPanelGradientEnd; }
		}
	}
}
