using UnityEngine;

[System.Serializable]
public class Tapa : ImpactoRefinadoBase
{

    public Tapa()
    {
        ValorDeDano = 2;
        TempoDeAtrasoParaCollider = 0;
        
        DeslocamentoDoColisor = 0;
        DistanciaDeRepulsao = 65;
        VelocidadeDeRepulsao = 33;
        TempoNoDano = .9f;
        DanoEmAliado = true;
        NoImpacto = NoImpactoDeDano.impactoPadrao;
        TempoDeMoveMin = 0.74f;
        TempoEmAnima = 1.2f;
        TempoDeVidaDoCollider = 1.4f;
        VelocidadeDeGolpe = 28;
        NomeAnima = "tapa";
        BloquearMultHit = true;
    }

    public override void StartAttack(GameObject G,Transform alvo)
    {
        base.StartAttack(G, alvo);

        dono.transform.rotation = Quaternion.LookRotation(
            Vector3.ProjectOnPlane(alvo.transform.position - dono.transform.position, Vector3.up));

    }
}