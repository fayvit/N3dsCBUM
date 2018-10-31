[System.Serializable]
public class Jab : ImpactBase
{
    public Jab()
    {
        ID = GolpeId.jab;
        ValorDeDano = 1;
        TempoHit = .5f;
        ExitHitTime = .25f;
        TempoDeAtrasoParaCollider = 0;
        TempoDeVidaDoCollider = .5f;
        DeslocamentoDoColisor = 1;
        DistanciaDeRepulsao = 0;
        VelocidadeDeRepulsao = 0;
        TempoNoDano = .9f;
        DanoEmAliado = true;        
        NomeAnimaGolpe = "atk1";
        IDAnimaGolpe = "punch_20";
        NoImpacto = NoImpactoDeDano.impactoPadrao;
    }
}