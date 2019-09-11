using System.Collections.Generic;
using UnityEngine;

public class BoundarExit : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
        {
            var go = new List<GameObject>(GameManager.global.spawnPoints);

            for (int i = 0; i < go.Count; i++)
            {
                int numRandom = Random.Range(0, go.Count);
                other.transform.position = go[numRandom].transform.position;
                other.transform.rotation = go[numRandom].transform.rotation;
            }
        }
    }
}
