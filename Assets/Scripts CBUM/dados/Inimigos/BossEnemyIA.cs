using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyIA : EnemyIaRefinedAttackBase {
    [SerializeField] private int zMax = 52;
    [SerializeField] private int zMin = 38;

    private int cont = 0;
    private int numRoll = 2;
    private Vector3 posAlvo;
    private RollManager roll;
    private PositionsFocus posF = new PositionsFocus();
    private BossEnemyState state = BossEnemyState.standardUpdate;
    private DadosDoPersonagem dados;
    private GolpeEspecialDoFelixcat golpeEx = new GolpeEspecialDoFelixcat();

    private struct PositionsFocus
    {
        public Vector3 MinzMinx;
        public Vector3 MinzMaxx;
        public Vector3 MaxzMinx;
        public Vector3 MaxzMaxx;
    }

    private enum BossEnemyState
    {
        rollPosition,
        specialPosition,
        standardUpdate,
        posicaoDoImpactoVeloz,
        iniciandoGolpeex,
        atualizarGolpeEx
    }

    public BossEnemyIA(GameObject G, CaracteristicasDeMovimentacao carac,DadosDoPersonagem dados) : base(G, carac)
    {
        this.dados = dados;
        roll = new RollManager(move.Mov.Animador, move.Mov.Controle, 2, 1);
    }

    public void StartRoll(GameObject pv, string dir)
    {
        roll.EventInitRoll(pv, dir);
    }
    /*PUN
    public void StartRoll(PhotonView pv,string dir)
    {
        roll.EventInitRoll(pv, dir);
    }*/

    protected override void EstadoDeSondando()
    {
        if (move.UpdatePosition() && Vector3.Distance(alvo.position, dono.transform.position) < 15)
        {
            if (Vector3.Distance(alvo.position, dono.transform.position) < 6)
            {
                SorteiaAttack();
                estado = EstadoDaIa.posicionandoAtaque;
            }
        }
    }

    protected override void PosicionandoAtaque()
    {
        switch (attack)
        {
            case 0:

                state = BossEnemyState.rollPosition;
                InitRoll();
                cont = 1;

            break;
            case 1:

                state = BossEnemyState.specialPosition;
                InitRollSpecialPosition();

            break;
            case 2:
                refAttack.StartAttack(new GarraImediata(), alvo);
                state = BossEnemyState.standardUpdate;
                estado = EstadoDaIa.atacando;
            break;
        }
    }

    public override void Update()
    {
        if (golpeEx.VerificaGolpeEx(dados))
        {
            golpeEx.StartAttack(dono,null);
            InitRollSpecialPosition();
            state = BossEnemyState.iniciandoGolpeex;
            
        }
        switch (state)
        {
            case BossEnemyState.standardUpdate:
                base.Update();
            break;
            case BossEnemyState.rollPosition:

                if (roll.UpdateRoll())
                {
                    dono.tag = "Enemy";
                    if (cont < numRoll && Vector3.Distance(dono.transform.position, posAlvo) < 1)
                    {
                        cont++;
                        InitRoll();
                    }
                    else if (cont >= numRoll && Vector3.Distance(dono.transform.position, posAlvo) < 1)
                    {
                        state = BossEnemyState.standardUpdate;
                        estado = EstadoDaIa.emEspera;
                        move.Mov.AplicadorDeMovimentos(Vector3.zero);
                    }
                    else if (Vector3.Distance(dono.transform.position, posAlvo) >= 1)
                    {
                        move.Mov.AplicadorDeMovimentos((posAlvo - dono.transform.position).normalized);
                    }
                }
            break;
            case BossEnemyState.specialPosition:
                if (roll.UpdateRoll())
                {
                    dono.tag = "Enemy";

                    if (Vector3.Distance(dono.transform.position, posAlvo) >= 1)
                    {
                        move.Mov.AplicadorDeMovimentos((posAlvo - dono.transform.position).normalized);
                    }
                    else
                    {
                        state = BossEnemyState.posicaoDoImpactoVeloz;
                    }
                }
            break;
            case BossEnemyState.posicaoDoImpactoVeloz:
                if (Mathf.Abs(dono.transform.position.z-alvo.position.z)>=0.25f)
                {
                    move.Mov.AplicadorDeMovimentos(new Vector3(0,0, -dono.transform.position.z + alvo.position.z));
                }
                else {
                    state = BossEnemyState.standardUpdate;
                    estado = EstadoDaIa.atacando;
                    move.Mov.AplicadorDeMovimentos(Vector3.zero);
                    refAttack.StartAttack(new ImpulsoVelozDoChefe(), alvo);
                }
            break;
            case BossEnemyState.iniciandoGolpeex:
                if (roll.UpdateRoll())
                {
                    if (Vector3.Distance(dono.transform.position, posAlvo) >= 1)
                    {
                        move.Mov.AplicadorDeMovimentos((posAlvo - dono.transform.position).normalized);
                    }
                    else
                    {
                        move.Mov.AplicadorDeMovimentos(Vector3.zero);
                        SelecionaAlvo();
                        dono.transform.rotation = Quaternion.LookRotation(alvo.position-dono.transform.position,Vector3.up);
                        state = BossEnemyState.atualizarGolpeEx;
                    }
                }
            break;
            case BossEnemyState.atualizarGolpeEx:
                if (golpeEx.UpdateAttack())
                {
                    /* PUN
                    RPC_Listener.RPC(NameOfRPC.EventPublisher, 
                        PhotonTargets.All, 
                        EventKey.endSpecialAttack,
                        BytesTransform.ToBytes(
                        new StandardSendIntEvent(dono,dono.GetComponent<PhotonView>().viewID, EventKey.endSpecialAttack))
                        );*/
                    RPC_Listener.EventPublisher(
                    EventKey.endSpecialAttack,
                    BytesTransform.ToBytes(
                    new StandardSendIntEvent(dono, 0, EventKey.endSpecialAttack))
                    );
                    state = BossEnemyState.standardUpdate;
                }
            break;
        }
        
    }

    void StartRoll()
    {
        dono.tag = "Untagged";
        dono.transform.rotation = Quaternion.LookRotation(
            posAlvo - dono.transform.position
            );
        roll.StartRoll(dono.transform.forward);
    }

    void InitRoll()
    {        
        SelecionaPosAlvo();

        StartRoll();
    }

    void InitRollSpecialPosition()
    {
        SetarPosFocus();
        SelecionarMelhorCanto();

        StartRoll();
    }

    void SelecionarMelhorCanto()
    {
        Vector3[] Vs = new Vector3[4]
            {
                posF.MinzMinx,
                posF.MaxzMinx,
                posF.MinzMaxx,
                posF.MaxzMaxx
            };

        posAlvo = Vs[0];

        SloteMultiplayer[] SM = GlobalController.g.Jogadores.ToArray();
        
        for (int j = 1; j < 4; j++)
        {
            bool foi = true;

            for (int i = 0; i < SM.Length; i++)
            {
                Vector3 dist = SM[i].Manager.transform.position;                
                foi &= (Vector3.Distance(dist, Vs[j]) < Vector3.Distance(dist, posAlvo));
            }

            if(foi)
                posAlvo = Vs[j];
                
        }
    }

    private void SetarPosFocus()
    {
        posF.MaxzMaxx = SetarPosFocusVector(zMax, Vector3.right);
        posF.MaxzMinx = SetarPosFocusVector(zMax, Vector3.left);
        posF.MinzMinx = SetarPosFocusVector(zMin, Vector3.left)+Vector3.right;
        posF.MinzMaxx = SetarPosFocusVector(zMin, Vector3.right)+Vector3.left;
    }

    private Vector3 SetarPosFocusVector(int zfoco,Vector3 xDir)
    {
        RaycastHit hit = new RaycastHit();
        Vector3 origin = new Vector3(dono.transform.position.x,dono.transform.position.y,zfoco);
        Ray ray = new Ray(origin, xDir);

        if (Physics.Raycast(ray, out hit, 200, 1024))
        {
            return hit.point;
        }
        else
        {
            Debug.Log("não bateu na parede [uéeeee!!!] "+zfoco+" : "+xDir);
            return origin;
        }
    }

    void SorteiaPosAlvo()
    {
        int zPos = Random.Range(zMin,zMax);
        float xPos = Random.Range((posF.MinzMinx + posF.MaxzMinx).x / 2, (posF.MinzMinx + posF.MaxzMaxx).x / 2);

        posAlvo = new Vector3(xPos, dono.transform.position.y, zPos);

        if (!EstaNoTrapezio())
        {
            posAlvo.z = zMin+zMax-posAlvo.z;
        }
        
    }

    bool EstaNoTrapezio()
    {
        if (RetaDelimitadora.RelativoAReta(posF.MinzMinx, posF.MaxzMinx, true, posAlvo)
                &&
                RetaDelimitadora.RelativoAReta(posF.MaxzMinx, posF.MaxzMaxx, true, posAlvo)
                )
            return true;
        else
            return false;
    }

    private void SelecionaPosAlvo()
    {
        SetarPosFocus();
        SorteiaPosAlvo();
    }

    protected override void SorteiaAttack()
    {
        float sorteio = Random.Range(0,1f);

        if (sorteio < 0.15f)
            attack = 0;
        else if (sorteio >= 0.15f && sorteio < 0.4f)
            attack = 2;
        else
            attack = 1;
    }
}
