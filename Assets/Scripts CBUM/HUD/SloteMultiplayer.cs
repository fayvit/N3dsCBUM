using UnityEngine;
[System.Serializable]
public struct SloteMultiplayer
{
    private EstadoDoSlot estado;
    private Controlador control;
    //PUN private int viewOwner;
    private int viewID;    
    private string nomeNoJogo;

    public enum EstadoDoSlot
    {
        abertoParaLocal,
        abertoParaRede,
        ocupadoLocal,
        ocupadoRede,
        fechado,
        desconexaoAgendada
    }

    public int ViewID
    {
        get { return viewID; }
        set { viewID = value; }
    }

    public string NomeNoJogo
    {
        get { return nomeNoJogo; }
        set { nomeNoJogo = value; }
    }

    public EstadoDoSlot Estado
    {
        get { return estado; }
        set { estado = value; }
    }

    public Controlador Control
    {
        get { return control; }
        set { control = value; }
    }

    /* PUN
    public PhotonPlayer ViewOwner
    {
        get { return PhotonPlayer.Find(viewOwner); }
        set { viewOwner = value.ID; }
    }*/


    /// <summary>
    /// essa propriedade chama Photonview.Find com o viewID guardado
    /// </summary>
    public CharacterManager Manager
    {
        get {
            //PUN  PhotonView p = PhotonView.Find(viewID);
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null)
                return p.GetComponent<CharacterManager>();
            else return null;
            }
    }

    public static int PrimeiroDoEstado(SloteMultiplayer[] s, EstadoDoSlot estadoProcurado)
    {
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i].estado == estadoProcurado)
                return i;
        }

        return -1;
    }

    public static bool NaoControloNinguem(SloteMultiplayer[] s, int controlador)
    {
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i].estado == EstadoDoSlot.ocupadoLocal && (int)s[i].control == controlador)
                return false;
        }

        return true;
    }
}
