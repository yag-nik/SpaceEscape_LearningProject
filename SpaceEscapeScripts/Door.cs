using UnityEngine;

public class Door : MonoBehaviour
{
    Animator animator;
    AudioSource audioSource;
    bool hasTriggered = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;
        hasTriggered = true;
        animator.enabled = true;
        audioSource.Play();
    }
}
