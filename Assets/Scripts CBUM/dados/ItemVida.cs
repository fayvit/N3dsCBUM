using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemVida : Coletavel
{

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void ElementosDoColetavel(GameObject G)
    {
        
        CharacterBase m = G.GetComponent<CharacterBase>();
        m.Dados.SetarVidaMax();
        EventAgregator.Publish(EventKey.heroHealth, new CharacterDamageEvent(m as CharacterManager));
        

        Destroy(
               Instantiate(PrefabList.Load(PrefabListName.particulaPositiva), G.transform.position, Quaternion.identity), 3
               );
        Destroy(
            Instantiate(PrefabList.Load(PrefabListName.pegueiMaisVida), transform.position, Quaternion.identity), 3
            );

        Destroy(gameObject);
    }

    /* PUN
    public override void ElementosDoColetavel(PhotonView P)
    {
        if (P.isMine)
        {
            CharacterBase m = P.GetComponent<CharacterBase>();
            m.Dados.SetarVidaMax();
            EventAgregator.Publish(EventKey.heroHealth, new CharacterDamageEvent(m as CharacterManager));
        }

        Destroy(
               Instantiate(PrefabList.Load(PrefabListName.particulaPositiva), P.transform.position, Quaternion.identity), 3
               );
        Destroy(
            Instantiate(PrefabList.Load(PrefabListName.pegueiMaisVida), transform.position, Quaternion.identity), 3
            );

        Destroy(gameObject);
    }*/
}
