using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using Ubiq.Messaging;
using Ubiq.Rooms.Messages;
using Unity.VisualScripting;


public class DeleteScreen_heart : MonoBehaviour
{
    NetworkContext context;
    public XRSimpleInteractable StartKeybutton;
    
    // public XRSimpleInteractable EndKeybutton;

    public struct Message{
        public string gameobjectTagname;
    }

    void Start(){
        context = NetworkScene.Register(this);
    }


    private void OnEnable()
    {
        StartKeybutton.onSelectEntered.AddListener(OnStartKeySelected);
        // EndKeybutton.onSelectEntered.AddListener(OnEndKeySelected);
    }

    private void OnDisable()
    {
        StartKeybutton.onSelectEntered.RemoveListener(OnStartKeySelected);
        // EndKeybutton.onSelectEntered.RemoveListener(OnEndKeySelected);
    }

    private void OnStartKeySelected(XRBaseInteractor interactor)
    {
        Debug.Log("点击了开机键" + gameObject);
        Message m = new Message();
        m.gameobjectTagname = "screen3";
        context.SendJson(m);
        Destroy(gameObject);
    }


    public void ProcessMessage(ReferenceCountedSceneGraphMessage m){
        var message = m.FromJson<Message>();
        Debug.Log("传输成功了!");
        Debug.Log(GameObject.FindWithTag(message.gameobjectTagname));
        Destroy(GameObject.FindWithTag(message.gameobjectTagname));

        // GameObject screen = GameObject.FindWithTag(message.gameobjectTagname);
        
    }
}
