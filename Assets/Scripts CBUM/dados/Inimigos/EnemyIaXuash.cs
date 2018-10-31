using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyIaXuash : EnemyIaRefinedAttackBase
{
    private float tempoDePulo = 0;

    public EnemyIaXuash(GameObject G, CaracteristicasDeMovimentacao carac) : base(G, carac) { }

    protected override void PosicionandoAtaque()
    {
        if (attack == 1)
        {
            if (move.Mov.NoChao() && tempoDePulo > 1.5f && tempoDePulo < 100)
            {
                move.Mov._Pulo.IniciaAplicaPulo(dono.transform.position.y);
                tempoDePulo = 101;
            }
            else
            {
                tempoDePulo += Time.deltaTime;
            }

            if (move.UpdatePosition() && move.Mov.NoChao())
            {
                refAttack.StartAttack(new ProjetilDeAgua(), alvo);

                estado = EstadoDaIa.atacando;
            }
        }
        else if (attack == 0)
        {
            move.UpdatePosition();

            if (Vector3.Distance(alvo.position, dono.transform.position) < 15)
            {
                if (move.Mov.NoChao() && Vector3.Distance(dono.transform.position, alvo.position) < 10)
                {
                    refAttack.StartAttack(new Tapa(), alvo);
                    move.Mov.AplicadorDeMovimentos(Vector3.zero);
                    estado = EstadoDaIa.atacando;
                }
                //cheguei onde eu queria
            }
            else if (Vector3.Distance(alvo.position, dono.transform.position) >= 15 && move.Mov.NoChao())
            {
                if (disGuardada > 1.05f * Vector3.Distance(alvo.position, dono.transform.position))
                {
                    move.ModificarOndeChegar(alvo.position, 5);
                }

                disGuardada = Vector3.Distance(alvo.position, dono.transform.position);
            }
        }
    }

    protected void VerificaPulo(bool moveUpdate)
    {
        if (dono.name == "esperandoTeste")
            Debug.Log("Verifica pulo: "+!moveUpdate+" : "+ move.Mov.NoChao() +" : "+ tempoDePulo);

        if (!moveUpdate && move.Mov.NoChao() && tempoDePulo > 1.5f)
        {
            Debug.Log(tempoDePulo+" disparando pulo");
            move.Mov._Pulo.IniciaAplicaPulo(dono.transform.position.y);
            tempoDePulo = 0;
        }
        else
        {
            tempoDePulo += Time.deltaTime;
        }
    }

    protected override void EstadoDeSondando()
    {
        bool moveUpdate = move.UpdatePosition();

        VerificaPulo(moveUpdate);

        AtualizaPosicaoDeSondagem(moveUpdate);
    }

    protected override void SorteiaAttack()
    {
        float s = Random.Range(0f, 1f);

        if (s < 0.75f)
            attack = 1;
        else
            attack = 0;

        if (attack == 1)
            move.ModificarOndeChegar(ProcuraPosDoAtaqueProjetil(), 4);
        else if (attack == 0)
            move.ModificarOndeChegar(alvo.position, 4);
    }
}