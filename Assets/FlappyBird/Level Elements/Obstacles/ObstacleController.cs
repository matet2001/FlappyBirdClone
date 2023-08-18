using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [SerializeField] GameObject pointColliderGameObject;
    [SerializeField] new Rigidbody2D rigidbody;

    public void StartObstacle(float speed)
    {
        pointColliderGameObject.SetActive(true);
        rigidbody.velocity = Vector2.left * speed;
    }
    public void StopObstacle()
    {
        rigidbody.velocity = Vector2.zero;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("ObstacleSpawner")) return;

        gameObject.SetActive(false);
    }
}
