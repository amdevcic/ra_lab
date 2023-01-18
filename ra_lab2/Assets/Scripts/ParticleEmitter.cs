using System.Collections;
using UnityEngine;

public class ParticleEmitter : MonoBehaviour
{
    public GameObject particlePrefab;
    private Particle particle;
    public int numberOfParticles;
    public Particle[] particles;

    private void Awake()
    {
        particle = particlePrefab.GetComponent<Particle>();
        particle.useGravity = false;
        particle.moveSpeed = 5;
    }

    void Start()
    {
        particle = particlePrefab.GetComponent<Particle>();
        particles = new Particle[numberOfParticles];
        StartCoroutine(SpawnParticles());
    }

    IEnumerator SpawnParticles()
    {
        for (int i = 0; i < numberOfParticles; i++)
        {
            particles[i] = Instantiate(particle);
            particles[i].p = transform;
            yield return new WaitForSeconds(particle.lifetime/numberOfParticles);
        }
    }

    public void Restart() 
    {
        foreach (Particle p in particles)
            {
                Destroy(p.gameObject);
            }
            Start();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Restart();
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
