using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        if (GameDataManager.Instance != null)
            moveSpeed = GameDataManager.Instance.playerSpeed;
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(h, 0, v).normalized * moveSpeed;
        Vector3 vel = rb.linearVelocity;
        vel.x = move.x;
        vel.z = move.z;
        rb.linearVelocity = vel;
        
        if (transform.position.y < -2f)
            GameManager.Instance.PlayerDied();
    }

    public void OnScorePlatform()
    {
        GameManager.Instance.AddScore(1);
    }
}
