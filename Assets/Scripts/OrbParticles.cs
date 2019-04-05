using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbParticles : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        StartCoroutine(WaitThenDie());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator WaitThenDie()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }
}
