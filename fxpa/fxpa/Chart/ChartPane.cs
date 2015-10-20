// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using System.ComponentModel;
using System.Text;
using System.Collections.Generic;

namespace fxpa
{
    /// <summary>
    /// Note - all the Brush and Pens do not have Get accessors on purpose, since defaults can not be modified. 
    /// So to change them directly use the "set".
    /// </summary>
    public partial class ChartPane : Control
    {
        const int MinimumAbsoluteSelectionWidth = 18;

        public enum SelectionModeEnum
        {
            Select,
            RectangleZoom,
            HorizontalZoom,
            VerticalZoom,
            None
        }

        public enum ScrollModeEnum
        {
            HorizontalScroll,
            HorizontalScrollAndFit,
            VerticalScroll,
            HorizontalZoom,
            VerticalZoom,
            ZoomToMouse,
            None
        }

        public enum AppearanceSchemeEnum
        {
            Default,
            Custom,
            Fast,
            Trade,
            TradeWhite,
            Dark,
            DarkNatural,
            Light,
            LightNatural,
            LightNaturalFlat,
            Alfonsina,
            Ground
        }

        #region General

        AppearanceSchemeEnum _appearanceScheme;
        public AppearanceSchemeEnum AppearanceScheme
        {
            get { return _appearanceScheme; }
            set
            {
                SetAppearanceScheme(value);
            }
        }

        bool _autoScrollToEnd = true;
        public bool AutoScrollToEnd
        {
            get { return _autoScrollToEnd; }
            set { _autoScrollToEnd = value; }
        }

        /// <summary>
        /// Needed to synchronize the view to the drawing space when resizing (show the same thing after resized)
        /// </summary>
        Size _lastControlSize = Size.Empty;

        ComponentResourceManager _resources;

        ContextMenuStrip _seriesTypeDynamicContextMenu;

        /// <summary>
        /// Keep in mind this also gets nulled on redraws to help the crosshair rendering properly,
        /// so use internally only.
        /// </summary>
        Point? _lastMouseMovePosition = null;

        ChartSeriesColorSelector _colorSelector = new ChartSeriesColorSelector();
        public ChartSeriesColorSelector ColorSelector
        {
            get { return _colorSelector; }
        }

        float _maximumXZoom = 1;

        /// <summary>
        /// Space between labels on X Axis (in number of items displayed).
        /// </summary>
        float _xAxisLabelSpacing = 20;

        /// <summary>
        /// Space between labels on Y Axis (auto assigned).
        /// </summary>
        float _yAxisLabelSpacing;

        bool _maximumZoomEnabled = true;
        public bool MaximumZoomEnabled
        {
            get { return _maximumZoomEnabled; }
            set
            {
                _maximumZoomEnabled = value;
                GeneralHelper.SafeEventRaise(ParametersUpdatedEvent, this);
            }
        }

        string _chartName = "";
        public string ChartName
        {
            get { return _chartName; }
            set
            {
                SetChartName(value);
            }
        }

        Font _titleFont;
        public Font TitleFont
        {
            get { return _titleFont; }
            set
            {
                _titleFont = value;
                GeneralHelper.SafeEventRaise(ParametersUpdatedEvent, this);
            }
        }

        Brush _titleFontBrush;
        public Brush TitleFontBrush
        {
            set
            {
                _titleFontBrush = value;
                GeneralHelper.SafeEventRaise(ParametersUpdatedEvent, this);
            }
        }

        Brush _fill;
        public Brush Fill
        {
            set
            {
                _fill = value;
                GeneralHelper.SafeEventRaise(ParametersUpdatedEvent, this);
            }
        }

        bool _considerAxisLabelsSpacingScale = true;
        /// <summary>
        /// Should spacing between axis-labels change with scale (prefferably yes).
        /// </summary>
        public bool ConsiderAxisLabelsSpacingScale
        {
            get { return _considerAxisLabelsSpacingScale; }
            set
            {
                _considerAxisLabelsSpacingScale = value;
                GeneralHelper.SafeEventRaise(ParametersUpdatedEvent, this);
            }
        }

        Font _axisLabelsFont;
        public Font AxisLabelsFont
        {
            get { return _axisLabelsFont; }
            set
            {
                _axisLabelsFont = value;
                GeneralHelper.SafeEventRaise(ParametersUpdatedEvent, this);
            }
        }

        Brush _xAxisLabelsFontBrush;
        public Brush XAxisLabelsFontBrush
        {
            set
            {
                _xAxisLabelsFontBrush = value;
                GeneralHelper.SafeEventRaise(ParametersUpdatedEvent, this);
            }
        }

        Brush _yAxisLabelsFontBrush;
        public Brush YAxisLabelsFontBrush
        {
            set
            {
                _yAxisLabelsFontBrush = value;
                GeneralHelper.SafeEventRaise(ParametersUpdatedEvent, this);
            }
        }

        string _xAxisLabelsFormat;
        public string XAxisLabelsFormat
        {
            get { return _xAxisLabelsFormat; }
            set
            {
                _xAxisLabelsFormat = value;
                GeneralHelper.SafeEventRaise(ParametersUpdatedEvent, this);
            }
        }

        string _yAxisLabelsFormat;

        Rectangle[] _currentLabelsRectangles = new Rectangle[] { };

        Font _labelsFont;
        public Font LabelsFont
        {
            get { return _labelsFont; }
            set
            {
                _labelsFont = value;
                GeneralHelper.SafeEventRaise(ParametersUpdatedEvent, this);
            }
        }

        Brush _labelsFontBrush;
        public Brush LabelsFontBrush
        {
            set
            {
                _labelsFontBrush = value;
                GeneralHelper.SafeEventRaise(ParametersUpdatedEvent, this);
            }
        }

        Brush _labelsFill;
        public Brush LabelsFill
        {
            set
            {
                _labelsFill = value;
                GeneralHelper.SafeEventRaise(ParametersUpdatedEvent, this);
            }
        }

        float _labelsTopMargin;
        public float LabelsTopMargin
        {
            get { return _labelsTopMargin; }
        }

        float _labelsMargin;
        public float LabelsMargin
        {
            get { return _labelsMargin; }
            set
            {
                _labelsMargin = value;
                GeneralHelper.SafeEventRaise(ParametersUpdatedEvent, this);
            }
        }
        // Harry Modified
        bool _showSeriesLabels = false;
        public bool ShowSeriesLabels
        {
            get { return _showSeriesLabels; }
            set
            {
                _showSeriesLabels = value;
                GeneralHelper.SafeEventRaise(ParametersUpdatedEvent, this);
            }
        }

        bool _showClippingRectangle = false;
        public bool ShowClippingRectangle
        {
            get { return _showClippingRectangle; }
            set
            {
                _showClippingRectangle = value;
                GeneralHelper.SafeEventRaise(ParametersUpdatedEvent, this);
            }
        }

        bool _unitUnificationOptimizationEnabled = true;
        /// <summary>
        /// Should unit unification be performed to inteligently combine the drawing of many units together to speed un drawing.
        /// </summary>
        public bool UnitUnificationOptimizationEnabled
        {
            get { return _unitUnificationOptimizationEnabled; }
            set
            {
                _unitUnificationOptimizationEnabled = value;
                GeneralHelper.SafeEventRaise(ParametersUpdatedEvent, this);
            }
        }

        public int CurrentUnitUnification
        {
            get
            {
                if (_unitUnificationOptimizationEnabled == false)
                {
                    return 1;
                }

                return 1 + (int)(0.2 / Math.Abs(GraphicsWrapper.DrawingSpaceTransform.Elements[0]));
            }
        }

        SmoothingMode _smoothingMode = SmoothingMode.None;
        public SmoothingMode SmoothingMode
        {
            get { return _smoothingMode; }
            set
            {
                _smoothingMode = value;
                GeneralHelper.SafeEventRaise(ParametersUpdatedEvent, this);
            }
        }

        CustomObjectsManager _customObjectsManager;
        public CustomObjectsManager CustomObjectsManager
        {
            get { return _customObjectsManager; }
        }

        Image _customObjectDrawingImage;

        #endregion

        #region Selection

        ChartSeries _lastSeriesClicked = null;

        /// <summary>
        /// What area of the drawing space the user has selected with the mouse.
        /// </summary>
        public RectangleF? CurrentUserSelectedRectangle
        {
            get
            {
                if (_rightMouseButtonSelectionMode == SelectionModeEnum.None || _lastDrawingSpaceMouseRightButtonPosition.HasValue == false
                    || _currentDrawingSpaceMousePosition.HasValue == false)
                {
                    return null;
                }

                // Normalize.
                PointF lowerLeftPoint = new PointF(Math.Min(_lastDrawingSpaceMouseRightButtonPosition.Value.X, _currentDrawingSpaceMousePosition.Value.X), Math.Min(_lastDrawingSpaceMouseRightButtonPosition.Value.Y, _currentDrawingSpaceMousePosition.Value.Y));
                PointF upperRightPoint = new PointF(Math.Max(_lastDrawingSpaceMouseRightButtonPosition.Value.X, _currentDrawingSpaceMousePosition.Value.X), Math.Max(_lastDrawingSpaceMouseRightButtonPosition.Value.Y, _currentDrawingSpaceMousePosition.Value.Y));

                return new RectangleF(lowerLeftPoint.X, lowerLeftPoint.Y, upperRightPoint.X - lowerLeftPoint.X, upperRightPoint.Y - lowerLeftPoint.Y);
            }
        }

        /// <summary>
        /// What selection is, after considering the selection mode.
        /// </summary>
        public RectangleF? CurrentSelectionRectangle
        {
            get
            {
                if (CurrentUserSelectedRectangle == null)
                {
                    return null;
                }

                RectangleF selectionRectangle = CurrentUserSelectedRectangle.Value;

                if (selectionRectangle.X < _drawingSpace.X)
                {
                    selectionRectangle.Width -= _drawingSpace.X - selectionRectangle.X;
                    selectionRectangle.X = _drawingSpace.X;
                }

                if (selectionRectangle.X + selectionRectangle.Width > _drawingSpace.X + _drawingSpace.Width)
                {
                    selectionRectangle.Width = _drawingSpace.X + _drawingSpace.Width - selectionRectangle.X;
                }

                if (selectionRectangle.Y < _drawingSpace.Y)
                {
                    selectionRectangle.Height -= _drawingSpace.Y - selectionRectangle.Y;
                    selectionRectangle.Y = _drawingSpace.Y;
                }

                if (selectionRectangle.Y + selectionRectangle.Height > _drawingSpace.Y + _drawingSpace.Height)
                {
                    selectionRectangle.Height = _drawingSpace.Y + _drawingSpace.Height - selectionRectangle.Y;
                }

                switch (_rightMouseButtonSelectionMode)
                {
                    case SelectionModeEnum.RectangleZoom:
                        break;
                    case SelectionModeEnum.Select:
                        selectionRectangle.Width = 0;
                        selectionRectangle.Height = 0;
                        break;
                    case SelectionModeEnum.HorizontalZoom:
                        selectionRectangle.Y = _drawingSpace.Y;
                        selectionRectangle.Height = _drawingSpace.Height;
                        break;
                    case SelectionModeEnum.VerticalZoom:
                        selectionRectangle.X = _drawingSpace.X;
                        selectionRectangle.Width = _drawingSpace.Width;
                        break;
                    case SelectionModeEnum.None:
                    default:
                        break;
                }

                return selectionRectangle;
            }
        }

        ScrollModeEnum _scrollMode = ScrollModeEnum.HorizontalScroll;
        public ScrollModeEnum ScrollMode
        {
            get { return _scrollMode; }
            set
            {
                _scrollMode = value;
                GeneralHelper.SafeEventRaise(ParametersUpdatedEvent, this);
            }
        }

        SelectionModeEnum _rightMouseButtonSelectionMode = SelectionModeEnum.HorizontalZoom;
        public SelectionModeEnum RightMouseButtonSelectionMode
        {
            get { return _rightMouseButtonSelectionMode; }
            set
            {
                _rightMouseButtonSelectionMode = value;
                GeneralHelper.SafeEventRaise(ParametersUpdatedEvent, this);
            }
        }

        Pen _selectionPen;
        public Pen SelectionPen
        {
            set
            {
                _selectionPen = value;
                GeneralHelper.SafeEventRaise(ParametersUpdatedEvent, this);
            }
        }

        Brush _selectionFill;
        public Brush SelectionFill
        {
            set
            {
                _selectionFill = value;
                GeneralHelper.SafeEventRaise(ParametersUpdatedEvent, this);
            }
        }

        #endregion

        #region Actual Space


        int _actualDrawingSpaceAreaMarginLeft = 45;
        int _actualDrawingSpaceAreaMarginTop = 30;
        int _actualDrawingSpaceAreaMarginRight = 15;
        int _actualDrawingSpaceAreaMarginBottom = 20;

        Rectangle _actualDrawingSpaceArea;
        public Rectangle ActualDrawingSpaceArea
        {
            get { return _actualDrawingSpaceArea; }
        }

        Pen _actualDrawingSpaceAreaBorderPen;
        public Pen ActualDrawingSpaceAreaBorderPen
        {
            set
            {
                _actualDrawingSpaceAreaBorderPen = value;
                GeneralHelper.SafeEventRaise(ParametersUpdatedEvent, this);
            }
        }

        Brush _actualDrawingSpaceAreaFill;
        public Brush ActualDrawingSpaceAreaFill
        {
            set
            {
                _actualDrawingSpaceAreaFill = value;
                GeneralHelper.SafeEventRaise(ParametersUpdatedEvent, this);
            }
        }

        ChartGrid _actualSpaceGrid = new ChartGrid();
        public ChartGrid ActualSpaceGrid
        {
            get { return _actualSpaceGrid; }
        }

        #endregion

        #region Drawing Space

        RectangleF _drawingSpace;
        public RectangleF DrawingSpace
        {
            get { return _drawingSpace; }
        }

        bool _limitedView = true;

        /// <summary>
        /// Should we be able to see only the drawing area.
        /// </summary>
        public bool LimitedView
        {
            get { return _limitedView; }
            set
            {
                _limitedView = value;
                GeneralHelper.SafeEventRaise(ParametersUpdatedEvent, this);
            }
        }

        RectangleF _drawingSpaceDisplayLimit;
        public RectangleF DrawingSpaceDisplayLimit
        {
            get { return _drawingSpaceDisplayLimit; }

        }

        ChartGrid _drawingSpaceGrid = new ChartGrid();
        public ChartGrid DrawingSpaceGrid
        {
            get { return _drawingSpaceGrid; }
        }

        /// <summary>
        /// Add an extra layer over GDI+ to handle some bugs of GDI+.
        /// </summary>
        GraphicsWrapper _graphicsWrapper = new GraphicsWrapper();
        public GraphicsWrapper GraphicsWrapper
        {
            get { return _graphicsWrapper; }
        }

