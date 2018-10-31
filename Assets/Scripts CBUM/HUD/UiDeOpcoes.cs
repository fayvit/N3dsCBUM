using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class UiDeOpcoes
{
    [SerializeField] protected GameObject itemDoContainer;
    [SerializeField] protected RectTransform painelDeTamanhoVariavel;
    [SerializeField] protected ScrollRect sr;

    public abstract void SetarComponenteAdaptavel(GameObject G, int indice);
    protected abstract void FinalizarEspecifico();

    private int opcaoEscolhida = 0;
    private float contadorDeTempo = 0;
    private const float TEMPO_DE_SCROLL = .15F;

    public int OpcaoEscolhida
    {
        get { return opcaoEscolhida; }
        private set { opcaoEscolhida = value; }
    }

    public bool EstaAtivo
    {
        get { return painelDeTamanhoVariavel.parent.parent.gameObject.activeSelf; }
    }

    protected void IniciarHUD(int quantidade,TipoDeRedimensionamento tipo = TipoDeRedimensionamento.vertical)
    {
        OpcaoEscolhida = 0;
        painelDeTamanhoVariavel.parent.parent.gameObject.SetActive(true);

        itemDoContainer.SetActive(true);

        if(tipo==TipoDeRedimensionamento.vertical)
            RedimensionarUI.NaVertical(painelDeTamanhoVariavel, itemDoContainer, quantidade);
        else if(tipo==TipoDeRedimensionamento.emGrade)
            RedimensionarUI.EmGrade(painelDeTamanhoVariavel, itemDoContainer, quantidade);
        else if(tipo==TipoDeRedimensionamento.horizontal)
            RedimensionarUI.NaHorizontal(painelDeTamanhoVariavel, itemDoContainer, quantidade);

        for (int i = 0; i < quantidade; i++)
        {
            GameObject G = ParentearNaHUD.Parentear(itemDoContainer, painelDeTamanhoVariavel);
            SetarComponenteAdaptavel(G,i);

            G.name += i.ToString();

            if (i == OpcaoEscolhida)
            {
                
                //if (GameController.g != null)
                  //  G.GetComponent<UmaOpcao>().SpriteDoItem.sprite = GameController.g.El.UiDestaque;
                //else
                {
                    Color C;
                    ColorUtility.TryParseHtmlString("#B61B1BFF", out C);
                    G.GetComponent<UmaOpcao>().SpriteDoItem.color = C;
                }
                        

            }
        }

        itemDoContainer.SetActive(false);

        if (sr != null)
            if (sr.verticalScrollbar)
                sr.verticalScrollbar.value = 1;

        //AgendaScrollPos();
        
    }

    
    void AgendaScrollPos()
    {
        if (GameController.g)
            GameController.g.StartCoroutine(ScrollPos());
        
    }
    

    public void MudarOpcaoComVal(int quanto,int rowCellCount = -1)
    {
        if (quanto != 0)
        {
            UmaOpcao[] umaS = painelDeTamanhoVariavel.GetComponentsInChildren<UmaOpcao>();

            if (quanto > 0)
            {
                if (OpcaoEscolhida + quanto < umaS.Length)
                    OpcaoEscolhida += quanto;
                else
                    OpcaoEscolhida = 0;
            }
            else if (quanto < 0)
            {
                if (OpcaoEscolhida + quanto >= 0)
                    OpcaoEscolhida += quanto;
                else
                    OpcaoEscolhida = umaS.Length - 1;
            }

            for (int i = 0; i < umaS.Length; i++)
            {
                if (i == OpcaoEscolhida)
                {
                    Color C;
                    ColorUtility.TryParseHtmlString("#B61B1BFF", out C);
                    umaS[i].GetComponent<UmaOpcao>().SpriteDoItem.color = C;
                }
                else
                {
                    Color C;
                    ColorUtility.TryParseHtmlString("#1B4AB6FF", out C);
                    umaS[i].GetComponent<UmaOpcao>().SpriteDoItem.color = C;
                }

                   
            }

            if (sr != null)
                if (sr.verticalScrollbar || sr.horizontalScrollbar)
                {
                    
                    AjeitaScroll(umaS,rowCellCount);
                }
                else
                {
                    Debug.Log("erro scroll 2");
                }

            else
                Debug.Log("erro no scrool");

            EventAgregator.Publish(EventKey.UiDeOpcoesChange,null);
        }
    }

    public virtual void MudarOpcao()
    {

        int quanto = -CommandReader.ValorDeGatilhos("VDpad", 1);

        if (quanto == 0)
            quanto = -CommandReader.ValorDeGatilhos("vertical", -1);

        
        MudarOpcaoComVal(quanto);
    }

    public virtual void MudarOpcao_H()
    {

        int quanto = CommandReader.ValorDeGatilhos("HDpad", 1);

        if (quanto == 0)
            quanto = -CommandReader.ValorDeGatilhos("horizontal", -1);


        MudarOpcaoComVal(quanto);
    }

    void AjeitaScroll(UmaOpcao[] umaS,int rowCellCount)
    {
        contadorDeTempo = 0;
        if(GameController.g)
            GameController.g.StartCoroutine(MovendoScroll(umaS,rowCellCount));
        
    }

    protected virtual IEnumerator MovendoScroll(UmaOpcao[] umaS, int rowCellCount)
    {
        yield return new WaitForSecondsRealtime(0.01f);
        int val = (rowCellCount==-1)?umaS.Length: Mathf.CeilToInt((float)umaS.Length / rowCellCount);
        int opc = OpcaoEscolhida /( (rowCellCount==-1)?1:rowCellCount);
        
        contadorDeTempo += 0.01f;
        float destiny = Mathf.Clamp((float)(val - opc-1) / Mathf.Max(val-1, 1), 0, 1);
        
        sr.verticalScrollbar.value = Mathf.Lerp(sr.verticalScrollbar.value,
            destiny, contadorDeTempo / TEMPO_DE_SCROLL);
        
        if (sr.verticalScrollbar.value != destiny)  
            if (GameController.g)
            {
                GameController.g.StartCoroutine(MovendoScroll(umaS, rowCellCount));
            }
        
        GameController.g.StartCoroutine(MovendoScroll(umaS, rowCellCount));
    }

    protected virtual IEnumerator MovendoScroll_H(UmaOpcao[] umaS, int rowCellCount)
    {
        yield return new WaitForSecondsRealtime(0.01f);
        int val = (rowCellCount == -1) ? umaS.Length : Mathf.CeilToInt((float)umaS.Length / rowCellCount);
        int opc = OpcaoEscolhida / ((rowCellCount == -1) ? 1 : rowCellCount);

        contadorDeTempo += 0.01f;
        float destiny = 1-Mathf.Clamp((float)(val - opc - 1) / Mathf.Max(val - 1, 1), 0, 1);
        
        sr.horizontalScrollbar.value = Mathf.Lerp(sr.horizontalScrollbar.value,
            destiny, contadorDeTempo / TEMPO_DE_SCROLL);

        
        if (sr.horizontalScrollbar.value != destiny)
            if (GameController.g)
            {
                GameController.g.StartCoroutine(MovendoScroll_H(umaS, rowCellCount));
            }
        
        GameController.g.StartCoroutine(MovendoScroll(umaS, rowCellCount));
    }



    IEnumerator ScrollPos()
    {
        yield return new WaitForSecondsRealtime(0.01f);

        if (sr != null)
            if (sr.verticalScrollbar)
            {
                sr.verticalScrollbar.value = 1;
            }


        if (sr != null)
            if (sr.horizontalScrollbar)
                sr.horizontalScrollbar.value = 0;

        yield return new WaitForEndOfFrame();

        if (sr != null)
            if (sr.verticalScrollbar)
            {
                
                if (sr.verticalScrollbar.value != 1)
                    AgendaScrollPos();
                    
            }

        
        if (sr != null)
            if (sr.horizontalScrollbar)
                if (sr.horizontalScrollbar.value != 0)
                    AgendaScrollPos();
                    

    }

    public void FinalizarHud()
    {
        for (int i = 1; i < painelDeTamanhoVariavel.transform.childCount; i++)
        {
            MonoBehaviour.Destroy(painelDeTamanhoVariavel.GetChild(i).gameObject);
            painelDeTamanhoVariavel.parent.parent.gameObject.SetActive(false);
        }

        FinalizarEspecifico();
    }

    
}
