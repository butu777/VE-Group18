using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ubiq.Messaging;
using System;
using Ubiq.Rooms.Messages;
using UnityEngine.XR.Interaction.Toolkit;

public class OpenOrClose1 : MonoBehaviour
{
    Animator Ani_cover;

    NetworkContext context;

    bool isOpen;

    public XRSimpleInteractable Casebutton;

    string state = "OpenOrClose";
    // Start is called before the first frame update
    private void Start()
    {
        context = NetworkScene.Register(this);
        Ani_cover = GetComponent<Animator>();
        Ani_cover.speed = 0;
        isOpen = false;
    }

    public void OnEnable(){
        Casebutton.onSelectEntered.AddListener(onCasebuttonSelected);
    }

    public void OnDisable(){
        Casebutton.onSelectEntered.RemoveListener(onCasebuttonSelected);
    }

    private void onCasebuttonSelected(XRBaseInteractor interactor){
        Debug.Log("点击了箱子打开按钮" + isOpen);
        if(isOpen == true){
            Close();
            Message m = new Message();
            m.speed = 0;
            context.SendJson(m);
            isOpen = false;
        }
        else if (isOpen == false){
            Open();
            Message m = new Message();
            m.speed = 1;
            context.SendJson(m);
            isOpen = true;
        }
    }


    private struct Message
    {
        public Int32 speed;
    }

    public void Open(){
        if(Ani_cover.speed == 0){
            Ani_cover.speed = 1;
        }
        Ani_cover.SetBool(state, true);
    }

    public void Close(){
        if(Ani_cover.speed == 0){
            return ;
        }
        Ani_cover.SetBool(state, false);
    }


public void ProcessMessage(ReferenceCountedSceneGraphMessage m)
{
    // Parse the message
    var message= m.FromJson<Message>();

    // Use the message to update the Component's rotation
    
    if (message.speed == 1){
        Open();
        Debug.Log("i enter open!");
    }
    else if (message.speed == 0){
        Close();
        Debug.Log("i enter close");
    }
    // Make sure the logic in Update doesn't trigger as a result of this message
    // lastRotation = transform.localRotation;
    
    Debug.Log(gameObject.name + "updated");
}

}