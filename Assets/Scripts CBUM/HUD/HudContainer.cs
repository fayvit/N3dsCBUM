using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HudContainer
{
    [SerializeField] private MenuBasico menu_Basico;
    [SerializeField] private DisparaTexto disparaT;

    public DisparaTexto DisparaT
    {
        get { return disparaT; }
        private set { disparaT = value; }
    }

    public MenuBasico Menu_Basico
    {
        get { return menu_Basico; }
        private set { menu_Basico = value; }
    }
}