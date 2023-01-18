using UnityEngine;

public class Particle : MonoBehaviour
{
    public float lifetime;
    private float t;
    public Vector3 direction;
    public float moveSpeed;
    public bool useGravity;
    private Vector3 speed;
    private Vector3 gravityVector = Vector3.down * 9.81f;
    public Gradient gradient;
    public Transform p;

    private Renderer particleRenderer;

    private void Awake()
    {
        particleRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        t = lifetime;
        direction = Random.insideUnitSphere;
        speed = direction.normalized * moveSpeed;
        transform.position = p.position;
    }

    void Update()
    {
        if ((t -= Time.deltaTime) < 0)
        {
            Start();
        }
        transform.position += speed * Time.deltaTime;
        if (useGravity)
            speed += gravityVector*Time.deltaTime;
        particleRenderer.material.color = gradient.Evaluate(1-t/lifetime);

        transform.LookAt(Camera.main.transform);
    }

    public void setGravity(bool gravity)
    {
        useGravity = gravity;
    }

    public void setMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }
}
