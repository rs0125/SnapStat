using UnityEngine;

public class TargetHit : MonoBehaviour
{
    public TargetSpawner spawner;

    private static Vector3? lastHitPoint = null;

    public void ProcessHit(Vector3 hitPoint)
    {
        Vector3 center = transform.position;
        Vector3 deviation = hitPoint - center;

        if (lastHitPoint != null)
        {
            Vector3 flick = hitPoint - lastHitPoint.Value;
            Vector3 targetDirection = center - lastHitPoint.Value;
            float overshoot = flick.magnitude - targetDirection.magnitude;
            Debug.Log(overshoot > 0 ? "Overflick" : "Underflick");
        }
        
        DeviationUIManager.Instance?.AddDeviation(deviation);

        lastHitPoint = hitPoint;
        spawner.NotifyTargetDestroyed(gameObject);
        Destroy(gameObject);
    }
}