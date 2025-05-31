using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            TargetHit target = collision.gameObject.GetComponent<TargetHit>();
            if (target != null)
            {
                Vector3 hitPoint = collision.contacts[0].point;
                target.ProcessHit(hitPoint);
            }

            Destroy(gameObject);
        }
    }
}   