using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Missile : MonoBehaviour
{

    public GameObject explosionPrefab;
    public AudioClip explosionSFX;
    AudioSource audioSource;
    public int damage = 20;
    float speed = 20f;
    bool doMove;
    Vector3 targetPos;

    private void Start()
    {
        doMove = false;
        audioSource = GetComponent<AudioSource>();
    }

    public void Launch(GameObject target)
    {
        targetPos = target.transform.position;
        doMove = true;
    }

    private void Update()
    {
        if (!doMove) return;
        //move towards target
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
        if(Vector3.Distance(transform.position, targetPos) < .1f)
        {
            doMove = false;
            Explode();
            //hide the missile
            gameObject.SetActive(false);
        }
    }

    public void Explode()
    {
        audioSource.clip = explosionSFX;
        audioSource.Play();

        //make explosion object
        GameObject explo = Instantiate(explosionPrefab);
        explo.transform.position = transform.position;
        
        var player = GameObject.FindGameObjectWithTag("Player");
        float dist = Vector3.Distance(transform.position, player.transform.position);
        if(dist< 1.5f)
        {
            Marine marine = player.GetComponent<Marine>();
            marine.Damage(damage);
        }

        Destroy(explo, 1.2f);
        gameObject.SetActive(true);
    }
}
