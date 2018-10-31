using UnityEngine;

[System.Serializable]
public abstract class GolpeRefinadoBase : IGolpeBase
{
    [System.NonSerialized] protected GameObject dono;

    public float TempoDecorrido { get; set; }
    public float TempoEmAnima { get; set; }
    public float TempoDeMoveMin { get; set; }
    public string NomeAnima { get; set; }

    public abstract void StartAttack(GameObject G, Transform alvo);

    public abstract bool UpdateAttack();

    public virtual void ColocaMeuHitCollider(GameObject dono)
    {
        Debug.Log("Deve ser sobrescrito pela classe herdeira");
    }

}