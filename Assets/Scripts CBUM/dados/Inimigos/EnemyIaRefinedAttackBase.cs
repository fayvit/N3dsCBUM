using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyIaRefinedAttackBase : EnemyIaBase {

    protected float disGuardada = 0;
    protected int attack = 0;
    protected RefinedAttackManager refAttack;

    public EnemyIaRefinedAttackBase(GameObject G, CaracteristicasDeMovimentacao carac) : base(G, carac)
    {
        refAttack = new RefinedAttackManager(G);
    }

    protected abstract void PosicionandoAtaque();

    protected abstract void EstadoDeSondando();

    protected abstract void SorteiaAttack();

    public override void Update()
    {
        switch (estado)
        {
            case EstadoDaIa.aoAtaque:
                estado = EstadoDaIa.posicionandoAtaque;
                break;
            case EstadoDaIa.emEspera:

                move.Mov.AplicadorDeMovimentos(Vector3.zero);

                if (tempoDecorrido > TempoParaAtualizarEspera)
                {
                    tempoDecorrido = 0;
                    SelecionaAlvo();
                    Vector2 dir = Random.insideUnitCircle;
                    Vector3 posDeSondagem = alvo.position + 5 * new Vector3(dir.x, 0, dir.y);
                    move.ModificarOndeChegar(posDeSondagem, 6);
                    estado = EstadoDaIa.sondando;
                    //InsereElementosDeControle(dono, posDeSondagem, 2);
                }
                else
                    tempoDecorrido += Time.deltaTime;

            break;
            case EstadoDaIa.sondando:
                EstadoDeSondando();
                break;
            case EstadoDaIa.posicionandoAtaque:
                PosicionandoAtaque();
             break;
            case EstadoDaIa.atacando:
                if (refAttack.UpdateAttack())
                {
                    estado = EstadoDaIa.emEspera;
                }
            break;
        }
    }

    protected void AtualizaPosicaoDeSondagem(bool moveUpdate)
    {
        if (moveUpdate && Vector3.Distance(alvo.position, dono.transform.position) < 15 && move.Mov.NoChao())
        {
            if (Vector3.Distance(alvo.position, dono.transform.position) < 6)
            {
                estado = EstadoDaIa.posicionandoAtaque;
                SorteiaAttack();
            }
            else
            {
                estado = EstadoDaIa.emEspera;
            }

            //cheguei onde eu queria
        }
        else if (Vector3.Distance(alvo.position, dono.transform.position) >= 15 && move.Mov.NoChao())
        {
            if (disGuardada > 1.05f * Vector3.Distance(alvo.position, dono.transform.position))
            {
                estado = EstadoDaIa.emEspera;
            }

            disGuardada = Vector3.Distance(alvo.position, dono.transform.position);
        }
    }

    protected Vector3 ProcuraPosDoAtaqueProjetil()
    {
        SelecionaAlvo();
        RaycastHit hit;
        Vector3 point = default(Vector3);
        if (Physics.Raycast(alvo.transform.position, Vector3.right, out hit, 100, 1024))
        {
            point = hit.point;
        }
        if (Physics.Raycast(alvo.transform.position, Vector3.left, out hit, 100, 1024))
        {
            if (point != default(Vector3))
            {
                if (Vector3.Distance(hit.point, alvo.transform.position) > Vector3.Distance(point, alvo.transform.position))
                    point = hit.point;
            }
            else
                point = hit.point;
        }

        if (point == default(Vector3))
            Debug.Log("parece que algo deu errado [Xuash não encontrou ponto para ir]");

        return point;

    }

    public override void MudeParaAoAtaque(Transform T)
    {
        //base.MudeParaAoAtaque(T);
        alvo = T;
        tempoDecorrido = 0;
        estado = EstadoDaIa.posicionandoAtaque;
        SorteiaAttack();
    }


}
