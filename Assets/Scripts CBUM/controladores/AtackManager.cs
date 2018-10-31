using UnityEngine;
using System.Collections;

public class AtackManager : AttackBase
{
    private int numHit = -1;
    private float tempoDecorrido = 0;
    private ImpactBase[] seqBasica;
    /*
    private readonly float[] tempoHit = new float[6] { 0.5f, 0.5f,0.5f,0.85f,0.5f, 0.9f };
    private readonly float[] exitHit = new float[6] { 0f, 0.25f, 0.25f, 0.25f, 0.5f,0.15f };
    */
    #region constructors
    public AtackManager(ImpactBase[] seqBasica,Transform T)
    {
        this.seqBasica = seqBasica;
        Animador = T.GetComponent<Animator>();
        EventListener();
    }

    public AtackManager(ImpactBase[] seqBasica, Animator T)
    {
        this.seqBasica = seqBasica;
        Animador = T;
        EventListener();
    }
    #endregion

    void EventListener()
    {
        EventAgregator.AddListener(EventKey.heroDamage, ResetaAtkManager);
        EventAgregator.AddListener(EventKey.enemyDamage, ResetaAtkManager);
       // EventAgregator.AddListener(EventKey.gameObjectDisabled, LimpadorDeEventos);
    }

    public void DisparaAtaque()
    {
        Animador.SetBool("emAtk", true);
        numHit++;
        //estado = EstadoDePersonagem.emAtk;
        ResetaAnimaAndTime();
        GameController.g.StartCoroutine(VerificaAnimaAndColliderB());
    }

    public bool VerificaComandoDeAtaque(int controlador)
    {
        if (CommandReader.ButtonDown(0, controlador))
          //  &&
          //  (estado==EstadoDePersonagem.aPasseio||estado==EstadoDePersonagem.emAtk))
        {
            bool foi = false;
            
            if (numHit < seqBasica.Length && numHit > 0)
            {
                if (tempoDecorrido >= seqBasica[numHit].ExitHitTime)
                {
                    foi = true;
                }
            }
            else if (numHit < seqBasica.Length)
                foi = true;

            if (foi)
            {
                if (numHit == -1)
                {
                    Transform alvo = FindEnemies.ProcureUmBomAlvo(Animador.gameObject, 3);

                    if (alvo)
                        Animador.transform.rotation = Quaternion.LookRotation(
                            Vector3.ProjectOnPlane(
                                 alvo.position - Animador.transform.position,
                                Vector3.up));
                }

                DisparaAtaque();
                return true;
            }
        }

        return false;
    }

    public bool Update(int controlador)
    {
        tempoDecorrido += Time.deltaTime;

        VerificaComandoDeAtaque(controlador);
        
        if (tempoDecorrido >= seqBasica[numHit].TempoHit)
        {
            ResetaAtkManager();
            return true;
        }

        return false;
    }

    public bool SeqUpdate()
    {

        tempoDecorrido += Time.deltaTime;

        if (numHit + 1 < seqBasica.Length)
        {
            if (tempoDecorrido >= seqBasica[numHit].ExitHitTime)
                DisparaAtaque();

        }
        else if (tempoDecorrido >= seqBasica[numHit].TempoHit)
        {
            ResetaAtkManager();
            return true;
        }

        return false;
    }

    void ResetaAtkManager(IGameEvent e)
    {
        if (Animador != null)
        {
            if (e.Sender == Animador.gameObject)
                ResetaAtkManager();
        }
        else
        {
            LimpaEventos();
            Debug.Log("chamou depois de destruido");
        }
    
    }

    public void ResetaAtkManager()
    {
        numHit = -1;
        Animador.SetBool("emAtk", false);
        ResetaAnimaAndTime();
    }

    public void ResetaAnimaAndTime()
    {        
        tempoDecorrido = 0;
        Animador.SetBool("atk1", false);
        Animador.SetBool("atk2", false);
        Animador.SetBool("atk3", false);
    }

    IEnumerator VerificaAnimaAndColliderB()
    {
        yield return new WaitForEndOfFrame();

        VerificaAnimaAndCollider();
    }

    void VerificaAnimaAndCollider()
    {
        ResetaAnimaAndTime();
        
        //Debug.Log(Animador + " : " + seqBasica + " : " + numHit + " : " + seqBasica[numHit].NomeAnimaGolpe);

        Animador.SetBool(seqBasica[numHit].NomeAnimaGolpe, true);

        /*
        PhotonNetwork.RPC(GameController.g.MyView, "AnimaPlay", 
            PhotonTargets.All, false, 
            seqBasica[numHit].IDAnimaGolpe, 
            animator.GetComponent<PhotonView>().viewID);  */
                
        GameController.g.StartCoroutine(ColocaHitCollider(seqBasica[numHit]));
        
       
    }

    public void LimpaEventos()
    {
        if(GameController.g)
            GameController.g.StartCoroutine(ELimpaEventos());
        
    }

    IEnumerator ELimpaEventos()
    {
        yield return new WaitForEndOfFrame();
        
        EventAgregator.RemoveListener(EventKey.enemyDamage,ResetaAtkManager);
        EventAgregator.RemoveListener(EventKey.heroDamage, ResetaAtkManager);
        //EventAgregator.RemoveListener(EventKey.gameObjectDisabled, LimpadorDeEventos);
        
    }

}