using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnarMais : MonoBehaviour {

    [SerializeField] private int ignorarSeNumPlayersMenorQue = 0;
    [SerializeField] private EnemyBase[] enemies;

    private void Start()
    {
        EventAgregator.AddListener(EventKey.enemyDefeated, OnEnemieDefeated);
        EventAgregator.AddListener(EventKey.enemyDisable, OnEnemieDisabled);
    }

    private void OnDestroy()
    {
        EventAgregator.RemoveListener(EventKey.enemyDefeated, OnEnemieDefeated);
        EventAgregator.RemoveListener(EventKey.enemyDisable, OnEnemieDisabled);
    }

    private void OnEnemieDefeated(IGameEvent obj)
    {
        if (obj.Sender == gameObject && GlobalController.g.Jogadores.Count >= ignorarSeNumPlayersMenorQue)
        {
            int[] IDs = new int[enemies.Length];
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].IniciarIa();
                IDs[i] = 0;
            }

            EventAgregator.Publish(EventKey.addEnemyInManager, new AddEnemyInManagerEvent(gameObject, enemies));
        }
        else if (obj.Sender == gameObject)
        {
            OnEnemieDisabled(obj);
        }     
    }

    void OnEnemieDisabled(IGameEvent e)
    {
        if (e.Sender == gameObject)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].gameObject.SetActive(false);
            }
        }
    }
}
