using System.Collections;
using System.Collections.Generic;
using Ubiq.Messaging;
using Ubiq.Rooms;
using UMA.PoseTools;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;
using Ubiq.Spawning;

public class PlayerDisDetect : MonoBehaviour
{

    // Start is called before the first frame update
    public Transform PushPoint;
    public Transform InsideChestPoint;
    public Transform PlayerLeftHand;
    public Transform PlayerRightHand;
    public Transform RightChestTransform;
    public Transform LeftChestTransform;
    public Transform HeadPoint;
    public Transform JawPoint;
    public UMAExpressionPlayer expressionPlayer;
    private Vector3 originalRightChestPosition;  // 右胸部原始位置
    private Vector3 originalLeftChestPosition;  // 坐胸部原始位置
    //private Vector3 insideChestPosition; // 中间胸内点位置
    private InputDevice leftHandDevice;
    private InputDevice rightHandDevice;
    public float openThreshold = 0.2f; // 开口的距离阈值
    public float farThreshold = 0.25f; // 闭口的距离阈值
    bool isMouthOpen = false; // 嘴巴当前是否张开
    bool handWasFar = true;   // 手是否曾经远离嘴巴
    bool isCompressing = false; // determine if you are doing chest compression, false 
    int press_time = 0;
    NetworkContext context;
    //Vector3 LeftControllerVelocity;
    //Vector3 RightControllerVelocity;
    void Start()
    {
        context = NetworkScene.Register(this);
        originalRightChestPosition = RightChestTransform.localPosition;
        originalLeftChestPosition = LeftChestTransform.localPosition;
        leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

    }
    private struct Message
    {
        public Vector3 RightChestTransform;
        public Vector3 LeftChestTransform;
        public bool MouthOpen;
        //public bool isCompressing;
    }
    // Update is called once per frame
    void Update()
    {
        expressionPlayer.overrideMecanimJaw = true;
        if (!leftHandDevice.isValid || !rightHandDevice.isValid)
        {
            leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        }
        //leftHandDevice.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position);
        if (leftHandDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 leftVelocity))
        {
            //Debug.Log("Left Hand Velocity: " + leftVelocity);
        }
        if (rightHandDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 rightVelocity))
        {
            //Debug.Log("Left Hand Velocity: " + rightVelocity);
        }
        // Chest compressions action
        if (Vector3.Distance(PushPoint.position, PlayerLeftHand.position) < 0.1f || Vector3.Distance(PushPoint.position, PlayerRightHand.position) < 0.1f)
        { // 


            //if(RightChestTransform.position.y < )
            // Set a minimum height for the Y position of the chest points   
            //Debug.Log(LeftChestTransform.position.y); //0.19
            isCompressing = true;
            if (RightChestTransform.position.y > 0.007 || LeftChestTransform.position.y > 0.01)
            {
                Debug.Log("Hit chest");
                RightChestTransform.position -= -Vector3.down * 2 / 2 * 0.01f; //(leftVelocity.y + rightVelocity.y)
                LeftChestTransform.position -= -Vector3.down * 2 / 2 * 0.01f;  //(leftVelocity.y + rightVelocity.y)
                Message m = new Message();
                m.RightChestTransform = RightChestTransform.position;
                m.LeftChestTransform = LeftChestTransform.position;
                //m.isCompressing = false; 
                context.SendJson(m);
                //Debug.Log(position); 
            }
            else
            {
                press_time += 1;
                Debug.Log("Hit chest" + press_time);
            }
        }
        else if (isCompressing)
        {
            // 将胸部移回原始位置
            RightChestTransform.localPosition = originalRightChestPosition;
            LeftChestTransform.localPosition = originalLeftChestPosition;
            isCompressing = false;
            Message m = new Message();
            m.RightChestTransform = RightChestTransform.position;
            m.LeftChestTransform = LeftChestTransform.position;
        }

        JawUpdate();
    }
    void JawUpdate()
    {
        // Open jaw action

        float RightToJaw = Vector3.Distance(JawPoint.position, PlayerRightHand.position);
        float LeftToJaw = Vector3.Distance(JawPoint.position, PlayerLeftHand.position);

        float LeftToHand = Vector3.Distance(HeadPoint.position, PlayerLeftHand.position);
        float RightToHand = Vector3.Distance(HeadPoint.position, PlayerRightHand.position);
        if (((RightToJaw < openThreshold && LeftToHand < openThreshold) || (LeftToJaw < openThreshold && RightToHand < openThreshold)) && handWasFar)
        {
            // 切换嘴巴的状态
            isMouthOpen = !isMouthOpen;
            // 更新手的状态
            handWasFar = false;

            // 根据嘴巴的状态执行开或闭
            if (isMouthOpen)
            {
                Debug.Log("Open Jaw");
                expressionPlayer.jawOpen_Close = 1.0f;
                isMouthOpen = true;
                Message m = new Message();
                m.MouthOpen = true;
                context.SendJson(m);
            }
            else
            {
                Debug.Log("Close Jaw");
                expressionPlayer.jawOpen_Close = -1.0f;
                isMouthOpen = false;
                Message m = new Message();
                m.MouthOpen = false;
                context.SendJson(m);
            }
        }
        else if (RightToJaw > farThreshold || LeftToHand > farThreshold || LeftToJaw > farThreshold || RightToHand > farThreshold)
        {
            // 更新手的状态，表示手已远离
            handWasFar = true;
        }
    }
    public void ProcessMessage(ReferenceCountedSceneGraphMessage m)
    {
        // Parse the message
        var message = m.FromJson<Message>();
        //isCompressing = message.isLearner;
        RightChestTransform.position = message.RightChestTransform;
        LeftChestTransform.position = message.LeftChestTransform;
        Debug.Log("RightChestTransform" + message.RightChestTransform);
        if (message.MouthOpen == true)
        {
            expressionPlayer.jawOpen_Close = 1.0f;
            Debug.Log("Receive mouth open");
        }
        else if (message.MouthOpen == false)
        {
            expressionPlayer.jawOpen_Close = -1.0f;
            Debug.Log("Receive mouth close");
        }

        //Debug.Log(gameObject.name + "updated");
    }
}


