using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandHapticFeedback : MonoBehaviour
{
    public Transform LeftHand;
    public Transform RightHand;
    public Transform InsideChestPoint; // 目标点
    public Transform LeftShoulder;
    public Transform RightShoulder;
    public Transform MouthPoint;
    public float triggerDistance = 0.5f; // 触发震动的距离
    private InputDevice RightController;
    private InputDevice LeftController;
    private InputDevice headDevice;
    private Vector3  headsetPosition;
    void Start()
    {
        // 获取手柄设备，这里以右手为例
        RightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        LeftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        headDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);
    }

    void Update()
    {
        // 检查设备是否有效
        if (!RightController.isValid)
        {
            RightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        }
        if (!LeftController.isValid)
        {
            LeftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        }
        // 获取手柄的位置
        if (Vector3.Distance(LeftHand.position, InsideChestPoint.position) < triggerDistance && Vector3.Distance(RightHand.position, InsideChestPoint.position) < triggerDistance)
        {
            // 触发震动
            Debug.Log("chest vibrate");
            HapticFeedback();
        }
        if (Vector3.Distance(LeftHand.position, LeftShoulder.position) < 0.2 && Vector3.Distance(RightHand.position, RightShoulder.position) < 0.2)
        {
            // 触发震动
            Debug.Log("shoulder vibrate");
            HapticFeedback();
        }


        // 检查设备是否有效
        if (headDevice.isValid)
        {
            // 获取头显的位置和旋转
            if (headDevice.TryGetFeatureValue(CommonUsages.devicePosition, out headsetPosition) && Vector3.Distance(headsetPosition, MouthPoint.position) < 0.2)
            {
                // 触发震动
                Debug.Log("breath vibrate");
                HapticFeedback();
            }
        }
    }
    void HapticFeedback()
    {
        // 触发一次简单的震动
        RightController.SendHapticImpulse(0, 1.0f, 0.1f);
        LeftController.SendHapticImpulse(0, 1.0f, 0.1f);
    }
}
