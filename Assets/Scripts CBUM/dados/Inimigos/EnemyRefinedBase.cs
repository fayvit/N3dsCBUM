using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRefinedBase : EnemyBase
{

    protected void ParaAwake()
    {
        EventAgregator.AddListener(EventKey.startRefinedAttack, OnStartRefinedAttack);
        EventAgregator.AddListener(EventKey.endRefinedAttack, OnEndRefinedAttack);
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    private void OnEndRefinedAttack(IGameEvent obj)
    {
        if (obj.Sender == gameObject)
        {
            _Animator.SetBool("emAtk", false);
        }
    }

    private void OnStartRefinedAttack(IGameEvent obj)
    {
        if (obj.Sender == gameObject)
        {
            StandardSendStringEvent s = obj as StandardSendStringEvent;

            _Animator.SetBool("emAtk", true);
            //PUN RPC_Listener.RPC(NameOfRPC.AnimaPlay, PhotonTargets.All, s.StringContent, MyView.viewID);
            RPC_Listener.AnimaPlay(s.StringContent, gameObject);
        }
    }

    protected override void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.startRefinedAttack, OnStartRefinedAttack);
        EventAgregator.RemoveListener(EventKey.endRefinedAttack, OnEndRefinedAttack);

        base.OnDestroy();
    }

    // Update is called once per frame [usando da clase base]
    //void Update () {

    //}
}
