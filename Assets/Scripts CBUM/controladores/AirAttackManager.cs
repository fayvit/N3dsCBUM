using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirAttackManager:AttackBase
{
    private CharacterController controle;
    private Transform alvo;
    private bool atkAtivo = false;

    protected bool AtkAtivo
    {
        get { return atkAtivo; }
        set { atkAtivo = value; }
    }

    public AirAttackManager(Animator T,CharacterController controle)
    {
        Animador = T;
        this.controle = controle;
        EventAgregator.AddListener(EventKey.enemyDamage, ResetaAirAttack);
        EventAgregator.AddListener(EventKey.heroDamage, ResetaAirAttack);
        EventAgregator.AddListener(EventKey.gameObjectDisabled, LimpaEventos);
    }

    public virtual void DisparaAirAttack()
    {
        alvo = FindEnemies.ProcureUmBomAlvo(controle.gameObject, 10);
        AtkAtivo = true;
        VerificaAnimaAndCollider();
    }

    public bool VerificaComandoDeAttack(int controlador)
    {
        if (CommandReader.ButtonDown(0, controlador)&&!AtkAtivo)
        {
            DisparaAirAttack();
            return true;
        }

        return false;
    }

    void ResetaAirAttack(IGameEvent e)
    {
        if (Animador != null)
        {
            if (e.Sender == Animador.gameObject)
                ResetaAirAttack();
        }
        else
        {
            GameController.g.StartCoroutine(ELimpaEventos(e));
            Debug.Log("chamou depois de destruido");
        }
    }

    public void ResetaAirAttack()
    {
        Animador.SetBool("atk1", false);
        AtkAtivo = false;
    }

    public bool Update(bool noChao)
    {
        if (noChao)
        {
            ResetaAirAttack();
            return true;
        }
        else if (AtkAtivo)
        {
            if (alvo != null)
                controle.Move((alvo.position-controle.transform.position)*Time.deltaTime*0.5f);
        }
        return false;
    }

    protected virtual void VerificaAnimaAndCollider()
    {
        Animador.SetBool("atk1", true);
        GameController.g.StartCoroutine(ColocaHitCollider(new SocoGiratorio() { DeslocamentoDoColisor=1f,TempoDeAtrasoParaCollider=0.1f}));
    }

    void LimpaEventos(IGameEvent e)
    {
        if (e.Sender == Animador.gameObject)
        {
            GameController.g.StartCoroutine(ELimpaEventos(e));
        }
    }

    IEnumerator ELimpaEventos(IGameEvent e)
    {
        yield return new WaitForEndOfFrame();
        EventAgregator.RemoveListener(EventKey.enemyDamage, ResetaAirAttack);
        EventAgregator.RemoveListener(EventKey.heroDamage, ResetaAirAttack);
        EventAgregator.RemoveListener(EventKey.gameObjectDisabled, LimpaEventos);
    }
}