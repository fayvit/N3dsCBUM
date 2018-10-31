using UnityEngine;
using System.Collections;

[System.Serializable]
public class ElementosDoJogo 
{
    [SerializeField] private Sprite uiDestaque;
    [SerializeField] private Sprite uiDefault;

    public Sprite UiDestaque
    {
        get { return uiDestaque; }
    }

    public Sprite UiDefault
    {
        get { return uiDefault; }
    }
    
}
