using UnityEngine;

[System.Serializable]
public class MovimentacaoBasica
{

    [SerializeField] private CaracteristicasDeMovimentacao caracMov;
    [SerializeField] private ElementosDeMovimentacao elementos;

    private Pulo pulo;
    private Pulo pulo2;
    private Transform verificaChao;
    private Vector3 direcaoMovimento = Vector3.zero;
    private bool gravidadeAplicavel = true;
    private float velocidadeDescendo = 0;

    private float targetSpeed = 0;

    public bool GravidadeAplicavel
    {
        get { return gravidadeAplicavel; }

        set { gravidadeAplicavel = value; }
    }

    public CharacterController Controle
    {
        get { return elementos.Controle; }
    }

    public Pulo _Pulo
    {
        get { return pulo; }
    }

    public float VelocidadeAndando
    {
        get { return caracMov.velocidadeAndando; }
        set { caracMov.velocidadeAndando = value; }
    }

    public Animator Animador
    {
        get { return elementos.Animador; }
    }

    public CaracteristicasDeMovimentacao Carac
    {
        get { return caracMov; }
        set { caracMov = value; }
    }


    public MovimentacaoBasica(CaracteristicasDeMovimentacao caracMov, ElementosDeMovimentacao elementos)
    {
        this.caracMov = caracMov;
        this.elementos = elementos;
        pulo = new Pulo(caracMov.caracPulo, elementos);
        pulo2 = new Pulo(caracMov.caracPulo2, elementos);
    }

    /*
    public Vector3 DirecaoAlvoControlada(EstadoDeCamera estadoC)
    {
        float h = Input.GetAxis("joy " + (int)elementos.gerente.controlador + " horizontal");
        float v = Input.GetAxis("joy " + (int)elementos.gerente.controlador + " vertical");

        Vector3 forward = new Vector3(1, 0, 0);
        if (elementos.cam.MinhaCamera)
            forward = elementos.cam.DirecaoInduzida(estadoC, h, v);

        forward.y = 0;
        forward = forward.normalized;

        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        return (h * right + v * forward);
    }
    */

    void InstanciaVerificaChao()
    {
        Transform T = elementos.transform.Find("verificaChao");
        GameObject G;

        if (T == null)
        {
            G = new GameObject
            {
                name = "verificaChao"
            };
        }
        else
            G = T.gameObject;

        G.transform.position = elementos.transform.position+elementos.Controle.center + 
            Mathf.Max(elementos.Controle.radius,elementos.Controle.height * 0.5f)*Vector3.down;
        G.transform.parent = elementos.transform;
        verificaChao = G.transform;
    }

    public bool NoChao()
    {
        //Debug.Log(elementos.Controle.collisionFlags);

        if (verificaChao == null)
            InstanciaVerificaChao();
        //Debug.Log(Physics.OverlapSphere(verificaChao.position, 0.12f, 512).Length + " : " + Controle.collisionFlags);

        if (Physics.OverlapSphere(verificaChao.position, 0.1f+Controle.height*0.01f,512).Length > 0 ||Controle.collisionFlags==CollisionFlags.CollidedBelow)
        {
            if (_Pulo != null && Controle.name == "esperandoTeste")
            {
                // if (Controle.collisionFlags != CollisionFlags.None&&_Pulo.EstouPulando)
                //   Debug.Log(_Pulo.AlcancouTempoMin+" : "+pulo2.AlcancouTempoMin);
                Debug.Log("tempomin1: " + _Pulo.AlcancouTempoMin + " estou subindo 1" + _Pulo.EstouSubindo + " estou pulando 2" + pulo2.EstouPulando
                    + " temppo min 2 " + pulo2.AlcancouTempoMin + " estou pulando 2 " + pulo2.EstouPulando);

                Debug.Log(((!_Pulo.AlcancouTempoMin || _Pulo.EstouSubindo) && !pulo2.EstouPulando) ||
                        !pulo2.AlcancouTempoMin && pulo2.EstouPulando);
            }

                if (_Pulo!=null)
                if (((!_Pulo.AlcancouTempoMin||_Pulo.EstouSubindo)&&!pulo2.EstouPulando) ||
                    !pulo2.AlcancouTempoMin&&pulo2.EstouPulando)
                    return false;

            velocidadeDescendo = 0;
            return true;
        }
        else
            return false;
        /*
        if (Time.timeScale > 0)
            noChao = noChaoS(elementos.controle, distanciaFundamentadora);

        return noChao;*/
    }

    public void AplicadorDeMovimentos(
        Vector3 dir,
        float distanciaFundamentadora = 0.01f,
        Transform T = null,
        System.Action acaoNaMovimentacao = null
        )
    {
        if (elementos.transform == null && T != null)
        {
            elementos.transform = T;
            elementos.Controle = T.GetComponent<CharacterController>();
            elementos.Animador = T.GetComponent<Animator>();
        }
            

        if (NoChao())
        {

            Animador.SetBool("pulo", false);
            gravidadeAplicavel = true;
            Movimentacao(dir);
            if (acaoNaMovimentacao != null)
                acaoNaMovimentacao();
        }
        else if (caracMov.caracPulo.estouPulando)
        {

            if (caracMov.caracPulo2.estouPulando)
                pulo2.VerificaPulo(dir);
            else
                _Pulo.VerificaPulo(dir);
        }
        else if (GravidadeAplicavel)
        {
             AplicaGravidade();
            _Pulo.NaoEstouPulando();
            //_Pulo.VerificaPulo(dir);
            Animador.SetBool("pulo",true);
        }

    }

