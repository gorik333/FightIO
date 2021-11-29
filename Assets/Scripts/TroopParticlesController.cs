using System.Collections;
using UnityEngine;

public class TroopParticlesController : MonoBehaviour
{
    [SerializeField]
    private GameObject _deathParticles;

    [SerializeField]
    private Transform _deathSpawnPosition;

    private SkinnedMeshRenderer _skinnedMeshRenderer;


    private void Start()
    {
        _skinnedMeshRenderer = transform.parent.GetComponentInChildren<SkinnedMeshRenderer>();
    }


    public void SpawnDeathParticles(float delay)
    {
        StartCoroutine(SpawnDeathParticlesDelay(delay));
    }


    private IEnumerator SpawnDeathParticlesDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        ParticleSystem particleSystem = Instantiate(_deathParticles, _deathSpawnPosition.position, Quaternion.identity).GetComponent<ParticleSystem>();

        particleSystem.GetComponent<Renderer>().material = _skinnedMeshRenderer.material;

        particleSystem.Play();

        Destroy(particleSystem.gameObject, 2f);
    }
}
