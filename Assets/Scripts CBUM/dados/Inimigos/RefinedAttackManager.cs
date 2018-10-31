using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefinedAttackManager
{
    private GolpeRefinadoBase golpeAtual;
    private GameObject dono;

    public RefinedAttackManager(GameObject G)
    {
        dono = G;
    }

    public void StartAttack(GolpeRefinadoBase golpe,Transform alvo)
    {
        golpeAtual = golpe;
        golpe.StartAttack(dono,alvo);
    }

    public bool UpdateAttack()
    {
        return golpeAtual.UpdateAttack();
    }
}
