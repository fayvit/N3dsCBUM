using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkTriggerManager : MonoBehaviour {

    [SerializeField] private string ID;
    [SerializeField] private Transform[] alvos;
    [SerializeField] private TalkManagerState state = TalkManagerState.emEspera;
    [SerializeField] private Animator animatorDoNPC;
    [SerializeField] private NPCcomOpcoesDeConversa npcOpcoes;

    private bool[] movimentoAlcancado = new bool[4];
    private ControlledMoveForCharacter[] controlls = new ControlledMoveForCharacter[4];

    private enum TalkManagerState
    {
        emEspera,
        movimentando,
        conversando,
        finalizado
    }

	// Use this for initialization
	void Start () {
        EventAgregator.AddListener(EventKey.startControlledMovement, OnStartControlledMovement);
        EventAgregator.AddListener(EventKey.externalMovementPositionOk, OnPositionOk);
        EventAgregator.AddListener(EventKey.finallyTalk, OnFinallyTalk);
    }

    private void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.startControlledMovement, OnStartControlledMovement);
        EventAgregator.RemoveListener(EventKey.externalMovementPositionOk, OnPositionOk);
        EventAgregator.RemoveListener(EventKey.finallyTalk, OnFinallyTalk);
    }

    // Update is called once per frame
    void Update () {
        switch (state)
        {
            case TalkManagerState.movimentando:
                Movimentando();
            break;
            case TalkManagerState.conversando:
                if (npcOpcoes.Update())
                {
                    //PUN RPC_Listener.RPC(NameOfRPC.FinalTalkEvent, PhotonTargets.Others, ID);
                    RPC_Listener.FinalTalkEvent(ID);
                    FinalizandoConversa();
                }
            break;
        }
	}

    void FinalizandoConversa()
    {
        GlobalController.g.Music.MudaPara(IdMusic.faseInicial);
        SloteMultiplayer[] SM = GlobalController.g.Jogadores.ToArray();
        for (int i = 0; i < SM.Length; i++)
        {
            CharacterManager m = SM[i].Manager;
           // if (m.MyView.isMine)
            //{
                m.VoltarParaPasseio();
                m.Dados.SetarVidaMax();
                EventAgregator.Publish(EventKey.heroHealth, new CharacterDamageEvent(m));
            //}

            Destroy(
                Instantiate(PrefabList.Load(PrefabListName.particulaPositiva),m.transform.position,Quaternion.identity),3
                );
        }

        state = TalkManagerState.finalizado;

        npcOpcoes.ForcarParada();
    }

    void Movimentando()
    {
        bool foi = true;
        SloteMultiplayer[] SM = GlobalController.g.Jogadores.ToArray();
        for (int i = 0; i < SM.Length; i++)
        {
            CharacterManager m = SM[i].Manager;
           // if (m.MyView.isMine)
           // {
                if (controlls[i].UpdatePosition())
                {
                    movimentoAlcancado[i] = true;
                //RPC_Listener.RPC(NameOfRPC.ExternalMovementPositionOk, PhotonTargets.Others, ID,i);
                RPC_Listener.ExternalMovementPositionOk(ID, i);
                }
         //   }

            foi &= movimentoAlcancado[i];
        }

        if (foi)
        {
            npcOpcoes.Start(transform);
            npcOpcoes.IniciarOpcoes();
            npcOpcoes.IniciaConversa();
            animatorDoNPC.SetBool("apoieChao", true);
            state = TalkManagerState.conversando;
        }
    }

    private void OnValidate()
    {
        BuscadorDeID.Validate(ref ID, this);
    }

    private void OnPositionOk(IGameEvent e)
    {
        StandardSendIntAndStringEvent s = e as StandardSendIntAndStringEvent;

        if (s.MyString == ID)
        {
            movimentoAlcancado[s.MyInt] = true;
        }
    }

    void OnFinallyTalk(IGameEvent e)
    {
        StandardSendStringEvent s = e as StandardSendStringEvent;

        if (s.StringContent == ID)
        {
            FinalizandoConversa();
        }
    }

    void OnStartControlledMovement(IGameEvent e)
    {
        StandardSendStringEvent s = e as StandardSendStringEvent;

        if (s.StringContent == ID)
        {
            IniciandoMovimentoControlado();
        }
    }

    void IniciandoMovimentoControlado()
    {
        GlobalController.g.Music.MudaPara(IdMusic.deConversa);
        SloteMultiplayer[] SM = GlobalController.g.Jogadores.ToArray();
        for (int i = 0; i < SM.Length; i++)
        {
            CharacterManager m = SM[i].Manager;
            //if (m.MyView.isMine)
            //{
                m.MudarParaMovimentoDeFora();
                controlls[i] = new ControlledMoveForCharacter(new CaracteristicasDeMovimentacao()
                {
                    caracPulo = new CaracteristicasDePulo(),
                    caracPulo2 = new CaracteristicasDePulo()
                }
                    , m.transform);
                controlls[i].ModificarOndeChegar(alvos[i].position, 6);
            //}
        }

        state = TalkManagerState.movimentando;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            IniciandoMovimentoControlado();
            //PUN RPC_Listener.RPC(NameOfRPC.RequestExternalMovement, PhotonTargets.Others,ID);
            RPC_Listener.RequestExternalMovement(ID);
        }
    }
}
