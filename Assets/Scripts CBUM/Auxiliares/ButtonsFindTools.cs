using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsFindTools
{
    private static int indiceDaOpcaoEscolhida = 0;
    private static Button[] btns;

    struct IndexadorDeButtons
    {
        public Button b;
        public int indexVal;
    }

    public static void BusqueComUltimoDestacado()
    {

        BuscarBotoes();

        Color C;
        ColorUtility.TryParseHtmlString("#B61B1BFF", out C);
        
        btns[btns.Length - 1].GetComponent<Image>().color = C;

        indiceDaOpcaoEscolhida = btns.Length - 1;
    }

    public static void ModificaOpcao()
    {
        int quanto = 0;
        if (GerenciadorDasOpcoesIniciais.g.MeuControlador == Controlador.teclado)
            quanto = CommandReader.ValorDeGatilhos("vertical", -1);
        else
            quanto = CommandReader.ValorDeGatilhos("VDpad", (int)GerenciadorDasOpcoesIniciais.g.MeuControlador);

        if (indiceDaOpcaoEscolhida + quanto < btns.Length && indiceDaOpcaoEscolhida + quanto > -1)
            indiceDaOpcaoEscolhida += quanto;
        else if (indiceDaOpcaoEscolhida + quanto >= btns.Length)
            indiceDaOpcaoEscolhida = 0;
        else
            indiceDaOpcaoEscolhida = btns.Length - 1;

        for (int i = 0; i < btns.Length; i++)
        {

            Color C;
            if (btns[i] != null)
            {
                if (i == indiceDaOpcaoEscolhida)
                {
                    ColorUtility.TryParseHtmlString("#B61B1BFF", out C);
                    btns[i].GetComponent<Image>().color = C;
                }
                else
                {
                    ColorUtility.TryParseHtmlString("#1B4AB6FF", out C);
                    btns[i].GetComponent<Image>().color = C;
                }
            }
            else
                BuscarBotoes();
        }

        if (quanto != 0)
            EventAgregator.Publish(EventKey.UiDeOpcoesChange, null);
    }

    public static void ActiveSelfButtons(bool ligue)
    {
        BuscarBotoes();
        foreach (Button b in btns)
            b.interactable = ligue;
    }

    public static Button[] BuscarBotoes()
    {
        btns = MonoBehaviour.FindObjectsOfType<Button>();
        return ReorganizarBtns();
    }

    static Button[] ReorganizarBtns()
    {
        Transform T;
        List<IndexadorDeButtons> list = new List<IndexadorDeButtons>();
        IndexadorDeButtons esse;

        int cont;

        for (int i = 0; i < btns.Length; i++)
        {
            cont = 0;
            esse = new IndexadorDeButtons() { b = btns[i], indexVal = 0 };
            T = btns[i].transform;
            while (T != null)
            {
                esse.indexVal += (int)(T.GetSiblingIndex() * Mathf.Pow(10, cont));
                cont++;
                T = T.parent;
            }

            list.Add(esse);
        }

        for (int j = 0; j < list.Count; j++)
        {
            for (int i = 0; i < j; i++)
            {
                if (list[i].indexVal < list[j].indexVal)
                {
                    esse = list[i];
                    list[i] = list[j];
                    list[j] = esse;
                }
            }
        }

        for (int i = 0; i < btns.Length; i++)
            btns[i] = list[i].b;

        return btns;
    }

    public static void ClickNoDestacado()
    {
        btns[indiceDaOpcaoEscolhida].onClick.Invoke();
    }
}