        //protected Matrix _drawingSpaceTransform = new Matrix();
        ///// <summary>
        ///// Transforms from drawing to actual space.
        ///// </summary>
        //public Matrix DrawingSpaceTransform
        //{
        //    get { return _drawingSpaceTransform; }
        //}

        #endregion

        #region User Input

        bool _isControlKeyDown = false;
        public bool IsControlKeyDown
        {
            get { return _isControlKeyDown; }
        }

        bool _isShiftKeyDown = false;
        public bool IsShiftKeyDown
        {
            get { return _isShiftKeyDown; }
        }

        PointF? _currentDrawingSpaceMousePosition;
        public PointF? CurrentDrawingSpaceMousePosition
        {
            get { return _currentDrawingSpaceMousePosition; }
        }

        PointF? _lastDrawingSpaceMouseRightButtonPosition;

        /// <summary>
        /// Updated also on mouse move to handle drag.
        /// </summary>
        PointF? _lastDrawingSpaceMouseDownLeftButton;

        PointF? _lastDrawingSpaceMouseDownMiddleButton;

        bool _crosshairVisible;
        public bool CrosshairVisible
        {
            get { return _crosshairVisible; }
            set
            {
                _crosshairVisible = value;
                if (_crosshairVisible == false)
                {
                    timeLabel.Visible = false;
                    priceLabel.Visible = false;
                }
                GeneralHelper.SafeEventRaise(ParametersUpdatedEvent, this);
            }
        }

        ContextMenuStrip _chartContextMenu;
        ToolStripMenuItem _crossHairContextMenuItem;
        ToolStripMenuItem _labelsContextMenuItem;
        ToolStripMenuItem _limitViewContextMenuItem;
        ToolStripMenuItem _selectedObjectsContextMenuItem;

        #endregion

        #region Series

        ListEx<ChartSeries> _series = new ListEx<ChartSeries>();
        public ChartSeries[] Series
        {
            get { return _series.ToArray(); }
        }

        public int SeriesCount
        {
            get { return _series.Count; }
        }

        float _seriesItemWidth;
        /// <summary>
        /// All series must share item sizes.
        /// </summary>
        public float SeriesItemWidth
        {
            get { return _seriesItemWidth; }
            set
            {
                _seriesItemWidth = value;
                GeneralHelper.SafeEventRaise(ParametersUpdatedEvent, this);
            }
        }

        float _seriesItemMargin;
        /// <summary>
        /// All series must share item sizes.
        /// </summary>
        public float SeriesItemMargin
        {
            get { return _seriesItemMargin; }
            set
            {
                _seriesItemMargin = value;
                GeneralHelper.SafeEventRaise(ParametersUpdatedEvent, this);
            }
        }

        #endregion

        /// <summary>
        /// The DrawingSpace transformation was modified, so now a different part of the drawing space is visible.
        /// </summary>
        /// <param name="pane"></param>
        public delegate void DrawingSpaceViewTransformationChangedDelegate(ChartPane pane, Matrix previousTransformation, Matrix currentTransformation);
        public event DrawingSpaceViewTransformationChangedDelegate DrawingSpaceViewTransformationChangedEvent;

        public delegate void DrawingSpaceUpdatedDelegate(ChartPane pane);
        public event DrawingSpaceUpdatedDelegate DrawingSpaceUpdatedEvent;

        public delegate void AppearanceSchemeChangedDelegate(ChartPane pane, AppearanceSchemeEnum scheme);
        public event AppearanceSchemeChangedDelegate AppearanceSchemeChangedEvent;

        public delegate int ActualDrawingSpaceMarginLeftUpdateDelegate(ChartPane pane, int currentValue);
        public event ActualDrawingSpaceMarginLeftUpdateDelegate ActualDrawingSpaceMarginLeftUpdateEvent;

        public delegate void ParametersUpdatedDelegate(ChartPane pane);
        public event ParametersUpdatedDelegate ParametersUpdatedEvent;

        /// <summary>
        /// 
        /// </summary>
        public ChartPane()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            _drawingSpace = new RectangleF(0, 0, 0, 0);
            _drawingSpaceDisplayLimit = _drawingSpace;

            _actualSpaceGrid.Visible = false;
            _actualSpaceGrid.Pen = new Pen(Color.Gray);

            //_drawingSpaceTransform.Scale(1, -1);

            _crosshairVisible = false;

            //SetAppearanceScheme(AppearanceSchemeEnum.Default);
            SetAppearanceScheme(AppearanceSchemeEnum.DarkNatural);

            _resources = new ComponentResourceManager(typeof(ChartPane));

            _seriesTypeDynamicContextMenu = new ContextMenuStrip();


            _chartContextMenu = new ContextMenuStrip();

            ToolStripMenuItem item;

            item = new ToolStripMenuItem("Zoom In", ((Image)(_resources.GetObject("imageZoomIn"))), new EventHandler(ZoomInChartContextMenuItem_Click));
            _chartContextMenu.Items.Add(item);
            item = new ToolStripMenuItem("Zoom Out", ((Image)(_resources.GetObject("imageZoomOut"))), new EventHandler(ZoomOutChartContextMenuItem_Click));
            _chartContextMenu.Items.Add(item);
            _chartContextMenu.Items.Add(new ToolStripSeparator());

            item = new ToolStripMenuItem("Fit screen", ((Image)(_resources.GetObject("imageLayoutCenter"))), new EventHandler(FitToScreenChartContextMenuItem_Click));
            _chartContextMenu.Items.Add(item);
            //item = new ToolStripMenuItem("Layout Horizontal", ((Image)(_resources.GetObject("imageLayoutHorizontal"))), new EventHandler(FitHorizontalChartContextMenuItem_Click));
            //_chartContextMenu.Items.Add(item);
            //item = new ToolStripMenuItem("Layout Vertical", ((Image)(_resources.GetObject("imageLayoutVertical"))), new EventHandler(FitVerticalChartContextMenuItem_Click));
            //_chartContextMenu.Items.Add(item);
            //_chartContextMenu.Items.Add(new ToolStripSeparator());

            //ToolStripMenuItem selectionContextMenuItem = new ToolStripMenuItem("Selection");
            //foreach(string name in Enum.GetNames(typeof(SelectionModeEnum)))
            //{
            //    ToolStripItem subItem = selectionContextMenuItem.DropDownItems.Add(name, null, new EventHandler(SelectionChartContextMenuItem_Click));
            //    subItem.Tag = name;
            //}
            //_chartContextMenu.Items.Add(selectionContextMenuItem);
            //_chartContextMenu.Items.Add(new ToolStripSeparator());

            //ToolStripMenuItem scrollContextMenuItem = new ToolStripMenuItem("Scroll");
            //foreach (string name in Enum.GetNames(typeof(ScrollModeEnum)))
            //{
            //    ToolStripItem subItem = scrollContextMenuItem.DropDownItems.Add(name, null, new EventHandler(ScrollChartContextMenuItem_Click));
            //    subItem.Tag = name;
            //}
            //_chartContextMenu.Items.Add(scrollContextMenuItem);
            //_chartContextMenu.Items.Add(new ToolStripSeparator());

            ToolStripMenuItem appearanceContextMenuItem = new ToolStripMenuItem("Scheme");
            foreach (string name in Enum.GetNames(typeof(AppearanceSchemeEnum)))
            {
                ToolStripItem subItem = appearanceContextMenuItem.DropDownItems.Add(name, null, new EventHandler(AppearanceChartContextMenuItem_Click));
                subItem.Tag = name;
            }
            _chartContextMenu.Items.Add(appearanceContextMenuItem);
            _chartContextMenu.Items.Add(new ToolStripSeparator());

            _crossHairContextMenuItem = new ToolStripMenuItem("Cross Hair", ((Image)(_resources.GetObject("imageTarget"))), new EventHandler(CrosshairChartContextMenuItem_Click));
            _crossHairContextMenuItem.CheckOnClick = true;
            _crossHairContextMenuItem.Checked = false;
            _chartContextMenu.Items.Add(_crossHairContextMenuItem);
            //_labelsContextMenuItem = new ToolStripMenuItem("Label Chart", ((Image)(_resources.GetObject("imageText"))), new EventHandler(LabelsChartContextMenuItem_Click));
            //_labelsContextMenuItem.CheckOnClick = true;
            //_labelsContextMenuItem.Checked = true;
            //_chartContextMenu.Items.Add(_labelsContextMenuItem);
            //_limitViewContextMenuItem = new ToolStripMenuItem("Limit View Chart", ((Image)(_resources.GetObject("imageElementSelection"))), new EventHandler(LimitViewChartContextMenuItem_Click));
            //_limitViewContextMenuItem.CheckOnClick = true;
            //_limitViewContextMenuItem.Checked = true;
            //_chartContextMenu.Items.Add(_limitViewContextMenuItem);

            //_chartContextMenu.Items.Add(new ToolStripSeparator());
            //_selectedObjectsContextMenuItem = new ToolStripMenuItem("Properties");
            //_chartContextMenu.Items.Add(_selectedObjectsContextMenuItem);

            _customObjectsManager = new CustomObjectsManager(this);

            _customObjectDrawingImage = ((Image)(_resources.GetObject("imageBrush")));




        }

        public virtual void Add(ChartSeries series)
        {
            this.Add(series, false, false);
        }

        public virtual void Add(ChartSeries series, bool usePaneColorSelector, bool replaceSeriesWithSameName)
        {
            if (replaceSeriesWithSameName)
            {
                this.RemoveByName(series.Name);
            }

            if (_series.Contains(series))
            {// Already present.
                return;
            }

            if (usePaneColorSelector)
            {
                _colorSelector.SetupSeries(series);
            }

            //if (series.ChartType == ChartSeries.ChartTypeEnum.ColoredArea)
            //{// Colored areas better be inserted first, in rendering they will not overlap other drawings.
            //    _series.Insert(0, series);
            //}
            //else
            //{
            _series.Add(series);
            //}

            series.SeriesUpdatedEvent += new ChartSeries.SeriesUpdatedDelegate(series_SeriesUpdatedEvent);
            series.AddedToChart();

            UpdateDrawingSpace();

            this.Invalidate();
        }

