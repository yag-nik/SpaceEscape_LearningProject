using UnityEngine;

public class ScanAction : CooldownAction
{
    public float turnTime = 1f; //seconds to turn fourth of a circle

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Top");
    }
    protected override void DoUpdate()
    {
        RaycastHit hit;

        if(Physics.Raycast(target.transform.position, target.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(target.transform.position, target.transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
            if (!(hit.collider.CompareTag("Player"))) return;
            iTween.StopByName("turning"); //interrupt turning
            Reset(); //reset cooldown
            OnEnded.Invoke(); //fire event alert listeners

        }
    }

    protected override void Triggered()
    {
        PlaySFX(actionSFX); // method in parent 
        iTween.RotateBy(target, iTween.Hash(
            "name", "turning",
            "y", 0.25, //quater turn
            "time", turnTime,
            "easetype", "easeOutBack"
            ));
    }

    protected override void Ready()
    {
        //when cooled down, retrigger
        Trigger();
    }
}
