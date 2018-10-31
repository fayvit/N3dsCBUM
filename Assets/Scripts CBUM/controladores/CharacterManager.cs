using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterManager : CharacterBase {
    
    [SerializeField] private MovimentacaoBasica mov;
    [SerializeField] private EstadoDePersonagem estado = EstadoDePersonagem.naoIniciado;
    [SerializeField] private Controlador controlador = Controlador.teclado;

    private AtackManager atkManager;
    private AirAttackManager airAtk;
    private MagicAttackManager magicAttack;
    private RollManager roll;
    private int numDerrotas = 0;
    public bool eLoad = false;
    
    public EstadoDePersonagem Estado
    {
        get { return estado; }
        set { estado = value; }
    }

    public MovimentacaoBasica Mov
    {
        get { return mov; }
    }

    public Controlador Controlador
    {
        get { return controlador; }
        set { controlador = value; }
    }

    public int NumDerrotas
    {
        get { return numDerrotas; }
        private set { numDerrotas = value; }
    }

    public void MudarParaMovimentoDeFora()
    {
        estado = EstadoDePersonagem.movimentoDeFora;
    }

    public void VoltarParaPasseio()
    {
        roll.FinalizaRollManager(new StandardGameEvent(gameObject,EventKey.nulo));
        airAtk.ResetaAirAttack();
        atkManager.ResetaAtkManager();

        estado = EstadoDePersonagem.aPasseio;
    }

    // Use this for initialization
    void Start () {

        EventAgregator.AddListener(EventKey.morteDoPlayer, OnPlayerDefeated);
        EventAgregator.AddListener(EventKey.retornaDaMortePlayer, OnPlayerDefeatedReturn);
        EventAgregator.AddListener(EventKey.initRoll, OnStartRoll);

        _Animator = GetComponent<Animator>();

        CharacterController controle = GetComponent<CharacterController>();

        atkManager = new AtackManager(new ImpactBase[6] {
            new Jab(){ ExitHitTime = 0},new Jab(),new Jab(),new SocoDireto(),new Jab(),new SocoGiratorio()
        },_Animator);

        airAtk = new AirAttackManager(_Animator,controle);

        EmDano = new EstouEmDano(controle);

        estado = EstadoDePersonagem.aPasseio;

        //MyView = GetComponent<PhotonView>();

        //PUN magicAttack = new MagicAttackManager(_Animator, MyView.viewID);
        magicAttack = new MagicAttackManager(_Animator, 0);

        roll = new RollManager(_Animator, controle, 0.4f, 0.37f);

        /*
        if (!PhotonNetwork.connected)
        {
            PhotonNetwork.offlineMode = true;
            PhotonNetwork.CreateRoom("sala de festas");
        }*/

        if (GlobalController.g.Jogadores.Count == 0)
        {
            GlobalController.g.Jogadores.Add(new SloteMultiplayer()
            {
                Control = this.Controlador,
                NomeNoJogo = "Player 1",
                ViewID = 0//MyView.viewID,
                //ViewOwner = MyView.owner
            });
        }

        /*PUN
        if (!MyView.isMine)
            enabled = false;*/

    }

    private void OnStartRoll(IGameEvent obj)
    {
        StandardSendIntAndStringEvent s = obj as StandardSendIntAndStringEvent;
        /*   if (s.MyInt == MyView.viewID)
           {
               roll.EventInitRoll(MyView,s.MyString);
           }*/

        roll.EventInitRoll(gameObject, s.MyString);
    }

    private void OnPlayerDefeatedReturn(IGameEvent e)
    {
        StandardSendIntEvent s = e as StandardSendIntEvent;

        //Debug.Log(s.MyInt+" : "+MyView.viewID);
        /*if (s.MyInt == MyView.viewID)
        {
            RetornadoDaMorte();
        }*/

        RetornoDaMorte();
    }

    void RetornadoDaMorte()
    {
        _Animator.Play("padrao", 0, 0);
        GetComponent<CapsuleCollider>().enabled = true;
        GetComponent<CharacterController>().enabled = true;
    }

    private void OnPlayerDefeated(IGameEvent obj)
    {
        StandardSendIntEvent s = obj as StandardSendIntEvent;
       // if (s.MyInt==MyView.viewID)
       // {
            _Animator.Play("cair", 0, 0);
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<CharacterController>().enabled = false;
            HUDManager.h.AtualizaNumeroDeMortes();

          //  if (MyView.isMine)
            {
                StartCoroutine(RetornoDaMorte());
            }
       // }
    }

    IEnumerator RetornoDaMorte()
    {
        yield return new WaitForSeconds(3);

        transform.position += Vector3.up * 8;
        estado = EstadoDePersonagem.aPasseio;
        Dados.SetarVidaMax();
        Dados.SetarManaMax();

        EventAgregator.Publish(EventKey.heroDamage, new CharacterDamageEvent(this));//usado para atualizar HUD em rede e resetar varios itens

        /*PUN
        RPC_Listener.RPC(NameOfRPC.EventPublisher, 
            PhotonTargets.Others, 
            EventKey.retornaDaMortePlayer, 
            BytesTransform.ToBytes(
            new StandardSendIntEvent(gameObject, MyView.viewID,EventKey.retornaDaMortePlayer)));
            */

        RPC_Listener.EventPublisher(
            EventKey.retornaDaMortePlayer,
            BytesTransform.ToBytes(
            new StandardSendIntEvent(gameObject, 0, EventKey.retornaDaMortePlayer)));
        RetornadoDaMorte();
    }

    private void OnDestroy()
    {
        atkManager.LimpaEventos();
        roll.LimparEventos();
        EventAgregator.RemoveListener(EventKey.morteDoPlayer, OnPlayerDefeated);
        EventAgregator.RemoveListener(EventKey.retornaDaMortePlayer, OnPlayerDefeatedReturn);
        EventAgregator.RemoveListener(EventKey.initRoll, OnStartRoll);
    }

    // Update is called once per frame
    void Update () {

        int numControl = (int)Controlador;

        if (Controlador != Controlador.rede && Controlador!=Controlador.nulo)
        {
            switch (estado)
            {
                case EstadoDePersonagem.aPasseio:
                    #region aPasseio
                    mov.AplicadorDeMovimentos(CommandReader.VetorDirecao(numControl));

                    VerificaIniciaPulo();

                    if (mov.NoChao())
                    {
                        if (atkManager.VerificaComandoDeAtaque(numControl))
                            estado = EstadoDePersonagem.emAtk;
                        else if (CommandReader.ButtonDown(3, numControl) && Dados.PontosDeMana > 0)
                        {
                            Dados.ConsomeMana(1);

                            RPC_Listener.ModificaHudMana(0,
                                (float)Dados.PontosDeMana / Dados.MaxMana);

                            /*PUN
                            RPC_Listener.RPC(
                                NameOfRPC.ModificaHudMana,
                                PhotonTargets.All,
                                GlobalController.g.IndiceEntreJogadores(MyView.viewID),
                                (float)Dados.PontosDeMana / Dados.MaxMana);*/

                            estado = EstadoDePersonagem.magicAttack;
                            _Animator.SetBool("magicInvoke", true);
                        }
                        else if (CommandReader.ButtonDown(2, numControl))
                        {
                            mov.AplicadorDeMovimentos(Vector3.zero);
                            estado = EstadoDePersonagem.roll;
                            roll.StartRoll(roll.SetarDirecao(transform,numControl));
                        }

                    }
                    else if (airAtk.VerificaComandoDeAttack(numControl))
                        estado = EstadoDePersonagem.airAtk;
                    #endregion
                break;
                case EstadoDePersonagem.roll:
                    #region roll
                    if (roll.UpdateRoll())
                    {
                        estado = EstadoDePersonagem.aPasseio;
                    }
                    else if (!mov.NoChao())
                        mov.AplicaGravidade();
                    #endregion
                break;
                case EstadoDePersonagem.magicAttack:
                    #region magicAttack
                    mov.AplicadorDeMovimentos(Vector3.zero);
                    if (magicAttack.Update())
                    {
                        _Animator.SetBool("magicInvoke", false);
                        estado = EstadoDePersonagem.aPasseio;
                    }
                    #endregion
                break;
                case EstadoDePersonagem.airAtk:
                    #region airAtk
                    mov.AplicadorDeMovimentos(CommandReader.VetorDirecao(numControl));
                    if (airAtk.Update(mov.NoChao()))
                    {
                        estado = EstadoDePersonagem.aPasseio;
                    }
                    #endregion
                break;
                case EstadoDePersonagem.emAtk:
                    #region emAtk
                    mov.AplicadorDeMovimentos(Vector3.zero);
                    if (atkManager.Update(numControl))
                    {
                        estado = EstadoDePersonagem.aPasseio;
                    }
                    #endregion
                break;
                case EstadoDePersonagem.parado:
                    mov.AplicadorDeMovimentos(Vector3.zero);
                break;
                case EstadoDePersonagem.emDano:
                    #region emDano
                    if (EmDano.Update(transform))
                    {
                        mov.AplicadorDeMovimentos(Vector3.zero);
                    }else
                    {
                        estado = EstadoDePersonagem.aPasseio;
                    }
                    #endregion
                break;
            }
        }
	}

    public void VerificaIniciaPulo()
    {
        if (estado == EstadoDePersonagem.aPasseio&& CommandReader.ButtonDown(1, (int)Controlador))
            mov.VerificaComandoDePulo();       
    }

    public override void TomaDano(IGolpeBase golpe, GameObject atacante)
    {
        //PUN RPC_Listener.RPC(NameOfRPC.AnimaAtk,MyView.owner, MyView.viewID,BytesTransform.ToBytes(golpe));  
        RPC_Listener.AnimaAtk(gameObject, BytesTransform.ToBytes(golpe));
    }

    public void AnimaAtk(IGolpeBase golpe)
    {
        Dados.AplicaDano(golpe.ValorDeDano);        

        _Animator.SetBool("magicInvoke", false);

        if (VerificaDerrota())
        {
            EventAgregator.Publish(EventKey.heroDamage, new CharacterDamageEvent(this));
            /*PUN 
            RPC_Listener.RPC(NameOfRPC.EventPublisher, PhotonTargets.All, EventKey.morteDoPlayer, 
                BytesTransform.ToBytes(
                new StandardSendIntEvent(gameObject,MyView.viewID,EventKey.morteDoPlayer)));*/
            RPC_Listener.EventPublisher(EventKey.morteDoPlayer,
            BytesTransform.ToBytes(
            new StandardSendIntEvent(gameObject, 0, EventKey.morteDoPlayer)));
            //RPC_Listener.RPC(NameOfRPC.AnimaMorteDoPlayer, PhotonTargets.All, "cair", GetComponent<PhotonView>().viewID);
            _Animator.Play("cair", 0, 0);
            mov.Controle.enabled = false;
            estado = EstadoDePersonagem.derrotado;
            numDerrotas++;
        }
        else
        {
            /*PUN 
            RPC_Listener.RPC(NameOfRPC.AnimaPlay, PhotonTargets.All,
            "dano1",
            MyView.viewID
            );*/
            RPC_Listener.AnimaPlay(
            "dano1",
            gameObject
            );
            estado = EstadoDePersonagem.emDano;

            if (EmDano == null)
                EmDano = new EstouEmDano(mov.Controle);

            EmDano.Start(transform.position, 0, golpe);

            EventAgregator.Publish(EventKey.heroDamage, new CharacterDamageEvent(this));
        }   
    }

    private void OnTriggerEnter(Collider hit)
    {
        
        if (hit.gameObject.tag == "Enemy")
        {

            TomaDano(new IGolpeBase()
            {
                ValorDeDano = 0,
                DirDeREpulsao = (transform.position-hit.transform.position).normalized,
                DistanciaDeRepulsao = 3,
                VelocidadeDeRepulsao = 15,
                TempoNoDano = 1
            }, gameObject
                );
        }

    }
}

public enum EstadoDePersonagem
{
    naoIniciado = -1,
    aPasseio,
    parado,
    emAtk,
    emDano,
    movimentoDeFora,
    derrotado,
    airAtk,
    magicAttack,
    roll
}

public enum Controlador
{
    teclado = -1,
    nulo = 0,
    joystick1,
    joystick2,
    joystick3,
    joystick4,
    rede
}
