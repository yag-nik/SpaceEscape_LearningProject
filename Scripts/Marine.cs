using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Marine : MonoBehaviour
{
    
    float moveInput;

    AudioSource audioSource;
    Animator animator;
    bool isMoving;
    public float speed = 3.5f;
    public float rotSpeed = 5f;

    RaycastHit hit;

    float lookAheadDistance = .25f; // also for walls 
    public GameObject muzzle;

    MuzzleShootAction muzzleShootAction;
    public int hitPoints = 30;
    public float deathPause = 5f;
    GUIStyle style; // for health bar
    public bool isDead;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        if(muzzle == null)
        {
            Debug.Log("Muzzle Null");
        }

        muzzleShootAction = muzzle.GetComponent<MuzzleShootAction>();

        if (muzzleShootAction == null) Debug.Log("Shoot Action Null");

        style = new GUIStyle();
        style.normal.textColor = Color.red;
        style.fontSize = 35;
    }

    private void Update()
    {

        if (isDead) return;

        if (Input.GetKey(KeyCode.D))
        {
            DoTurnRight();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            DoTurnLeft();
        }

        if (Input.GetKey(KeyCode.W))
        {
            if (!isMoving)
            {
                Debug.Log("is Walking");
                animator.SetTrigger("isWalking");
                isMoving = true;
            }
            else
            {
                DoMove();
            }
        }
        else
        {
            // no movement
            if(isMoving)
            {
                Debug.Log("is Idle");
                animator.SetTrigger("isIdle");
                isMoving = false;
            }
        }

        if ((Input.GetKeyDown(KeyCode.Space)))
        {
            //spacebar
            muzzleShootAction.Trigger(); //fire
        }
    }

    void DoTurnLeft()
    {
        transform.Rotate(0, -rotSpeed, 0);
    }
    void DoTurnRight()
    {
        transform.Rotate(0, rotSpeed, 0);
    }

    void DoMove()
    {
        var moveDistance = Time.deltaTime * speed;
        Ray ray = new Ray(muzzle.transform.position, transform.forward);

        Debug.DrawRay(muzzle.transform.position, transform.forward, Color.red);

        //cast a ray forward. if it hits an object tagged as a wall, don't move 
        if (Physics.Raycast(ray, out hit, lookAheadDistance) && (hit.transform.gameObject.tag == "Wall"))
        {
            Debug.Log("wall blocking");
        }
        else
        {
            //move the marine
            transform.position += transform.forward * moveDistance;
        }
    }

    public void Damage(int amount)
    {
        if (isDead) return;
        hitPoints -= amount;
        if(hitPoints <= 0)
        {
            hitPoints = 0;
            DoDying(); // if no hp then die
        }
    }

    public void DoDying()
    {
        StartCoroutine(Dying());
    }

    IEnumerator Dying()
    {
        isDead = true;
        animator.SetTrigger("isDead");

        yield return new WaitForSeconds(deathPause);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); // reload scene
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(15, 15, 100, 20), "HP: " + hitPoints.ToString(), style);
    }

    public void Boot1()
    {
        audioSource.pitch = 1.0f;
        audioSource.Play();
    }
    public void Boot2()
    {
        audioSource.pitch = .9f;
        audioSource.Play();
    }
}

