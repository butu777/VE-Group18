using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarMover : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveDistance = 1.0f; // Avatar移动的距离
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // 检查碰撞的对象是否是Player的手部
        if (other.CompareTag("LeftHand")) // 确保你已经设置了正确的Tag
        {
            // 向前移动Avatar
            transform.position += transform.forward * moveDistance;
        }
    }
}
