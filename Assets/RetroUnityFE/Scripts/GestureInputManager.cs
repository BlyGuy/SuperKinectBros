using UnityEngine;
//using System.Collections;
using System.Collections.Generic;
using Windows.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;
//using WindowsInput.Native;
//using WindowsInput;
//using System;
// Adapted from DiscreteGestureBasics-WPF by Momo the Monster 2014-11-25
// For Helios Interactive - http://heliosinteractive.com

public class GestureInputManager : MonoBehaviour
{
    //private Dictionary<string, float> gestureThresholds = new Dictionary<string, float>()
    //{
    //    //{ "left", 0.9F },
    //    //{ "right", 0.5F },
    //    //{ "leg_left", 0.9F },
    //    //{ "leg_right", 0.4F },
    //    { "jump", 0.2F },
    //    { "vuurbal", 0.5F },
    //    { "x", 0.8F }
    //};
    //private Dictionary<string, VirtualKeyCode> gestureKeys = new Dictionary<string, VirtualKeyCode>()
    //{
    //    //left = right, right = left
    //    //{ "left", VirtualKeyCode.VK_D },
    //    //{ "right", VirtualKeyCode.VK_A },
    //    //{ "leg_left", VirtualKeyCode.VK_D },
    //    //{ "leg_right", VirtualKeyCode.VK_A },
    //    { "jump", VirtualKeyCode.VK_H },
    //    { "vuurbal", VirtualKeyCode.VK_J },
    //    { "x", VirtualKeyCode.RETURN }
    //};

    public struct EventArgs
    {
        public string name;
        public float confidence;

        public EventArgs(string _name, float _confidence)
        {
            name = _name;
            confidence = _confidence;
        }
    }

    public BodySourceView _BodySourceView;
    public BodySourceManager _BodySource;
    public string databasePath;
    private KinectSensor _Sensor;
    private VisualGestureBuilderFrameSource _Source;
    private VisualGestureBuilderFrameReader _Reader;
    private VisualGestureBuilderDatabase _Database;

    // Gesture Detection Events
    //public delegate void GestureAction(EventArgs e);
    //public event GestureAction OnGesture;

    // Use this for initialization
    void Start()
    {
        _Sensor = KinectSensor.GetDefault();
        if (_Sensor != null)
        {

            if (!_Sensor.IsOpen)
            {
                _Sensor.Open();
            }

            // Set up Gesture Source
            _Source = VisualGestureBuilderFrameSource.Create(_Sensor, 0);

            // open the reader for the vgb frames
            _Reader = _Source.OpenReader();
            if (_Reader != null)
            {
                _Reader.IsPaused = true;
                _Reader.FrameArrived += GestureFrameArrived;
            }

            // load the gestures from the gesture database
            string path = System.IO.Path.Combine(Application.streamingAssetsPath, databasePath);
            _Database = VisualGestureBuilderDatabase.Create(path);
            //Debug.Log("Database: " + path);

            // Load all gestures
            IList<Gesture> gesturesList = _Database.AvailableGestures;
            for (int g = 0; g < gesturesList.Count; g++)
            {
                //Debug.Log("Found gesture: " + gesturesList[g].Name);
                Gesture gesture = gesturesList[g];
                _Source.AddGesture(gesture);
            }

        }
    }

    // Public setter for Body ID to track
    public void SetBody(ulong id)
    {
        if (id > 0)
        {
            _Source.TrackingId = id;
            _Reader.IsPaused = false;
        }
        else
        {
            _Source.TrackingId = 0;
            _Reader.IsPaused = true;
        }
    }

    // Update Loop, set body if we need one
    void Update()
    {
        if (!_Source.IsTrackingIdValid)
        {
            FindValidBody();
        }
    }

    // Check Body Manager, grab first valid body
    void FindValidBody()
    {

        if (_BodySource != null)
        {
            Body[] bodies = _BodySource.GetData();
            if (bodies != null)
            {
                foreach (Body body in bodies)
                {
                    if (body.IsTracked)
                    {
                        SetBody(body.TrackingId);
                        break;
                    }
                }
            }
        }

    }

    /// Handles gesture detection results arriving from the sensor for the associated body tracking Id
    private void GestureFrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs e)
    {
        VisualGestureBuilderFrameReference frameReference = e.FrameReference;
        using (VisualGestureBuilderFrame frame = frameReference.AcquireFrame())
        {
            if (frame != null)
            {
                // get the discrete gesture results which arrived with the latest frame
                IDictionary<Gesture, DiscreteGestureResult> discreteResults = frame.DiscreteGestureResults;

                if (discreteResults != null)
                {
                    foreach (Gesture gesture in _Source.Gestures)
                    {
                        if (gesture.GestureType == GestureType.Discrete)
                        {
                            DiscreteGestureResult result = null;
                            discreteResults.TryGetValue(gesture, out result);

                            if (result != null && gesture.Name == "running")
                            {
                                _BodySourceView.RunningConfidence = result.Confidence;
                                //var inputSimulator = new InputSimulator();
                                //if (gestureKeys.ContainsKey(gesture.Name))
                                //{
                                //    if (result.Confidence > gestureThresholds[gesture.Name])
                                //        inputSimulator.Keyboard.KeyDown(gestureKeys[gesture.Name]);
                                //    else
                                //        inputSimulator.Keyboard.KeyUp(gestureKeys[gesture.Name]);
                                //}
                            }
                        }
                    }
                }
            }
        }
    }
}