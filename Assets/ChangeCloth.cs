using System.Collections;
using System.Collections.Generic;
using Ubiq.Messaging;
using UMA.CharacterSystem;
using UMA.PoseTools;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ChangeCloth : MonoBehaviour
{
    public void ChangeClothes(DynamicCharacterAvatar avatar, string wardrobeItem)
    {
        // 移除当前所有的Wardrobe项，也可以指定移除特定部位的衣物
        avatar.ClearSlot("MaleShirt2");

        // 添加新的Wardrobe项
        // 这里的wardrobeItem是你想要角色穿上的衣服的名称，确保这个名称在你的WardrobeRecipes里有对应的项
        //avatar.SetSlot(wardrobeItem);

        // 更新UMA角色以应用更改
        avatar.BuildCharacter();
    }
}

