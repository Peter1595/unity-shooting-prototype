using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Gameplay;
using UnityEngine;

public class CoinDrop : Drop
{
    protected override void Start()
    {
        base.Start();

        m_Rigidbody.velocity = new Vector3(Random.Range(-0.5f, 0.5f), 1f, Random.Range(-0.5f, 0.5f));
    }

    protected override void OnClaim(PlayerCharacterController player)
    {
        base.OnClaim(player);

        
    }
}
