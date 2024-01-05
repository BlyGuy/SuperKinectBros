using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

using Windows.Kinect;
using Joint = Windows.Kinect.Joint;
//using Microsoft.Kinect.VisualGestureBuilder;

using WindowsInput.Native;
using WindowsInput;
using System;

public class BodySourceView : MonoBehaviour 
{
    public Material BoneMaterial;
    public float RunningConfidence = 0.0F;
    public BodySourceManager _BodyManager;
    
    public Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    //private CameraSpacePoint[] calibrationPositions = new CameraSpacePoint[25];
    
    private Dictionary<JointType, JointType> _BoneMap = new Dictionary<JointType, JointType>()
    {
        { JointType.FootLeft, JointType.AnkleLeft },
        { JointType.AnkleLeft, JointType.KneeLeft },
        { JointType.KneeLeft, JointType.HipLeft },
        { JointType.HipLeft, JointType.SpineBase },
        
        { JointType.FootRight, JointType.AnkleRight },
        { JointType.AnkleRight, JointType.KneeRight },
        { JointType.KneeRight, JointType.HipRight },
        { JointType.HipRight, JointType.SpineBase },
        
        { JointType.HandTipLeft, JointType.HandLeft },
        { JointType.ThumbLeft, JointType.HandLeft },
        { JointType.HandLeft, JointType.WristLeft },
        { JointType.WristLeft, JointType.ElbowLeft },
        { JointType.ElbowLeft, JointType.ShoulderLeft },
        { JointType.ShoulderLeft, JointType.SpineShoulder },
        
        { JointType.HandTipRight, JointType.HandRight },
        { JointType.ThumbRight, JointType.HandRight },
        { JointType.HandRight, JointType.WristRight },
        { JointType.WristRight, JointType.ElbowRight },
        { JointType.ElbowRight, JointType.ShoulderRight },
        { JointType.ShoulderRight, JointType.SpineShoulder },
        
        { JointType.SpineBase, JointType.SpineMid },
        { JointType.SpineMid, JointType.SpineShoulder },
        { JointType.SpineShoulder, JointType.Neck },
        { JointType.Neck, JointType.Head },
    };
    
    void Update () 
    {
        //Get Kinect data
        
        Body[] data = _BodyManager.GetData();
        if (data == null)
            return;

        //Get all tracked bodyIDs
        List<ulong> trackedIds = new List<ulong>();
        foreach(var body in data)
        {
            if (body == null)
                continue;
                
            if (body.IsTracked)
                trackedIds.Add(body.TrackingId);
        }
        
        // First delete untracked bodies
        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);
        foreach(ulong trackingId in knownIds)
        {
            if(!trackedIds.Contains(trackingId))
            {
                Destroy(_Bodies[trackingId]);
                _Bodies.Remove(trackingId);
            }
        }

