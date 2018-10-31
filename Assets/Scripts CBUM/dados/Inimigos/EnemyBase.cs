using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : CharacterBase {
    [SerializeField] private EnemyIaBase ia;
    [SerializeField] private EstadoDePersonagem estado = EstadoDePersonagem.naoIniciado;
    [SerializeField] private int desligarSeNumPlayersMenorQue = 0;
    [SerializeField] private float tempoParaEspera = 2.5f;

    [SerializeField] protected CaracteristicasDeMovimentacao carac;

    private Image barraDeVida;
    private CharacterController controle;    
    
    private float tempodeBarraDeVidaAtiva = 0;
    private float tempoDecorrido = 0;

    private const float tempoDeDestruicao = 2;

    protected EnemyIaBase Ia
    {
        get { return ia; }
        set { ia = value; }
    }

    // Use this for initialization
    protected virtual void Start () {
        controle = GetComponent<CharacterController>();
        //MyView = GetComponent<PhotonView>();

        VerificaBarraDeVida();

        EventAgregator.AddListener(EventKey.lanEnemyDefeated, OnDefeatedInLan);

    }

    protected void ConstruirIa(EnemyIaBase estaIa)
    {
        //if (PhotonNetwork.isMasterClient)
        {
            Ia = estaIa;

            Ia.TempoParaAtualizarEspera = tempoParaEspera;
        }
        //else
          //  enabled = false;
    }

    protected virtual void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.lanEnemyDefeated, OnDefeatedInLan);
        Ia.LimpaEventos();
        
    }

    public virtual void IniciarIa()
    {
        if (GlobalController.g.Jogadores.Count < desligarSeNumPlayersMenorQue)
        {
            EventAgregator.Publish(EventKey.enemyDisable, new StandardGameEvent(gameObject, EventKey.enemyDisable));
            //PUN PhotonNetwork.Destroy(gameObject);
            Destroy(gameObject);
        }
        else
        {
            estado = EstadoDePersonagem.aPasseio;
            enabled = true;
            Start();
        }
    }

    void VerificaBarraDeVida()
    {
        if (barraDeVida == null)
        {
            
            GameObject G = Instantiate((GameObject)Resources.Load(PrefabListName.barraDeVida.ToString()), 
                transform.position+controle.center+Vector3.up*(0.5f*controle.height+1), Quaternion.identity);

            G.transform.SetParent(transform);

            G.SetActive(false);
            barraDeVida = G.transform.GetChild(1).GetComponent<Image>();
        }
    }

    // Update is called once per frame
    void Update () {
        switch (estado)
        {
            case EstadoDePersonagem.aPasseio:
                Ia.Update();
            break;
            case EstadoDePersonagem.emDano:
                if (EmDano.Update(transform))
                {

                }
                else
                {
                    estado = EstadoDePersonagem.aPasseio;
                }
            break;
            case EstadoDePersonagem.derrotado:
                tempoDecorrido += Time.deltaTime;
                if (tempoDecorrido > tempoDeDestruicao)
                {
                    //PUN RPC_Listener.RPC(NameOfRPC.DestruirEnemyBase, PhotonTargets.Others, MyView.viewID);
                    RPC_Listener.DestruirEnemyBase(gameObject);
                    DestruirEnemyBase();
                }
            break;
        }


        if (barraDeVida.transform.parent.gameObject.activeSelf)
        {
            tempodeBarraDeVidaAtiva -= Time.deltaTime;

            if (tempodeBarraDeVidaAtiva < 0)
                //PUN RPC_Listener.RPC(NameOfRPC.SwitchLifeBarView, PhotonTargets.All, MyView.viewID, false, barraDeVida.fillAmount);
                RPC_Listener.SwitchLifeBarView(gameObject, false, barraDeVida.fillAmount);

        }

	}

    public override void TomaDano(IGolpeBase golpe, GameObject atacante)
    {

        VerificaBarraDeVida();
        Dados.AplicaDano(golpe.ValorDeDano);
        barraDeVida.fillAmount = Mathf.Max(0, (float)Dados.PontosDeVida / Dados.MaxVida);
        tempodeBarraDeVidaAtiva = 5;

        //PUN RPC_Listener.RPC(NameOfRPC.SwitchLifeBarView, PhotonTargets.All, MyView.viewID, true, barraDeVida.fillAmount);
        RPC_Listener.SwitchLifeBarView(gameObject, true, barraDeVida.fillAmount);

        Ia.MudeParaAoAtaque(atacante.transform);

        if (_Animator == null)
            _Animator = GetComponent<Animator>();

        if (VerificaDerrota())
        {
            AcoesDeDerrotado();

            //PUN RPC_Listener.RPC(NameOfRPC.BasicEnemyDefeated, PhotonTargets.Others,GetComponent<PhotonView>().viewID);

            estado = EstadoDePersonagem.derrotado;
        }
        else
        {
            EventAgregator.Publish(EventKey.enemyDamage, new EnemyDamageEvent(this));

            estado = EstadoDePersonagem.emDano;

            if (EmDano == null)
                EmDano = new EstouEmDano(GetComponent<CharacterController>());

            EmDano.Start(transform.position, 0, golpe);
            //PUN RPC_Listener.RPC(NameOfRPC.AnimaPlay, PhotonTargets.All, "dano1", GetComponent<PhotonView>().viewID);
            RPC_Listener.AnimaPlay("dano1", gameObject);
        }
    }

    public void OnDefeatedInLan(IGameEvent e)
    {
        StandardSendIntEvent s = e as StandardSendIntEvent;
        if(s.Sender==gameObject)//if (s.MyInt==MyView.viewID)
        {
            AcoesDeDerrotado();
        }
    }

    public void AcoesDeDerrotado()
    {
        EventAgregator.Publish(EventKey.enemyDefeated, new StandardGameEvent(gameObject, EventKey.enemyDefeated));
        MonoBehaviour.Destroy(MonoBehaviour.Instantiate(PrefabList.Load(PrefabListName.enemyBasicDeathSound)), 3);
        _Animator.Play("cair");
        controle.enabled = false;
    }

    public void SwitchViewLifeBar(bool view,float amount)
    {
        barraDeVida.fillAmount = Mathf.Max(0, amount);
        barraDeVida.transform.parent.gameObject.SetActive(view);
    }

    public void DestruirEnemyBase()
    {
        EventAgregator.Publish(EventKey.gameObjectDisabled,new EnemyDamageEvent(this));

        Destroy(
        Instantiate(PrefabList.Load(PrefabListName.destroyEnemyParticle),transform.position,Quaternion.identity),5);


        //    if(PhotonNetwork.isMasterClient)
        //PUN PhotonNetwork.Destroy(gameObject);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider hit)
    {
        
        if (tag=="enemy" && hit.gameObject.tag == "Player" /*&& MyView!=null*/)
        {
          //  if(MyView.isMine)
            TomaDano(new IGolpeBase()
            { ValorDeDano = 0,
              DirDeREpulsao = (transform.position - hit.transform.position).normalized,
                DistanciaDeRepulsao = 3,
              VelocidadeDeRepulsao = 15,
              TempoNoDano = 1
            },gameObject
                );
        }
    }

    private void OnDrawGizmos()
    {
        CharacterController controle = GetComponent<CharacterController>();
        Gizmos.DrawWireCube(transform.position+1.1f*transform.forward*controle.radius+controle.center, 
            new Vector3(controle.radius, 0.5f*(controle.height-controle.radius), 0.2f));
    }
}
