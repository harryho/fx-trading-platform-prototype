using System.Drawing;
using System.Windows.Forms;
namespace fxpa
{
    partial class FxAnalyzerControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ToolStripLabel toolStripLabel2;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FxAnalyzerControl));
            this.panel1 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.priceListView = new fxpa.DoubleBufferListView();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.titleStrip1 = new System.Windows.Forms.Samples.TitleStrip();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonCreateSession = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCloseSession = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonWindowed = new System.Windows.Forms.ToolStripButton();
            this.toolStrip = new System.Windows.Forms.QuickenHeaderToolStrip();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.closeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonShowScrollbars = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonZoomIn = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonZoomOut = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonFit = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFitHorizontal = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFitVertical = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonCrosshair = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.speakerToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonShowLabels = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonShowTimeGaps = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonLimitView = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonAutoScrollToEnd = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripDropDownButtonSelectionMode = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButtonScrollMode = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButtonAppearance = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButtonColor = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDrawLineSegment = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDrawLine = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonHorizontalLine = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonVerticalLine = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFibonacci = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonText = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItemMIN1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMIN3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMIN5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMIN15 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMIN30 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMIN60 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMIN120 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDAY1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.timeLabel = new System.Windows.Forms.Label();
            this.weekStatBtn = new System.Windows.Forms.Button();
            this.triDayStatBtn = new System.Windows.Forms.Button();
            this.dayStatBtn = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStrip1.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.toolStripMain.SuspendLayout();
            this.panel2.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new System.Drawing.Size(55, 24);
            toolStripLabel2.Text = "Sessions";
            toolStripLabel2.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.MaximumSize = new System.Drawing.Size(250, 0);
            this.panel1.MinimumSize = new System.Drawing.Size(250, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(250, 444);
            this.panel1.TabIndex = 47;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(200, 100);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // priceListView
            // 
            this.priceListView.BackColor = System.Drawing.SystemColors.Window;
            this.priceListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.priceListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.priceListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.priceListView.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.priceListView.ForeColor = System.Drawing.SystemColors.WindowText;
            this.priceListView.FullRowSelect = true;
            this.priceListView.GridLines = true;
            this.priceListView.Location = new System.Drawing.Point(0, 0);
            this.priceListView.Name = "priceListView";
            this.priceListView.OwnerDraw = true;
            this.priceListView.ShowItemToolTips = true;
            this.priceListView.Size = new System.Drawing.Size(220, 347);
            this.priceListView.SmallImageList = this.imageList1;
            this.priceListView.TabIndex = 44;
            this.priceListView.UseCompatibleStateImageBehavior = false;
            this.priceListView.View = System.Windows.Forms.View.Details;
            this.priceListView.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.priceListView_ColumnWidthChanged);
            this.priceListView.SelectedIndexChanged += new System.EventHandler(this.priceListView_SelectedIndexChanged);
            this.priceListView.SizeChanged += new System.EventHandler(this.priceListView_SizeChanged);
            this.priceListView.DoubleClick += new System.EventHandler(this.priceListView_DoubleClick);
            this.priceListView.Click += new System.EventHandler(this.priceListView_Click);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Symbol";
            this.columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Bit";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Ask";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader4.Width = 40;
            // 
            // columnHeader5
            // 
            //this.columnHeader5.Text = "Profit/Loss";
            //this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "up.png");
            this.imageList1.Images.SetKeyName(1, "down.png");
            // 
            // titleStrip1
            // 
            this.titleStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(239)))), ((int)(((byte)(244)))));
            this.titleStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.titleStrip1.DrawEndLine = false;
            this.titleStrip1.GradientEndColor = System.Drawing.Color.White;
            this.titleStrip1.GradientStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(186)))), ((int)(((byte)(214)))));
            this.titleStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.titleStrip1.Lines = 10;
            this.titleStrip1.Location = new System.Drawing.Point(0, 97);
            this.titleStrip1.Name = "titleStrip1";
            this.titleStrip1.Size = new System.Drawing.Size(69, 25);
            this.titleStrip1.TabIndex = 45;
            // 
            // splitter2
            // 
            this.splitter2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.splitter2.Location = new System.Drawing.Point(220, 25);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(5, 483);
            this.splitter2.TabIndex = 49;
            this.splitter2.TabStop = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            toolStripLabel2,
            this.toolStripButtonCreateSession,
            this.toolStripButtonCloseSession,
            this.toolStripButton1,
            this.toolStripButtonWindowed});
            this.toolStrip1.Location = new System.Drawing.Point(215, 481);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(689, 27);
            this.toolStrip1.TabIndex = 50;
            this.toolStrip1.TabStop = true;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.Visible = false;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripButtonCreateSession
            // 
            this.toolStripButtonCreateSession.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCreateSession.Image")));
            this.toolStripButtonCreateSession.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCreateSession.Name = "toolStripButtonCreateSession";
            this.toolStripButtonCreateSession.Size = new System.Drawing.Size(50, 24);
            this.toolStripButtonCreateSession.Text = "New";
            this.toolStripButtonCreateSession.Visible = true;
            // 
            // toolStripButtonCloseSession
            // 
            this.toolStripButtonCloseSession.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonCloseSession.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCloseSession.Image")));
            this.toolStripButtonCloseSession.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCloseSession.Name = "toolStripButtonCloseSession";
            this.toolStripButtonCloseSession.Size = new System.Drawing.Size(57, 24);
            this.toolStripButtonCloseSession.Text = "Close";
            this.toolStripButtonCloseSession.Click += new System.EventHandler(this.toolStripButtonDeleteSession_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = global::fxpa.Properties.Resources.coins;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(105, 24);
            this.toolStripButton1.Tag = "del";
            this.toolStripButton1.Text = "AUDUSD[M60]";
            this.toolStripButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton_Click);
            // 
            // toolStripButtonWindowed
            // 
            this.toolStripButtonWindowed.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonWindowed.Enabled = false;
            this.toolStripButtonWindowed.Image = global::fxpa.Properties.Resources.photo_scenery;
            this.toolStripButtonWindowed.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonWindowed.Name = "toolStripButtonWindowed";
            this.toolStripButtonWindowed.Size = new System.Drawing.Size(86, 24);
            this.toolStripButtonWindowed.Text = "Windowed";
            this.toolStripButtonWindowed.Visible = true;
            // 
            // toolStrip
            // 
            this.toolStrip.BackColor = System.Drawing.Color.Silver;
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel3,
            this.toolStripLabel4,
            this.closeToolStripButton});
            this.toolStrip.Location = new System.Drawing.Point(225, 483);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(679, 25);
            this.toolStrip.TabIndex = 51;
            this.toolStrip.Text = "quickenHeaderToolStrip1";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripLabel3.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripLabel3.Margin = new System.Windows.Forms.Padding(25, 6, 0, -1);
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(0, 20);
            this.toolStripLabel3.Text = "toolStripLabel2";
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripLabel4.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripLabel4.Margin = new System.Windows.Forms.Padding(3, 8, 0, -2);
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(0, 19);
            this.toolStripLabel4.Text = "toolStripLabel3";
            // 
            // closeToolStripButton
            // 
            this.closeToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.closeToolStripButton.Enabled = false;
            this.closeToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("closeToolStripButton.Image")));
            this.closeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.closeToolStripButton.Name = "closeToolStripButton";
            this.closeToolStripButton.Size = new System.Drawing.Size(57, 22);
            this.closeToolStripButton.Text = "Close";
            this.closeToolStripButton.Click += new System.EventHandler(this.toolStripButtonDeleteSession_Click);
            // 
            // toolStripMain
            // 
            this.toolStripMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(217)))), ((int)(((byte)(206)))));
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonShowScrollbars,
            this.toolStripSeparator8,
            this.toolStripButtonZoomIn,
            this.toolStripButtonZoomOut,
            this.toolStripSeparator2,
            this.toolStripButtonFit,
            this.toolStripButtonFitHorizontal,
            this.toolStripButtonFitVertical,
            this.toolStripSeparator9,
            this.toolStripButtonCrosshair,
            this.toolStripSeparator7,
            this.speakerToolStripButton,
            this.toolStripButtonShowLabels,
            this.toolStripSeparator3,
            this.toolStripButtonShowTimeGaps,
            this.toolStripButtonLimitView,
            this.toolStripSeparator13,
            this.toolStripButtonAutoScrollToEnd,
            this.toolStripSeparator6,
            this.toolStripDropDownButtonSelectionMode,
            this.toolStripDropDownButtonScrollMode,
            this.toolStripDropDownButtonAppearance,
            this.toolStripButtonColor,
            this.toolStripButtonDrawLineSegment,
            this.toolStripButtonDrawLine,
            this.toolStripButtonHorizontalLine,
            this.toolStripButtonVerticalLine,
            this.toolStripButtonFibonacci,
            this.toolStripButtonText,
            this.toolStripDropDownButton1,
            this.toolStripButton2});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStripMain.Size = new System.Drawing.Size(904, 25);
            this.toolStripMain.TabIndex = 52;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // toolStripButtonShowScrollbars
            // 
            this.toolStripButtonShowScrollbars.CheckOnClick = true;
            this.toolStripButtonShowScrollbars.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonShowScrollbars.Enabled = false;
            this.toolStripButtonShowScrollbars.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonShowScrollbars.Image")));
            this.toolStripButtonShowScrollbars.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonShowScrollbars.Name = "toolStripButtonShowScrollbars";
            this.toolStripButtonShowScrollbars.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonShowScrollbars.Text = "Scrollbars";
            this.toolStripButtonShowScrollbars.ToolTipText = "Scroll Bar";
            this.toolStripButtonShowScrollbars.Visible = true;
            this.toolStripButtonShowScrollbars.Click += new System.EventHandler(this.toolStripButtonShowScrollbars_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator8.Visible = true;
            // 
            // toolStripButtonZoomIn
            // 
            this.toolStripButtonZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonZoomIn.Image = global::fxpa.Properties.Resources.magnifier_zoom_in;
            this.toolStripButtonZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonZoomIn.Name = "toolStripButtonZoomIn";
            this.toolStripButtonZoomIn.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonZoomIn.Text = "Zoom In";
            this.toolStripButtonZoomIn.ToolTipText = "Zoom In";
            this.toolStripButtonZoomIn.Click += new System.EventHandler(this.toolStripButtonZoomIn_Click);
            // 
            // toolStripButtonZoomOut
            // 
            this.toolStripButtonZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonZoomOut.Image = global::fxpa.Properties.Resources.magifier_zoom_out;
            this.toolStripButtonZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonZoomOut.Name = "toolStripButtonZoomOut";
            this.toolStripButtonZoomOut.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonZoomOut.Text = "Zoom Out";
            this.toolStripButtonZoomOut.ToolTipText = "Zoom Out";
            this.toolStripButtonZoomOut.Click += new System.EventHandler(this.toolStripButtonZoomOut_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonFit
            // 
            this.toolStripButtonFit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFit.Image = global::fxpa.Properties.Resources.arrow_in;
            this.toolStripButtonFit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFit.Name = "toolStripButtonFit";
            this.toolStripButtonFit.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonFit.Text = "Fit To Screen";
            this.toolStripButtonFit.ToolTipText = "Fit screen";
            this.toolStripButtonFit.Click += new System.EventHandler(this.toolStripButtonFit_Click);
            // 
            // toolStripButtonFitHorizontal
            // 
            this.toolStripButtonFitHorizontal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFitHorizontal.Enabled = true;
            this.toolStripButtonFitHorizontal.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonFitHorizontal.Image")));
            this.toolStripButtonFitHorizontal.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFitHorizontal.Name = "toolStripButtonFitHorizontal";
            this.toolStripButtonFitHorizontal.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonFitHorizontal.Text = "Fit To Screen Horizontal";
            this.toolStripButtonFitHorizontal.ToolTipText = "Layout Horizontal";
            this.toolStripButtonFitHorizontal.Visible = true;
            this.toolStripButtonFitHorizontal.Click += new System.EventHandler(this.toolStripButtonFitHorizontal_Click);
            // 
            // toolStripButtonFitVertical
            // 
            this.toolStripButtonFitVertical.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFitVertical.Enabled = true;
            this.toolStripButtonFitVertical.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonFitVertical.Image")));
            this.toolStripButtonFitVertical.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFitVertical.Name = "toolStripButtonFitVertical";
            this.toolStripButtonFitVertical.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonFitVertical.Text = "Fit To Screen Vertical";
            this.toolStripButtonFitVertical.ToolTipText = "Layout Vertical";
            this.toolStripButtonFitVertical.Visible = true;
            this.toolStripButtonFitVertical.Click += new System.EventHandler(this.toolStripButtonFitVertical_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonCrosshair
            // 
            this.toolStripButtonCrosshair.CheckOnClick = true;
            this.toolStripButtonCrosshair.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonCrosshair.Image = global::fxpa.Properties.Resources.kuf;
            this.toolStripButtonCrosshair.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCrosshair.Name = "toolStripButtonCrosshair";
            this.toolStripButtonCrosshair.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonCrosshair.Text = "Show Mouse Crosshair";
            this.toolStripButtonCrosshair.ToolTipText = "Cross Hair";
            this.toolStripButtonCrosshair.Click += new System.EventHandler(this.toolStripButtonCrosshair_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // speakerToolStripButton
            // 
            this.speakerToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.speakerToolStripButton.Image = global::fxpa.Properties.Resources.sound1;
            this.speakerToolStripButton.ImageTransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.speakerToolStripButton.Name = "speakerToolStripButton";
            this.speakerToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.speakerToolStripButton.Text = "Mute";
            this.speakerToolStripButton.Click += new System.EventHandler(this.speakerToolStripButton_Click);
            // 
            // toolStripButtonShowLabels
            // 
            this.toolStripButtonShowLabels.Checked = true;
            this.toolStripButtonShowLabels.CheckOnClick = true;
            this.toolStripButtonShowLabels.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButtonShowLabels.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonShowLabels.Enabled = true;
            this.toolStripButtonShowLabels.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonShowLabels.Image")));
            this.toolStripButtonShowLabels.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonShowLabels.Name = "toolStripButtonShowLabels";
            this.toolStripButtonShowLabels.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonShowLabels.Text = "Show Chart Labels";
            this.toolStripButtonShowLabels.ToolTipText = "Label Chart";
            this.toolStripButtonShowLabels.Visible = true;
            this.toolStripButtonShowLabels.Click += new System.EventHandler(this.toolStripButtonShowLabels_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator3.Visible = false;
            // 
            // toolStripButtonShowTimeGaps
            // 
            this.toolStripButtonShowTimeGaps.CheckOnClick = true;
            this.toolStripButtonShowTimeGaps.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonShowTimeGaps.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonShowTimeGaps.Image")));
            this.toolStripButtonShowTimeGaps.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonShowTimeGaps.Name = "toolStripButtonShowTimeGaps";
            this.toolStripButtonShowTimeGaps.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonShowTimeGaps.Text = "toolStripButton1";
            this.toolStripButtonShowTimeGaps.ToolTipText = "Show Time Gaps on Optimization Level 1";
            this.toolStripButtonShowTimeGaps.Visible = true;
            this.toolStripButtonShowTimeGaps.CheckedChanged += new System.EventHandler(this.toolStripButtonShowTimeGaps_CheckedChanged);
            // 
            // toolStripButtonLimitView
            // 
            this.toolStripButtonLimitView.Checked = true;
            this.toolStripButtonLimitView.CheckOnClick = true;
            this.toolStripButtonLimitView.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButtonLimitView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLimitView.Enabled = true;
            this.toolStripButtonLimitView.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLimitView.Image")));
            this.toolStripButtonLimitView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLimitView.Name = "toolStripButtonLimitView";
            this.toolStripButtonLimitView.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonLimitView.Text = "Limit View";
            this.toolStripButtonLimitView.ToolTipText = "Limit View Chart";
            this.toolStripButtonLimitView.Visible = true;
            this.toolStripButtonLimitView.Click += new System.EventHandler(this.toolStripButtonLimitView_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator13.Visible = false;
            // 
            // toolStripButtonAutoScrollToEnd
            // 
            this.toolStripButtonAutoScrollToEnd.Checked = true;
            this.toolStripButtonAutoScrollToEnd.CheckOnClick = true;
            this.toolStripButtonAutoScrollToEnd.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButtonAutoScrollToEnd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAutoScrollToEnd.Enabled = true;
            this.toolStripButtonAutoScrollToEnd.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAutoScrollToEnd.Image")));
            this.toolStripButtonAutoScrollToEnd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAutoScrollToEnd.Name = "toolStripButtonAutoScrollToEnd";
            this.toolStripButtonAutoScrollToEnd.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonAutoScrollToEnd.Text = "Auto Scroll To End";
            this.toolStripButtonAutoScrollToEnd.ToolTipText = "Go to the end";
            this.toolStripButtonAutoScrollToEnd.Visible = true;
            this.toolStripButtonAutoScrollToEnd.Click += new System.EventHandler(this.toolStripButtonAutoScrollToEnd_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator6.Visible = false;
            // 
            // toolStripDropDownButtonSelectionMode
            // 
            this.toolStripDropDownButtonSelectionMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3});
            this.toolStripDropDownButtonSelectionMode.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButtonSelectionMode.Image")));
            this.toolStripDropDownButtonSelectionMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonSelectionMode.Name = "toolStripDropDownButtonSelectionMode";
            this.toolStripDropDownButtonSelectionMode.Size = new System.Drawing.Size(119, 22);
            this.toolStripDropDownButtonSelectionMode.Text = "Selection Mode";
            this.toolStripDropDownButtonSelectionMode.ToolTipText = "Right Mouse Button Selection Mode ";
            this.toolStripDropDownButtonSelectionMode.Visible = true;
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.CheckOnClick = true;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItem1.Text = "toolStripMenuItem1";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.CheckOnClick = true;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItem2.Text = "toolStripMenuItem2";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Checked = true;
            this.toolStripMenuItem3.CheckOnClick = true;
            this.toolStripMenuItem3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItem3.Text = "toolStripMenuItem3";
            // 
            // toolStripDropDownButtonScrollMode
            // 
            this.toolStripDropDownButtonScrollMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6});
            this.toolStripDropDownButtonScrollMode.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButtonScrollMode.Image")));
            this.toolStripDropDownButtonScrollMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonScrollMode.Name = "toolStripDropDownButtonScrollMode";
            this.toolStripDropDownButtonScrollMode.Size = new System.Drawing.Size(102, 22);
            this.toolStripDropDownButtonScrollMode.Text = "Scroll Mode";
            this.toolStripDropDownButtonScrollMode.ToolTipText = "Right Mouse Button Selection Mode ";
            this.toolStripDropDownButtonScrollMode.Visible = true;
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.CheckOnClick = true;
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItem4.Text = "toolStripMenuItem1";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.CheckOnClick = true;
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItem5.Text = "toolStripMenuItem2";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Checked = true;
            this.toolStripMenuItem6.CheckOnClick = true;
            this.toolStripMenuItem6.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItem6.Text = "toolStripMenuItem3";
            // 
            // toolStripDropDownButtonAppearance
            // 
            this.toolStripDropDownButtonAppearance.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem7,
            this.toolStripMenuItem8,
            this.toolStripMenuItem9});
            this.toolStripDropDownButtonAppearance.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButtonAppearance.Image")));
            this.toolStripDropDownButtonAppearance.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonAppearance.Name = "toolStripDropDownButtonAppearance";
            this.toolStripDropDownButtonAppearance.Size = new System.Drawing.Size(101, 22);
            this.toolStripDropDownButtonAppearance.Text = "Appearance";
            this.toolStripDropDownButtonAppearance.ToolTipText = "Right Mouse Button Selection Mode ";
            this.toolStripDropDownButtonAppearance.Visible = true;
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.CheckOnClick = true;
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItem7.Text = "toolStripMenuItem1";
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.CheckOnClick = true;
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItem8.Text = "toolStripMenuItem2";
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Checked = true;
            this.toolStripMenuItem9.CheckOnClick = true;
            this.toolStripMenuItem9.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItem9.Text = "toolStripMenuItem3";
            // 
            // toolStripButtonColor
            // 
            this.toolStripButtonColor.AutoSize = false;
            this.toolStripButtonColor.BackColor = System.Drawing.Color.DarkSalmon;
            this.toolStripButtonColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonColor.Name = "toolStripButtonColor";
            this.toolStripButtonColor.Size = new System.Drawing.Size(16, 16);
            this.toolStripButtonColor.Text = "Color";
            this.toolStripButtonColor.ToolTipText = "Color";
            this.toolStripButtonColor.Visible = true;
            // 
            // toolStripButtonDrawLineSegment
            // 
            this.toolStripButtonDrawLineSegment.CheckOnClick = true;
            this.toolStripButtonDrawLineSegment.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDrawLineSegment.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDrawLineSegment.Image")));
            this.toolStripButtonDrawLineSegment.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDrawLineSegment.Name = "toolStripButtonDrawLineSegment";
            this.toolStripButtonDrawLineSegment.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDrawLineSegment.Text = "toolStripButton1";
            this.toolStripButtonDrawLineSegment.ToolTipText = "Draw Line Segment";
            this.toolStripButtonDrawLineSegment.Visible = true;
            // 
            // toolStripButtonDrawLine
            // 
            this.toolStripButtonDrawLine.CheckOnClick = true;
            this.toolStripButtonDrawLine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDrawLine.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDrawLine.Image")));
            this.toolStripButtonDrawLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDrawLine.Name = "toolStripButtonDrawLine";
            this.toolStripButtonDrawLine.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDrawLine.Text = "toolStripButton1";
            this.toolStripButtonDrawLine.Visible = true;
            // 
            // toolStripButtonHorizontalLine
            // 
            this.toolStripButtonHorizontalLine.CheckOnClick = true;
            this.toolStripButtonHorizontalLine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonHorizontalLine.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonHorizontalLine.Image")));
            this.toolStripButtonHorizontalLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonHorizontalLine.Name = "toolStripButtonHorizontalLine";
            this.toolStripButtonHorizontalLine.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonHorizontalLine.Text = "toolStripButton1";
            this.toolStripButtonHorizontalLine.Visible = true;
            // 
            // toolStripButtonVerticalLine
            // 
            this.toolStripButtonVerticalLine.CheckOnClick = true;
            this.toolStripButtonVerticalLine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonVerticalLine.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonVerticalLine.Image")));
            this.toolStripButtonVerticalLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonVerticalLine.Name = "toolStripButtonVerticalLine";
            this.toolStripButtonVerticalLine.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonVerticalLine.Text = "toolStripButton1";
            this.toolStripButtonVerticalLine.Visible = true;
            // 
            // toolStripButtonFibonacci
            // 
            this.toolStripButtonFibonacci.CheckOnClick = true;
            this.toolStripButtonFibonacci.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFibonacci.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonFibonacci.Image")));
            this.toolStripButtonFibonacci.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFibonacci.Name = "toolStripButtonFibonacci";
            this.toolStripButtonFibonacci.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonFibonacci.Text = "Fibonacci Retracement";
            this.toolStripButtonFibonacci.Visible = true;
            // 
            // toolStripButtonText
            // 
            this.toolStripButtonText.CheckOnClick = true;
            this.toolStripButtonText.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonText.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonText.Image")));
            this.toolStripButtonText.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonText.Name = "toolStripButtonText";
            this.toolStripButtonText.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonText.Text = "Text";
            this.toolStripButtonText.Visible = true;
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemMIN1,
            this.toolStripMenuItemMIN3,
            this.toolStripMenuItemMIN5,
            this.toolStripMenuItemMIN15,
            this.toolStripMenuItemMIN30,
            this.toolStripMenuItemMIN60,
            this.toolStripMenuItemMIN120,
            this.toolStripMenuItemDAY1});
            this.toolStripDropDownButton1.Enabled = true;
            this.toolStripDropDownButton1.Image = global::fxpa.Properties.Resources.alarmclock_preferences;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButton1.Text = "Intervals";
            this.toolStripDropDownButton1.Visible = true;
            // 
            // toolStripMenuItemMIN1
            // 
            this.toolStripMenuItemMIN1.Name = "toolStripMenuItemMIN1";
            this.toolStripMenuItemMIN1.Size = new System.Drawing.Size(112, 22);
            this.toolStripMenuItemMIN1.Text = "MIN1";
            this.toolStripMenuItemMIN1.Click += new System.EventHandler(this.toolStripMenuItemMIN1_Click);
            // 
            // toolStripMenuItemMIN3
            // 
            this.toolStripMenuItemMIN3.Name = "toolStripMenuItemMIN3";
            this.toolStripMenuItemMIN3.Size = new System.Drawing.Size(112, 22);
            this.toolStripMenuItemMIN3.Text = "MIN5";
            this.toolStripMenuItemMIN3.Click += new System.EventHandler(this.toolStripMenuItemMIN3_Click);
            // 
            // toolStripMenuItemMIN5
            // 
            this.toolStripMenuItemMIN5.Name = "toolStripMenuItemMIN5";
            this.toolStripMenuItemMIN5.Size = new System.Drawing.Size(112, 22);
            this.toolStripMenuItemMIN5.Text = "MIN15";
            this.toolStripMenuItemMIN5.Click += new System.EventHandler(this.toolStripMenuItemMIN5_Click);
            // 
            // toolStripMenuItemMIN15
            // 
            this.toolStripMenuItemMIN15.Name = "toolStripMenuItemMIN15";
            this.toolStripMenuItemMIN15.Size = new System.Drawing.Size(112, 22);
            this.toolStripMenuItemMIN15.Text = "MIN30";
            this.toolStripMenuItemMIN15.Click += new System.EventHandler(this.toolStripMenuItemMIN15_Click);
            // 
            // toolStripMenuItemMIN30
            // 
            this.toolStripMenuItemMIN30.Name = "toolStripMenuItemMIN30";
            this.toolStripMenuItemMIN30.Size = new System.Drawing.Size(112, 22);
            this.toolStripMenuItemMIN30.Text = "H1";
            this.toolStripMenuItemMIN30.Click += new System.EventHandler(this.toolStripMenuItemMIN30_Click);
            // 
            // toolStripMenuItemMIN60
            // 
            this.toolStripMenuItemMIN60.Name = "toolStripMenuItemMIN60";
            this.toolStripMenuItemMIN60.Size = new System.Drawing.Size(112, 22);
            this.toolStripMenuItemMIN60.Text = "H4";
            this.toolStripMenuItemMIN60.Click += new System.EventHandler(this.toolStripMenuItemMIN60_Click);
            // 
            // toolStripMenuItemMIN120
            // 
            this.toolStripMenuItemMIN120.Name = "toolStripMenuItemMIN120";
            this.toolStripMenuItemMIN120.Size = new System.Drawing.Size(112, 22);
            this.toolStripMenuItemMIN120.Text = "D1";
            this.toolStripMenuItemMIN120.Click += new System.EventHandler(this.toolStripMenuItemMIN120_Click);
            // 
            // toolStripMenuItemDAY1
            // 
            this.toolStripMenuItemDAY1.Name = "toolStripMenuItemDAY1";
            this.toolStripMenuItemDAY1.Size = new System.Drawing.Size(112, 22);
            this.toolStripMenuItemDAY1.Text = "W1";
            this.toolStripMenuItemDAY1.Click += new System.EventHandler(this.toolStripMenuItemDAY1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::fxpa.Properties.Resources.EXCHANGE;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "toolStripButton2";
            this.toolStripButton2.ToolTipText = "Indicator";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.splitContainer1);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 25);
            this.panel2.MaximumSize = new System.Drawing.Size(220, 0);
            this.panel2.MinimumSize = new System.Drawing.Size(220, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(220, 483);
            this.panel2.TabIndex = 47;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 108);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.timeLabel);
            this.splitContainer1.Panel1.Controls.Add(this.weekStatBtn);
            this.splitContainer1.Panel1.Controls.Add(this.triDayStatBtn);
            this.splitContainer1.Panel1.Controls.Add(this.dayStatBtn);
            this.splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.priceListView);
            this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.Panel2MinSize = 50;
            this.splitContainer1.Size = new System.Drawing.Size(220, 375);
            this.splitContainer1.SplitterDistance = 27;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 1;
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.timeLabel.Location = new System.Drawing.Point(0, 9);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(0, 15);
            this.timeLabel.TabIndex = 53;
            this.timeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // weekStatBtn
            // 
            this.weekStatBtn.Location = new System.Drawing.Point(177, 3);
            this.weekStatBtn.Name = "weekStatBtn";
            this.weekStatBtn.Size = new System.Drawing.Size(41, 25);
            this.weekStatBtn.TabIndex = 56;
            this.weekStatBtn.Text = "Week";
            this.weekStatBtn.UseVisualStyleBackColor = true;
            this.weekStatBtn.Click += new System.EventHandler(this.weekStatBtn_Click);
            // 
            // triDayStatBtn
            // 
            this.triDayStatBtn.Location = new System.Drawing.Point(136, 3);
            this.triDayStatBtn.Name = "triDayStatBtn";
            this.triDayStatBtn.Size = new System.Drawing.Size(41, 25);
            this.triDayStatBtn.TabIndex = 55;
            this.triDayStatBtn.Text = "3Days";
            this.triDayStatBtn.UseVisualStyleBackColor = true;
            this.triDayStatBtn.Click += new System.EventHandler(this.triDayStatBtn_Click);
            // 
            // dayStatBtn
            // 
            this.dayStatBtn.Location = new System.Drawing.Point(95, 3);
            this.dayStatBtn.Name = "dayStatBtn";
            this.dayStatBtn.Size = new System.Drawing.Size(41, 25);
            this.dayStatBtn.TabIndex = 53;
            this.dayStatBtn.Text = "Day";
            this.dayStatBtn.UseVisualStyleBackColor = true;
            this.dayStatBtn.Click += new System.EventHandler(this.dayStatBtn_Click);
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(220, 108);
            this.panel3.TabIndex = 0;
            // 
            //TradeAnalyzerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.toolStripMain);
            this.Controls.Add(this.toolStrip1);
            this.ImageName = "breakpoint.png";
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ManualTradeAnalyzerControl";
            this.Size = new System.Drawing.Size(904, 508);
            this.Title = "Manual Trading";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonCreateSession;
        private System.Windows.Forms.ToolStripButton toolStripButtonCloseSession;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButtonWindowed;
        private System.Windows.Forms.ImageList imageList1;
        //private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton toolStripButtonShowScrollbars;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomIn;
        private System.Windows.Forms.ToolStripButton toolStripButtonZoomOut;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonFit;
        private System.Windows.Forms.ToolStripButton toolStripButtonFitHorizontal;
        private System.Windows.Forms.ToolStripButton toolStripButtonFitVertical;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripButton toolStripButtonCrosshair;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton toolStripButtonShowLabels;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButtonShowTimeGaps;
        private System.Windows.Forms.ToolStripButton toolStripButtonLimitView;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripButton toolStripButtonAutoScrollToEnd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonSelectionMode;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonScrollMode;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonAppearance;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem9;
        private System.Windows.Forms.ToolStripButton toolStripButtonColor;
        private System.Windows.Forms.ToolStripButton toolStripButtonDrawLineSegment;
        private System.Windows.Forms.ToolStripButton toolStripButtonDrawLine;
        private System.Windows.Forms.ToolStripButton toolStripButtonHorizontalLine;
        private System.Windows.Forms.ToolStripButton toolStripButtonVerticalLine;
        private System.Windows.Forms.ToolStripButton toolStripButtonFibonacci;
        private System.Windows.Forms.ToolStripButton toolStripButtonText;
        public DoubleBufferListView priceListView;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Samples.TitleStrip titleStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMIN1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMIN3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMIN5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMIN15;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMIN30;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMIN60;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMIN120;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDAY1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton closeToolStripButton;
        protected System.Windows.Forms.QuickenHeaderToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton speakerToolStripButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button dayStatBtn;
        private System.Windows.Forms.Button weekStatBtn;
        private System.Windows.Forms.Button triDayStatBtn;
        //private System.Windows.Forms.Label label1;




    }
}
