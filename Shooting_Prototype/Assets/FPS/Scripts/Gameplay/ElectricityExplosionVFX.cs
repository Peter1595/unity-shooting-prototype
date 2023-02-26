using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.VFX;

public class ElectricityExplosionVFX : MonoBehaviour
{
    public VisualEffect VFX;

    public float Lifetime;

    float time;
    float timeDistance;

    float flashScale;
    float ElectricityScale;
    float SparkesScale;

    // Start is called before the first frame update
    void Start()
    {
        flashScale = VFX.GetFloat("FlashScale");
        ElectricityScale = VFX.GetFloat("ElectricityScale");
        SparkesScale = VFX.GetFloat("SparkesScale");

        time = Time.time + Lifetime;

        timeDistance = time - Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = time - Time.time;

        float timeRatio = distance / timeDistance;

        VFX.SetFloat("FlashScale", flashScale * timeRatio);
        VFX.SetFloat("ElectricityScale", ElectricityScale * timeRatio);
        VFX.SetFloat("SparkesScale", SparkesScale * timeRatio);

        Debug.Log(timeRatio + " - " + distance + " - " + timeDistance);

        if (Time.time > time)
        {
            Destroy(this);
            Destroy(VFX);
        }
    }
}
