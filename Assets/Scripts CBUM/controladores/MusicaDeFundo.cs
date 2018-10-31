using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class MusicaDeFundo
{
    [SerializeField] private DictionaryMusic[] dMusic;
    [SerializeField] private AudioSource[] audios;

    private int inicia = -1;
    private int termina = -1;

    //private string cenaIniciada = "";
    private bool parando;

    private float velocidadeDeFadeIn = 1;
    private float velocidadeDeFadeOut = 2;
    private float volumeAlvo = 0.25f;
    private float volumeAlvoAnterior = 0.25f;

    public void IniciarMusica(AudioClip esseClip)
    {
        parando = false;
        AudioSource au = audios[0];

        if (au.isPlaying)
        {
            termina = 0;
            inicia = 1;
        }
        else
        {
            termina = 1;
            inicia = 0;
        }

        if (audios[termina].clip == esseClip)
        {
            int temp = inicia;
            inicia = termina;
            termina = temp;
        }
        else
        {
            audios[inicia].volume = 0;
            audios[inicia].clip = esseClip;
            audios[inicia].Play();
        }
        
    }

    public void PararMusicas(float velocidade = 1)
    {
        inicia = 0;termina = 1;
        velocidadeDeFadeIn = velocidade;
        parando = true;
    }

    public void ReiniciarMusicas(bool doZero = false)
    {
        parando = false;

        if (doZero)
        {
            audios[inicia].Stop();
            audios[inicia].Play();
        }
    }

    public void Update()
    {
        if (audios.Length>0)
        {
            if (!parando)
            {
                if (inicia != -1 && termina != -1)
                {
                    if (audios[inicia].volume < 0.9f)
                        audios[inicia].volume = Mathf.Lerp(audios[inicia].volume, volumeAlvo, Time.deltaTime * velocidadeDeFadeIn);
                    else
                        audios[inicia].volume = volumeAlvo;

                    if (audios[termina].volume < .2f*volumeAlvoAnterior)
                    {
                        audios[termina].Stop();
                    }
                    else
                        audios[termina].volume = Mathf.Lerp(audios[termina].volume, 0, Time.deltaTime *velocidadeDeFadeOut);

                }
                VerificaCena();
            }
            else
            {
                audios[termina].volume = Mathf.Lerp(audios[termina].volume, 0, Time.fixedDeltaTime* velocidadeDeFadeOut);
                audios[inicia].volume = Mathf.Lerp(audios[inicia].volume, 0, Time.fixedDeltaTime * velocidadeDeFadeOut);
            }

            
        }
    }

    public virtual void MudaPara(IdMusic clip,float velocidadeDeFadeIn = 0.25f,float velocidadeDeFadeOut = 2)
    {
        this.velocidadeDeFadeIn = velocidadeDeFadeIn;
        this.velocidadeDeFadeOut = velocidadeDeFadeOut;
        DictionaryMusic d = BusqueMusica(clip);

        IniciarMusica(d.clip);
        volumeAlvoAnterior = volumeAlvo;
        volumeAlvo = d.volume;

      //  cenaIniciada = SceneManager.GetActiveScene().name;
    }

    public DictionaryMusic BusqueMusica(IdMusic id)
    {
        List<DictionaryMusic> retorno = new List<DictionaryMusic>();
        for (int i = 0; i < dMusic.Length; i++)
        {
            if (dMusic[i].idMusic == id)
                retorno.Add(dMusic[i]);
        }

        if (retorno.Count > 1)
            Debug.Log("Varias entradas com a mesma ID");
        else if(retorno.Count==0)
            Debug.Log("Nenhuma entradas com a mesma ID");

        return retorno[0];
    }

    public virtual void VerificaCena()
    {
        /*
        if(cenaIniciada!= SceneManager.GetActiveScene().name)
            switch (SceneManager.GetActiveScene().name)
            {
                case "titulo":
                case "novoTitulo":
                case "PreJogo":
                    MudaPara(intro);
                break;
                case "inicial":
                    MudaPara(jogo);
                break;
                case "contadoraDePontos":
                case "contadoraDePontos_plus":
                case "nossosPatrocinadores":
                    MudaPara(pontos);
                break;
                case "equipamentos":
                case "equipamentos_plus":
                    MudaPara(equips);
                break;
            }
            */
    }
}

[System.Serializable]
public class DictionaryMusic
{
    public IdMusic idMusic;
    [Range(0, 1)] public float volume = 1;    
    public AudioClip clip;
    

}

public enum IdMusic
{
    faseInicial,
    deConversa,
    bossinicial
}

