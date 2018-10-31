using UnityEngine;
using System.Collections;

public class EstouEmDano
{

    private float tempoDeDano = 0;
    private float alturaAtual;
    private float alturaDoDano;
    private Vector3 direcao = Vector3.zero;
    private Vector3 vMove = Vector3.zero;
    private Vector3 posInicial;


    private CharacterController controle;

    public IGolpeBase esseGolpe;
    public Animator animator;

    //public Vector3 direcaoDoDano = Vector3.back;

    // Use this for initialization
    public EstouEmDano(CharacterController controle)
    {
        this.controle = controle;
        animator = controle.GetComponent<Animator>();
    }

    public void Start(Vector3 posInicial,float alturaDoDano,IGolpeBase golpe)
    {
        esseGolpe = golpe;
        tempoDeDano = 0;
        this.posInicial = posInicial;
        this.alturaDoDano = alturaDoDano;//        alturaDoDano = transform.position.y;

        /*
        if (esseGolpe.Derruba)
        {
            animator.Play("danoDeQueda");
        }
        */
    }



    // Update is called once per frame
    public bool Update(Transform transform)
    {
        tempoDeDano += Time.deltaTime;

        alturaAtual = transform.position.y;
        direcao = Vector3.zero;
        if (alturaAtual < alturaDoDano + 0.5f)
        {
            direcao += 12 * Vector3.up;
        }
        if ((transform.position - posInicial).sqrMagnitude < esseGolpe.DistanciaDeRepulsao)
            direcao += esseGolpe.VelocidadeDeRepulsao * esseGolpe.DirDeREpulsao;//direcaoDoDano;

        vMove = Vector3.Lerp(vMove, direcao, 10 * Time.deltaTime);
        controle.Move(vMove * Time.deltaTime);

        //Debug.Log(tempoDeDano+" : "+esseGolpe.TempoNoDano);
        if (tempoDeDano > esseGolpe.TempoNoDano)
        {
            return false;
            //gerente.LiberaMovimento(CreatureManager.CreatureState.emDano);
            //animator.SetBool("dano1", false);
        }

        return true;
    }


}
