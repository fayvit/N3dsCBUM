using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirAttackEnemy : AirAttackManager
{
    public AirAttackEnemy(Animator T, CharacterController controle) : base(T, controle)
    {
    }

    public override void DisparaAirAttack()
    {
        AtkAtivo = true;
        VerificaAnimaAndCollider();
    }

    protected override void VerificaAnimaAndCollider()
    {
        Animador.SetBool("atk1", true);
        GameController.g.StartCoroutine(ColocaHitCollider(new SocoGiratorio() {
            DeslocamentoDoColisor = 1f,
            TempoDeAtrasoParaCollider = 0.1f,
            ValorDeDano = 1}));
    }
}
