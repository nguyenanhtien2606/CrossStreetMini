using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject carPrefab;
    public List<GameObject> spawnPoints;

    public GameObject characterPrefab;
    public GameObject startPoint;
    public GameObject boundary;
    public ParticleSystem moneyParticle;

    public int score;
    public Text scoreText;
    
    public AudioClip scoreSound;
    public AudioClip deathSound;

    public float waitTime;

    public static GameManager global;

    // Start is called before the first frame update
    void Start()
    {
        global = this;
        score = 0;

        StartCoroutine(SpawmCar(waitTime));
        SpawnCharacter();
    }

    public void SpawnCharacter()
    {
        Instantiate(characterPrefab, startPoint.transform.position, startPoint.transform.rotation);
    }
    
    IEnumerator SpawmCar(float waitTime)
    {
        var go = new List<GameObject>(spawnPoints);

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            int numRandom = Random.Range(0, go.Count);
            Instantiate(carPrefab, go[numRandom].transform.position, go[numRandom].transform.rotation);
            go.Remove(go[numRandom]);

            yield return new WaitForSeconds(waitTime);
        }

    }
}