    public void Movimentacao(Vector3 direcaoAlvo)
    {
        if (pulo == null)
        {
            pulo = new Pulo(caracMov.caracPulo, elementos);
            pulo2 = new Pulo(caracMov.caracPulo2, elementos);
        }

        pulo.NaoEstouPulando();
        pulo2.NaoEstouPulando();
        targetSpeed = Mathf.Min(direcaoAlvo.magnitude, 1.0f);

        
        if (elementos.transform.tag=="Player")
        {
            targetSpeed *= //(Input.GetButton("Run")) ?
                //caracMov.velocidadeCorrendo :
                caracMov.velocidadeAndando;
        }
        else
            targetSpeed *= caracMov.velocidadeAndando;

        if (direcaoAlvo != Vector3.zero)
        {

            if (elementos.Controle.velocity.magnitude < caracMov.velocidadeAndando /*&& VerificaChao.noChao(elementos.controle, elementos.transform)*/)
            {
                direcaoMovimento = direcaoAlvo.normalized;
            }
            else
            {
                direcaoMovimento = Vector3.RotateTowards(direcaoMovimento, direcaoAlvo, 500 * Mathf.Deg2Rad * Time.deltaTime, 1000);

                direcaoMovimento = direcaoMovimento.normalized;
            }
        }
        else
        {
            direcaoMovimento = Vector3.Lerp(direcaoMovimento, Vector3.zero, 1);
        }

        if (direcaoAlvo.magnitude > 0.3f)
            elementos.transform.rotation = Quaternion.LookRotation(new Vector3(direcaoMovimento.x, 0, direcaoMovimento.z));
        
        elementos.Controle.Move((direcaoMovimento * targetSpeed + velocidadeDescendo * Vector3.down) * Time.deltaTime);
        elementos.Animador.SetFloat("velocidade", targetSpeed);

    }

    
    public void AplicaGravidade()
    {
        AplicaGravidade(9.8f, 5);
    }
    
    public void AplicaGravidade(float velMax, float amortecimento)
    {
        velocidadeDescendo = Mathf.Lerp(velocidadeDescendo, velMax, amortecimento * Time.deltaTime);
        elementos.Controle.Move((direcaoMovimento * targetSpeed + velocidadeDescendo * Vector3.down) * Time.deltaTime);
    }
    
    public void VerificaComandoDePulo()
    {

        if (!_Pulo.EstouPulando && NoChao())
        {
            pulo.IniciaAplicaPulo(elementos.transform.position.y);
        }
        else if (!NoChao() && elementos.Controle.velocity.y>0 && !pulo2.EstouPulando)
        {
            Debug.Log("inicia pulo 2: "+_Pulo.UltimoYFundamentado+" : "+elementos.Controle.transform.position.y);
            pulo2.IniciaAplicaPulo(_Pulo.UltimoYFundamentado);
        }
    }

    /*
    public void AplicaPulo(Vector3 direcaoAlvo)
    {
        pulo.VerificaPulo(direcaoAlvo);
    }*/

    
}

[System.Serializable]
public class CaracteristicasDeMovimentacao : System.ICloneable
{
    public float velocidadeAndando = 6;
    public float velocidadeCorrendo = 12;
    public CaracteristicasDePulo caracPulo = new CaracteristicasDePulo();
    public CaracteristicasDePulo caracPulo2 = new CaracteristicasDePulo();

    public object Clone()
    {
        return new CaracteristicasDeMovimentacao()
        {
            velocidadeAndando = this.velocidadeAndando,
            velocidadeCorrendo = this.velocidadeCorrendo,
            caracPulo = this.caracPulo,
            caracPulo2 = this.caracPulo2
        };
    }
}

[System.Serializable]
public class CaracteristicasDePulo
{
    public float alturaDoPulo = 2;
    public float tempoMaxPulo = 1;
    public float velocidadeSubindo = 7;
    public float velocidadeDescendo = 10f;
    public float velocidadeDuranteOPulo = 6;
    public float amortecimentoNaTransicaoDePulo = 0.9f;
    public float impulsoInicial = 0f;
    public bool estouPulando = false;
    public bool estavaPulando = false;
}

[System.Serializable]
public struct ElementosDeMovimentacao
{
    private CharacterController controle;
    public Transform transform;
    private Animator animador;

    public CharacterController Controle
    {
        get {
            if (controle == null && transform != null)
                controle = transform.GetComponent<CharacterController>();
            return controle;
                 }
        set { controle = value; }
    }

    public Animator Animador
    {
        get {
            if (animador == null && transform != null)
                animador = transform.GetComponent<Animator>();
            return animador;
        }
        set { animador = value; }
    }

    public ElementosDeMovimentacao(Transform transform)
    {        
        this.transform = transform;
        this.controle = transform.GetComponent<CharacterController>();
        this.animador = transform.GetComponent<Animator>();
        
    }
}