        /// <summary>
        /// Retrieve the first series found with this name.
        /// </summary>
        /// <param name="seriesName"></param>
        /// <returns></returns>
        ChartSeries GetSeriesByName(string seriesName)
        {
            for (int i = _series.Count - 1; i >= 0; i--)
            {
                if (_series[i].Name == seriesName)
                {
                    return _series[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Will remove all series with this name.
        /// </summary>
        public virtual void RemoveByName(string seriesName)
        {
            for (int i = _series.Count - 1; i >= 0; i--)
            {
                if (_series[i].Name == seriesName)
                {
                    Remove(_series[i]);
                }
            }
        }

        public virtual void Remove(ChartSeries series)
        {
            if (_series.Remove(series))
            {
                series.SeriesUpdatedEvent -= new ChartSeries.SeriesUpdatedDelegate(series_SeriesUpdatedEvent);
                series.RemovedFromChart();
            }

            UpdateDrawingSpace();
            this.Invalidate();
        }

        /// <summary>
        /// Needed since we use invoke to access it.
        /// </summary>
        public void SetChartName(string name)
        {
            _chartName = name;
            GeneralHelper.SafeEventRaise(ParametersUpdatedEvent, this);
        }

        public virtual void series_SeriesUpdatedEvent(ChartSeries series, bool updateUI)
        {
            this.BeginInvoke(new GeneralHelper.DefaultDelegate(UpdateDrawingSpace));
            this.Invalidate();
        }

        public void Clear(bool clearSeries, bool clearCustomObjects)
        {
            _chartName = "";

            for (int i = _series.Count - 1; clearSeries && i >= 0; i--)
            {
                Remove(_series[i]);
            }

            if (clearCustomObjects)
            {
                _customObjectsManager.Clear();
            }
        }

        /// <summary>
        /// Does NOT consider series visibility.
        /// </summary>
        protected void GetSingleSeriesMinMax(ChartSeries series, int? startIndex, int? endIndex, ref float min, ref float max)
        {
            int actualStartIndex = 0;
            int actualEndIndex = series.MaximumIndex;
            if (startIndex.HasValue)
            {
                actualStartIndex = startIndex.Value;
            }

            if (endIndex.HasValue)
            {
                actualEndIndex = endIndex.Value;
            }

            min = Math.Min(min, series.GetTotalMinimum(actualStartIndex, actualEndIndex));
            max = Math.Max(max, series.GetTotalMaximum(actualStartIndex, actualEndIndex));

            if (min < float.MaxValue && float.IsNaN(min) == false
                && max > float.MinValue && float.IsNaN(max) == false)
            {

                // Provide a 5% advance on min and max.
                float differencePart = Math.Abs(max - min) / 20f;

                // Harry -- Modified
                if (differencePart < 0.001)
                    differencePart = 0.001001f;


                //max += diff;
                //min -= diff;
                // Harry add diff between high and low 
                max += differencePart * 2f;
                min -= differencePart * 2f;
                //max += differencePart * 10f;
                //min -= differencePart * 10f;
            }
        }

        public virtual void UpdateDrawingSpace()
        {
            RectangleF? newDrawingSpace = null;

            // Establish drawing space.
            foreach (ChartSeries series in _series)
            {
                float yMin = float.MaxValue;
                float yMax = float.MinValue;

                GetSingleSeriesMinMax(series, null, null, ref yMin, ref yMax);

                if (newDrawingSpace.HasValue)
                {
                    yMin = Math.Min(yMin, newDrawingSpace.Value.Y);
                    yMax = Math.Max(yMax, newDrawingSpace.Value.Height + newDrawingSpace.Value.Y);

                    //yMin = Math.Min(newDrawingSpace.Value.Y, series.GetTotalMinimum(0, series.MaximumIndex));
                    //yMax = Math.Max(newDrawingSpace.Value.Height + newDrawingSpace.Value.Y, series.GetTotalMaximum(0, series.MaximumIndex));
                }
                //else
                //{
                //yMin = series.GetTotalMinimum(0, series.MaximumIndex);
                //yMax = series.GetTotalMaximum(0, series.MaximumIndex);
                //}

                // Series can provide
                if (float.IsInfinity(yMin) == false
                    && float.IsNaN(yMin) == false
                    && float.MinValue != yMin
                    && float.MaxValue != yMin
                    && float.IsInfinity(yMax) == false
                    && float.IsNaN(yMax) == false
                    && float.MinValue != yMax
                    && float.MaxValue != yMax)
                {
                    if (newDrawingSpace.HasValue == false)
                    {
                        newDrawingSpace = new RectangleF();
                    }

                    float width = Math.Max(series.CalculateTotalWidth(_seriesItemWidth, _seriesItemMargin), newDrawingSpace.Value.Width);
                    //Console.WriteLine("   width " + width + " yMax  " + yMax);
                    RectangleF space = new RectangleF(0, yMin, width, yMax - yMin);
                    if (space.Y > 0)
                    {
                        space.Height += space.Y;
                        space.Y = 0;
                    }

                    if (space.Y + space.Height < 0)
                    {
                        float difference = -space.Height - space.Y;
                        space.Height += difference;
                    }
                    //Console.WriteLine("   space.Height " + space.Height + " yMax  " + yMax);
                    newDrawingSpace = space;
                }

            }

            if (newDrawingSpace.HasValue == false)
            {
                return;
            }
            _drawingSpace = newDrawingSpace.Value;
            _drawingSpaceDisplayLimit = _drawingSpace;

            DataProvider dp = AppContext.TradeAnalyzerControl.GetProviderBySymbol(_series[0].Name);

            if (_autoScrollToEnd && (dp == null || !dp.IsPause))
            {// Now scroll to the end of the updated space.
                RectangleF area = GraphicsWrapper.ActualSpaceToDrawingSpace(_actualDrawingSpaceArea);

                // Scroll to end, but also fit zoom, so that all the data at the ending is visible.
                // To achieve it all - use a selection of the ending zone.
                float width = area.Width;
                if (width > _drawingSpaceDisplayLimit.Width)
                {
                    width = _drawingSpaceDisplayLimit.Width;
                }
                Console.WriteLine("_drawingSpaceDisplayLimit.X + _drawingSpaceDisplayLimit.Width - width, _drawingSpaceDisplayLimit.Y, width, _drawingSpaceDisplayLimit.Height)");
                Console.WriteLine(_drawingSpaceDisplayLimit.X + "      " + (_drawingSpaceDisplayLimit.Width - width) + "       " + _drawingSpaceDisplayLimit.Y + "     " + width + "      " + (_drawingSpaceDisplayLimit.Height));
                FitHorizontalAreaToScreen(new RectangleF(_drawingSpaceDisplayLimit.X + _drawingSpaceDisplayLimit.Width - width, _drawingSpaceDisplayLimit.Y, width, _drawingSpaceDisplayLimit.Height));
            }

            // Establish the drawing space limit.
            if (_series.Count > 0)
            {
                float min = float.MaxValue;
                float max = float.MinValue;
                float totalMin = float.MaxValue;
                float totalMax = float.MinValue;

                foreach (ChartSeries series in _series)
                {
                    GetSingleSeriesMinMax(series, null, null, ref totalMin, ref totalMax);

                    if (series.Visible)
                    {
                        min = Math.Min(min, totalMin);
                        max = Math.Max(max, totalMax);
                    }
                }

                if (max == float.MinValue || min == float.MaxValue)
                {// Normalize values, when no visible series, select as if all are visible.
                    if (totalMin == float.MaxValue || totalMax == float.MinValue)
                    {
                        totalMin = 0;
                        totalMax = 1;
                    }

                    min = totalMin;
                    max = totalMax;
                }


                //_drawingSpaceDisplayLimit.X = ActualSpaceToDrawingSpace(new PointF(0,0), true).X;

                _drawingSpaceDisplayLimit.Y = min;
                _drawingSpaceDisplayLimit.Height = max - min;
            }

            if (DrawingSpaceUpdatedEvent != null)
            {
                DrawingSpaceUpdatedEvent(this);
            }

            // Establish the proper label spacing.
            double temp = (_drawingSpaceDisplayLimit.Height / 10);

            double power = 0;
            if (temp < 1)
            {
                while (temp > 0 && temp < 0.1)
                {
                    temp *= 10;
                    power++;
                }
            }
            else
            {
                while (temp >= 10)
                {
                    temp /= 10;
                    power--;
                }
            }

            // Round to 0.2, 0.5 or 1.
            //if (temp <= 1 )
            //{
            //    temp = 0.1;
            //} 
            //else
            if (temp < (0.2 + 0.5) / 2f)
            {
                temp = 0.2;
            }
            else if (temp < (0.5 + 1) / 2f)
            {
                temp = 0.5;
            }
            else if (temp <= 1.5)
            {
                temp = 1;
            }
            else if (temp <= 3)
            {
                temp = 2;
            }
            else if (temp <= 7)
            {
                temp = 5;
            }
            else
            {
                temp = 10;
            }
            // Harry--Modified
            _yAxisLabelSpacing = (float)temp * (float)Math.Pow(10, -(power));
            _yAxisLabelSpacing = _yAxisLabelSpacing * 0.5f;
            //_yAxisLabelSpacing = (float)temp * (float)Math.Pow(10, - (power));
            Console.WriteLine("   _yAxisLabelSpacing    " + _yAxisLabelSpacing);
            //_drawingSpaceGrid.HorizontalLineSpacing = _yAxisLabelSpacing * 2;
            _drawingSpaceGrid.HorizontalLineSpacing = _yAxisLabelSpacing; // *0.2f;
            //_drawingSpaceGrid.HorizontalLineSpacing = _yAxisLabelSpacing * _drawingSpaceGrid.HorizontalLineSpacing;
        }

        protected override void OnPaint(PaintEventArgs paintArgs)
        {
            base.OnPaint(paintArgs);
            Draw(paintArgs.Graphics);
        }

        protected virtual void Draw(Graphics graphics)
        {
            // Needed, as otherwise the crosshair loses its drawing track and leaves artifacts.
            _lastMouseMovePosition = null;

            _graphicsWrapper.SetGraphics(graphics);

            if (this.DesignMode)
            {
                return;
            }

            _graphicsWrapper.SmoothingMode = _smoothingMode;
            if (_fill != null)
            {
                _graphicsWrapper.FillRectangle(_fill, _graphicsWrapper.VisibleClipBounds);
            }

            // The leading date series, can be null if there is no dated series in the chart.
            ChartSeries timeBasedSeries = null;
            // Series.
            foreach (ChartSeries series in _series)
            {
                if (series.SeriesType == ChartSeries.SeriesTypeEnum.TimeBased)
                {
                    if (timeBasedSeries != null)
                    {// There is another data series already. Two data series are not handled.
                        // SystemMonitor.Throw("Two data series in a single chart pane are currently not supported.");
                    }
                    timeBasedSeries = series;
                }
            }

            DrawInitialActualSpaceOverlays(_graphicsWrapper, timeBasedSeries);

            // Drawing area background.
            _graphicsWrapper.FillRectangle(_actualDrawingSpaceAreaFill, _actualDrawingSpaceArea);

            // Clip.
            _graphicsWrapper.ResetClip();
            _graphicsWrapper.SetClip(_actualDrawingSpaceArea);

            // Drawing space.
            //_graphicsWrapper.MultiplyTransform(_graphicsWrapper.DrawingSpaceTransform);
            _graphicsWrapper.DrawingSpaceMode = true;

            DrawDrawingSpace(_graphicsWrapper);

            DrawSelection(_graphicsWrapper);

            // Actual space.
            _graphicsWrapper.DrawingSpaceMode = false;

            // Unclip.
            _graphicsWrapper.ResetClip();

            DrawPostActualSpaceOverlays(_graphicsWrapper);
        }

        void DrawSelection(GraphicsWrapper g)
        {
            if (CurrentSelectionRectangle.HasValue == false)
            {
                return;
            }

            RectangleF selectionRectangle = CurrentSelectionRectangle.Value;

            if (selectionRectangle.Width > 0 && selectionRectangle.Height > 0 &&
                _lastDrawingSpaceMouseRightButtonPosition.HasValue && _currentDrawingSpaceMousePosition.HasValue)
            {
                if (_selectionPen != null)
                {
                    g.DrawRectangle(_selectionPen, selectionRectangle.X, selectionRectangle.Y, selectionRectangle.Width, selectionRectangle.Height);
                }

                if (_selectionFill != null)
                {
                    g.FillRectangle(_selectionFill, selectionRectangle);
                }
            }
        }


        void DrawGraphicSeriesLabels(GraphicsWrapper g, int initialMarginLeft)
        {
            _currentLabelsRectangles = new Rectangle[_series.Count];

            for (int i = 0; i < _series.Count; i++)
            {
                if (i == 0)
                {
                    _currentLabelsRectangles[0].X = initialMarginLeft;
                }
                else
                {
                    _currentLabelsRectangles[i].X = _currentLabelsRectangles[i - 1].Right + (int)_labelsMargin;
                }

                _currentLabelsRectangles[i].Y = (int)_labelsTopMargin;

                SizeF seriesSize = g.MeasureString(_series[i].Name, _labelsFont);
                _currentLabelsRectangles[i].Size = new Size((int)seriesSize.Width, (int)seriesSize.Height);

                int iconWidth = 18;

                // Add space for series icon
                _currentLabelsRectangles[i].Width += iconWidth;

                if (_labelsFill != null)
                {
                    g.FillRectangle(_labelsFill, _currentLabelsRectangles[i]);
                }

                if (_labelsFont != null)
                {
                    g.DrawString(_series[i].Name, _labelsFont, _labelsFontBrush, _currentLabelsRectangles[i].X + iconWidth, _currentLabelsRectangles[i].Y);
                }

                _series[i].DrawSeriesIcon(g, new Rectangle(_currentLabelsRectangles[i].X + 2, _currentLabelsRectangles[i].Y + 2, 14, _currentLabelsRectangles[i].Height - 4));
            }

        }


        protected virtual void DrawInitialActualSpaceOverlays(GraphicsWrapper g, ChartSeries timeBasedSeries)
        {
            {// X Axis labels.
                float totalItemWidth = _seriesItemWidth + _seriesItemMargin;
                float actualXSpacing = _xAxisLabelSpacing * totalItemWidth;
                if (_considerAxisLabelsSpacingScale)
                {
                    int xScaling = Math.Abs((int)(1 / _graphicsWrapper.DrawingSpaceTransform.Elements[0]));
                    if (xScaling > 1)
                    {
                        actualXSpacing = actualXSpacing * xScaling;
                    }
                }

                // Set starting to the closes compatible positionactualXSpacing
                // TODO : this can be optimized further by narrowing the range of xStart to end
                float xStart = (int)(_drawingSpaceDisplayLimit.X / actualXSpacing);
                xStart = xStart * actualXSpacing;

                for (float i = xStart; i < _drawingSpaceDisplayLimit.X + _drawingSpaceDisplayLimit.Width; i += actualXSpacing)
                {
                    PointF point = GraphicsWrapper.DrawingSpaceToActualSpace(new PointF(i, 0), true);
                    if (point.X > _actualDrawingSpaceArea.X - 10
                        && point.X < _actualDrawingSpaceArea.X + _actualDrawingSpaceArea.Width)
                    {
                        int index = (int)(i / totalItemWidth);
                        string message = string.Empty;
                        if (timeBasedSeries != null)
                        {// If there is a leading dateAssignedSeries show labels based on its timing.
                            if (timeBasedSeries.MaximumIndex > index)
                            {
                                message = GeneralHelper.GetShortDateTime(timeBasedSeries.GetTimeAtIndex(index));

                            }
                        }
                        else
                        {
                            message = index.ToString(_xAxisLabelsFormat);
                        }

                        if (_axisLabelsFont != null && _xAxisLabelsFontBrush != null)
                        {
                            g.DrawString(message, _axisLabelsFont, _xAxisLabelsFontBrush, point.X, _actualDrawingSpaceArea.Y + _actualDrawingSpaceArea.Height);
                        }

                        // Draw the small line indicating where the string applies for.
                        g.DrawLine(_actualDrawingSpaceAreaBorderPen, point.X, _actualDrawingSpaceArea.Y + _actualDrawingSpaceArea.Height, point.X, _actualDrawingSpaceArea.Y + _actualDrawingSpaceArea.Height + 5);
                    }
                }
            }

            if (_axisLabelsFont != null && _xAxisLabelsFontBrush != null)
            {
                _actualDrawingSpaceAreaMarginBottom = 20;
            }
            else
            {
                _actualDrawingSpaceAreaMarginBottom = 8;
            }

            {// Y Axis labels.

                int? yAxisLabelsWidth = null;

                float actualYSpacing = _yAxisLabelSpacing;
                if (_considerAxisLabelsSpacingScale)
                {
                    int yScaling = Math.Abs((int)(1 / _graphicsWrapper.DrawingSpaceTransform.Elements[3]));
                    if (yScaling > 1)
                    {
                        actualYSpacing = actualYSpacing * yScaling;
                    }
                }

                // Set starting to the closes compatible positionactualXSpacing
                int maxDecimalPlaces = actualYSpacing.ToString().Length - 1;
                float yStart = (int)(_drawingSpaceDisplayLimit.Y / actualYSpacing);
                yStart = yStart * actualYSpacing;
                // Round off to a fixed number of post decimal point digits, will only work for values under 1
                yStart = (float)Math.Round(yStart, maxDecimalPlaces);


                {
                    //float minLabelValue = yStart;
                    //float maxLabelValue = _drawingSpaceDisplayLimit.Y + _drawingSpaceDisplayLimit.Height;
                    // Harry--Modified
                    // This must auto adjust to format the number properly and always fit in 6 spaces.

                    // Specify positive, negative and zero formats.

                        _yAxisLabelsFormat = " #0.#####;-#0.#####; Zero";

                    //_yAxisLabelsFormat = " #0.##;-#0.##; Zero";
                    // original 
                    //_yAxisLabelsFormat = " #0.#####;-#0.#####; Zero";
                    //_yAxisLabelsFormat = " 0.000;-0.000;Zero";

                    // The default is 6 positions total for the y axis labels.
                    yAxisLabelsWidth = ((int)g.MeasureString("00.00000", _axisLabelsFont).Width);
                    //yAxisLabelsWidth = ((int)g.MeasureString("00.00", _axisLabelsFont).Width);
                }

                if (yAxisLabelsWidth.HasValue)
                {// Calculate the current margin and confirm with any controling subscriber.
                    _actualDrawingSpaceAreaMarginLeft = yAxisLabelsWidth.Value + 5;
                    if (ActualDrawingSpaceMarginLeftUpdateEvent != null)
                    {
                        _actualDrawingSpaceAreaMarginLeft = ActualDrawingSpaceMarginLeftUpdateEvent(this, _actualDrawingSpaceAreaMarginLeft);
                    }
                }

                // Pass 2 - actually draw the labels and label lines at the established and confirmed location.
                for (float i = yStart; i < _drawingSpaceDisplayLimit.Y + _drawingSpaceDisplayLimit.Height; i += actualYSpacing)
                {
                    i = (float)Math.Round(i, maxDecimalPlaces);
                    PointF point = GraphicsWrapper.DrawingSpaceToActualSpace(new PointF(0, i), true);
                    if (point.Y > _actualDrawingSpaceArea.Y &&
                        point.Y < _actualDrawingSpaceArea.Y + _actualDrawingSpaceArea.Height)
                    {
                        if (_axisLabelsFont != null && _yAxisLabelsFontBrush != null)
                        {
                            g.DrawString((i).ToString(_yAxisLabelsFormat), _axisLabelsFont, _yAxisLabelsFontBrush, _actualDrawingSpaceAreaMarginLeft - yAxisLabelsWidth.Value - 3, point.Y);
                        }

                        // Draw the small line indicating where the string applies for.
                        g.DrawLine(_actualDrawingSpaceAreaBorderPen, _actualDrawingSpaceAreaMarginLeft - 5, point.Y, _actualDrawingSpaceAreaMarginLeft, point.Y);
                    }
                }

            }

            if (ShowSeriesLabels)
            {
                _actualDrawingSpaceAreaMarginTop = 30;
            }
            else
            {
                _actualDrawingSpaceAreaMarginTop = 5;
            }

            UpdateActualDrawingSpaceArea();

            // Actual space, drawing area, grid.
            _actualSpaceGrid.Draw(g, _actualDrawingSpaceArea, _actualDrawingSpaceArea, 1);

            if (ShowSeriesLabels)
            {
                DrawGraphicSeriesLabels(g, _actualDrawingSpaceArea.Left);
            }

            // Show 
            if (_customObjectsManager.IsBuildingObject)
            {
                g.DrawImageUnscaledAndClipped(_customObjectDrawingImage, new Rectangle(4, (int)LabelsTopMargin, _customObjectDrawingImage.Width, _customObjectDrawingImage.Height));
            }
        }

        protected virtual void DrawPostActualSpaceOverlays(GraphicsWrapper g)
        {
            if (_titleFont != null && _titleFontBrush != null)
            {
                // Title
                Rectangle titleRectangle = new Rectangle(_actualDrawingSpaceArea.Left, _actualDrawingSpaceArea.Top, 0, 0);
                SizeF titleSize = g.MeasureString(_chartName, _titleFont);
                titleRectangle.Size = new Size((int)titleSize.Width, (int)titleSize.Height);
                g.DrawString(_chartName, _titleFont, _titleFontBrush, titleRectangle.Location);
            }

            if (_actualDrawingSpaceAreaBorderPen != null)
            {
                // Border
                g.DrawRectangle(_actualDrawingSpaceAreaBorderPen, _actualDrawingSpaceArea.X - 1, _actualDrawingSpaceArea.Y - 1, _actualDrawingSpaceArea.Width + 1, _actualDrawingSpaceArea.Height + 1);
            }
        }

        void DrawDrawingSpace(GraphicsWrapper g)
        {
            RectangleF drawingSpaceClipping = _actualDrawingSpaceArea;
            drawingSpaceClipping.X -= _seriesItemMargin + _seriesItemWidth;
            drawingSpaceClipping.Y -= _seriesItemMargin + _seriesItemWidth;

            drawingSpaceClipping.Width += 2 * (_seriesItemMargin + _seriesItemWidth);
            drawingSpaceClipping.Height += 2 * (_seriesItemMargin + _seriesItemWidth);

            drawingSpaceClipping = GraphicsWrapper.ActualSpaceToDrawingSpace(drawingSpaceClipping);
            //drawingSpaceClipping.Y = DrawingSpace.Y - 10;
            //drawingSpaceClipping.Height = DrawingSpace.Height + 10;

            // Grid.
            _drawingSpaceGrid.Draw(g, drawingSpaceClipping, _drawingSpace, _seriesItemMargin + _seriesItemWidth);

            // Show clipping rectangle.
            if (ShowClippingRectangle)
            {
                Pen clippingRectanglePen = (Pen)Pens.DarkGray.Clone();
                clippingRectanglePen.DashStyle = DashStyle.Dash;

                g.DrawRectangle(clippingRectanglePen, drawingSpaceClipping.X, drawingSpaceClipping.Y, drawingSpaceClipping.Width, drawingSpaceClipping.Height);
            }

            // Draw custom objects - pre series.
            _customObjectsManager.Draw(g, drawingSpaceClipping, CustomObject.DrawingOrderEnum.PreSeries);

            // Series.
            foreach (ChartSeries series in _series)
            {
                series.Draw(g, CurrentUnitUnification, drawingSpaceClipping, _seriesItemWidth, _seriesItemMargin);
            }

            // Draw custom objects - post series.
            _customObjectsManager.Draw(g, drawingSpaceClipping, CustomObject.DrawingOrderEnum.PostSeries);

        }

        protected virtual void UpdateActualDrawingSpaceArea()
        {
            //TracerHelper.Trace(TracerHelper.GetFullCallingMethodName(2));
            //TracerHelper.Trace(_actualDrawingSpaceAreaMarginLeft.ToString());

            Rectangle newArea = new Rectangle(_actualDrawingSpaceAreaMarginLeft, _actualDrawingSpaceAreaMarginTop,
                this.Width - _actualDrawingSpaceAreaMarginLeft - _actualDrawingSpaceAreaMarginRight, this.Height - _actualDrawingSpaceAreaMarginTop - _actualDrawingSpaceAreaMarginBottom);

            _actualDrawingSpaceArea = newArea;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (Size.Width == 0 || Size.Height == 0)
            {
                return;
            }

            if (_lastControlSize.IsEmpty == false)
            {// Not the first run.
                float xScaling = (float)Size.Width / (float)_lastControlSize.Width;
                float yScaling = (float)Size.Height / (float)_lastControlSize.Height;

                // Old style protection.
                //bool isValidSize = (xScaling > 0 && yScaling > 0 && float.IsNaN(xScaling) == false && float.IsNaN(yScaling) == false
                //        && float.IsInfinity(xScaling) == false && float.IsInfinity(yScaling) == false);
                //if (isValidSize)
                //{
                // Default - base zooming on left.
                PointF pointActualSpace = new PointF(_actualDrawingSpaceArea.Left, _actualDrawingSpaceArea.Top);
                PointF drawSpacePoint = GraphicsWrapper.ActualSpaceToDrawingSpace(pointActualSpace, true);

                this.HandleScale(drawSpacePoint, xScaling, yScaling);
                //}
            }

            _lastControlSize = this.Size;
            UpdateActualDrawingSpaceArea();
            this.Invalidate();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            CleanTimePriceLabel();
            if (_actualDrawingSpaceArea.Contains(e.Location) == false)
            {// Not in drawing area.
                return;
            }

            if (ScrollModeEnum.None == ScrollMode)
            {// Do nothing on scroll.
                return;
            }

            float delta = e.Delta;
            float ScalingFactor = 1000;

            // Normalize delta.
            delta = (float)Math.Max(delta, -0.8 * ScalingFactor);
            delta = (float)Math.Min(delta, 0.8 * ScalingFactor);

            float scaleApplied = 1 + delta / ScalingFactor;

            // Default - base zooming on left.
            PointF pointActualSpace = new PointF(_actualDrawingSpaceArea.Left, _actualDrawingSpaceArea.Top);
            PointF drawSpacePoint = GraphicsWrapper.ActualSpaceToDrawingSpace(pointActualSpace, true);

            ScrollModeEnum scrollMode = ScrollMode;
            if (_isControlKeyDown && _isShiftKeyDown)
            {
                scrollMode = ScrollModeEnum.ZoomToMouse;
            }
            else if (_isControlKeyDown)
            {
                scrollMode = ScrollModeEnum.HorizontalZoom;
            }
            else if (_isShiftKeyDown)
            {
                scrollMode = ScrollModeEnum.VerticalZoom;
            }


            if (ScrollModeEnum.HorizontalZoom == scrollMode)
            {// Scale horizontal.

                HandleScale(drawSpacePoint, scaleApplied, 1);
            }
            else if (ScrollModeEnum.VerticalZoom == scrollMode)
            {// Scale vertical.

                HandleScale(drawSpacePoint, 1, scaleApplied);
            }
            else if (ScrollModeEnum.HorizontalScroll == scrollMode
                    || ScrollModeEnum.HorizontalScrollAndFit == scrollMode)
            {
                // Elements 0 gives the zoom in X, so divide to scroll faster on faster zooms.
                float zoomElement = _graphicsWrapper.DrawingSpaceTransform.Elements[0];
                Point dragVector = new Point((int)(-e.Delta / zoomElement), 0);
                HandlePan(true, dragVector);

                if (ScrollModeEnum.HorizontalScrollAndFit == scrollMode)
                {// After scrolling also fit the data
                    RectangleF area = GraphicsWrapper.ActualSpaceToDrawingSpace(_actualDrawingSpaceArea);
                    FitHorizontalAreaToScreen(area);
                }

            }
            else if (ScrollModeEnum.VerticalScroll == scrollMode)
            {
                // Elements 0 gives the zoom in X, so divide to scroll faster on faster zooms.
                float zoomElement = _graphicsWrapper.DrawingSpaceTransform.Elements[0];
                Point dragVector = new Point(0, (int)(-e.Delta / zoomElement));
                HandlePan(_limitedView, dragVector);
            }
            else if (ScrollModeEnum.ZoomToMouse == scrollMode)
            {// Base zooming on mouse coordinates.
                drawSpacePoint = GraphicsWrapper.ActualSpaceToDrawingSpace(e.Location, true);

                HandleScale(drawSpacePoint, scaleApplied, scaleApplied);
            }

            //===================================================================
            ChartSeries series = _series[0];
            DataProvider dp = AppContext.TradeAnalyzerControl.GetProviderBySymbol(series.Name);
            if (dp != null)
            {
                dp.IsPause = true;
                dp.LastActionTime = System.DateTime.Now;
            }

            Console.WriteLine(" series width   " + series.CalculateTotalWidth(_seriesItemWidth, _seriesItemMargin));
            Console.WriteLine(" series max index    " + series.MaximumIndex);

            _lastMouseMovePosition = e.Location;

            Point pointLeft = new Point((int)(_actualDrawingSpaceArea.X), _lastMouseMovePosition.Value.Y);
            Point pointRight = new Point((int)(_actualDrawingSpaceArea.X + _actualDrawingSpaceArea.Width), _lastMouseMovePosition.Value.Y);

            PointF pointLeftX = GraphicsWrapper.ActualSpaceToDrawingSpace(pointLeft, true);
            PointF pointRightX = GraphicsWrapper.ActualSpaceToDrawingSpace(pointRight, true);

            Console.WriteLine(" (pointLeftX " + pointLeftX);
            Console.WriteLine("pointRightX   " + pointRightX);

            Console.WriteLine(" _actualDrawingSpaceArea.X  " + _actualDrawingSpaceArea.X);
            Console.WriteLine(" _actualDrawingSpaceArea.Width  " + _actualDrawingSpaceArea.Width);
            float totalItemWidth = _seriesItemWidth + _seriesItemMargin;
            Console.WriteLine(" totalItemWidth " + totalItemWidth);

            int startIndex = (int)((pointLeftX.X - scaleApplied) / totalItemWidth);
            int endIndex = (int)((pointRightX.X - scaleApplied) / totalItemWidth);

            Console.WriteLine(" startIndex " + startIndex + "  endIndex  " + endIndex);

            if (endIndex >= series.MaximumIndex) endIndex = series.MaximumIndex - 20;
            if (startIndex >= endIndex) { if (endIndex - 30 > 0) startIndex = endIndex - 30; else startIndex = 0; }
            if (startIndex < 0) startIndex = 0;
            float yMin = float.MaxValue;
            float yMax = float.MinValue;

            GetSingleSeriesMinMax(series, startIndex, endIndex, ref yMin, ref yMax);

            DateTime sdt = series.GetTimeAtIndex(startIndex);
            DateTime edt = series.GetTimeAtIndex(endIndex);

            Console.WriteLine(" sdt === " + sdt + "  edt ===  " + edt);
            Console.WriteLine(" yMax === " + yMax + "  yMin ===  " + yMin);

            _currentDrawingSpaceMousePosition = GraphicsWrapper.ActualSpaceToDrawingSpace(e.Location, true);
            _lastDrawingSpaceMouseDownLeftButton = _currentDrawingSpaceMousePosition;

            RectangleF xarea = GraphicsWrapper.ActualSpaceToDrawingSpace(_actualDrawingSpaceArea);

            float width = xarea.Width;
            if (width > _drawingSpaceDisplayLimit.Width)
            {
                width = _drawingSpaceDisplayLimit.Width;
            }

            FitHorizontalAreaToScreen(new RectangleF((pointLeftX.X - scaleApplied), 0, width, (float)(yMax - yMin)));
            //===================================================================

            this.Refresh();
        }

        void MenuSeriesItem_Click(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            _lastSeriesClicked.SetSelectedChartType((string)item.Tag);
            this.Refresh();
        }

        void seriesPropertiesMenuItem_Click(object sender, EventArgs e)
        {
            ChartSeries series = (ChartSeries)(((ToolStripItem)sender).Tag);
            CustomPropertiesControl control = new CustomPropertiesControl();
            control.SelectedObject = series;
            HostingForm form = new HostingForm(series.Name + " Properties", control);
            form.MaximizeBox = false;
            form.ShowCloseButton = true;
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
            form.Show();
        }

        void ShowChartContextMenu(Point position)
        {
            _crossHairContextMenuItem.Checked = this.CrosshairVisible;

            _chartContextMenu.Show(this, position);
        }

        void SelectedObjectsChartContextMenuItem_Click(object param, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)param;
            DynamicCustomObject dynamicObject = (DynamicCustomObject)item.Tag;
            CustomPropertiesControl control = new CustomPropertiesControl();
            control.SelectedObject = dynamicObject;
            control.AutoSize = true;
            HostingForm form = new HostingForm("Properties", control);
            form.Width = 400;
            form.Height = 600;
            form.Show();
            //this.Refresh();
        }

        void LabelsChartContextMenuItem_Click(object param, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)param;
            this.ShowSeriesLabels = item.Checked;
            this.Refresh();
        }

        void LimitViewChartContextMenuItem_Click(object param, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)param;
            this.LimitedView = item.Checked;
        }

        void CrosshairChartContextMenuItem_Click(object param, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)param;
            this.CrosshairVisible = item.Checked;
            this.Refresh();
        }

