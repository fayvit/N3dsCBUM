[System.Serializable]
public class WaterMagic : MagicBase
{
    public WaterMagic()
    {
        ID = MagicId.waterMagic;
        ValorDeDano = 4;
        TempoDeAnima = 2f;
        //ExitHitTime = 2.15f;
        TempoDeAtrasoParaCollider = .75f;
        TempoDeVidaDoCollider = 1.5f;
        DeslocamentoDoColisor = 0.15f;
        DistanciaDeRepulsao = 3;
        VelocidadeDeRepulsao = 15;
        TempoNoDano = .9f;
        DanoEmAliado = false;
        //NomeAnimaGolpe = "atk3";
        //IDAnimaGolpe = "punch_22";
        NoImpacto = NoImpactoDeDano.impactoPadrao;
    }
}