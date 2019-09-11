using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldBehavior : MonoBehaviour
{
    public IEnumerator AutoActiveSeft()
    {
        yield return new WaitForSeconds(5);

        if (gameObject.activeInHierarchy == false)
        {
            gameObject.SetActive(true);
            yield break;
        }
    }
}
