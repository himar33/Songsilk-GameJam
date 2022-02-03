using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{

    public Transform[] spawns;
    public Collider[] EndLevels;

    Vector3 Player;
    Vector3 Espawn;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        int spawnIndex = 0;
        for (int i = 0; i < EndLevels.Length; i++)
        {
            spawnIndex++;
            if (other == EndLevels[i])
            {
                gameObject.transform.position = spawns[spawnIndex].position;
                Debug.Log(gameObject.transform.position);
                Debug.Log(spawns[spawnIndex].transform.position);
            }
        }
        
    }
}
