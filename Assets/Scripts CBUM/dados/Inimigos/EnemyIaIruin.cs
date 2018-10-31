using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIaIruin : EnemyIaRefinedAttackBase {

    public EnemyIaIruin(GameObject G, CaracteristicasDeMovimentacao carac) : base(G, carac){ }

    protected override void EstadoDeSondando()
    {
        bool moveUpdate = move.UpdatePosition();

        AtualizaPosicaoDeSondagem(moveUpdate);
    }

    protected override void PosicionandoAtaque()
    {
        if (move.UpdatePosition() && move.Mov.NoChao())
        {
            refAttack.StartAttack(new ProjetilDeGosma(), alvo);

            estado = EstadoDaIa.atacando;
        }
    }

    protected override void SorteiaAttack()
    {
        move.ModificarOndeChegar(ProcuraPosDoAtaqueProjetil(), 4);
    }
    
}
