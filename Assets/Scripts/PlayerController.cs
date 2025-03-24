
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputActionReference move;
    public InputActionReference look;
    public InputActionReference jump;
    public float yaw = 0f; // H
    public float pitch = 0f; // V

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    void Update()
    {
        Vector2 l = look.action.ReadValue<Vector2>();
        yaw += l.x * 0.3f;
        yaw %= 360;
        pitch -= l.y * 0.2f;
        pitch = Mathf.Clamp(pitch, -90, 90);

        transform.GetChild(0).localRotation = Quaternion.Lerp(transform.GetChild(0).localRotation, Quaternion.Euler(pitch, yaw, 0), Time.deltaTime * 10f);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, -Vector3.up, 1f))
        {
            Vector2 m = move.action.ReadValue<Vector2>();
            m = m.normalized * 260f * Time.fixedDeltaTime;
            Vector3 mV = Quaternion.Euler(0, yaw, 0) * new Vector3(m.x, 0, m.y);
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.AddForce(new Vector3(mV.x - rb.linearVelocity.x, 0f, mV.z - rb.linearVelocity.z), ForceMode.VelocityChange);
            
            if (jump.action.inProgress)
            {

                if (rb.linearVelocity.y == 0 && Physics.Raycast(transform.position, -Vector3.up, 1f))
                {
                    rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
                }
            }
        }
    }
}
