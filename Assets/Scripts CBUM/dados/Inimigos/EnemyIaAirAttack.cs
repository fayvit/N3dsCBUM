using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIaAirAttack : EnemyIaBasic {

    private AirAttackEnemy air;
    public EnemyIaAirAttack(GameObject G, CaracteristicasDeMovimentacao carac,float distRevide,float distPosAtk) : base(G, carac)
    {
        distanciaDoAtaqueDeRevide = distRevide;
        distanciaDoPosicionamentoDeAtaque = distPosAtk;
        air = new AirAttackEnemy(G.GetComponent<Animator>(), G.GetComponent<CharacterController>());

        AtkManager = new AtackManager(new ImpactBase[2]{ new Jab() { ExitHitTime=0.5f,TempoNoDano=0.6f},
            new SocoDireto() {
                DistanciaDeRepulsao = 3,
                VelocidadeDeRepulsao = 15
            } }, G.transform);
    }

    public override void DisparaAtaqueEscolhido()
    {
        move.Mov._Pulo.IniciaAplicaPulo(dono.transform.position.y);
        air.DisparaAirAttack();
    }

    public override bool AtualizadorDeAtaque()
    {
        bool retorno = air.Update(move.Mov.NoChao());
        if(retorno)
            move.Mov.AplicadorDeMovimentos(Vector3.zero);
        else 
            move.Mov.AplicadorDeMovimentos(Vector3.ProjectOnPlane(alvo.position-dono.transform.position,Vector3.up).normalized);

        return retorno;
    }

    public override void MudeParaAoAtaque(Transform T)
    {
        air.ResetaAirAttack();
        base.MudeParaAoAtaque(T);
    }
}
