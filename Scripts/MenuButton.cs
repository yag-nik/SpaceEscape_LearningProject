using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    AudioSource audioSource;
    public AudioClip rolloverSound;
    public AudioClip clickSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (rolloverSound != null)
        {
            audioSource.PlayOneShot(rolloverSound);
            audioSource.Play();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
            audioSource.Play();
        }

        if(gameObject.name == "Newgame")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("SpaceEscape");
        }
    }

}
