using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AplicadorDeCamera : MonoBehaviour {

    [SerializeField] private CameraBasica camB;
    [SerializeField] private EstadoComplementarDaCamera estadoC = EstadoComplementarDaCamera.estavel;
    [SerializeField] private float tempoDeShake = 0.1f;
    [SerializeField] private float shakeAngle = 1;
    private float tempoDecorrido = 0;
    private int contShake = 0;
    private int totalShake = 5;
    private bool sinal = false;


    public static AplicadorDeCamera c;

    public enum EstadoComplementarDaCamera
    {
        shake,
        estabilizando,
        estavel
    }
    public CameraBasica CamB
    {
        get { return camB; }
    }

    // Use this for initialization
    void Start () {
        c = this;
        CamB.Start(transform);
        EventAgregator.AddListener(EventKey.localDamageColliderContact,ShakeCam);
	}
	
	// Update is called once per frame
	void Update () {
        CamB.Update();

        switch (estadoC)
        {
            case EstadoComplementarDaCamera.shake:
                tempoDecorrido += Time.deltaTime;
                if (contShake < totalShake)
                {
                    transform.localEulerAngles = new Vector3(
                        transform.localEulerAngles.x,
                        transform.localEulerAngles.y,
                        Mathf.Lerp(transform.localEulerAngles.y, (sinal ? 1 : -1) * shakeAngle, tempoDecorrido / tempoDeShake)
                        );

                    if (tempoDecorrido > tempoDeShake)
                    {
                        tempoDecorrido = 0;
                        contShake++;
                        sinal = !sinal;
                    }
                }
                else
                {
                    estadoC = EstadoComplementarDaCamera.estabilizando;
                    tempoDecorrido = 0;
                }
            break;
            case EstadoComplementarDaCamera.estabilizando:
                tempoDecorrido += Time.deltaTime;
                if (tempoDecorrido <= tempoDeShake)
                {
                    transform.localEulerAngles = new Vector3(
                        transform.localEulerAngles.x,
                        transform.localEulerAngles.y,
                        Mathf.Lerp(transform.localEulerAngles.y, 0, tempoDecorrido / tempoDeShake)
                        );
                }
                else
                {
                    estadoC = EstadoComplementarDaCamera.estavel;
                    transform.localEulerAngles = new Vector3(
                        transform.localEulerAngles.x,
                        transform.localEulerAngles.y,
                        0
                        );
                }
            break;
        }
	}

    public void ShakeCam(IGameEvent e)
    {
        ShakeCam();
    }
    public void ShakeCam(int totalShake = 5,float shakeAngle = 1)
    {
        this.totalShake = totalShake;
        this.shakeAngle = shakeAngle;
        tempoDecorrido = 0;
        contShake = 0;
        estadoC = EstadoComplementarDaCamera.shake;
    }
}
