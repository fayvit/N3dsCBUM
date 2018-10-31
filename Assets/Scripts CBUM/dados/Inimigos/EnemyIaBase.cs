using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyIaBase
{
    [SerializeField] private float tempoParaAtualizarEspera = 2.5f;
    [SerializeField] protected EstadoDaIa estado = EstadoDaIa.emEspera;
    [SerializeField] protected Transform alvo;
    [SerializeField] protected ControlledMoveForCharacter move;
    [SerializeField] protected GameObject dono;

    protected float tempoDecorrido = 0;
    protected float tempoDeAgressividade = 7.5f;

    public float TempoParaAtualizarEspera
    {
        get { return tempoParaAtualizarEspera; }
        set { tempoParaAtualizarEspera = value; }
    }

    protected enum EstadoDaIa
    {
        emEspera,
        sondando,
        posicionandoAtaque,
        aoAtaque,
        atacando,
        parado
    }

    public EnemyIaBase(GameObject G,CaracteristicasDeMovimentacao carac)
    {
        dono = G;
        move = new ControlledMoveForCharacter(carac,G.transform);
    }

    public virtual void MudeParaAoAtaque(Transform T)
    {
        estado = EstadoDaIa.aoAtaque;
        alvo = T;
        tempoDecorrido = 0;
        move.ModificarOndeChegar(alvo.position + (dono.transform.position - alvo.position).normalized * 2,3);
        //move.InsereElementosDeControle(dono, , 3);
    }

    public virtual void Update()
    {
        
    }

    public virtual void LimpaEventos()
    {

    }

    protected void SelecionaAlvo()
    {
        GameObject[] Gs = GameObject.FindGameObjectsWithTag("Player");

        int numAlvo = Random.Range(0, Gs.Length);

        alvo = Gs[numAlvo].transform;
    }

}