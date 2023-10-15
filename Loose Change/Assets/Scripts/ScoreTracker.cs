using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    public float score=0f;
    public float scoreGainRate = 0.1f;
    public int coinsCollected;
    public int enemysBeaten;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        score += scoreGainRate;
    }

    void CollectCoin(int coin)
    {
        coinsCollected++;
        score += coin;
    }

    void BeatEnemy(int enemy)
    {
        enemysBeaten++;
        score += enemy;
    }
}
