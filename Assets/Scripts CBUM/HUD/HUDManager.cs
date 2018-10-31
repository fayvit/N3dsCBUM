using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

    [SerializeField] private HUDCharacter[] hudC;
    [SerializeField] private Text numMortes;

    public static HUDManager h;

	// Use this for initialization
	void Start () {
        HUDManager[] hh = FindObjectsOfType<HUDManager>();

        //Debug.Log("huds?? : " + hh.Length);
        if (hh.Length > 1)
            Destroy(gameObject);
        else
            h = this;

        EventAgregator.AddListener(EventKey.heroDamage, AtualizaHudEmRede);
        EventAgregator.AddListener(EventKey.heroHealth, AtualizaHudEmRede);
        EventAgregator.AddListener(EventKey.novoHeroiSpawnado, OnNewHeroSpawned);

        for (int i = 0; i < hudC.Length; i++)
        {
            hudC[i].lifeBar.transform.parent.gameObject.SetActive(false);
            numMortes.transform.parent.gameObject.SetActive(false);
        }

	}

    private void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.heroDamage, AtualizaHudEmRede);
        EventAgregator.RemoveListener(EventKey.heroHealth, AtualizaHudEmRede);
        EventAgregator.RemoveListener(EventKey.novoHeroiSpawnado, OnNewHeroSpawned);
    }

    // Update is called once per frame
    void Update () {
		
	}

    void OnNewHeroSpawned(IGameEvent e)
    {
        DadosDoPersonagem D;

        List<SloteMultiplayer> l = GlobalController.g.Jogadores;
        for (int i = 0; i < l.Count; i++)
        {
            if (l[i].Estado == SloteMultiplayer.EstadoDoSlot.ocupadoLocal || l[i].Estado == SloteMultiplayer.EstadoDoSlot.ocupadoRede)
            {
                D = e.Sender.GetComponent<CharacterBase>().Dados;
                MostrarHud(i);
                AtualizaLifeBar(i, (float)D.PontosDeVida / D.MaxVida);
                AtualizaMagicBar(i, (float)D.PontosDeMana / D.MaxMana);
            }   
        }
    }

    public void MostrarHud(int numJogador)
    {
        if(hudC[numJogador].lifeBar!=null)
            hudC[numJogador].lifeBar.transform.parent.gameObject.SetActive(true);
    }

    public void AtualizaLifeBar(int numJogador,float quantidade)
    {
        if (hudC[numJogador].lifeBar != null)
            hudC[numJogador].lifeBar.fillAmount = quantidade;
    }

    public void AtualizaMagicBar(int numJogador, float quantidade)
    {
        if (hudC[numJogador].lifeBar != null)
            hudC[numJogador].magicBar.fillAmount = quantidade;
    }

    public void AtualizaHudEmRede(IGameEvent e)
    {
        CharacterDamageEvent cde = (CharacterDamageEvent)e;
        DadosDoPersonagem Dados = cde.ManagerSender.Dados;
        /*PUN
        RPC_Listener.RPC(
            NameOfRPC.ModificaHudLife,
            PhotonTargets.All,
            GlobalController.g.IndiceEntreJogadores(cde.ManagerSender.MyView.viewID),
            (float)Dados.PontosDeVida / Dados.MaxVida,
            (float)Dados.PontosDeMana / Dados.MaxMana
            );*/

        RPC_Listener.ModificaHudLife(0, (float)Dados.PontosDeVida / Dados.MaxVida,
            (float)Dados.PontosDeMana / Dados.MaxMana);
    }

    public void AtualizaNumeroDeMortes()
    {
        if (numMortes.transform.parent.gameObject.activeSelf)
        {
            numMortes.text = (int.Parse(numMortes.text) + 1).ToString();
        }
        else
        {
            numMortes.transform.parent.gameObject.SetActive(true);
            numMortes.text = "1";
        }
    }
}

[System.Serializable]
public class HUDCharacter
{
    [SerializeField] public Image lifeBar;
    [SerializeField] public Image magicBar;
    [SerializeField] public Image faceImage;
}
