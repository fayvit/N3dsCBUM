
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIaBasic : EnemyIaBase
{
    private AtackManager atkManager;
    private float tempoParaAtualizarPosAlvo = 0;

    protected float distanciaDoPosicionamentoDeAtaque = 0.9f;
    protected float distanciaDoAtaqueDeRevide = 2;

    protected AtackManager AtkManager
    {
        get { return atkManager; }
        set { atkManager = value; }
    }

    public EnemyIaBasic(GameObject G,CaracteristicasDeMovimentacao carac) : base(G,carac)
    {
        AtkManager = new AtackManager(new ImpactBase[2]{ new Jab() { ExitHitTime=0.5f,TempoNoDano=0.6f},
            new SocoDireto() {
                DistanciaDeRepulsao = 3,
                VelocidadeDeRepulsao = 15
            } }, G.transform);
    }

    public override void MudeParaAoAtaque(Transform T)
    {
        AtkManager.ResetaAtkManager();
        base.MudeParaAoAtaque(T);
    }

    public virtual void DisparaAtaqueEscolhido()
    {
        AtkManager.DisparaAtaque();
    }

    public virtual bool AtualizadorDeAtaque()
    {
        return AtkManager.SeqUpdate();
    }

    public override void LimpaEventos()
    {
        AtkManager.LimpaEventos();
        //base.LimpaEventos();
    }

    public void EstadoDeSondando()
    {
        if (move.UpdatePosition() && Vector3.Distance(alvo.position, dono.transform.position) < 15)
        {
            if (Vector3.Distance(alvo.position, dono.transform.position) < 6)
            {
                estado = EstadoDaIa.posicionandoAtaque;
            }
            else
            {
                estado = EstadoDaIa.emEspera;
            }
            //cheguei onde eu queria
        }
        else if (Vector3.Distance(alvo.position, dono.transform.position) >= 15 && tempoParaAtualizarPosAlvo <= 0)
        {
            tempoParaAtualizarPosAlvo = 1;
            SelecionaAlvo();
            Vector2 dir = Random.insideUnitCircle;
            Vector3 posDeSondagem = alvo.position + 5 * new Vector3(dir.x, 0, dir.y);
            estado = EstadoDaIa.sondando;
            move.ModificarOndeChegar(posDeSondagem, 4);
        }
        else if (tempoParaAtualizarPosAlvo > 0)
        {
            tempoParaAtualizarPosAlvo -= Time.deltaTime;
        }
    }

    

    public void MudeParaAoAtaque()
    {
        SelecionaAlvo();
        MudeParaAoAtaque(alvo);
    }


    public override void Update()
    {
        switch (estado)
        {
            case EstadoDaIa.emEspera:

                if (tempoDecorrido > TempoParaAtualizarEspera)
                {
                    tempoDecorrido = 0;
                    SelecionaAlvo();
                    Vector2 dir = Random.insideUnitCircle;
                    Vector3 posDeSondagem = alvo.position + 5 * new Vector3(dir.x, 0, dir.y);
                    estado = EstadoDaIa.sondando;
                    move.ModificarOndeChegar(posDeSondagem, 2);
//                    move.InsereElementosDeControle(dono, posDeSondagem, 2);
                }
                else
                    tempoDecorrido += Time.deltaTime;
            break;
            case EstadoDaIa.sondando:
                EstadoDeSondando();
            break;
            case EstadoDaIa.posicionandoAtaque:
                if (Vector3.Distance(dono.transform.position, alvo.position) > distanciaDoPosicionamentoDeAtaque
                    &&
                    Vector3.Distance(dono.transform.position, alvo.position) < 10)
                    move.ModificarOndeChegar(alvo.position + (dono.transform.position - alvo.position).normalized * distanciaDoPosicionamentoDeAtaque, 3);
                //move.InsereElementosDeControle(dono, alvo.position + (dono.transform.position - alvo.position).normalized * 0.9f, 3);
                else if (Vector3.Distance(dono.transform.position, alvo.position) >= 10)
                    estado = EstadoDaIa.emEspera;

                if (move.UpdatePosition())
                {
                    estado = EstadoDaIa.atacando;
                    DisparaAtaqueEscolhido();
                    tempoDecorrido = 0;
                }
            break;
            case EstadoDaIa.atacando:
                if (AtualizadorDeAtaque())
                    estado = EstadoDaIa.emEspera;
            break;
            case EstadoDaIa.aoAtaque:

                if (alvo)
                {
                    if (Vector3.Distance(dono.transform.position, alvo.position) > distanciaDoAtaqueDeRevide)
                        move.ModificarOndeChegar(alvo.position + (dono.transform.position - alvo.position).normalized * distanciaDoAtaqueDeRevide, 3);//InsereElementosDeControle(dono, alvo.position + (dono.transform.position - alvo.position).normalized * 2, 3);

                    if (move.UpdatePosition())
                    {
                        dono.transform.rotation = Quaternion.LookRotation(
                            Vector3.ProjectOnPlane(alvo.position - dono.transform.position, Vector3.up)
                            );
                        move.AnimacaoPadrao();
                        DisparaAtaqueEscolhido();
                        estado = EstadoDaIa.atacando;
                        tempoDecorrido = 0;
                    }
                    else
                    {
                        tempoDecorrido += Time.deltaTime;
                        if (tempoDecorrido >= tempoDeAgressividade)
                        {
                            tempoDecorrido = 0;
                            estado = EstadoDaIa.emEspera;
                            move.AnimacaoPadrao();
                        }
                    }
                }
                else
                    estado = EstadoDaIa.aoAtaque;
                break;
        }
    }

    /*
    public virtual void EstadoDeSondando()
    {

    }


    public virtual void DisparaAtaqueEscolhido()
    {

    }

    public virtual bool AtualizadorDeAtaque()
    {
        return false;
    }*/

    
}
