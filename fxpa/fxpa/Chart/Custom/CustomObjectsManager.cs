// -----
// GNU General Public License
// The Forex Professional Analyzer is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. 
// The Forex Professional Analyzer is distributed in the hope that it will be useful, but without any warranty; without even the implied warranty of merchantability or fitness for a particular purpose.  
// See the GNU Lesser General Public License for more details.
// -----

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace fxpa
{
    /// <summary>
    /// Handles creation of custom objects on the ChartPane.
    /// </summary>
    public class CustomObjectsManager
    {
        ListEx<DynamicCustomObject> _dynamicCustomObjects = new ListEx<DynamicCustomObject>();
        public ICollection<DynamicCustomObject> DynamicCustomObjects
        {
            get { return _dynamicCustomObjects; }
        }

        ListEx<CustomObject> _staticCustomObjects = new ListEx<CustomObject>();
        public ICollection<CustomObject> StaticCustomObjects
        {
            get { return _staticCustomObjects; }
        }

        ListEx<DynamicCustomObject> _selectedDynamicCustomObjects = new ListEx<DynamicCustomObject>();
        public ICollection<DynamicCustomObject> SelectedDynamicCustomObjects
        {
            get { return _selectedDynamicCustomObjects; }
        }

        ChartPane _pane;

        DynamicCustomObject _currentObjectBuilt = null;

        PointF? _dragLastDrawingSpaceMouseLocation = null;

        float _defaultAbsoluteSelectionMargin = 7;
        /// <summary>
        /// In pixels, the selection margin to any object or control point of object.
        /// </summary>
        public float DefaultAbsoluteSelectionMargin
        {
            get { return _defaultAbsoluteSelectionMargin; }
            set { _defaultAbsoluteSelectionMargin = value; }
        }

        public bool IsBuildingObject
        {
            get { return _currentObjectBuilt != null; }
        }

        public delegate void DynamicObjectBuiltDelegate(CustomObjectsManager manager, DynamicCustomObject dynamicObject);
        /// <summary>
        /// If the object is null, this means creation was canceled.
        /// </summary>
        public event DynamicObjectBuiltDelegate DynamicObjectBuiltEvent;

        /// <summary>
        /// 
        /// </summary>
        public CustomObjectsManager(ChartPane pane)
        {
            _pane = pane;
        }

        public bool BuildDynamicObject(DynamicCustomObject dynamicObject)
        {
            if (_currentObjectBuilt != null)
            {
                return false;
            }

            if (dynamicObject.Initialize(this) == false)
            {
                return false;
            }

            _currentObjectBuilt = dynamicObject;
            return true;
        }

        public void StopBuildingDynamicObject()
        {
            _currentObjectBuilt = null;
            DynamicObjectBuiltEvent(this, null);
            _pane.Invalidate();
        }

        public void HandleDynamicObjectUpdated(DynamicCustomObject dynamicObject)
        {
            _pane.Invalidate();
        }

        public bool Add(CustomObject customObject)
        {
            if (customObject.IsInitialized == false && customObject.Initialize(this) == false)
            {
                return false;
            }
            
            if (customObject is DynamicCustomObject)
            {
                _dynamicCustomObjects.Add((DynamicCustomObject)customObject);
            }
            else
            {
                _staticCustomObjects.Add(customObject);
            }

            _pane.Invalidate();
            return true;
        }

        public bool Remove(CustomObject customObject)
        {
            bool result;
            if (customObject is DynamicCustomObject)
            {
                result = _dynamicCustomObjects.Remove((DynamicCustomObject)customObject);
            }
            else
            {
                result = _staticCustomObjects.Remove(customObject);
            }

            customObject.UnInitialize();

            _pane.Invalidate();
            return result;
        }

        public void Clear()
        {
            for (int i = _dynamicCustomObjects.Count - 1; i > 0; i--)
            {
                Remove(_dynamicCustomObjects[i]);
            }
        }

        public void Draw(GraphicsWrapper g, RectangleF drawingSpaceClipping, CustomObject.DrawingOrderEnum drawingOrder)
        {
            foreach (CustomObject customObject in _dynamicCustomObjects)
            {
                if (customObject.DrawingOrder == drawingOrder)
                {
                    customObject.Draw(g, _pane.CurrentDrawingSpaceMousePosition, drawingSpaceClipping, _pane.DrawingSpace);
                }
            }

            if (_currentObjectBuilt != null)
            {
                _currentObjectBuilt.Draw(g, _pane.CurrentDrawingSpaceMousePosition, drawingSpaceClipping, _pane.DrawingSpace);
            }
        }

        void UpdateSelectedObjects()
        {
            _selectedDynamicCustomObjects.Clear();
            foreach (DynamicCustomObject dynamicObject in _dynamicCustomObjects)
            {
                if (dynamicObject.Selected)
                {
                    _selectedDynamicCustomObjects.Add(dynamicObject);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool OnMouseDoubleClick(MouseEventArgs e)
        {
            bool result = false;

            PointF drawingSpaceLocation = _pane.GraphicsWrapper.ActualSpaceToDrawingSpace(e.Location, true);

            if (_currentObjectBuilt != null)
            {
                _currentObjectBuilt.TrySelect(_pane.GraphicsWrapper.DrawingSpaceTransform, drawingSpaceLocation, _defaultAbsoluteSelectionMargin, !_pane.IsControlKeyDown);
            }
            else
            if (_currentObjectBuilt == null && _dragLastDrawingSpaceMouseLocation.HasValue == false)
            {// Only if dragging is not running try to perform typical selection.

                foreach (DynamicCustomObject dynamicObject in _dynamicCustomObjects)
                {
                    if (dynamicObject.TrySelect(_pane.GraphicsWrapper.DrawingSpaceTransform, drawingSpaceLocation, _defaultAbsoluteSelectionMargin, !_pane.IsControlKeyDown))
                    {
                        if (result == true)
                        {// Only one NEW selection per turn so deny this selection, 
                            // but keep cycling to perform needed deselections.
                            dynamicObject.Selected = false;
                        }

                        result = true;
                    }
                }

                UpdateSelectedObjects();
                _pane.Invalidate();
            }

            return result;
        }

        /// <summary>
        /// Returns true to specify the event has been handled and should not be processed any further.
        /// </summary>
        public bool OnMouseMove(MouseEventArgs e)
        {
            if (IsBuildingObject)
            {
                _pane.Focus();

                // If we refresh, the value of the incoming mouse position lags with 1 iteration.
                _pane.Invalidate();
                return false;
            }

            bool refreshRequired = false;

            PointF drawingSpaceLocation = _pane.GraphicsWrapper.ActualSpaceToDrawingSpace(e.Location, true);

            // Handle mouse hover requests.
            foreach (DynamicCustomObject dynamicObject in _dynamicCustomObjects)
            {
                if (dynamicObject.SetMouseHover(drawingSpaceLocation))
                {
                    refreshRequired = true;
                }
            }

            // Handle drag.
            if (_selectedDynamicCustomObjects.Count > 0 && _dragLastDrawingSpaceMouseLocation.HasValue)
            {
                PointF drawingSpaceDragVector = new PointF(drawingSpaceLocation.X - _dragLastDrawingSpaceMouseLocation.Value.X, drawingSpaceLocation.Y - _dragLastDrawingSpaceMouseLocation.Value.Y);
                foreach (DynamicCustomObject dynamicObject in _selectedDynamicCustomObjects)
                {
                    dynamicObject.Drag(drawingSpaceDragVector);
                }

                _dragLastDrawingSpaceMouseLocation = drawingSpaceLocation;
                refreshRequired = true;
            }


            if (refreshRequired)
            {
                _pane.Refresh();
            }

            return false;
        }

        /// <summary>
        /// Returns true to specify the event has been handled and should not be processed any further.
        /// </summary>
        public bool OnMouseLeave(EventArgs e)
        {
            return false;
        }

        /// <summary>
        /// Returns true to specify the event has been handled and should not be processed any further.
        /// </summary>
        public bool OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PointF drawingSpaceLocation = _pane.GraphicsWrapper.ActualSpaceToDrawingSpace(e.Location, true);

                if (_currentObjectBuilt != null)
                {// Building new object.
                    if (_currentObjectBuilt.AddBuildingPoint(drawingSpaceLocation))
                    {
                        Add(_currentObjectBuilt);
                        DynamicObjectBuiltEvent(this, _currentObjectBuilt);
                        _currentObjectBuilt = null;
                    }
                    return true;
                }
                else
                {// Selection.

                    bool result = false;

                    // First check if we must start dragging.
                    foreach (DynamicCustomObject dynamicObject in _selectedDynamicCustomObjects)
                    {
                        if (dynamicObject.TrySelect(_pane.GraphicsWrapper.DrawingSpaceTransform, drawingSpaceLocation, _defaultAbsoluteSelectionMargin, false))
                        {// One of the selected objects was clicked on so start dragging.
                            _dragLastDrawingSpaceMouseLocation = drawingSpaceLocation;
                            result = true;
                            break;
                        }
                    }

                    //if (_dragLastDrawingSpaceMouseLocation.HasValue == false)
                    //{// Only if dragging is not running try to perform typical selection.
                        
                    //    foreach (DynamicCustomObject dynamicObject in _dynamicCustomObjects)
                    //    {
                    //        if (dynamicObject.TrySelect(_pane.DrawingSpaceTransform, drawingSpaceLocation, _defaultAbsoluteSelectionMargin, !_pane.IsControlKeyDown))
                    //        {
                    //            if (result == true)
                    //            {// Only one NEW selection per turn so deny this selection, 
                    //                // but keep cycling to perform needed deselections.
                    //                dynamicObject.Selected = false;
                    //            }

                    //            result = true;
                    //        }
                    //    }
                    //}

                    if (result)
                    {
                        _pane.Invalidate();
                    }
                    
                    UpdateSelectedObjects();
                    return result;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true to specify the event has been handled and should not be processed any further.
        /// </summary>
        public bool OnMouseUp(MouseEventArgs e)
        {
            if (_dragLastDrawingSpaceMouseLocation.HasValue)
            {// Stop dragging.
                _dragLastDrawingSpaceMouseLocation = null;
                return true;
            }

            if (_currentObjectBuilt != null)
            {// Building object.
                if (e.Button == MouseButtons.Left)
                {
                    UpdateSelectedObjects();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true to specify the event has been handled and should not be processed any further.
        /// </summary>
        public bool OnKeyPress(KeyPressEventArgs e)
        {
            if (_currentObjectBuilt != null)
            {// Building an object - send the key to it.
                if (_currentObjectBuilt.AddBuildingKey(e))
                {// Handled by object.

                    Add(_currentObjectBuilt);
                    DynamicObjectBuiltEvent(this, _currentObjectBuilt);
                    _currentObjectBuilt = null;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true to specify the event has been handled and should not be processed any further.
        /// </summary>
        public bool OnKeyDown(KeyEventArgs e)
        {
            if (IsBuildingObject && e.KeyCode == Keys.Escape)
            {
                StopBuildingDynamicObject();
                return true;
            }

            if (_currentObjectBuilt != null)
            {// Building an object - send the key to it.
                if (_currentObjectBuilt.AddBuildingKey(e))
                {// Handled by object.

                    Add(_currentObjectBuilt);
                    DynamicObjectBuiltEvent(this, _currentObjectBuilt);
                    _currentObjectBuilt = null;

                    return true;
                }
            }

            if (e.KeyCode == Keys.Delete && _selectedDynamicCustomObjects.Count > 0)
            {// Delete.
                foreach (DynamicCustomObject dynamicObject in _selectedDynamicCustomObjects)
                {
                    _dynamicCustomObjects.Remove(dynamicObject);
                }

                _selectedDynamicCustomObjects.Clear();
                _pane.Invalidate();
                return false;
            }

            return false;
        }

        /// <summary>
        /// Returns true to specify the event has been handled and should not be processed any further.
        /// </summary>
        public bool OnKeyUp(KeyEventArgs e)
        {
            return false;
        }
    }
}
