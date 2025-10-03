using UnityEngine;

namespace JuiceBits
{
    public class Spawner : MonoBehaviour
    {
        public GameObject Prefab;

        [Header("Spawn Rate")]
        public float SpawnTime = 1f;
        public float SpawnDelay = 3f;

        [Header("Object Party Size")]
        public int MinPartySize;
        public int MaxPartySize;

        [Header("Object Party Speed")]
        public float MinSpeed;
        public float MaxSpeed;

        [Header("Spacing of Objects")]
        public float Spacing;

        void Start()
        {
            // Spawns the parties
            InvokeRepeating("SpawnPrefab", SpawnTime, SpawnDelay);
        }

        void Update()
        {

        }

        void SpawnPrefab()
        {
            int PartySize = Random.Range(MinPartySize, MaxPartySize + 1);
            float partySpeed = Random.Range(MinSpeed, MaxSpeed);

            // Creates different party sizes and sets the speed of the parties
            for (int i = 0; i < PartySize; i++)
            {
                Vector3 spawnPosition = gameObject.transform.position + new Vector3(i * Spacing, 0f, 0f);
                GameObject spawnedObject = Instantiate(Prefab, spawnPosition, Quaternion.identity);

                spawnedObject.GetComponent<SpawnedObjects>().SetPartySpeed(partySpeed);
            }
        }
    }
}