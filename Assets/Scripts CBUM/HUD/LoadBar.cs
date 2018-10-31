using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadBar : MonoBehaviour
{
    [SerializeField]private RectTransform bar;

    private Image img;
    private float posOriginalMaxDaAncora;
    private float posOriginalMinDaAncora;

    //[Range(0,1)]public float teste = 1;
    // Use this for initialization
    void Awake()
    {
        posOriginalMaxDaAncora = bar.anchorMax.x;
        posOriginalMinDaAncora = bar.anchorMin.x;
    }
    void Start()
    {
        img = bar.GetComponent<Image>();
        //GetComponentInChildren<Text>().text = Application.persistentDataPath;
    }

    public void Init()
    {
        //PUN PhotonNetwork.RPC(GetComponent<PhotonView>(), "naoDestrua",PhotonTargets.All,false);
        naoDestrua();
    }

    //[PunRPC]
    void naoDestrua()
    {
        DontDestroyOnLoad(gameObject.transform.parent.gameObject);
    }
    // Update is called once per frame
    public void ValorParaBarra(float x)
    {
        gameObject.SetActive(true);
        PercentagemDeBarraNoY(bar,x);
    }

    void PercentagemDeBarraNoY(RectTransform barra, float percentagem)
    {
        if (img.color.a == 0 && percentagem > 0)
            img.color = new Color(img.color.r,img.color.g,img.color.b,1);

        barra.anchorMax = new Vector2(
            (posOriginalMaxDaAncora - posOriginalMinDaAncora) * percentagem + posOriginalMinDaAncora,
            barra.anchorMax.y
            );
    }

    /* PUN
    void OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo info)
    {
        Vector2 V = bar.anchorMax;
        if (PhotonNetwork.isMasterClient)
        {
            if (stream.isWriting)
            {
                stream.Serialize(ref V);
            }
        }
        else
        {
            if (stream.isReading)
            {
                stream.Serialize(ref V);
                img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
                bar.anchorMax = V;
            }
        }
    }*/
}
