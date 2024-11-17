using UnityEngine;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] obstacles;
    public GameObject roadPrefab;
    public Transform player;
    private float planeLength = 30;
    private int minObstacles = 6;
    private int maxObstacles = 12;
    private float zSpawn = 90;
    private float distanceToDelete = 90;

    private List<GameObject> activeRoads = new List<GameObject>();
    private List<GameObject> activeObstacles = new List<GameObject>();

    void Update()
    {
        if (player.localPosition.z > zSpawn - planeLength)
        {
            SpawnRoadAndObstacles();
            zSpawn += planeLength;
        }

        DeleteOldRoadsAndObstacles();
    }

    void SpawnRoadAndObstacles()
    {
        GameObject road = Instantiate(roadPrefab, Vector3.forward * zSpawn, Quaternion.identity);
        activeRoads.Add(road);

        SpawnMultipleObstacles();
    }

    void SpawnMultipleObstacles()
    {
        int numberOfobstacles = Random.Range(minObstacles, maxObstacles + 1);

        for (int i = 0; i < numberOfobstacles; i++)
        {
            SpawnRandomObstacle();
        }
    }

    void SpawnRandomObstacle()
    {
        int maxAttempts = 10; // Um Endlosschleifen zu vermeiden

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            GameObject randomObstacle = obstacles[Random.Range(0, obstacles.Length)];
            string obstacleName = randomObstacle.name;

            float[] defaultXPositions = { -1.5f, 0f, 1.5f };
            float randomX = defaultXPositions[Random.Range(0, defaultXPositions.Length)];
            float randomZ = zSpawn + Random.Range(-planeLength / 2, planeLength / 2);
            Vector3 spawnPosition = new Vector3(randomX, transform.position.y, randomZ);

            // Bounding Box Check
            Bounds obstacleBounds = GetObstacleBounds(randomObstacle, spawnPosition);

            // Erst wenn diese Bedingungen stimmen darf ein Objekt gespawnt werden
            if (IsAreaFree(obstacleBounds) && AreNeighborLanesFree(randomX, obstacleBounds, obstacleName))
            {
                GameObject obstacle = Instantiate(randomObstacle, spawnPosition, randomObstacle.transform.rotation);
                activeObstacles.Add(obstacle);
                break; // Wenn das Hindernis erfolgreich platziert wurde, Schleife abbrechen
            }
        }
    }

    bool AreNeighborLanesFree(float xPosition, Bounds obstacleBounds, string obstacleName)
    {
        if (obstacleName == "SlopePrefab(Clone)" || obstacleName == "table_001(Clone)") //Bei diesen Obstacles will ich keine Überprüfung machen
        {
            return true;
        }

        float[] neighborLanes = { -1.5f, 0f, 1.5f };

        foreach (float neighborX in neighborLanes)
        {
            if (Mathf.Approximately(neighborX, xPosition))
            {
                continue;
            }

            // Erstelle die Bounds für die benachbarte Lane
            Bounds neighborBounds = obstacleBounds;
            neighborBounds.center = new Vector3(neighborX, neighborBounds.center.y, neighborBounds.center.z);

            // Prüfe, ob die Nachbar-Bounds mit bestehenden Hindernissen kollidieren
            foreach (GameObject obstacle in activeObstacles)
            {
                if (obstacle == null) continue;

                Renderer renderer = obstacle.GetComponent<Renderer>();
                if (renderer != null && neighborBounds.Intersects(renderer.bounds))
                {
                    return false; // Eine der Nachbar-Lanes ist belegt
                }
            }
        }

        return true; // Beide Nachbar-Lanes sind frei
    }


    Bounds GetObstacleBounds(GameObject obstacle, Vector3 position)
    {
        Renderer renderer = obstacle.GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogWarning($"Obstacle {obstacle.name} hat keinen Renderer. Standardgröße wird verwendet.");
            return new Bounds(position, Vector3.one); // Fallback für Hindernisse ohne Renderer
        }
        return new Bounds(position, renderer.bounds.size);
    }

    bool IsAreaFree(Bounds newObstacleBounds)
    {
        float padding = 2.0f; //Je nachdem wie unfair das Spiel sich anfühlt kann man den Wert erhöhen

        // Erweitere die Bounds um das Padding
        Bounds paddedBounds = newObstacleBounds;
        paddedBounds.Expand(new Vector3(0, 0, padding * 2));

        foreach (GameObject obstacle in activeObstacles)
        {
            if (obstacle == null) continue; // Falls Hindernisse bereits gelöscht wurden

            Renderer renderer = obstacle.GetComponent<Renderer>();
            if (renderer != null)
            {
                Bounds existingBounds = renderer.bounds;
                if (paddedBounds.Intersects(existingBounds))
                {
                    return false; // Wenn es eine Überschneidung gibt, ist der Bereich nicht frei
                }
            }
        }
        return true; // Bereich ist frei
    }

    void DeleteOldRoadsAndObstacles()
    {
        for (int i = activeRoads.Count - 1; i >= 0; i--)
        {
            if (player.localPosition.z - activeRoads[i].transform.position.z > distanceToDelete)
            {
                Destroy(activeRoads[i]);
                activeRoads.RemoveAt(i);
            }
        }

        for (int i = activeObstacles.Count - 1; i >= 0; i--)
        {
            if (player.localPosition.z - activeObstacles[i].transform.position.z > distanceToDelete)
            {
                Destroy(activeObstacles[i]);
                activeObstacles.RemoveAt(i);
            }
        }
    }
}