        //Create all the Kinect skeletons/bodies
        foreach(var body in data)
        {
            if (body == null)
                continue;
            
            if(body.IsTracked)
            {
                //If new body has been tracked, create the body
                if(!_Bodies.ContainsKey(body.TrackingId))
                    _Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                //Always update the position of the body
                UpdateBodyObject(body, _Bodies[body.TrackingId]);
                ApplyMovementControls(body, _Bodies[body.TrackingId]);
            }
        }
    }
    
    private GameObject CreateBodyObject(ulong id)
    {
        GameObject body = new GameObject("Body:" + id);
        body.transform.position = transform.position;
        body.transform.localScale = transform.localScale;
        for (JointType jt = JointType.SpineBase; jt <= JointType.ThumbRight; jt++)
        {
            //Create the joint Object for the body
            GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            jointObj.name = jt.ToString();
            jointObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            jointObj.transform.parent = body.transform;
            
            //Give the joint Object a line renderer to render bones with
            LineRenderer lr = jointObj.AddComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.material = BoneMaterial;
            lr.startWidth = 0.05f;
            lr.endWidth = 0.05f;
        }
        
        return body;
    }
    
    private void UpdateBodyObject(Body body, GameObject bodyObject)
    {
        //For each joint jt of the skeletons joints
        for (JointType jt = JointType.SpineBase; jt <= JointType.ThumbRight; jt++)
        {
            //Get source joint
            Joint sourceJoint = body.Joints[jt];

            //Get target joint
            Joint? targetJoint = null;
            if(_BoneMap.ContainsKey(jt))
                targetJoint = body.Joints[_BoneMap[jt]];
            
            //Set the position of the joint object in the body
            Transform jointObj = bodyObject.transform.Find(jt.ToString());
            jointObj.localPosition = GetVector3FromJoint(sourceJoint);

            LineRenderer lr = jointObj.GetComponent<LineRenderer>();
            if(targetJoint.HasValue)
            {
                lr.SetPosition(0, jointObj.position);
                lr.SetPosition(1, GetVector3FromJoint(targetJoint.Value) + bodyObject.transform.position);
                lr.startColor = GetColorForState(sourceJoint.TrackingState);
                lr.endColor = GetColorForState(targetJoint.Value.TrackingState);
            }
            else
            {
                lr.enabled = false;
            }
        }
    }
    
    private static Color GetColorForState(TrackingState state)
    {
        switch (state)
        {
        case TrackingState.Tracked:
            return Color.green;

        case TrackingState.Inferred:
            return Color.red;

        default:
            return Color.black;
        }
    }
    
    private static Vector3 GetVector3FromJoint(Joint joint)
    {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }

    private void ApplyMovementControls(Body body, GameObject bodyObject)
    {
        //Apply simple jump
        //var inputSimulator = new InputSimulator();
        CameraSpacePoint shoulderMiddle = body.Joints[JointType.SpineShoulder].Position;
        CameraSpacePoint shoulderLeft   = body.Joints[JointType.ShoulderLeft].Position;
        CameraSpacePoint shoulderRight  = body.Joints[JointType.ShoulderRight].Position;
        CameraSpacePoint neck           = body.Joints[JointType.Neck].Position;
        CameraSpacePoint kneeRight      = body.Joints[JointType.KneeRight].Position;
        CameraSpacePoint kneeLeft       = body.Joints[JointType.KneeLeft].Position;
        CameraSpacePoint spineMiddle    = body.Joints[JointType.SpineMid].Position;
        CameraSpacePoint hipMiddle      = body.Joints[JointType.SpineBase].Position;
        CameraSpacePoint leftHand       = body.Joints[JointType.HandLeft].Position;
        CameraSpacePoint rightHand      = body.Joints[JointType.HandRight].Position;

        float shoulderDistanceRight = shoulderRight.X - shoulderMiddle.X;
        float shoulderDistanceLeft = shoulderLeft.X - shoulderMiddle.X;
        if ((leftHand.Y > neck.Y /* && leftHand.X > fireBallDistanceLeft*/)
            ||
            (rightHand.Y > neck.Y /* && rightHand.X < fireBallDistanceRight*/))
        {
            var inputSimulator = new InputSimulator();
            inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_H);
        }
        else {
            var inputSimulator = new InputSimulator();
            inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_H);
        }

        //Apply running
        //real running gesture code
        if (RunningConfidence >= 0.97F)
        {
            var inputSimulator = new InputSimulator();
            //Determine running direction
            float ZfacingDifference = body.Joints[JointType.HipLeft].Position.Z - body.Joints[JointType.HipRight].Position.Z;
            bool isFacingLeft = ZfacingDifference > 0;
            //Debug.Log("Zdiff: " + ZfacingDifference.ToString() + " -> " + isFacingLeft.ToString());
            if (isFacingLeft)
            {
                inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_A);
                inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_D);
            }
            else
            {

                inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_D);
                inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_A);
            }
        }
        //Knee-raising alternative gesture
        else if (kneeRight.Y > kneeLeft.Y /* && footRight.Y > footLeft.Y */ && kneeRight.X > hipMiddle.X + shoulderDistanceRight * 1.5F)
        {
            var inputSimulator = new InputSimulator();
            inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_D);
            inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_A);
        } else if (kneeLeft.Y > kneeRight.Y /* && footLeft.Y > footRight.Y */ && kneeLeft.X < hipMiddle.X + shoulderDistanceLeft * 1.5F) {
            var inputSimulator = new InputSimulator();
            inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_A);
            inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_D);
        } else {
            var inputSimulator = new InputSimulator();
            inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_D);
            inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_A);
        }

        //Apply Pause/cross
        if ( leftHand.X > rightHand.X && /*rightHand.X < shoulderMiddle.X &&*/
             leftHand.Y > shoulderMiddle.Y && rightHand.Y > shoulderMiddle.Y)
        {
            var inputSimulator = new InputSimulator();
            inputSimulator.Keyboard.KeyDown(VirtualKeyCode.RETURN);
        }
        else {
            var inputSimulator = new InputSimulator();
            inputSimulator.Keyboard.KeyUp(VirtualKeyCode.RETURN);
        }

        //Apply fireball
        float fireBallDistanceRight = shoulderMiddle.X + shoulderDistanceRight * 2.5F;
        float fireBallDistanceLeft = shoulderMiddle.X + shoulderDistanceLeft * 2.5F;
        //TODO: Calculate distance from shoulder to hand
        //if that distance is mostly horizontal, then fireball
        if ((leftHand.Y < shoulderMiddle.Y && leftHand.Y > spineMiddle.Y &&
        //if ((leftHand.Y < spineMiddle.Y &&
            (leftHand.X < fireBallDistanceLeft || leftHand.X > fireBallDistanceRight) )
            ||
            (rightHand.Y < shoulderMiddle.Y && rightHand.Y > spineMiddle.Y && 
            //(rightHand.Y < spineMiddle.Y &&
            (rightHand.X < fireBallDistanceLeft || rightHand.X > fireBallDistanceRight)))
        {
            var inputSimulator = new InputSimulator();
            //Debug.Log("LHand: " + leftHand.X.ToString() + " RHand: " + rightHand.X.ToString() + " ShoulderMiddle: " + shoulderMiddle.X.ToString());
            inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_J);
        } else {
            var inputSimulator = new InputSimulator();
            inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_J);
        }
    }
}
