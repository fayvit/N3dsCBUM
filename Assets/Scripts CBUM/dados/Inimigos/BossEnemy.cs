using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : EnemyRefinedBase {

    private void Awake()
    {
        ParaAwake();
    }

    // Use this for initialization
    protected override void Start()
    {
        ConstruirIa(new BossEnemyIA(gameObject, carac,Dados));

        base.Start();

        AdicionaEventos();

    }

    protected override void OnDestroy()
    {
        RemoveEventos();
        base.OnDestroy();
    }

    public void AdicionaEventos()
    {
        EventAgregator.AddListener(EventKey.bossSpecialAttackStart, OnStartSpecialAttack);
        EventAgregator.AddListener(EventKey.endSpecialAttack, OnEndSpecialAttack);
        EventAgregator.AddListener(EventKey.initRoll, OnStartRoll);
        
    }

    private void OnStartRoll(IGameEvent obj)
    {
        StandardSendIntAndStringEvent s = obj as StandardSendIntAndStringEvent;
       if(s.Sender==gameObject)// if (s.MyInt == MyView.viewID && PhotonNetwork.isMasterClient)
       // {
            (Ia as BossEnemyIA).StartRoll(gameObject, s.MyString);
        //}
    }

    public void RemoveEventos()
    {
        EventAgregator.RemoveListener(EventKey.bossSpecialAttackStart, OnStartSpecialAttack);
        EventAgregator.RemoveListener(EventKey.endSpecialAttack, OnEndSpecialAttack);
        EventAgregator.RemoveListener(EventKey.initRoll, OnStartRoll);
    }

    void OnEndSpecialAttack(IGameEvent e)
    {
        StandardSendIntEvent s = e as StandardSendIntEvent;
        if(s.Sender==gameObject)//if (s.MyInt == MyView.viewID)
        {
            _Animator.Play("padrao");
            tag = "Enemy";
        }
    }

    void OnStartSpecialAttack(IGameEvent e)
    {
        StandardSendIntEvent s = e as StandardSendIntEvent;
        //PUN Animator A = PhotonView.Find(s.MyInt).GetComponent<Animator>();
        Animator A = e.Sender.GetComponent<Animator>();

        GolpeEspecialDoFelixcat.StartSpecialAttack(A);
    }
}
