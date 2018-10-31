[System.Serializable]
public class SocoDireto : ImpactBase
{
    public SocoDireto()
    {
        ID = GolpeId.socoDireto;
        ValorDeDano = 1;
        TempoHit = .85f;
        ExitHitTime = .5f;
        TempoDeAtrasoParaCollider = 0.1f;
        TempoDeVidaDoCollider = .5f;
        DeslocamentoDoColisor = 1.25f;
        DistanciaDeRepulsao = 0;
        VelocidadeDeRepulsao = 0;
        TempoNoDano = .9f;
        DanoEmAliado = true;
        NomeAnimaGolpe = "atk2";
        IDAnimaGolpe = "punch_21";
        NoImpacto = NoImpactoDeDano.impactoPadrao;
    }
}