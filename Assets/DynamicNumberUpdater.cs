using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DynamicNumberUpdater : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UpdateDisplay(); // 初始时更新一次显示
    }

    // Update is called once per frame
    void Update()
    {
        {
            // 每秒更新数字
            if (Time.frameCount % 60 == 0) // 这是一个简单的例子，实际应用中你可能需要一个更复杂的更新逻辑
            {
                number++; // 改变数字
                UpdateDisplay();
            }
        }
    }
    //public Text numberText; // 引用UI Text组件
    private int number = 0; // 要显示的数字


    void UpdateDisplay()
    {
        //numberText.text = number.ToString(); // 将数字转换为字符串并显示在Text组件上
    }
}