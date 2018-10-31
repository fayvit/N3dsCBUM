using UnityEngine;
using System.Collections;

[System.Serializable]
public class CameraBasica{

    [SerializeField] private Transform alvo;
    [SerializeField] private float altura = 20;
    [SerializeField] private float distanciaHorizontal=20;
    [SerializeField] private float velocidadeDeCamera = 10;
    [SerializeField] private float distanciaAFrenteParaFoco = 2;
    [SerializeField] private bool retornarEmX = true;
    [SerializeField] private float xAlvo = 0;
    [SerializeField] private bool fixarX = false;

    private Transform transform;
    private Vector3 dirAlvo;
    private float velDeLerp = 1;

    public Transform Alvo
    {
        get { return alvo; }
        set { alvo = value; }
    }

    public bool RetornarEmX
    {
        get { return retornarEmX; }
        set { retornarEmX = value; }
    }


    // Use this for initialization
    public void Start (Transform T) {
        transform = T;
        if (!Alvo)
        {
            GameObject doAlvo = GameObject.FindGameObjectWithTag("Player");
            if (doAlvo)
                Alvo = doAlvo.transform;
        }

        dirAlvo = Alvo.position-distanciaHorizontal*Vector3.forward+altura*Vector3.up;
        transform.position = dirAlvo;
            transform.LookAt(Alvo.position+distanciaAFrenteParaFoco*Vector3.forward);
	}
	
	// Update is called once per frame
	public void Update () {
        dirAlvo = Alvo.position-distanciaHorizontal*Vector3.forward+altura*Vector3.up;

        velDeLerp = velocidadeDeCamera*Mathf.Max(1,
            Vector3.Distance(dirAlvo,transform.position)/Mathf.Sqrt(Mathf.Pow(altura,2) + Mathf.Pow(distanciaHorizontal,2)
            ));

        if (!retornarEmX && dirAlvo.x < transform.position.x)
        {
            if (dirAlvo.z > transform.position.z)
            {
                dirAlvo = new Vector3(transform.position.x, dirAlvo.y, transform.position.z);
            }
            else
                dirAlvo = new Vector3(transform.position.x, dirAlvo.y, dirAlvo.z);
        }

        if (fixarX && dirAlvo.x > xAlvo)
        {
            if (dirAlvo.z > transform.position.z)
            {
                dirAlvo = new Vector3(transform.position.x, dirAlvo.y, transform.position.z);
            }
            else
                dirAlvo = new Vector3(transform.position.x, dirAlvo.y, dirAlvo.z);

            retornarEmX = true;
        }
        else if (fixarX && dirAlvo.x <= xAlvo)
        {
            retornarEmX = false;
        }


        transform.position = Vector3.Lerp(transform.position,
            dirAlvo
            , velDeLerp * Time.deltaTime);
        
	}

    public void NovoFoco(Transform alvo,float altura,float distancia)
    {
        this.altura = altura;
        this.distanciaHorizontal = distancia;
        this.Alvo = alvo;

        transform.rotation = Quaternion.LookRotation(this.Alvo.position + distanciaAFrenteParaFoco * Vector3.forward-
            (alvo.position - distanciaHorizontal * Vector3.forward + altura * Vector3.up));
    }

    public void FixarEmX(float xAlvo)
    {
        fixarX = true;
        this.xAlvo = xAlvo;
    }

    public void LiberarX(bool retornarEmX)
    {
        this.retornarEmX = retornarEmX;
        fixarX = false;
    }
}
