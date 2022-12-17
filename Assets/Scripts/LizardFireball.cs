using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardFireball : MonoBehaviour
{
    public Transform targetPlayer;
    public Transform enemyPosition;
    private float speed = 3f;

    private Vector2 fromEnemyToPlayer;
    private void Start()
    {

        fromEnemyToPlayer = targetPlayer.position;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector2.MoveTowards(transform.position, fromEnemyToPlayer, speed * Time.deltaTime);
    }
}
