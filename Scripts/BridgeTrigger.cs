using UnityEngine;
using UnityEngine.Playables;

public class BridgeTrigger : MonoBehaviour
{
    public PlayableDirector bridgeDirector;
    bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        if(!other.CompareTag("Player")) return;
        hasTriggered = true;
        bridgeDirector.Play();
    }
}