        void FitToScreenChartContextMenuItem_Click(object param, EventArgs e)
        {
            this.FitDrawingSpaceToScreen(true, true);
        }

        void FitHorizontalChartContextMenuItem_Click(object param, EventArgs e)
        {
            this.FitDrawingSpaceToScreen(true, false);
        }

        void FitVerticalChartContextMenuItem_Click(object param, EventArgs e)
        {
            this.FitDrawingSpaceToScreen(false, false);
        }

        void ZoomInChartContextMenuItem_Click(object param, EventArgs e)
        {
            this.ZoomIn(2);
        }

        void ZoomOutChartContextMenuItem_Click(object param, EventArgs e)
        {
            this.ZoomOut(0.5f);
        }

        void AppearanceChartContextMenuItem_Click(object param, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)param;
            this.SetAppearanceScheme((AppearanceSchemeEnum)Enum.Parse(typeof(AppearanceSchemeEnum), item.Tag as string));
            this.Refresh();
        }

        void ScrollChartContextMenuItem_Click(object param, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)param;
            this.ScrollMode = (ScrollModeEnum)Enum.Parse(typeof(ScrollModeEnum), item.Tag as string);
        }

        void SelectionChartContextMenuItem_Click(object param, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)param;
            this.RightMouseButtonSelectionMode = (SelectionModeEnum)Enum.Parse(typeof(SelectionModeEnum), item.Tag as string);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            CleanTimePriceLabel();
            _crosshairVisible = !_crosshairVisible;
            _crossHairContextMenuItem.Checked = !_crossHairContextMenuItem.Checked;
            if (_customObjectsManager != null && _customObjectsManager.OnMouseDoubleClick(e))
            {// Handled by custom objects manager.
                return;
            }
        }

        private void CleanTimePriceLabel()
        {
            timeLabel.Visible = false;
            timeLabel.Invalidate();

            priceLabel.Visible = false;
            priceLabel.Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            CleanTimePriceLabel();
            if (ShowSeriesLabels)
            {
                for (int i = 0; i < _currentLabelsRectangles.Length; i++)
                {// Find if click was inside one of the labels.
                    if (_currentLabelsRectangles[i].Contains(e.Location))
                    {// Label clicked.
                        if (e.Button == MouseButtons.Left)
                        {// Show/Hide series.
                            _series[i].Visible = !_series[i].Visible;
                        }
                        else if (e.Button == MouseButtons.Right)
                        {// Prepare context sensitive right click menu items.

                            string[] seriesTypeNames = _series[i].ChartTypes;

                            _seriesTypeDynamicContextMenu.Items.Clear();
                            for (int j = 0; j < seriesTypeNames.Length; j++)
                            {
                                ToolStripMenuItem item = new ToolStripMenuItem(seriesTypeNames[j]);

                                _seriesTypeDynamicContextMenu.Items.Add(item);
                                item.Click += new EventHandler(MenuSeriesItem_Click);
                                item.Checked = _series[i].SelectedChartType == seriesTypeNames[j];
                                if (item.Checked)
                                {// Mark the currently selected item
                                    item.Image = (Image)(_resources.GetObject("imageDot"));
                                }
                                item.Tag = seriesTypeNames[j];
                            }


                            _seriesTypeDynamicContextMenu.Items.Add(new ToolStripSeparator());

                            ToolStripItem propertiesMenuItem = _seriesTypeDynamicContextMenu.Items.Add("Properties");
                            propertiesMenuItem.Click += new EventHandler(seriesPropertiesMenuItem_Click);
                            propertiesMenuItem.Tag = _series[i];

                            _lastSeriesClicked = _series[i];
                            _seriesTypeDynamicContextMenu.Show(this, e.Location);
                        }

                        this.Invalidate();
                        return;
                    }
                }
            }

            if (e.Button == MouseButtons.Right)
            {
                if (CurrentUserSelectedRectangle.HasValue)
                {
                    PointF actualSpace = GraphicsWrapper.DrawingSpaceToActualSpace(new PointF(CurrentUserSelectedRectangle.Value.Width, CurrentUserSelectedRectangle.Value.Height), false);

                    if (actualSpace.X < MinimumAbsoluteSelectionWidth)
                    {// Selection applied is too small and a context menu is displayed.
                        ShowChartContextMenu(e.Location);
                    }
                }
            }

        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            CleanTimePriceLabel();
            if (_customObjectsManager != null && _customObjectsManager.OnMouseDown(e))
            {
                return;
            }

            this.Focus();

            if (_actualDrawingSpaceArea.Contains(e.Location) == false)
            {// Not clicked in drawing area.
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                _lastDrawingSpaceMouseDownLeftButton = GraphicsWrapper.ActualSpaceToDrawingSpace(e.Location, true);
            }
            if (e.Button == MouseButtons.Right)
            {
                _lastDrawingSpaceMouseRightButtonPosition = GraphicsWrapper.ActualSpaceToDrawingSpace(e.Location, true);
            }
            else if (e.Button == MouseButtons.Middle)
            {
                _lastDrawingSpaceMouseDownMiddleButton = GraphicsWrapper.ActualSpaceToDrawingSpace(e.Location, true);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (_customObjectsManager != null && _customObjectsManager.OnMouseUp(e))
            {
                return;
            }

            _currentDrawingSpaceMousePosition = GraphicsWrapper.ActualSpaceToDrawingSpace(e.Location, true);

            if (e.Button == MouseButtons.Left)
            {
                _lastDrawingSpaceMouseDownLeftButton = null;
            }
            if (e.Button == MouseButtons.Right)
            {
                if (CurrentSelectionRectangle.HasValue)
                {
                    PointF actualSpace = GraphicsWrapper.DrawingSpaceToActualSpace(new PointF(CurrentUserSelectedRectangle.Value.Width, CurrentUserSelectedRectangle.Value.Height), false);

                    if (actualSpace.X < MinimumAbsoluteSelectionWidth
                        || float.IsNaN(CurrentUserSelectedRectangle.Value.Height)
                        || float.IsInfinity(CurrentUserSelectedRectangle.Value.Height))
                    {// Minimum selection applied.
                        this.Invalidate();
                    }
                    else
                    {
                        HandleSelect(CurrentSelectionRectangle.Value);
                    }
                }
                _lastDrawingSpaceMouseRightButtonPosition = null;
            }
            else if (e.Button == MouseButtons.Middle)
            {
                _lastDrawingSpaceMouseDownMiddleButton = null;
            }
        }

        //Label timeLable = new Label();

        string time = "";
        string price = "";
        string message = "";
        double high;
        double low;
        double open;
        double close;
        int currentIndex=-1;
        string sid = "";
        double exHigh = 0;
        double exLow = 0;
        double sar = 0;
        double ar = 0;
        double ar2 = 0;
        double rsi = 0;
        double cr = 0;
        double rsi2 = 0; double rsi3 = 0;
        double rsi4 = 0;
        double cci = 0;
        double cci2 = 0;
        double cci3 = 0;
        double wr1 = 0;
        double wr2 = 0;
        double lwr1 = 0;
        double lwr2 = 0;
        double bup = 0;
        double bmid = 0;
        double blow = 0;

        void DrawCrossHairLocationInfo(Point mouseLocation)
        {

            time = "";
            price = "";
            message = "";
            high=0;
            low=0;
            open=0;
            close=0;
            currentIndex=-1;
            exHigh = 0;
            exLow = 0;
            rsi = 0;
            rsi2 = 0;
            rsi3 = 0;
            rsi4 = 0;
            cr = 0;
            sar = 0;
            ar = 0;
            ar2 = 0;
            cci = 0;
            cci2 = 0;
            cci3 = 0;
            wr1 = 0;
            wr2 -= 0;
            lwr1 = 0;
            lwr2 = 0;
             bup = 0;
             bmid = 0;
             blow = 0;
            // Show cross hair location message.
            // Do not use createGraphics
            Graphics g = this.CreateGraphics();

            PointF drawingPoint = GraphicsWrapper.ActualSpaceToDrawingSpace(mouseLocation, true);
            float totalItemWidth = _seriesItemWidth + _seriesItemMargin;
            int index = (int)(drawingPoint.X / totalItemWidth);
            currentIndex = index;

            // The leading date series, can be null if there is no dated series in the chart.
            ChartSeries timeBasedSeries = null;
            // Series.
            foreach (ChartSeries series in _series)
            {
                if (series.SeriesType == ChartSeries.SeriesTypeEnum.TimeBased)
                {
                    if (timeBasedSeries != null)
                    {// There is another data series already. Two data series are not handled.
                        // SystemMonitor.Throw("Two data series in a single chart pane are currently not supported.");
                    }
                    timeBasedSeries = series;
                    sid = series.Name;
                }
            }

            message = string.Empty;
            string numformat = "";
                        string priceformat = "";
               Symbol symbol = AppUtil.ParseSymbol(sid);
                    if (!AppConst.JPYGOLD.Contains(symbol))
                    {
                        numformat = "0.0000; -0.0000";
                           priceformat = "0.00000; -0.00000";
                    }
                    else
                    {
                        if(symbol ==Symbol.GOLD )
                            priceformat = "0.00; -0.00";
                        else if( symbol==Symbol.EURJPY || symbol==Symbol.GBPJPY)
                            priceformat = "0.000; -0.000";
                        else
                            priceformat = "0.0000; -0.0000";

                         numformat = "0.00000; -0.00000";
                    }
            if (timeBasedSeries != null)
            {// If there is a leading dateAssignedSeries show labels based on its timing.
                if (timeBasedSeries.MaximumIndex > index && index >= 0)
                {
                    message = GeneralHelper.GetShortDateTime(timeBasedSeries.GetTimeAtIndex(index));
                    time = GeneralHelper.GetShortDateTime(timeBasedSeries.GetTimeAtIndex(index));
                    open = timeBasedSeries.GetOpenAtIndex(index);
                    close = timeBasedSeries.GetCloseAtIndex(index);
                    low = timeBasedSeries.GetLowAtIndex(index);
                    high = timeBasedSeries.GetHightAtIndex(index);
                    exLow = timeBasedSeries.GetExLowAtIndex(index);
                    exHigh = timeBasedSeries.GetExHightAtIndex(index);
                    sar = timeBasedSeries.GetSarAtIndex(index);
                    rsi = timeBasedSeries.GetRsiAtIndex(index);
                    rsi2 = timeBasedSeries.GetRsi2AtIndex(index);
                    rsi3 = timeBasedSeries.GetRsi3AtIndex(index);
                    rsi4 = timeBasedSeries.GetRsi4AtIndex(index);
                    cr = timeBasedSeries.GetCrAtIndex(index);
                    ar = timeBasedSeries.GetArAtIndex(index);
                    ar2 = timeBasedSeries.GetAr2AtIndex(index);
                    cci = timeBasedSeries.GetCciAtIndex(index);
                    cci2 = timeBasedSeries.GetCci2AtIndex(index);
                    cci3 = timeBasedSeries.GetCci3AtIndex(index);
                    wr1 = timeBasedSeries.GetWrAtIndex(index);
                    wr2 = timeBasedSeries.GetWr2AtIndex(index);
                    Dictionary<string, double> lwr = timeBasedSeries.GetLwrAtIndex(index);
                    Dictionary<string, double> indicators = timeBasedSeries.GetIndicatorsAtIndex(index);
                    Dictionary<string, double> boll = timeBasedSeries.GetBollAtIndex(index);
                    if(lwr!=null && lwr.ContainsKey(LWR.LWR1))
                    lwr1 = lwr[LWR.LWR1];
                      if(lwr!=null && lwr.ContainsKey(LWR.LWR2))
                    lwr2 = lwr[LWR.LWR2];
                      if (boll != null)
                      {
                          bup = boll[BOLL.UPPER];
                          bmid = boll[BOLL.MID];
                          blow = boll[BOLL.LOWER];
                      }
                    //message += " SAR:" + timeBasedSeries.GetSARAtIndex(index).ToString("0.00000; -0.00000");
                    //message += " , RSI:" + timeBasedSeries.GetRSIAtIndex(index).ToString("0.00000; -0.00000");
                    //message += " , CR:" + timeBasedSeries.GetCRAtIndex(index).ToString("0.00000; -0.00000");
                    message += " | SAR: " + sar.ToString();
                    message += " | AR: " + ar.ToString();
                    message += " | RSI-6: " + rsi.ToString();
                    //message += " | RSI-8: " + rsi2.ToString();
                    message += " | RSI-14: " + rsi3.ToString();
                    //message += " | RSI-22: " + rsi4.ToString();
                    message += " | CCI-14: " + cci.ToString();
                    message += " | CCI-24: " + cci2.ToString();
                    message += " | 开盘: " +open.ToString(numformat); 
                    message += " , 收盘: " + close.ToString(numformat);
                    message += " , 最低: " +low.ToString(numformat);
                    message += " , 最高: " + high.ToString(numformat);
                    //"#0.#####;-#0.#####;"
                }
            }
            else
            {
                message = index.ToString(_xAxisLabelsFormat);
            }

            /** comment by herry
            message = message + " | " + drawingPoint.Y.ToString(_yAxisLabelsFormat);
             * **/
            price = drawingPoint.Y.ToString(priceformat);
            /** 
             * Commented by Harry
            //SizeF stringSize = g.MeasureString(message, _axisLabelsFont);
            //g.FillRectangle(_fill, new Rectangle(_actualDrawingSpaceArea.X + _actualDrawingSpaceArea.Width - (int)stringSize.Width - 10, (int)_labelsTopMargin, (int)stringSize.Width + 10, (int)stringSize.Height));

            //Brush brush = _xAxisLabelsFontBrush;
            //if (brush == null)
            //{
            //    brush = SystemBrushes.ControlText;
            //}

            //g.DrawString(message, new Font("Arial", 8, FontStyle.Bold), Brushes.LightBlue, _actualDrawingSpaceArea.X + _actualDrawingSpaceArea.Width - stringSize.Width, _labelsTopMargin);
            Commented by Harry 
             * **/
        }

        //public ChartControl chartControl;
        //delegate void UpdateTimeLabelDelegate(object obj, int x, int y);

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            //candleLabel.Visible = false;
            if (_customObjectsManager != null && _customObjectsManager.OnMouseMove(e))
            {// Event handled by user objects handler.
                return;
            }

            if (_crosshairVisible && _actualDrawingSpaceArea.Contains(e.Location))
            {
                Graphics g = this.CreateGraphics();
                DrawCrossHairLocationInfo(e.Location);
                timeLabel.Location = new Point((int)e.Location.X, (int)(_actualDrawingSpaceArea.Y + _actualDrawingSpaceArea.Height));
                timeLabel.Text = time;
                timeLabel.ForeColor = Color.LightBlue;

                SizeF stringSize = g.MeasureString("00.00000", _axisLabelsFont);
                priceLabel.Text = price;
                priceLabel.Location = new Point((int)(_actualDrawingSpaceArea.X - stringSize.Width), (int)(e.Location.Y - stringSize.Height));
                priceLabel.ForeColor = Color.White;

                this.Refresh();


                Point pointLeft, pointRight, pointUp, pointDown;
                if (_lastMouseMovePosition.HasValue)
                {
                    pointLeft = new Point(_actualDrawingSpaceArea.X, _lastMouseMovePosition.Value.Y);
                    pointRight = new Point(_actualDrawingSpaceArea.X + _actualDrawingSpaceArea.Width, _lastMouseMovePosition.Value.Y);

                    pointUp = new Point(_lastMouseMovePosition.Value.X, _actualDrawingSpaceArea.Y);
                    pointDown = new Point(_lastMouseMovePosition.Value.X, _actualDrawingSpaceArea.Y + _actualDrawingSpaceArea.Height);
                    Console.WriteLine(" _actualDrawingSpaceArea.X  " + _actualDrawingSpaceArea.X);
                    Console.WriteLine(" hasv  l " + pointLeft + " r " + pointRight + "  u " + pointUp + " d " + pointDown);
                    // Erase previous line
                    ControlPaint.DrawReversibleLine(this.PointToScreen(pointLeft), this.PointToScreen(pointRight), this.BackColor);
                    ControlPaint.DrawReversibleLine(this.PointToScreen(pointUp), this.PointToScreen(pointDown), this.BackColor);
                }

                _lastMouseMovePosition = e.Location;

                pointLeft = new Point(_actualDrawingSpaceArea.X, _lastMouseMovePosition.Value.Y);
                pointRight = new Point(_actualDrawingSpaceArea.X + _actualDrawingSpaceArea.Width, _lastMouseMovePosition.Value.Y);

                pointUp = new Point(_lastMouseMovePosition.Value.X, _actualDrawingSpaceArea.Y);
                pointDown = new Point(_lastMouseMovePosition.Value.X, _actualDrawingSpaceArea.Y + _actualDrawingSpaceArea.Height);

                Console.WriteLine(" _actualDrawingSpaceArea.X  " + _actualDrawingSpaceArea.X);
                Console.WriteLine(" NO value  l " + pointLeft + " r " + pointRight + "  u " + pointUp + " d " + pointDown);

                stringSize = g.MeasureString(message, _axisLabelsFont);
                g.FillRectangle(_fill, new Rectangle(_actualDrawingSpaceArea.X + _actualDrawingSpaceArea.Width - (int)stringSize.Width - 10, (int)_labelsTopMargin, (int)stringSize.Width + 10, (int)stringSize.Height));

                Brush brush = _xAxisLabelsFontBrush;
                if (brush == null)
                {
                    brush = SystemBrushes.ControlText;
                }

                g.DrawString(message, new Font("Arial", 8, FontStyle.Bold), Brushes.LightCyan, _actualDrawingSpaceArea.X + _actualDrawingSpaceArea.Width - stringSize.Width, _labelsTopMargin);

                // New line.
                ControlPaint.DrawReversibleLine(this.PointToScreen(pointLeft), this.PointToScreen(pointRight), this.BackColor);
                ControlPaint.DrawReversibleLine(this.PointToScreen(pointUp), this.PointToScreen(pointDown), this.BackColor);

                candleLabel.Visible = false;
                priceLabel.Visible = true;
                timeLabel.Visible = true;

            }


            Cursor = Cursors.Default;

            // Show hand on label point.
            if (ShowSeriesLabels)
            {
                foreach (Rectangle rectangle in _currentLabelsRectangles)
                {
                    if (rectangle.Contains(e.Location))
                    {
                        Cursor = Cursors.Hand;
                        break;
                    }
                }
            }

            _currentDrawingSpaceMousePosition = GraphicsWrapper.ActualSpaceToDrawingSpace(e.Location, true);

            if ((_rightMouseButtonSelectionMode == SelectionModeEnum.RectangleZoom
                || _rightMouseButtonSelectionMode == SelectionModeEnum.HorizontalZoom
                || _rightMouseButtonSelectionMode == SelectionModeEnum.VerticalZoom)
                && _actualDrawingSpaceArea.Contains(e.Location)
                && _lastDrawingSpaceMouseRightButtonPosition != null)
            {// Mark current drawing point for selection purposes.
                this.Invalidate();
            }

            if (e.Button == MouseButtons.Left && _lastDrawingSpaceMouseDownLeftButton != null)
            {
                Cursor = Cursors.Hand;
                candleLabel.Visible = false;
                try
                {
                    ChartSeries series = _series[0];

                    DataProvider dp = AppContext.TradeAnalyzerControl.GetProviderBySymbol(series.Name);
                    if (dp != null)
                    {
                        dp.IsPause = true;
                        dp.LastActionTime = System.DateTime.Now;
                    }

                    float offSet = _currentDrawingSpaceMousePosition.Value.X - _lastDrawingSpaceMouseDownLeftButton.Value.X;
                    PointF dragVector = new PointF(_currentDrawingSpaceMousePosition.Value.X - _lastDrawingSpaceMouseDownLeftButton.Value.X, _currentDrawingSpaceMousePosition.Value.Y - _lastDrawingSpaceMouseDownLeftButton.Value.Y);
                    HandlePan(_limitedView && _isControlKeyDown == false, dragVector);
                    _currentDrawingSpaceMousePosition = GraphicsWrapper.ActualSpaceToDrawingSpace(e.Location, true);
                    _lastDrawingSpaceMouseDownLeftButton = _currentDrawingSpaceMousePosition;

                    _lastMouseMovePosition = e.Location;

                    Point pointLeft = new Point((int)(_actualDrawingSpaceArea.X), _lastMouseMovePosition.Value.Y);
                    Point pointRight = new Point((int)(_actualDrawingSpaceArea.X + _actualDrawingSpaceArea.Width), _lastMouseMovePosition.Value.Y);

                    PointF pointLeftX = GraphicsWrapper.ActualSpaceToDrawingSpace(pointLeft, true);
                    PointF pointRightX = GraphicsWrapper.ActualSpaceToDrawingSpace(pointRight, true);

                    float totalItemWidth = _seriesItemWidth + _seriesItemMargin;

                    int startIndex = (int)((pointLeftX.X - offSet) / totalItemWidth);
                    int endIndex = (int)((pointRightX.X - offSet) / totalItemWidth);

                    if (endIndex >= series.MaximumIndex) endIndex = series.MaximumIndex - 2;
                    if (startIndex >= endIndex) { if (endIndex - 30 > 0) startIndex = endIndex - 30; else startIndex = 0; }
                    if (startIndex < 0) startIndex = 0;
                    float yMin = float.MaxValue;
                    float yMax = float.MinValue;

                    GetSingleSeriesMinMax(series, startIndex, endIndex, ref yMin, ref yMax);

                    DateTime sdt = series.GetTimeAtIndex(startIndex);
                    DateTime edt = series.GetTimeAtIndex(endIndex);

                    Console.WriteLine(" sdt === " + sdt + "  edt ===  " + edt);
                    Console.WriteLine(" yMax === " + yMax + "  yMin ===  " + yMin);

                    RectangleF area = GraphicsWrapper.ActualSpaceToDrawingSpace(_actualDrawingSpaceArea);

                    float width = area.Width;
                    if (width > _drawingSpaceDisplayLimit.Width)
                    {
                        width = _drawingSpaceDisplayLimit.Width;
                    }

                    FitHorizontalAreaToScreen(new RectangleF((pointLeftX.X), 0, (width), (float)(yMax - yMin)));
                }
                catch (Exception E) { }

                this.Refresh();
            }
            else if (!_crosshairVisible && _actualDrawingSpaceArea.Contains(e.Location))
            {

                DrawCrossHairLocationInfo(e.Location);

                PointF drawingPoint = GraphicsWrapper.ActualSpaceToDrawingSpace(e.Location, true);
                //if (drawingPoint.Y > low && drawingPoint.Y < high)
                if (drawingPoint.Y > exLow && drawingPoint.Y < exHigh)
                {
                    ChartSeries series = _series[0];
                    int count = 0;
                    if (currentIndex <= series.MaximumIndex && currentIndex >= 0)
                    {

                        candleLabel.Caption = "";
                        Console.WriteLine("e.X ::: " + e.X + " e.Y:::  " + e.Y);
                        Console.WriteLine("W ::: " + _actualDrawingSpaceArea.Width + " H::: " + _actualDrawingSpaceArea.Height + " X:::  " + _actualDrawingSpaceArea.X + " Y " + _actualDrawingSpaceArea.Y);
                        TradeChartSeries ts = (TradeChartSeries)series;
                        BarData candle = ts.GetCandleAtIndex(currentIndex);
                        StringBuilder sb = new StringBuilder();
                        count = 0;
                        Symbol symbol = AppUtil.ParseSymbol(sid);
                        if (candle.SignalList != null && candle.SignalList.Count > 0)
                        {

                            sb.Append("[").Append(symbol).Append("]\n");
                            //count += 1;
                            foreach (CandleSignal cs in candle.SignalList)
                            {
                                    sb.Append(cs.GetCnSignalLabel()).Append("\n");
                                    sb.Append("入场Time: ").Append(cs.ArrowTime.ToString("M-d HH:mm")).Append("\n");
                                    sb.Append(cs.GetCnPriceLabel()).Append(cs.SignalPrice).Append("\n");
                                    count++;
                            }
                            candleLabel.Caption = sb.ToString();
                        }

                        //message += " | SAR: " + timeBasedSeries.GetSARAtIndex(index).ToString();
                        //message += " , RSI: " + timeBasedSeries.GetRSIAtIndex(index).ToString();
                        //message += " , CR: " + timeBasedSeries.GetCRAtIndex(index).ToString();      
                        if (count <= 0)
                            candleLabel.Caption += " \n Time: " + time+" \n";
                        if (!AppConst.JPYGOLD.Contains(symbol))
                        {
                            candleLabel.Caption += " \n SAR   : " + sar.ToString("0.0000; -0.0000");
                            candleLabel.Caption += " \n AR-26 : " + ar.ToString("0.00; -0.00");
                            candleLabel.Caption += "    |    AR-42 : " + ar2.ToString("0.00; -0.00");
                            candleLabel.Caption += " \n RSI-6 : " + rsi.ToString("0.00; -0.00");
                            candleLabel.Caption += "    |    RSI-14 : " + rsi3.ToString("0.00; -0.00");
                            candleLabel.Caption += " \n RSI-22: " + rsi4.ToString("0.00; -0.00");
                            candleLabel.Caption += "    |    RSI-42: " + rsi2.ToString("0.00; -0.00");
                            candleLabel.Caption += " \n CCI-14: " + cci.ToString("0.00; -0.00");
                            candleLabel.Caption += " | CCI-24: " + cci2.ToString("0.00; -0.00");
                            candleLabel.Caption += " | CCI-42: " + cci3.ToString("0.00; -0.00");
                            candleLabel.Caption += " \n WR  : " + wr1.ToString("0.00; -0.00");
                            candleLabel.Caption += "    |    WR-24  : " + wr2.ToString("0.00; -0.00");
                            candleLabel.Caption += " \n LWR-1: " + lwr1.ToString("0.00; -0.00");
                            candleLabel.Caption += " | LWR-2: " + lwr2.ToString("0.00; -0.00");
                            //candleLabel.Caption += "\n";
                            candleLabel.Caption += "\n O:  " + open.ToString("0.0000; -0.0000") + " Boll-Up: " + bup.ToString("0.0000; -0.0000");
                            candleLabel.Caption += "\n H:  " + high.ToString("0.0000; -0.0000") + " Boll-Dist: " + (bup - blow).ToString("0.0000; -0.0000");
                            candleLabel.Caption += "\n L:  " + low.ToString("0.0000; -0.0000") + " Boll-Low: " + blow.ToString("0.0000; -0.0000");
                            candleLabel.Caption += "\n C:  "  + close.ToString("0.0000; -0.0000") + " Boll-Mid: " + bmid.ToString("0.0000; -0.0000");
                        }
                        else
                        {
                            candleLabel.Caption += " \n SAR   : " + sar.ToString("0.0000; -0.0000");
                            candleLabel.Caption += " \n AR-26 : " + ar.ToString("0.00; -0.00");
                            candleLabel.Caption += "    |    AR-42 : " + ar2.ToString("0.00; -0.00");
                            candleLabel.Caption += " \n RSI-6 : " + rsi.ToString("0.00; -0.00");
                            candleLabel.Caption += "    |    RSI-14 : " + rsi3.ToString("0.00; -0.00");
                            candleLabel.Caption += " \n RSI-22: " + rsi4.ToString("0.00; -0.00");
                            candleLabel.Caption += "    |    RSI-42: " + rsi2.ToString("0.00; -0.00");
                            candleLabel.Caption += " \n CCI-14: " + cci.ToString("0.00; -0.00");
                            candleLabel.Caption += " | CCI-24: " + cci2.ToString("0.00; -0.00");
                            candleLabel.Caption += " | CCI-42: " + cci3.ToString("0.00; -0.00");
                            candleLabel.Caption += " \n WR     : " + wr1.ToString("0.00; -0.00");
                            candleLabel.Caption += "    |    WR-24     : " + wr2.ToString("0.00; -0.00");
                            candleLabel.Caption += " \n LWR-1: " + lwr1.ToString("0.00; -0.00");
                            candleLabel.Caption += " | LWR-2: " + lwr2.ToString("0.00; -0.00");
                            //candleLabel.Caption += "\n";
                            //candleLabel.Caption += " \n CR: " + cr.ToString("0.0000; -0.0000");
                            candleLabel.Caption += "\n Open:  " + open.ToString("0.00; -0.00") +" Boll-Up: " + bup.ToString("0.00; -0.00");
                            candleLabel.Caption += "\n Close:  " + close.ToString("0.00; -0.00") + " Boll-Mid: " + bmid.ToString("0.00; -0.00");
                            candleLabel.Caption += "\n Low:  " + low.ToString("0.00; -0.00") + " Boll-Low: " + blow.ToString("0.00; -0.00");
                            candleLabel.Caption += "\n High:  " + high.ToString("0.00; -0.00") + " Boll-Dist: " + (bup-blow).ToString("0.00; -0.00"); 
                        }
                        count +=4 ;
                    }
                    if (!string.IsNullOrEmpty(candleLabel.Caption))
                    {
                        Graphics g = this.CreateGraphics();
                        Pen pen = Pens.LightBlue;
                        float yts = Math.Abs(this.GraphicsWrapper.DrawingSpaceTransform.Elements[0] / this.GraphicsWrapper.DrawingSpaceTransform.Elements[3]);
                        float totalItemWidth = _seriesItemWidth + _seriesItemMargin;
                        g.DrawEllipse(pen, e.X, e.Y, _seriesItemWidth / 2f, _seriesItemWidth / 2f);
                        this.Refresh();
                        int width = 300;
                        int offset = 5;
                        int height = 60 * count;
                        //int height = 120 * count;
                        candleLabel.Size = new Size(width, height);
                        candleLabel.Visible = true;
                        if (_actualDrawingSpaceArea.Width - e.X < width && _actualDrawingSpaceArea.Height - e.Y < height)
                        {
                            candleLabel.Location = new Point(e.X - width - offset, e.Y - height-offset);
                        }
                        else if (_actualDrawingSpaceArea.Width - e.X < width)
                        {
                            candleLabel.Location = new Point(e.X - width-offset, e.Y + offset);
                        }
                        else if (_actualDrawingSpaceArea.Height - e.Y < height)
                        {
                            candleLabel.Location = new Point(e.X +offset, e.Y -height-offset);
                        }
                        else
                        {
                            candleLabel.Location = new Point(e.X + offset, e.Y + offset);
                        }
                    }
                    else
                        candleLabel.Visible = false;
                }
                else
                {
                    candleLabel.Visible = false;
                }
            }


            if (_crosshairVisible)
            {

            }

        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if (_customObjectsManager != null
                && _customObjectsManager.OnMouseLeave(e))
            {// Event handled by user objects handler.
                return;
            }

            this.Cursor = Cursors.Default;
        }

        static int c = 0;

        static int cc = 0;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            CleanTimePriceLabel();
            candleLabel.Visible = false;
            float totalItemWidth = _seriesItemWidth + _seriesItemMargin;
            float step = (totalItemWidth * 6f);
            switch (keyData)
            {
                case Keys.Left:
                    MoveStepByStep(step);
                    break;
                case Keys.Right:
                    MoveStepByStep(step * (-1f));
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void MoveStepByStep(float stepLength)
        {
            float totalItemWidth = _seriesItemWidth + _seriesItemMargin;
            ChartSeries series = _series[0];
            DataProvider dp = AppContext.TradeAnalyzerControl.GetProviderBySymbol(series.Name);
            if (dp != null)
            {
                dp.IsPause = true;
                dp.LastActionTime = System.DateTime.Now;
            }

            Point pointLeft = new Point((int)(_actualDrawingSpaceArea.X), 0);
            Point pointRight = new Point((int)(_actualDrawingSpaceArea.X + _actualDrawingSpaceArea.Width), 0);

            PointF pointLeftX = GraphicsWrapper.ActualSpaceToDrawingSpace(pointLeft, true);
            PointF pointRightX = GraphicsWrapper.ActualSpaceToDrawingSpace(pointRight, true);

            int startIndex = (int)((pointLeftX.X - stepLength) / totalItemWidth);
            int endIndex = (int)((pointRightX.X - stepLength) / totalItemWidth);

            if (endIndex >= series.MaximumIndex) endIndex = series.MaximumIndex - 30;
            if (startIndex >= endIndex) { if (endIndex - 30 > 0) startIndex = endIndex - 30; else startIndex = 0; }
            if (startIndex < 0) startIndex = 0;
            float yMin = float.MaxValue;
            float yMax = float.MinValue;

            GetSingleSeriesMinMax(series, startIndex, endIndex, ref yMin, ref yMax);

            DateTime sdt = series.GetTimeAtIndex(startIndex);
            DateTime edt = series.GetTimeAtIndex(endIndex);

            Console.WriteLine(" sdt === " + sdt + "  edt ===  " + edt);
            Console.WriteLine(" yMax === " + yMax + "  yMin ===  " + yMin);

            RectangleF area = GraphicsWrapper.ActualSpaceToDrawingSpace(_actualDrawingSpaceArea);

            float width = area.Width;
            if (width > _drawingSpaceDisplayLimit.Width)
            {
                width = _drawingSpaceDisplayLimit.Width;
            }

            FitHorizontalAreaToScreen(new RectangleF((pointLeftX.X - stepLength), 0, width, (float)(yMax - yMin)));
            this.Refresh();
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {

            base.OnKeyPress(e);

            Console.WriteLine(" OnKeyPress   _customObjectsManager != null   " + e.KeyChar + cc);
            if (_customObjectsManager != null && _customObjectsManager.OnKeyPress(e))
            {// Event handled by user objects handler.
                return;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (_customObjectsManager != null && _customObjectsManager.OnKeyDown(e))
            {// Event handled by user objects handler.
                return;
            }

            if (e.KeyCode == Keys.Escape)
            {// Deny selection.
                _lastDrawingSpaceMouseRightButtonPosition = null;
                this.Invalidate();
            }

            _isControlKeyDown = (e.Modifiers & Keys.Control) != 0;
            _isShiftKeyDown = (e.Modifiers & Keys.Shift) != 0;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            Console.WriteLine("  _customObjectsManager != null   " + _customObjectsManager != null);
            Console.WriteLine(" OnKeyUp   _customObjectsManager != null   " + e.KeyCode + "  " + e.KeyValue + "  " + e.KeyData);
            if (_customObjectsManager != null && _customObjectsManager.OnKeyUp(e))
            {// Event handled by user objects handler.
                return;
            }

            // We do not use the Alt keys, since it is for menues in windows, and causes weird focus losing.
            _isControlKeyDown = (e.Modifiers & Keys.Control) != 0;
            _isShiftKeyDown = (e.Modifiers & Keys.Shift) != 0;

        }

        /// <summary>
        /// 
        /// </summary>
        public void FitDrawingSpaceToScreen(bool horizontal, bool vertical)
        {
            //UpdateDrawingSpace();
            UpdateActualDrawingSpaceArea();
            FitDrawingSpaceToScreen(horizontal, vertical, DrawingSpaceDisplayLimit);
        }

        /// <summary>
        /// If fittingSpace is null, DrawingSpaceDisplayLimit is used (zoom all).
        /// </summary>
        public void FitDrawingSpaceToScreen(bool horizontal, bool vertical, RectangleF fittingSpaceRectangle)
        {
            Matrix initialTransform = (Matrix)_graphicsWrapper.DrawingSpaceTransform.Clone();
            if (fittingSpaceRectangle.Width == 0 || fittingSpaceRectangle.Height == 0)
            {
                return;
            }

            if (float.IsInfinity(fittingSpaceRectangle.Width) || float.IsNaN(fittingSpaceRectangle.Width)
                || float.IsInfinity(fittingSpaceRectangle.Height) || float.IsNaN(fittingSpaceRectangle.Height)
                || fittingSpaceRectangle.Height == 0
                || fittingSpaceRectangle.Width == 0)
            {
                MessageBox.Show("Chart pane error, invalid parameters input. Operation will not continue.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (vertical)
            {
                // Those are reversed, since they reverse their positions entering real space (scaled -1 on Y).
                PointF drawingSpaceBottomPoint = GraphicsWrapper.DrawingSpaceToActualSpace(new PointF(0, fittingSpaceRectangle.Height + fittingSpaceRectangle.Y), true);
                PointF drawingSpaceTopPoint = GraphicsWrapper.DrawingSpaceToActualSpace(new PointF(0, fittingSpaceRectangle.Y), true);

                PointF areaTopPoint = new PointF(0, ActualDrawingSpaceArea.Height + ActualDrawingSpaceArea.Y);
                PointF areaBottomPoint = new PointF(0, ActualDrawingSpaceArea.Y);

                float areaDifference = areaTopPoint.Y - areaBottomPoint.Y;
                float spaceDifference = drawingSpaceTopPoint.Y - drawingSpaceBottomPoint.Y;

                // Scale to size.
                //_drawingSpaceTransform.Scale(1, areaDifference / spaceDifference);
                _graphicsWrapper.ScaleDrawingSpaceTransform(1, areaDifference / spaceDifference);

                // Move to place.
                PointF translatedTopPoint = GraphicsWrapper.ActualSpaceToDrawingSpace(areaTopPoint, true);
                //_drawingSpaceTransform.Translate(0, translatedTopPoint.Y - fittingSpaceRectangle.Y);
                _graphicsWrapper.TranslateDrawingSpaceTransfrom(0, translatedTopPoint.Y - fittingSpaceRectangle.Y);
            }

            if (horizontal)
            {
                PointF drawingSpaceLeftPoint = GraphicsWrapper.DrawingSpaceToActualSpace(new PointF(fittingSpaceRectangle.X, 0), true);
                PointF drawingSpaceRightPoint = GraphicsWrapper.DrawingSpaceToActualSpace(new PointF(fittingSpaceRectangle.X + fittingSpaceRectangle.Width, 0), true);

                PointF areaVerticalLeftPoint = new PointF(ActualDrawingSpaceArea.X, 0);
                PointF areaVerticalRightPoint = new PointF(ActualDrawingSpaceArea.X + ActualDrawingSpaceArea.Width, 0);

                float areaDifference = areaVerticalRightPoint.X - areaVerticalLeftPoint.X;
                float spaceDifference = drawingSpaceRightPoint.X - drawingSpaceLeftPoint.X;

                //TracerHelper.Trace(ActualDrawingSpaceArea.X.ToString() + ", " + ActualDrawingSpaceArea.Width.ToString());

                // Scale to size.
                //_drawingSpaceTransform.Scale(areaDifference / spaceDifference, 1);
                _graphicsWrapper.ScaleDrawingSpaceTransform(areaDifference / spaceDifference, 1);

                if (MaximumZoomEnabled && _graphicsWrapper.DrawingSpaceTransform.Elements[0] > _maximumXZoom)
                {// Limit x scaling to 1.
                    //_drawingSpaceTransform.Scale(_maximumXZoom / _graphicsWrapper.DrawingSpaceTransform.Elements[0], 1);
                    _graphicsWrapper.ScaleDrawingSpaceTransform(_maximumXZoom / _graphicsWrapper.DrawingSpaceTransform.Elements[0], 1);
                }

                // Move to place.
                PointF areaVerticalLeftPointTranslated = GraphicsWrapper.ActualSpaceToDrawingSpace(areaVerticalLeftPoint, true);
                //_drawingSpaceTransform.Translate(areaVerticalLeftPointTranslated.X - fittingSpaceRectangle.X, 0);
                _graphicsWrapper.TranslateDrawingSpaceTransfrom(areaVerticalLeftPointTranslated.X - fittingSpaceRectangle.X, 0);
            }

            if (_graphicsWrapper.DrawingSpaceTransform != initialTransform && this.DrawingSpaceViewTransformationChangedEvent != null)
            {
                DrawingSpaceViewTransformationChangedEvent(this, initialTransform, _graphicsWrapper.DrawingSpaceTransform);
            }
        }

        protected void FitHorizontalAreaToScreen(RectangleF areaSelectionRectangle)
        {
            if (MaximumZoomEnabled && areaSelectionRectangle.Width < _actualDrawingSpaceArea.Width / _maximumXZoom)
            {// Limit to maximum zoom to calculate properly mins and maxes.
                areaSelectionRectangle.Width = _actualDrawingSpaceArea.Width / _maximumXZoom;
            }

            // Selection must be above the minimum.
            areaSelectionRectangle.X = Math.Max(areaSelectionRectangle.X, _drawingSpace.X);
            if (areaSelectionRectangle.X + areaSelectionRectangle.Width > _drawingSpace.X + _drawingSpace.Width)
            {
                areaSelectionRectangle.Width = _drawingSpace.X + _drawingSpace.Width - areaSelectionRectangle.X;
            }

            float xStart = areaSelectionRectangle.X;
            float xEnd = areaSelectionRectangle.X + areaSelectionRectangle.Width;

            xStart = xStart / (_seriesItemWidth + _seriesItemMargin);
            xEnd = xEnd / (_seriesItemWidth + _seriesItemMargin);

            float yMin = float.MaxValue;
            float yMax = float.MinValue;
            foreach (ChartSeries series in _series)
            {
                if (series.Visible)
                {
                    GetSingleSeriesMinMax(series, (int)xStart, (int)xEnd, ref yMin, ref yMax);
                    //yMax = Math.Max(yMax, series.GetTotalMaximum((int)xStart, (int)xEnd));
                    //yMin = Math.Min(yMin, series.GetTotalMinimum((int)xStart, (int)xEnd));
                }
            }

            foreach (DynamicCustomObject dynamicObject in _customObjectsManager.DynamicCustomObjects)
            {
                RectangleF containingRectange = dynamicObject.GetContainingRectangle(_drawingSpace);
                if (dynamicObject.Visible && containingRectange.IntersectsWith(areaSelectionRectangle))
                {
                    yMax = Math.Max(yMax, containingRectange.Y + containingRectange.Height);
                    yMin = Math.Min(yMin, containingRectange.Y);
                }
            }
            if (yMin == float.MaxValue || yMax == float.MinValue)
            {
                return;
            }
            // Harry--Modified
            //yMin -= yMin * (0.00001f);
            //yMax += yMax * (0.00001f);

            areaSelectionRectangle.Y = yMin;
            areaSelectionRectangle.Height = yMax - yMin;

            // Zoom to selection.
            FitDrawingSpaceToScreen(true, true, areaSelectionRectangle);
        }

        /// <summary>
        /// The input selection rectangle must be in drawing space.
        /// </summary>
        protected void HandleSelect(RectangleF selectionRectangle)
        {
            if (float.IsInfinity(selectionRectangle.Width) || float.IsNaN(selectionRectangle.Width)
                || float.IsInfinity(selectionRectangle.Height) || float.IsNaN(selectionRectangle.Height)
                || selectionRectangle.Height == 0
                || selectionRectangle.Width == 0)
            {
                if (RightMouseButtonSelectionMode == SelectionModeEnum.HorizontalZoom
                    || RightMouseButtonSelectionMode == SelectionModeEnum.VerticalZoom
                    || RightMouseButtonSelectionMode == SelectionModeEnum.RectangleZoom)
                {
                    MessageBox.Show("Chart pane error, invalid parameters input. Operation will not continue.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }

            if (_rightMouseButtonSelectionMode == SelectionModeEnum.HorizontalZoom)
            {// Establish the range of the data in this rectangle, and zoom on it only.
                FitHorizontalAreaToScreen(selectionRectangle);
            }
            else
            {
                // Zoom to selection.
                FitDrawingSpaceToScreen(true, true, selectionRectangle);
            }

            this.Invalidate();
        }

        public void ZoomIn(float xScaleIncrease)
        {
            PointF pointActualSpace = new PointF(ActualDrawingSpaceArea.Left, ActualDrawingSpaceArea.Top);
            PointF drawSpacePoint = GraphicsWrapper.ActualSpaceToDrawingSpace(pointActualSpace, true);
            HandleScale(drawSpacePoint, xScaleIncrease, 1);
            Invalidate();
        }

        public void ZoomOut(float xScaleReduction)
        {
            PointF pointActualSpace = new PointF(ActualDrawingSpaceArea.Left, ActualDrawingSpaceArea.Top);
            PointF drawSpacePoint = GraphicsWrapper.ActualSpaceToDrawingSpace(pointActualSpace, true);
            HandleScale(drawSpacePoint, xScaleReduction, 1);
            Invalidate();
        }

        public virtual void HandleScale(PointF scalingCenter, float xScale, float yScale)
        {
            Matrix initialTransform = (Matrix)_graphicsWrapper.DrawingSpaceTransform.Clone();

            _graphicsWrapper.TranslateDrawingSpaceTransfrom(scalingCenter.X, scalingCenter.Y, MatrixOrder.Prepend);
            _graphicsWrapper.ScaleDrawingSpaceTransform(xScale, yScale, MatrixOrder.Prepend);

            if (MaximumZoomEnabled && _graphicsWrapper.DrawingSpaceTransform.Elements[0] > _maximumXZoom)
            {// Limit x scaling to 1.
                //_drawingSpaceTransform.Scale(_maximumXZoom / _drawingSpaceTransform.Elements[0], 1);
                _graphicsWrapper.ScaleDrawingSpaceTransform(_maximumXZoom / _graphicsWrapper.DrawingSpaceTransform.Elements[0], 1);
            }

            _graphicsWrapper.TranslateDrawingSpaceTransfrom(-scalingCenter.X, -scalingCenter.Y, MatrixOrder.Prepend);

            if (ScrollModeEnum.HorizontalScrollAndFit == _scrollMode)
            {// After scaling also fit the data if we are in fit mode.
                RectangleF area = GraphicsWrapper.ActualSpaceToDrawingSpace(_actualDrawingSpaceArea);
                FitHorizontalAreaToScreen(area);
            }

            if (DrawingSpaceViewTransformationChangedEvent != null)
            {
                DrawingSpaceViewTransformationChangedEvent(this, initialTransform, _graphicsWrapper.DrawingSpaceTransform);
            }
        }

        public virtual void HandlePan(bool applySpaceLimit, PointF dragVector)
        {
            Matrix initialTransform = (Matrix)_graphicsWrapper.DrawingSpaceTransform.Clone();

            //_drawingSpaceTransform.Translate(dragVector.X, dragVector.Y);
            _graphicsWrapper.TranslateDrawingSpaceTransfrom(dragVector.X, dragVector.Y);

            RectangleF screen = GraphicsWrapper.ActualSpaceToDrawingSpace(_actualDrawingSpaceArea);

            if (applySpaceLimit)
            {

                // Owner allowed drawing limits.
                if (screen.X < _drawingSpaceDisplayLimit.X)
                {// X left
                    //_drawingSpaceTransform.Translate(-_drawingSpaceDisplayLimit.X + screen.X, 0);
                    _graphicsWrapper.TranslateDrawingSpaceTransfrom(-_drawingSpaceDisplayLimit.X + screen.X, 0);
                }
                else if (screen.X + screen.Width > _drawingSpaceDisplayLimit.X + _drawingSpaceDisplayLimit.Width)
                {// X right.
                    if (screen.Width < _drawingSpaceDisplayLimit.Width)
                    {
                        //_drawingSpaceTransform.Translate(screen.X + screen.Width - _drawingSpaceDisplayLimit.X - _drawingSpaceDisplayLimit.Width, 0);
                        _graphicsWrapper.TranslateDrawingSpaceTransfrom(screen.X + screen.Width - _drawingSpaceDisplayLimit.X - _drawingSpaceDisplayLimit.Width, 0);
                    }
                    else if (dragVector.X < 0)
                    {// In this special case, negate the already made translation.
                        //_drawingSpaceTransform.Translate(-dragVector.X, 0);
                        _graphicsWrapper.TranslateDrawingSpaceTransfrom(-dragVector.X, 0);
                    }
                }

                if (screen.Y + screen.Height > _drawingSpaceDisplayLimit.Y + _drawingSpaceDisplayLimit.Height)
                {// Y top.
                    //_drawingSpaceTransform.Translate(0, -_drawingSpaceDisplayLimit.Y - _drawingSpaceDisplayLimit.Height + screen.Y + screen.Height);
                    _graphicsWrapper.TranslateDrawingSpaceTransfrom(0, -_drawingSpaceDisplayLimit.Y - _drawingSpaceDisplayLimit.Height + screen.Y + screen.Height);
                }
                else if (screen.Y < _drawingSpaceDisplayLimit.Y)
                {// Y bottom.
                    if (screen.Height < _drawingSpaceDisplayLimit.Height)
                    {
                        //_drawingSpaceTransform.Translate(0, screen.Y - _drawingSpaceDisplayLimit.Y);
                        _graphicsWrapper.TranslateDrawingSpaceTransfrom(0, screen.Y - _drawingSpaceDisplayLimit.Y);
                    }
                    else if (dragVector.Y > 0)
                    {// In this special case, negate the already made translation.
                        //_drawingSpaceTransform.Translate(0, -dragVector.Y);
                        _graphicsWrapper.TranslateDrawingSpaceTransfrom(0, -dragVector.Y);
                    }
                }
            }

            if (DrawingSpaceViewTransformationChangedEvent != null)
            {
                DrawingSpaceViewTransformationChangedEvent(this, initialTransform, _graphicsWrapper.DrawingSpaceTransform);
            }
        }

        /// <summary>
        /// Set one of the predefined appearance schemes.
        /// </summary>
        public void SetAppearanceScheme(AppearanceSchemeEnum scheme)
        {
            if (scheme == AppearanceSchemeEnum.Custom)
            {// Changes nothing
                return;
            }

            _appearanceScheme = scheme;

            _seriesItemMargin = 2;
            _seriesItemWidth = 6;

            _actualDrawingSpaceAreaBorderPen = Pens.Gray;
            _actualSpaceGrid.Visible = false;
            _axisLabelsFont = new Font("Tahoma", 8);
            _considerAxisLabelsSpacingScale = true;

            _drawingSpaceGrid.Visible = true;

            _labelsFont = new Font("Tahoma", 8);

            _labelsMargin = 10;
            _labelsTopMargin = 5;

            _showClippingRectangle = false;

            _titleFont = new Font("Tahoma", 10);

            Point gradientBrushPoint1 = new Point();
            Point gradientBrushPoint2 = new Point(0, 1180);

            switch (scheme)
            {

                case AppearanceSchemeEnum.Trade:
                    {
                        _actualDrawingSpaceAreaFill = Brushes.Black;

                        _xAxisLabelsFontBrush = Brushes.DarkGray;
                        _yAxisLabelsFontBrush = _xAxisLabelsFontBrush;
                        _drawingSpaceGrid.Pen = Pens.DimGray;
                        _fill = Brushes.Black;

                        _labelsFill = Brushes.DarkGray;
                        _labelsFontBrush = Brushes.Black;
                        _selectionFill = new SolidBrush(Color.FromArgb(70, 15, 15, 15));
                        _selectionPen = Pens.Gray;

                        _titleFontBrush = Brushes.DarkGray;
                    }
                    break;

                case AppearanceSchemeEnum.TradeWhite:
                    {
                        _actualDrawingSpaceAreaFill = Brushes.WhiteSmoke;

                        _xAxisLabelsFontBrush = Brushes.Gray;
                        _yAxisLabelsFontBrush = Brushes.Gray;
                        _drawingSpaceGrid.Pen = Pens.DimGray;
                        _fill = Brushes.WhiteSmoke;

                        _labelsFill = null;
                        _labelsFontBrush = Brushes.Black;

                        _selectionFill = new SolidBrush(Color.FromArgb(70, 15, 15, 15));
                        _selectionPen = Pens.Gray;

                        _titleFontBrush = Brushes.DarkGray;
                    }
                    break;

                case AppearanceSchemeEnum.Fast:
                    {
                        _actualDrawingSpaceAreaFill = Brushes.Black;

                        _xAxisLabelsFontBrush = Brushes.WhiteSmoke;
                        _yAxisLabelsFontBrush = _xAxisLabelsFontBrush;

                        _drawingSpaceGrid.Pen = Pens.DimGray;
                        _fill = Brushes.Black;

                        _labelsFill = Brushes.Gainsboro;
                        _labelsFontBrush = Brushes.Black;
                        _selectionFill = null;
                        _selectionPen = Pens.Gray;

                        _titleFontBrush = Brushes.White;
                    }
                    break;

                case AppearanceSchemeEnum.Default:
                    {
                        _actualDrawingSpaceAreaFill = Brushes.Black;

                        _xAxisLabelsFontBrush = Brushes.WhiteSmoke;
                        _yAxisLabelsFontBrush = _xAxisLabelsFontBrush;

                        _drawingSpaceGrid.Pen = Pens.DimGray;
                        _fill = Brushes.Black;

                        _labelsFill = Brushes.Gainsboro;
                        _labelsFontBrush = Brushes.Black;
                        _selectionFill = new SolidBrush(Color.FromArgb(70, 15, 15, 15));
                        _selectionPen = Pens.Gray;

                        _titleFontBrush = Brushes.White;
                    }
                    break;

                case AppearanceSchemeEnum.Dark:
                    {
                        _actualDrawingSpaceAreaFill = new LinearGradientBrush(gradientBrushPoint1, gradientBrushPoint2, Color.FromArgb(52, 52, 64), Color.FromArgb(84, 77, 84));

                        _xAxisLabelsFontBrush = Brushes.Gainsboro;
                        _yAxisLabelsFontBrush = _xAxisLabelsFontBrush;

                        _drawingSpaceGrid.Pen = Pens.DimGray;
                        _fill = new SolidBrush(Color.FromArgb(53, 39, 54));

                        _labelsFill = new LinearGradientBrush(new Point(0, 0), new Point(0, 30), Color.Gainsboro, Color.FromArgb(53, 39, 54));
                        _labelsFontBrush = Brushes.Black;
                        _selectionFill = new SolidBrush(Color.FromArgb(70, 15, 15, 15));
                        _selectionPen = Pens.Gray;

                        _titleFontBrush = Brushes.Gainsboro;
                    }
                    break;

                case AppearanceSchemeEnum.Light:
                    {
                        _actualDrawingSpaceAreaFill = new LinearGradientBrush(gradientBrushPoint1, gradientBrushPoint2, Color.FromArgb(255, 246, 254), Color.FromArgb(166, 177, 147));

                        _xAxisLabelsFontBrush = Brushes.WhiteSmoke;
                        _yAxisLabelsFontBrush = _xAxisLabelsFontBrush;

                        _drawingSpaceGrid.Pen = Pens.DimGray;
                        _fill = new SolidBrush(Color.FromArgb(122, 125, 112));

                        _labelsFill = new LinearGradientBrush(new Point(0, 0), new Point(0, 30), Color.Gainsboro, Color.FromArgb(122, 125, 112));
                        _labelsFontBrush = Brushes.Black;
                        _selectionFill = new SolidBrush(Color.FromArgb(70, 15, 15, 15));
                        _selectionPen = Pens.Gray;

                        _titleFontBrush = Brushes.Black;
                    }
                    break;

                case AppearanceSchemeEnum.DarkNatural:
                    {  // Color.FromArgb(93, 88, 70)
                        _actualDrawingSpaceAreaFill = new LinearGradientBrush(gradientBrushPoint1, gradientBrushPoint2, Color.FromArgb(5, 41, 46), Color.FromArgb(5, 41, 46));

                        _xAxisLabelsFontBrush = Brushes.LightGray;
                        _yAxisLabelsFontBrush = _xAxisLabelsFontBrush;

                        _drawingSpaceGrid.Pen = Pens.DimGray;
                        _fill = new LinearGradientBrush(gradientBrushPoint1, gradientBrushPoint2, Color.FromArgb(5, 41, 46), Color.FromArgb(5, 41, 46));

                        _labelsFill = null;
                        _labelsFontBrush = Brushes.LightGray;

                        _selectionFill = new SolidBrush(Color.FromArgb(70, 15, 15, 15));
                        _selectionPen = Pens.Gray;

                        _titleFontBrush = Brushes.LightGray;
                    }
                    break;

                case AppearanceSchemeEnum.LightNatural:
                case AppearanceSchemeEnum.LightNaturalFlat:
                    {

                        if (scheme == AppearanceSchemeEnum.LightNatural)
                        {
                            _actualDrawingSpaceAreaFill = new LinearGradientBrush(gradientBrushPoint1, gradientBrushPoint2, Color.FromArgb(223, 203, 164), Color.FromArgb(173, 165, 130));
                        }
                        else
                        {
                            _actualDrawingSpaceAreaFill = new SolidBrush(Color.FromArgb(223, 203, 164));
                        }

                        _xAxisLabelsFontBrush = Brushes.Black;
                        _yAxisLabelsFontBrush = _xAxisLabelsFontBrush;

                        _drawingSpaceGrid.Pen = Pens.DimGray;
                        _fill = _actualDrawingSpaceAreaFill;

                        _labelsFill = null;
                        _labelsFontBrush = Brushes.Black;

                        _selectionFill = new SolidBrush(Color.FromArgb(70, 15, 15, 15));
                        _selectionPen = Pens.Gray;

                        _titleFontBrush = Brushes.Black;
                    }
                    break;
                case AppearanceSchemeEnum.Alfonsina:
                    {
                        _actualDrawingSpaceAreaFill = new LinearGradientBrush(gradientBrushPoint1, gradientBrushPoint2, Color.FromArgb(214, 201, 141), Color.FromArgb(171, 161, 118));

                        _xAxisLabelsFontBrush = Brushes.WhiteSmoke;
                        _yAxisLabelsFontBrush = _xAxisLabelsFontBrush;

                        _drawingSpaceGrid.Pen = Pens.DimGray;
                        _fill = new LinearGradientBrush(new Point(), new Point(0, 2000), Color.FromArgb(94, 90, 66), Color.FromArgb(5, 35, 40));

                        _labelsFill = null;
                        _labelsFontBrush = Brushes.WhiteSmoke;

                        _selectionFill = new SolidBrush(Color.FromArgb(70, 15, 15, 15));
                        _selectionPen = Pens.Gray;

                        _titleFontBrush = Brushes.Black;
                    }
                    break;
                case AppearanceSchemeEnum.Ground:
                    {
                        _actualDrawingSpaceAreaFill = new LinearGradientBrush(gradientBrushPoint1, gradientBrushPoint2, Color.FromArgb(173, 144, 110), Color.FromArgb(151, 120, 95));

                        _xAxisLabelsFontBrush = Brushes.WhiteSmoke;
                        _yAxisLabelsFontBrush = _xAxisLabelsFontBrush;

                        _drawingSpaceGrid.Pen = Pens.DimGray;
                        _fill = new SolidBrush(Color.FromArgb(118, 85, 73));

                        _labelsFill = null;
                        _labelsFontBrush = Brushes.WhiteSmoke;

                        _selectionFill = new SolidBrush(Color.FromArgb(70, 15, 15, 15));
                        _selectionPen = Pens.Gray;

                        _titleFontBrush = Brushes.WhiteSmoke;
                    }
                    break;

                default:
                    break;
            }

            GeneralHelper.SafeEventRaise(AppearanceSchemeChangedEvent, this, scheme);
            GeneralHelper.SafeEventRaise(ParametersUpdatedEvent, this);
        }


    }
 }

