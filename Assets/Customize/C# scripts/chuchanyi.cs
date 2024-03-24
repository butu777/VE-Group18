using System;
using System.Collections;
using System.Collections.Generic;
using Ubiq.Messaging;
using Ubiq.Rooms.Messages;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class chuchanyi : MonoBehaviour
{
    // Start is called before the first frame update
    Animator Ani_chuchan;
    public XRSimpleInteractable Chuchanbutton;
    NetworkContext context;

    public Transform BodyObject;

    bool isChuchan;

    // bool isPadCollidingWithBody = false;

    string state = "IsChuchan";
    void Start()
    {
        context = NetworkScene.Register(this);
        Ani_chuchan = GetComponent<Animator>();
        Ani_chuchan.speed = 0;
        isChuchan = false;

        // GameObject foundObject = GameObject.FindWithTag("Body");
        // if (foundObject != null) {
        //     BodyObject = foundObject.transform;
        // }

    }

    private void OnEnable(){
        Chuchanbutton.onSelectEntered.AddListener(OnChuchanbuttonSelected);
    }

    private void OnDisable(){
        Chuchanbutton.onSelectEntered.RemoveListener(OnChuchanbuttonSelected);
    }


    // void OnTriggerEnter(Collider other) {
    //     if (other.transform == BodyObject) {
    //         isPadCollidingWithBody = true;
    //     }
    // }

    private void OnChuchanbuttonSelected(XRBaseInteractor interactor){
        Debug.Log("点击了除颤按键");
        Ani_chuchan.SetBool("startChuchan", true);
        if (isChuchan ){
            Notchuchan();
            Message m = new Message();
            m.speed = 0;
            context.SendJson(m);
            
            isChuchan = false;
        }
        else if (isChuchan == false)
        {
            Chuchan();
            Message m = new Message();
            m.speed = 1;
            context.SendJson(m);
          
            isChuchan = true;
        }
        
    }

    public struct Message
    {
        public Int32 speed;
    }
    public void Chuchan(){
        if(Ani_chuchan.speed == 0){
            Ani_chuchan.speed = 1;
        }
        Ani_chuchan.SetBool(state, true);
    }

    public void Notchuchan(){
        if (Ani_chuchan.speed == 0){
            return ;
        }
        Ani_chuchan.SetBool(state, false);
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage m){
        var message = m.FromJson<Message>();
        Debug.Log("除颤仪传输成功了");
        Ani_chuchan.SetBool("startChuchan", true);
        if (message.speed == 1){
            Chuchan();
            Debug.Log(" chuchaning !!!");
        }
        else if(message.speed == 0){
            Notchuchan();
            Debug.Log(" not chuchaning !!!");
        }
    } 
}
