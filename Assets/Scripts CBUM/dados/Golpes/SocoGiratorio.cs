[System.Serializable]
public class SocoGiratorio : ImpactBase
{
    public SocoGiratorio()
    {
        ID = GolpeId.socoGiratorio;
        ValorDeDano = 2;
        TempoHit = .8f;
        ExitHitTime = 2.15f;
        TempoDeAtrasoParaCollider = 0.25f;
        TempoDeVidaDoCollider = .5f;
        DeslocamentoDoColisor = 1.5f;
        DistanciaDeRepulsao = 3;
        VelocidadeDeRepulsao = 15;
        TempoNoDano = .9f;
        DanoEmAliado = true;
        NomeAnimaGolpe = "atk3";
        IDAnimaGolpe = "punch_22";
        NoImpacto = NoImpactoDeDano.impactoPadrao;
    }
}