using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MagicBase : IGolpeBase {
    
    public float TempoDeAnima { get; set; }
    //public float ExitHitTime { get; set;}
    
    public MagicId ID { get; protected set; }   

}

public enum MagicId
{    
    waterMagic
}
