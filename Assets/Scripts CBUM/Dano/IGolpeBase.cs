using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IGolpeBase {
    private float[] dirDeRepulsao = new float[3];

    public int ValorDeDano { get; set; }
    public float TempoDeAtrasoParaCollider { get; set; }
    public float TempoDeVidaDoCollider { get; set; }
    public float DeslocamentoDoColisor{ get; set; }
    public float DistanciaDeRepulsao{ get; set; }
    public float VelocidadeDeRepulsao{ get; set; }
    public float TempoNoDano { get; set; }
    public bool DanoEmAliado { get; set; }
    public bool BloquearMultHit { get; set; }
    
    
    public NoImpactoDeDano NoImpacto { get; set; }
    
    public Vector3 DirDeREpulsao
    {
        get { return new Vector3(dirDeRepulsao[0],dirDeRepulsao[1],dirDeRepulsao[2]); }
        set { dirDeRepulsao = new float[3] { value.x, value.y, value.z }; }
    }

}

public enum NoImpactoDeDano
{
    impactoPadrao,
    impactoDeAgua,
    impactoDeGosma,
    impactoDeEnergia
}

public enum GolpeId
{
    jab,
    socoDireto,
    socoGiratorio,
    waterMagic
}
