using System.Windows.Forms;
using System.Drawing;
namespace fxpa
{
    partial class FxpaForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FxpaForm));
            fxpa.ToolBarButtonStatus toolBarButtonStatus1 = new fxpa.ToolBarButtonStatus();
            this.warningStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.welcomeStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.asdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.signalTabPage = new System.Windows.Forms.TabPage();
            this.signalListView = new fxpa.DoubleBufferListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader22 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader23 = new System.Windows.Forms.ColumnHeader();
            this.signalImageList = new System.Windows.Forms.ImageList(this.components);
            this.statTabPage = new System.Windows.Forms.TabPage();
            this.statListView = new fxpa.DoubleBufferListView();
            this.columnHeader16 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader17 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader18 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader19 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader20 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader25 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader21 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader24 = new System.Windows.Forms.ColumnHeader();
            this.InfoTabPage = new System.Windows.Forms.TabPage();
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.menuItemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.newAccountMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indicatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rSIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cCIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bBandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aDXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.williamsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.emulatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadDataFileCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemRelogin = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.fxAnalyzerControl = new fxpa.FxAnalyzerControl();
            this.tabPage11 = new System.Windows.Forms.TabPage();
            this.infoListView = new fxpa.DoubleBufferListView();
            this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader14 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader15 = new System.Windows.Forms.ColumnHeader();
            this.contextMenuStrip1.SuspendLayout();
            this.statusStripMain.SuspendLayout();
            this.mainTabControl.SuspendLayout();
            this.signalTabPage.SuspendLayout();
            this.statTabPage.SuspendLayout();
            this.menuStripMain.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.SuspendLayout();
            // 
            // warningStatusLabel
            // 
            this.warningStatusLabel.Name = "warningStatusLabel";
            this.warningStatusLabel.Size = new System.Drawing.Size(777, 17);
            this.warningStatusLabel.Spring = true;
            this.warningStatusLabel.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // welcomeStatusLabel
            // 
            this.welcomeStatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
                        | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.welcomeStatusLabel.Name = "welcomeStatusLabel";
            this.welcomeStatusLabel.Size = new System.Drawing.Size(4, 17);
            this.welcomeStatusLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "briefcase2.png");
            this.imageList.Images.SetKeyName(1, "TV.PNG");
            this.imageList.Images.SetKeyName(2, "breakpoint.png");
            this.imageList.Images.SetKeyName(3, "elements_selection.png");
            this.imageList.Images.SetKeyName(4, "breakpoints.png");
            this.imageList.Images.SetKeyName(5, "text_tree.png");
            this.imageList.Images.SetKeyName(6, "magic-wand.png");
            this.imageList.Images.SetKeyName(7, "breakpoint_selection.png");
            this.imageList.Images.SetKeyName(8, "dot.png");
            this.imageList.Images.SetKeyName(9, "HELP.PNG");
            this.imageList.Images.SetKeyName(10, "environment_information.png");
            this.imageList.Images.SetKeyName(11, "alarmclock_preferences.png");
            this.imageList.Images.SetKeyName(12, "CONTRACT.PNG");
            this.imageList.Images.SetKeyName(13, "element_next.png");
            this.imageList.Images.SetKeyName(14, "cube_green.png");
            this.imageList.Images.SetKeyName(15, "cube_yellow.png");
            this.imageList.Images.SetKeyName(16, "EXCHANGE.PNG");
            this.imageList.Images.SetKeyName(17, "BOOKS.PNG");
            this.imageList.Images.SetKeyName(18, "address_book.png");
            this.imageList.Images.SetKeyName(19, "WARNING.PNG");
            this.imageList.Images.SetKeyName(20, "navigate_right.png");
            this.imageList.Images.SetKeyName(21, "ANGEL.ICO");
            this.imageList.Images.SetKeyName(22, "ANGEL.PNG");
            this.imageList.Images.SetKeyName(23, "environment.ico");
            this.imageList.Images.SetKeyName(24, "arrow_down_red.24.png");
            this.imageList.Images.SetKeyName(25, "environment.png");
            this.imageList.Images.SetKeyName(26, "arrow_up_green1.png");
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.asdToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(127, 26);
            // 
            // asdToolStripMenuItem
            // 
            this.asdToolStripMenuItem.Name = "asdToolStripMenuItem";
            this.asdToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.asdToolStripMenuItem.Text = "Close Tab";
            // 
            // statusStripMain
            // 
            this.statusStripMain.AllowMerge = false;
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.warningStatusLabel,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel4,
            this.welcomeStatusLabel});
            this.statusStripMain.Location = new System.Drawing.Point(0, 551);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusStripMain.Size = new System.Drawing.Size(792, 22);
            this.statusStripMain.SizingGrip = false;
            this.statusStripMain.TabIndex = 19;
            this.statusStripMain.Text = "statusStrip1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(32, 17);
            this.toolStripStatusLabel2.Text = "Help";
            this.toolStripStatusLabel2.Visible = false;
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(69, 17);
            this.toolStripStatusLabel3.Text = "Message(s)";
            this.toolStripStatusLabel3.Visible = false;
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(36, 17);
            this.toolStripStatusLabel4.Text = "More";
            this.toolStripStatusLabel4.Visible = false;
            // 
            // mainTabControl
            // 
            this.mainTabControl.Controls.Add(this.signalTabPage);
            this.mainTabControl.Controls.Add(this.statTabPage);
            this.mainTabControl.Controls.Add(this.InfoTabPage);
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.mainTabControl.Location = new System.Drawing.Point(0, 384);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(792, 167);
            this.mainTabControl.TabIndex = 36;
            this.mainTabControl.DoubleClick += new System.EventHandler(this.mainTabControl_DoubleClick);
            this.mainTabControl.Click += new System.EventHandler(this.mainTabControl_Click);
            // 
            // signalTabPage
            // 
            this.signalTabPage.Controls.Add(this.signalListView);
            this.signalTabPage.Location = new System.Drawing.Point(4, 22);
            this.signalTabPage.Name = "signalTabPage";
            this.signalTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.signalTabPage.Size = new System.Drawing.Size(784, 141);
            this.signalTabPage.TabIndex = 0;
            this.signalTabPage.Text = "Orders";
            this.signalTabPage.Click += new System.EventHandler(this.TabPageControl_Click);
            this.signalTabPage.GotFocus += new System.EventHandler(this.signalTabPage_GotFocus);
            // 
            // signalListView
            // 
            this.signalListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader22,
            this.columnHeader23});
            this.signalListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.signalListView.FullRowSelect = true;
            this.signalListView.GridLines = true;
            this.signalListView.Location = new System.Drawing.Point(3, 3);
            this.signalListView.Name = "signalListView";
            this.signalListView.OwnerDraw = true;
            this.signalListView.Size = new System.Drawing.Size(778, 135);
            this.signalListView.SmallImageList = this.signalImageList;
            this.signalListView.TabIndex = 29;
            this.signalListView.UseCompatibleStateImageBehavior = false;
            this.signalListView.View = System.Windows.Forms.View.Details;
            this.signalListView.SizeChanged += new System.EventHandler(this.signalListView_SizeChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Symbol";
            this.columnHeader1.Width = 80;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Open Price";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader5.Width = 80;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Open Time";
            this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader6.Width = 84;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Buy/Sell";
            this.columnHeader7.Width = 81;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Close Time";
            this.columnHeader8.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader8.Width = 91;
            // 
            // columnHeader22
            // 
            this.columnHeader22.Text = "Close Price";
            this.columnHeader22.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader22.Width = 120;
            // 
            // columnHeader23
            // 
            this.columnHeader23.Text = "Profit";
            this.columnHeader23.Width = 200;
            // 
            // signalImageList
            // 
            this.signalImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("signalImageList.ImageStream")));
            this.signalImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.signalImageList.Images.SetKeyName(0, "arrow_down.png");
            this.signalImageList.Images.SetKeyName(1, "arrow_up.png");
            // 
            // statTabPage
            // 
            this.statTabPage.Controls.Add(this.statListView);
            this.statTabPage.Location = new System.Drawing.Point(4, 22);
            this.statTabPage.Name = "statTabPage";
            this.statTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.statTabPage.Size = new System.Drawing.Size(784, 141);
            this.statTabPage.TabIndex = 1;
            this.statTabPage.Text = "Statistics";
            // 
            // statListView
            // 
            this.statListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader16,
            this.columnHeader17,
            this.columnHeader18,
            this.columnHeader19,
            this.columnHeader20,
            this.columnHeader25,
            this.columnHeader21,
            this.columnHeader24});
            this.statListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statListView.FullRowSelect = true;
            this.statListView.GridLines = true;
            this.statListView.Location = new System.Drawing.Point(3, 3);
            this.statListView.Name = "statListView";
            this.statListView.OwnerDraw = true;
            this.statListView.Size = new System.Drawing.Size(778, 135);
            this.statListView.SmallImageList = this.signalImageList;
            this.statListView.TabIndex = 29;
            this.statListView.UseCompatibleStateImageBehavior = false;
            this.statListView.View = System.Windows.Forms.View.Details;
            this.statListView.SizeChanged += new System.EventHandler(this.statListView_SizeChanged);
            // 
            // columnHeader16
            // 
            this.columnHeader16.Text = "Symbol";
            this.columnHeader16.Width = 64;
            // 
            // columnHeader17
            // 
            this.columnHeader17.Text = "Open Price";
            this.columnHeader17.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader17.Width = 123;
            // 
            // columnHeader18
            // 
            this.columnHeader18.Text = "Buy/Sell";
            this.columnHeader18.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader18.Width = 89;
            // 
            // columnHeader19
            // 
            this.columnHeader19.Text = "Open Time";
            this.columnHeader19.Width = 81;
            // 
            // columnHeader20
            // 
            this.columnHeader20.Text = "Close Time";
            this.columnHeader20.Width = 83;
            // 
            // columnHeader25
            // 
            this.columnHeader25.Text = "Close Price";
            this.columnHeader25.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader25.Width = 80;
            // 
            // columnHeader21
            // 
            this.columnHeader21.Text = "Profit/Loss (Point)";
            this.columnHeader21.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader21.Width = 168;
            // 
            // columnHeader24
            // 
            this.columnHeader24.Text = "";
            this.columnHeader24.Width = 200;
            // 
            // InfoTabPage
            // 
            this.InfoTabPage.Location = new System.Drawing.Point(4, 22);
            this.InfoTabPage.Name = "InfoTabPage";
            this.InfoTabPage.Size = new System.Drawing.Size(784, 141);
            this.InfoTabPage.TabIndex = 2;
            // 
            // splitter3
            // 
            this.splitter3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter3.Location = new System.Drawing.Point(0, 379);
            this.splitter3.Name = "splitter3";
            this.splitter3.Size = new System.Drawing.Size(792, 5);
            this.splitter3.TabIndex = 37;
            this.splitter3.TabStop = false;
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemFile,
            this.toolTToolStripMenuItem,
            this.dataDToolStripMenuItem,
            this.menuItemHelp});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStripMain.Size = new System.Drawing.Size(792, 24);
            this.menuStripMain.TabIndex = 48;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // menuItemFile
            // 
            this.menuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newAccountMenuItem,
            this.toolStripSeparator,
            this.menuItemExit});
            this.menuItemFile.Name = "menuItemFile";
            this.menuItemFile.Size = new System.Drawing.Size(53, 20);
            this.menuItemFile.Text = "File(&F)";
            // 
            // newAccountMenuItem
            // 
            this.newAccountMenuItem.Image = global::fxpa.Properties.Resources.lock_edit;
            this.newAccountMenuItem.Name = "newAccountMenuItem";
            this.newAccountMenuItem.Size = new System.Drawing.Size(144, 22);
            this.newAccountMenuItem.Text = "New Account";
            this.newAccountMenuItem.Click += new System.EventHandler(this.menuItemResetPwd_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(141, 6);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Image = global::fxpa.Properties.Resources.cancel;
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.Size = new System.Drawing.Size(144, 22);
            this.menuItemExit.Text = "Exit";
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // toolTToolStripMenuItem
            // 
            this.toolTToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.indicatorToolStripMenuItem,
            this.emulatorToolStripMenuItem});
            this.toolTToolStripMenuItem.Name = "toolTToolStripMenuItem";
            this.toolTToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.toolTToolStripMenuItem.Text = "Tool(&T)";
            // 
            // indicatorToolStripMenuItem
            // 
            this.indicatorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mAToolStripMenuItem,
            this.rSIToolStripMenuItem,
            this.cCIToolStripMenuItem,
            this.bBandToolStripMenuItem,
            this.aDXToolStripMenuItem,
            this.williamsToolStripMenuItem});
            this.indicatorToolStripMenuItem.Name = "indicatorToolStripMenuItem";
            this.indicatorToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.indicatorToolStripMenuItem.Text = "Indicator";
            // 
            // mAToolStripMenuItem
            // 
            this.mAToolStripMenuItem.Name = "mAToolStripMenuItem";
            this.mAToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.mAToolStripMenuItem.Text = "MA";
            // 
            // rSIToolStripMenuItem
            // 
            this.rSIToolStripMenuItem.Name = "rSIToolStripMenuItem";
            this.rSIToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.rSIToolStripMenuItem.Text = "RSI";
            this.rSIToolStripMenuItem.Click += new System.EventHandler(this.rSIToolStripMenuItem_Click);
            // 
            // cCIToolStripMenuItem
            // 
            this.cCIToolStripMenuItem.Name = "cCIToolStripMenuItem";
            this.cCIToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.cCIToolStripMenuItem.Text = "CCI";
            // 
            // bBandToolStripMenuItem
            // 
            this.bBandToolStripMenuItem.Name = "bBandToolStripMenuItem";
            this.bBandToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.bBandToolStripMenuItem.Text = "BBand";
            // 
            // aDXToolStripMenuItem
            // 
            this.aDXToolStripMenuItem.Name = "aDXToolStripMenuItem";
            this.aDXToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aDXToolStripMenuItem.Text = "ADX";
            // 
            // williamsToolStripMenuItem
            // 
            this.williamsToolStripMenuItem.Name = "williamsToolStripMenuItem";
            this.williamsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.williamsToolStripMenuItem.Text = "Williams";
            // 
            // emulatorToolStripMenuItem
            // 
            this.emulatorToolStripMenuItem.Name = "emulatorToolStripMenuItem";
            this.emulatorToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.emulatorToolStripMenuItem.Text = "Emulator";
            // 
            // dataDToolStripMenuItem
            // 
            this.dataDToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadDataFileCSVToolStripMenuItem});
            this.dataDToolStripMenuItem.Name = "dataDToolStripMenuItem";
            this.dataDToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.dataDToolStripMenuItem.Text = "Data(&D)";
            // 
            // loadDataFileCSVToolStripMenuItem
            // 
            this.loadDataFileCSVToolStripMenuItem.Name = "loadDataFileCSVToolStripMenuItem";
            this.loadDataFileCSVToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.loadDataFileCSVToolStripMenuItem.Text = "Load Data File (CSV)";
            // 
            // menuItemHelp
            // 
            this.menuItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsToolStripMenuItem,
            this.aboutMenuItem,
            this.toolStripSeparator5});
            this.menuItemHelp.Name = "menuItemHelp";
            this.menuItemHelp.Size = new System.Drawing.Size(60, 20);
            this.menuItemHelp.Text = "Help(&H)";
            // 
            // contentsToolStripMenuItem
            // 
            this.contentsToolStripMenuItem.Enabled = false;
            this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
            this.contentsToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.contentsToolStripMenuItem.Text = "Help Topic";
            this.contentsToolStripMenuItem.Visible = false;
            // 
            // aboutMenuItem
            // 
            this.aboutMenuItem.Image = global::fxpa.Properties.Resources.flag_blue;
            this.aboutMenuItem.Name = "aboutMenuItem";
            this.aboutMenuItem.Size = new System.Drawing.Size(131, 22);
            this.aboutMenuItem.Text = "About ";
            this.aboutMenuItem.Click += new System.EventHandler(this.aboutMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(128, 6);
            // 
            // menuItemRelogin
            // 
            this.menuItemRelogin.Image = global::fxpa.Properties.Resources.CHECKS;
            this.menuItemRelogin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuItemRelogin.Name = "menuItemRelogin";
            this.menuItemRelogin.Size = new System.Drawing.Size(152, 22);
            this.menuItemRelogin.Text = "Re-Login";
            this.menuItemRelogin.Click += new System.EventHandler(this.menuItemRelogin_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage5);
            this.tabControl.Controls.Add(this.tabPage11);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.ImageList = this.imageList;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(792, 384);
            this.tabControl.TabIndex = 40;
            // 
            // tabPage5
            // 
            this.tabPage5.AutoScroll = true;
            this.tabPage5.Controls.Add(this.fxAnalyzerControl);
            this.tabPage5.ImageIndex = 7;
            this.tabPage5.Location = new System.Drawing.Point(4, 23);
            this.tabPage5.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(784, 357);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "Analyzer Editor";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tradeAnalyzerControl
            // 
            this.fxAnalyzerControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.fxAnalyzerControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.fxAnalyzerControl.Description = "";
            this.fxAnalyzerControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fxAnalyzerControl.Host = null;
            this.fxAnalyzerControl.ImageName = "breakpoint.png";
            this.fxAnalyzerControl.IsPause = false;
            this.fxAnalyzerControl.Location = new System.Drawing.Point(0, 0);
            this.fxAnalyzerControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.fxAnalyzerControl.Name = "tradeAnalyzerControl";
            toolBarButtonStatus1.AutoScrollStatus = false;
            toolBarButtonStatus1.ChartLabelsStatus = false;
            toolBarButtonStatus1.LimitViewStatus = false;
            toolBarButtonStatus1.ScrollbarsStatus = false;
            toolBarButtonStatus1.ShowMouseStatus = false;
            toolBarButtonStatus1.TimePeriod = fxpa.Interval.MIN1;
            this.fxAnalyzerControl.OpToolBarStatus = toolBarButtonStatus1;
            this.fxAnalyzerControl.ASession = null;
            this.fxAnalyzerControl.Size = new System.Drawing.Size(784, 357);
            this.fxAnalyzerControl.TabIndex = 49;
            this.fxAnalyzerControl.Title = "";
            this.fxAnalyzerControl.Load += new System.EventHandler(this.manualTradeAnalyzerControl1_Load);
            // 
            // tabPage11
            // 
            this.tabPage11.ImageIndex = 17;
            this.tabPage11.Location = new System.Drawing.Point(4, 23);
            this.tabPage11.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage11.Name = "tabPage11";
            this.tabPage11.Size = new System.Drawing.Size(784, 357);
            this.tabPage11.TabIndex = 8;
            this.tabPage11.Text = "Resources";
            this.tabPage11.UseVisualStyleBackColor = true;
            // 
            // infoListView
            // 
            this.infoListView.Location = new System.Drawing.Point(0, 0);
            this.infoListView.Name = "infoListView";
            this.infoListView.Size = new System.Drawing.Size(121, 97);
            this.infoListView.TabIndex = 0;
            this.infoListView.UseCompatibleStateImageBehavior = false;
            // 
            // FxpaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 573);
            this.Controls.Add(this.splitter3);
            this.Controls.Add(this.menuStripMain);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.mainTabControl);
            this.Controls.Add(this.statusStripMain);
            this.IsMdiContainer = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FxpaForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FXPA";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FxpaMainForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FxpaMainForm_FormClosing);
            this.contextMenuStrip1.ResumeLayout(false);
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.mainTabControl.ResumeLayout(false);
            this.signalTabPage.ResumeLayout(false);
            this.statTabPage.ResumeLayout(false);
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        void signalTabPage_GotFocus(object sender, System.EventArgs e)
        {
            ((TabPage)sender).ForeColor = Color.Orange;
            //mainTabControl.SelectedTab.BackColor = Color.Orange;
            ((TabPage)sender).Invalidate();
        }

        #endregion

        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem asdToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        //private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage signalTabPage;
        private System.Windows.Forms.TabPage InfoTabPage;
        private System.Windows.Forms.TabPage statTabPage;
        public DoubleBufferListView signalListView;
        public DoubleBufferListView statListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        public DoubleBufferListView infoListView;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.Splitter splitter3;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.ColumnHeader columnHeader15;
        private System.Windows.Forms.ColumnHeader columnHeader16;
        private System.Windows.Forms.ColumnHeader columnHeader17;
        private System.Windows.Forms.ColumnHeader columnHeader18;
        private System.Windows.Forms.ColumnHeader columnHeader19;
        private System.Windows.Forms.ColumnHeader columnHeader20;
        private System.Windows.Forms.ColumnHeader columnHeader21;
        private System.Windows.Forms.ColumnHeader columnHeader22;
        private System.Windows.Forms.ColumnHeader columnHeader23;
		private System.Windows.Forms.ColumnHeader columnHeader24;
        private System.Windows.Forms.ColumnHeader columnHeader25;
        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem menuItemFile;
        private System.Windows.Forms.ToolStripMenuItem menuItemRelogin;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.ToolStripMenuItem menuItemHelp;
        private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripMenuItem newAccountMenuItem;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabPage5;
		private FxAnalyzerControl fxAnalyzerControl;
		private System.Windows.Forms.TabPage tabPage11;
        private System.Windows.Forms.ImageList signalImageList;
        //private Sunisoft.IrisSkin.SkinEngine skinEngine;
        public System.Windows.Forms.ToolStripStatusLabel warningStatusLabel;
        public System.Windows.Forms.ToolStripStatusLabel welcomeStatusLabel;
        private ToolStripMenuItem dataDToolStripMenuItem;
        private ToolStripMenuItem loadDataFileCSVToolStripMenuItem;
        private ToolStripMenuItem toolTToolStripMenuItem;
        private ToolStripMenuItem indicatorToolStripMenuItem;
        private ToolStripMenuItem mAToolStripMenuItem;
        private ToolStripMenuItem rSIToolStripMenuItem;
        private ToolStripMenuItem emulatorToolStripMenuItem;
        private ToolStripMenuItem cCIToolStripMenuItem;
        private ToolStripMenuItem bBandToolStripMenuItem;
        private ToolStripMenuItem aDXToolStripMenuItem;
        private ToolStripMenuItem williamsToolStripMenuItem;
    }
}