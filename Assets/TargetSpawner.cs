using UnityEngine;
using System.Collections.Generic;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab;
    public Transform spawnAreaQuad; // Assign a quad to define the spawn area
    public int maxTargets = 3;

    private List<GameObject> activeTargets = new List<GameObject>();

    public void SpawnTarget()
    {
        if (spawnAreaQuad == null)
        {
            Debug.LogError("Spawn area quad not assigned!");
            return;
        }

        Vector3 center = spawnAreaQuad.position;
        Vector3 right = spawnAreaQuad.right.normalized;
        Vector3 up = spawnAreaQuad.up.normalized;

        float width = spawnAreaQuad.localScale.x;
        float height = spawnAreaQuad.localScale.y;

        float offsetX = Random.Range(-0.5f, 0.5f) * width;
        float offsetY = Random.Range(-0.5f, 0.5f) * height;

        Vector3 spawnPos = center + right * offsetX + up * offsetY;

        GameObject newTarget = Instantiate(targetPrefab, spawnPos, Quaternion.identity);

        TargetHit hitScript = newTarget.GetComponent<TargetHit>();
        hitScript.spawner = this;

        activeTargets.Add(newTarget);
    }

    public void NotifyTargetDestroyed(GameObject target)
    {
        if (activeTargets.Contains(target))
            activeTargets.Remove(target);

        if (activeTargets.Count < maxTargets)
            SpawnTarget();
    }

    private void Start()
    {
        for (int i = 0; i < maxTargets; i++)
            SpawnTarget();
    }
}