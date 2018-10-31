using System;
using System.Collections;
using UnityEngine;

public class BossTrigger : MonoBehaviour {

    [SerializeField] private ElementosDoDeslocamentoControlado elDesl;
    [SerializeField] private Transform novoFoco;
    [SerializeField] private EnemyBase boss;
    [SerializeField] private Transform alvoDaMovimentacaoDoBoss;
    [SerializeField] private FinalizouChefe fim;
    [SerializeField] private string ID;
    [SerializeField] private int xAlvo;
    

    private ControlledMoveForCharacter controllBoss;
    private BossTriggerState state = BossTriggerState.wait;

    private enum BossTriggerState
    {
        wait,
        movement,
        movementBoss,
        animaBoss,
        lutaIniciada,
        bossDerrotado
    }


    private void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.startControlledMovement, OnStartControlledMovement);
        EventAgregator.RemoveListener(EventKey.externalMovementPositionOk, OnPositionOk);
        EventAgregator.RemoveListener(EventKey.finallyBosswalk, OnFinallyBossWalk);
        EventAgregator.RemoveListener(EventKey.finallyPresentation, OnFinallyPresentation);
        EventAgregator.RemoveListener(EventKey.enemyDefeated, OnBossDefeated);
        EventAgregator.RemoveListener(EventKey.stopMovementForEndBoss, OnStopForEndBoss);
        //EventAgregator.RemoveListener(EventKey.finallyTalk, OnFinallyTalk);
    }

    // Use this for initialization
    void Start()
    {
        fim.ID = ID;
        EventAgregator.AddListener(EventKey.startControlledMovement, OnStartControlledMovement);
        EventAgregator.AddListener(EventKey.externalMovementPositionOk, OnPositionOk);
        EventAgregator.AddListener(EventKey.finallyBosswalk, OnFinallyBossWalk);
        EventAgregator.AddListener(EventKey.finallyPresentation, OnFinallyPresentation);
        EventAgregator.AddListener(EventKey.enemyDefeated, OnBossDefeated);
        EventAgregator.AddListener(EventKey.stopMovementForEndBoss, OnStopForEndBoss);
    }

    private void OnStopForEndBoss(IGameEvent obj)
    {
        Debug.Log("Eu estive aqui");
        if ((obj as StandardSendStringEvent).StringContent == ID)
        {
            fim.PararHerois();
        }
    }

    void OnBossDefeated(IGameEvent e)
    {
        if (e.Sender == boss.gameObject)
        {
            state = BossTriggerState.bossDerrotado;
            fim.Start();
        }
    }

    private void OnFinallyPresentation(IGameEvent obj)
    {
        if ((obj as StandardSendStringEvent).StringContent == ID)
        {
            FinallyPresentation();
        }
    }

    void FinallyPresentation()
    {
        SloteMultiplayer[] SM = GlobalController.g.Jogadores.ToArray();

        for (int i = 0; i < SM.Length; i++)
        {
            CharacterManager m = SM[i].Manager;
           // if (m.MyView.isMine)
            {
                m.VoltarParaPasseio();
            }
        }

        //if (PhotonNetwork.isMasterClient)
        {
            boss.IniciarIa();
        }

        AplicadorDeCamera.c.CamB.NovoFoco(novoFoco, 10, 12);
        if (xAlvo != 0)
        {
            AplicadorDeCamera.c.CamB.FixarEmX(xAlvo);
        }
    }


    private void OnFinallyBossWalk(IGameEvent e)
    {
        if ((e as StandardSendStringEvent).StringContent == ID)
        {
            StartCoroutine(AnimouBoss());

            Destroy(Instantiate(PrefabList.Load(PrefabListName.gritoDoMonstro)), 3);
            AplicadorDeCamera.c.ShakeCam(24,2);
            Destroy(
                Instantiate(PrefabList.Load(PrefabListName.enfaseDoBoss), boss.transform.position + 3 * Vector3.up, Quaternion.identity), 3
                );
        }
    }

    private void OnPositionOk(IGameEvent obj)
    {
        elDesl.OnPositionOk(obj, ID);
    }

    // Update is called once per frame
    void Update () {
        switch (state)
        {
            case BossTriggerState.movement:
                if (elDesl.Movimentando(ID))
                {
                    GlobalController.g.Music.PararMusicas();
                    Transform antigoAlvo = AplicadorDeCamera.c.CamB.Alvo;
                    AplicadorDeCamera.c.CamB.NovoFoco(novoFoco,10,12);
                    novoFoco = antigoAlvo;
                    controllBoss = new ControlledMoveForCharacter(new CaracteristicasDeMovimentacao(), boss.transform);
                    controllBoss.ModificarOndeChegar(alvoDaMovimentacaoDoBoss.position, 6);
                    state = BossTriggerState.movementBoss;
                }
            break;
            case BossTriggerState.movementBoss:
               // if (PhotonNetwork.isMasterClient)
                {
                    if (controllBoss.UpdatePosition())
                    {
                        //PUN RPC_Listener.RPC(NameOfRPC.FinallyBossWalk, PhotonTargets.All, ID, "posDeGrito", boss.MyView.viewID);
                        RPC_Listener.FinallyBossWalk(ID, "posDeGrito",boss.gameObject);
                        state = BossTriggerState.animaBoss;
                    }
                }
            break;
            case BossTriggerState.bossDerrotado:
                fim.Update();
            break;
        }
	}

    IEnumerator AnimouBoss()
    {
        yield return new WaitForSeconds(1.5f);
        GlobalController.g.Music.MudaPara(IdMusic.bossinicial,2);

        yield return new WaitForSeconds(0.5f);
        //PUN RPC_Listener.RPC(NameOfRPC.FinallyPresentation, PhotonTargets.Others, ID);
        RPC_Listener.FinallyPresentation(ID);
        FinallyPresentation();
    }

    private void OnValidate()
    {
        BuscadorDeID.Validate(ref ID, this);
    }

    void OnStartControlledMovement(IGameEvent e)
    {
        StandardSendStringEvent s = e as StandardSendStringEvent;

        if (s.StringContent == ID)
        {
            GlobalController.g.Music.PararMusicas();
            elDesl.IniciandoMovimentoControlado();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            elDesl.IniciandoMovimentoControlado();
            state = BossTriggerState.movement;
            //PUN RPC_Listener.RPC(NameOfRPC.RequestExternalMovement, PhotonTargets.Others, ID);
            RPC_Listener.RequestExternalMovement(ID);
            GlobalController.g.Music.MudaPara(IdMusic.deConversa);
        }
    }
}
