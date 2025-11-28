using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float fallDeathY = -2f;   // optional: for death on falling

    private float moveSpeed = 3f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        if (ConfigManager.Instance != null)
            moveSpeed = ConfigManager.Instance.PlayerSpeed;
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0f, v).normalized;
        Vector3 vel = rb.linearVelocity;
        vel.x = dir.x * moveSpeed;
        vel.z = dir.z * moveSpeed;
        rb.linearVelocity = vel;

    }
}
