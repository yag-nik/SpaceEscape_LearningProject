using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(AudioSource))]
public abstract class CooldownAction : MonoBehaviour
{
    public AudioClip actionSFX;
    [SerializeField] float cooldownTime; //how much time must pass between triggers
    [SerializeField] bool isEnabled = true; //indicated whether the action is active or not
    public GameObject target; //generic ref to a gameobject which get affected

    [HideInInspector]
    public UnityEvent OnReady = new UnityEvent();
    public UnityEvent OnEnded = new UnityEvent();

    protected bool isTriggered = false; //indicates whether the action has been triggered
    protected AudioSource audioSource;  //feedback of parent's component 
    private float elapsedTime = 0; //track the action cooldown

    private void Awake()
    {
        audioSource = GetComponentInParent<AudioSource>();
        DoAwake();
    }

    private void Update()
    {
        if ((!isEnabled) || (!isTriggered))
        {
            return;
        }
        elapsedTime += Time.deltaTime; // increment elapsed time
        if (elapsedTime >= cooldownTime)
        {
            Reset(); 
            Ready(); //call virtual method
            OnReady.Invoke();//fire event alert listeners
        }
        if (isTriggered) DoUpdate(); // call virtual method
    }

    protected virtual void DoAwake() { }
    protected virtual void DoUpdate() { }
    protected virtual void Triggered() { }
    protected virtual void Ready() { }

    public void Trigger()
    {
        if ((!isEnabled) || isTriggered) return;
        isTriggered = true;
        Triggered();
    }

    protected void Reset()
    {
        elapsedTime = 0; // restart cooldown
        isTriggered = false; // allow retriggering

    }

    protected void PlaySFX(AudioClip clip)
    {
        audioSource.clip = clip; // clip to play
        audioSource.Play(); //play it
    }
}
