using UnityEngine;
using System.Collections.Generic;

public class ListaDeTextosPT_BR
{
    static Dictionary<ChaveDeTexto, List<string>> txt = new Dictionary<ChaveDeTexto, List<string>> {
        { ChaveDeTexto.bomDia,new List<string>{
            "Bom dia pra você",
            "Bom dia pra você",
            "Bom dia.....",
            "Bom dia pra você"
        } },
        {
            ChaveDeTexto.ContinuarJogo,new List<string>()
            {
                "Continuar em frente"
            }
        },
        {
            ChaveDeTexto.falaComNpcCaido,new List<string>()
            {
                "Você veio para nos ajudar? \r\n Muito obrigado!!",
                "Eles vieram do sul para nos atacar. É tudo tão terrível!!",
                ""
            }
        },
        {
            ChaveDeTexto.faleMaisNPC,new List<string>()
            {
                "Houve uma eleição para o primeiro ministro do reino.",
                "Um dos candidatos dizia que a culpa pela crise no reino era dos habitantes do norte",
                "Sua campanha foi impulsionada por frases de efeito como:",
                "Um habitante do norte bom é um habitante do norte morto",
                "Tudo acabou resultando no que vemos agora"
            }
        },
        {
            ChaveDeTexto.opcaoDoNpcCaido,new List<string>()
            {
                "Fale mais sobre o ataque"
            }
        }
    };
    public static Dictionary<ChaveDeTexto, List<string>> Txt
    {
        get {
            if (txt == null)
            {
                txt = new Dictionary<ChaveDeTexto, List<string>>();

                /*
                ColocaTextos(ref txt, TextosChaveEmPT_BR.txt);
                ColocaTextos(ref txt, TextosDeBarreirasPT_BR.txt);
                ColocaTextos(ref txt, TextosDaCavernaInicialPT_BR.txt);
                ColocaTextos(ref txt, TextosDeKatidsPT_BR.txt);
                ColocaTextos(ref txt, TextosDeMarjanPT_BR.txt);
                ColocaTextos(ref txt, TextosDeInfoPT_BR.txt);
                */
            }

            return txt;
        }
    }

    static void ColocaTextos(ref Dictionary<ChaveDeTexto, List<string>>  retorno, Dictionary<ChaveDeTexto, List<string>> inserir)
    {
        foreach (ChaveDeTexto k in inserir.Keys)
        {
            retorno[k] = inserir[k];
        }
    }
    
}