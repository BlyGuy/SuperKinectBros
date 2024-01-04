using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Windows.Kinect;
using Joint = Windows.Kinect.Joint;

public class BodyMenuController : MonoBehaviour 
{
    public BodySourceManager mBodySourceManager;
    public GameObject mJointObject;
    
    private Dictionary<ulong, GameObject> mBodies = new Dictionary<ulong, GameObject>();
    private List<JointType> _joints = new List<JointType>
    {
        JointType.HandLeft,
        JointType.HandRight,
        //JointType.SpineShoulder,
    };

    private void OnDisable()
    {
        cleanBodies();
    }

    void Update () 
    {
        //Get Kinect data
        Body[] data = mBodySourceManager.GetData();
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
        List<ulong> knownIds = new List<ulong>(mBodies.Keys);
        foreach(ulong trackingId in knownIds)
        {
            if(!trackedIds.Contains(trackingId))
            {
                Destroy(mBodies[trackingId]);
                mBodies.Remove(trackingId);
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
                if(!mBodies.ContainsKey(body.TrackingId))
                    mBodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                //Always update the position of the body
                UpdateBodyObject(body, mBodies[body.TrackingId]);
                UpdateHands(body, mBodies[body.TrackingId]);
            }
        }
    }
    
    private GameObject CreateBodyObject(ulong id)
    {
        GameObject body = new GameObject("Body:" + id);
        body.transform.position = transform.position;
        body.transform.localScale = transform.localScale;
        foreach (JointType joint in _joints)
        {
            //Create the joint Object for the body
            GameObject jointObj = Instantiate(mJointObject);
            jointObj.name = joint.ToString();
            //Parent to body
            jointObj.transform.parent = body.transform;
        }
        body.GetComponentInChildren<SpriteRenderer>().flipX = true;
        return body;
    }
    
    private void UpdateBodyObject(Body body, GameObject bodyObject)
    {
        //For each joint jt of the skeletons joints
        //TODO: Make the middle the center of the screen
        Vector3 middleHeight = GetVector3FromJoint(body.Joints[JointType.SpineShoulder]);
        foreach (JointType _joint in _joints)
        {
            //Get source joint
            Joint sourceJoint = body.Joints[_joint];
            Vector3 targetPosition = GetVector3FromJoint(sourceJoint);
            targetPosition -= middleHeight;
            targetPosition.z = -0.1f;
            
            //Set the position of the joint object in the body
            Transform jointObj = bodyObject.transform.Find(_joint.ToString());
            jointObj.localPosition = targetPosition;
        }

    }
    
    private static Vector3 GetVector3FromJoint(Joint joint)
    {
        return new Vector3(joint.Position.X * 15, joint.Position.Y * 15, joint.Position.Z * 15);
    }

    private void UpdateHands(Body body, GameObject bodyObject)
    {
        Hand[] hands = bodyObject.GetComponentsInChildren<Hand>(false);
        if (body.HandLeftState == HandState.Closed)
            hands[0].handClosed = true;
        else
            hands[0].handClosed = false;

        if (body.HandRightState == HandState.Closed)
            hands[1].handClosed = true;
        else
            hands[1].handClosed = false;
    }

    public void cleanBodies()
    {
        //Delete all body objects
        List<ulong> knownIds = new List<ulong>(mBodies.Keys);
        foreach (ulong trackingId in knownIds)
        {
            Destroy(mBodies[trackingId]);
            mBodies.Remove(trackingId);
        }
    }
}
