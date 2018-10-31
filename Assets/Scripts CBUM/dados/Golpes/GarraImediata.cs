using UnityEngine;

[System.Serializable]
public class GarraImediata : ImpactoRefinadoBase
{

    public GarraImediata()
    {
        ValorDeDano = 1;
        TempoDeAtrasoParaCollider = 0;

        DeslocamentoDoColisor = 0;
        DistanciaDeRepulsao = 65;
        VelocidadeDeRepulsao = 33;
        TempoNoDano = .9f;
        DanoEmAliado = true;
        NoImpacto = NoImpactoDeDano.impactoPadrao;
        TempoDeMoveMin = 0.74f;
        TempoEmAnima = 1.2f;
        TempoDeVidaDoCollider = 0.8f;
        VelocidadeDeGolpe = 56;
        NomeAnima = "impulsoVeloz";
        BloquearMultHit = true;
        prefabName = PrefabListName.clawCollider;
    }

    public override void StartAttack(GameObject G, Transform alvo)
    {
        base.StartAttack(G, alvo);

        MonoBehaviour.Destroy(
        MonoBehaviour.Instantiate(PrefabList.Load(PrefabListName.gritoDeGolpeDoBoss)), 3);

        dono.transform.rotation = Quaternion.LookRotation(
            Vector3.ProjectOnPlane(alvo.transform.position - dono.transform.position, Vector3.up));

    }
}