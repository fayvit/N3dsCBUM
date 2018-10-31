using UnityEngine;

[System.Serializable]
public class ProjetilDeAgua : ProjetilRefinedBase
{
    public ProjetilDeAgua()
    {
        //ID = GolpeId.jab;
        ValorDeDano = 2;
        //TempoHit = .5f;
        //ExitHitTime = .25f;
        TempoDeAtrasoParaCollider = 0;
        TempoDeVidaDoCollider = 5f;
        DeslocamentoDoColisor = 0;
        DistanciaDeRepulsao = 0;
        VelocidadeDeRepulsao = 10;
        TempoNoDano = .9f;
        DanoEmAliado = false;
        //NomeAnimaGolpe = "atk1";
        //IDAnimaGolpe = "punch_20";
        NoImpacto = NoImpactoDeDano.impactoDeAgua;
        TempoEmAnima = 1f;
        NomeAnima = "emissor";
        nomeProjetil = PrefabListName.projetilDeAgua.ToString();
    }
}