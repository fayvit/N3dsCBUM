using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMana : Coletavel {

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public override void ElementosDoColetavel(GameObject P)
    {
        
        CharacterBase m = P.GetComponent<CharacterBase>();
        m.Dados.SetarManaMax();
        EventAgregator.Publish(EventKey.heroHealth, new CharacterDamageEvent(m as CharacterManager));
        

        Destroy(
               Instantiate(PrefabList.Load(PrefabListName.particulaPositivaMana), P.transform.position, Quaternion.identity), 3
               );
        Destroy(
            Instantiate(PrefabList.Load(PrefabListName.pegueiMaisMana), transform.position, Quaternion.identity), 3
            );

        Destroy(gameObject);
    }

    /* PUN
    public override void ElementosDoColetavel(PhotonView P)
    {
        if (P.isMine)
        {
            CharacterBase m = P.GetComponent<CharacterBase>();
            m.Dados.SetarManaMax();
            EventAgregator.Publish(EventKey.heroHealth, new CharacterDamageEvent(m as CharacterManager));
        }

        Destroy(
               Instantiate(PrefabList.Load(PrefabListName.particulaPositivaMana), P.transform.position, Quaternion.identity), 3
               );
        Destroy(
            Instantiate(PrefabList.Load(PrefabListName.pegueiMaisMana), transform.position, Quaternion.identity), 3
            );

        Destroy(gameObject);
    }*/
}
