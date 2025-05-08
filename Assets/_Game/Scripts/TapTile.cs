using System.Collections;
using UnityEngine;

public class TapTile : MonoBehaviour
{
    private GameContoller _gameController;
    private AudioClips _audioClips;
    private Animator _animator;
    [SerializeField] ParticleSystem _tapParticle = null;

    private bool _isTappable = false;

    private void Start()
    {
        _gameController = FindFirstObjectByType<GameContoller>();
        _audioClips = FindFirstObjectByType<AudioClips>();
        _animator = FindFirstObjectByType<Animator>();
    }

    public void MakeTileTappable()
    {
        _isTappable = true;
    }

    public void DestroyTile()
    {
        //if (!_isTappable) return;
        
        _gameController._score++;

        Destroy(GetComponent<Collider>());
        PlayParticle();
        AudioSource audioSource = AudioController.PlayClip2D(_audioClips._tileTap, 1);
        audioSource.pitch = UnityEngine.Random.Range(.8f, 1.2f);

        _animator.SetTrigger("PlayTapped");

        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(0.3f);

        Destroy(gameObject);
    }

    public void TriggerMissAnimation()
    {
        _animator.SetTrigger("TileExplode");
    }

    public void PlayParticle()
    {
        _tapParticle.Play();
    }
}
