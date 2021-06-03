using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticle : MonoBehaviour
{
    [SerializeField] float duration = 0.5f;
    void Start()
    {
        GetComponent<ParticleSystem>().Play();
        StartCoroutine(ParticleDestroy(duration));
    }

    private IEnumerator ParticleDestroy(float dur)
    {
        yield return new WaitForSeconds(dur);
        Destroy(this.gameObject);
    }
}
