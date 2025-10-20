using UnityEngine;

public class ShootAction : CooldownAction
{
    private int shotsFired = 0;
    private Light glow; //the blaster glow
    private LineRenderer beam; //the blaster beam
    public int damage = 3;
    public float range = 40f;


    protected override void DoAwake()
    {
        target.SetActive(false); //hide the blaster
        glow = target.GetComponent<Light>();
        beam = target.GetComponent<LineRenderer>();
    }

    protected override void Triggered()
    {


        shotsFired++;
        PlaySFX(actionSFX); // method in parent
        target.SetActive(true); //show the blaster
        beam.enabled = true;

        var origin = transform.TransformPoint(Vector3.zero);
        var adjOrigin = new Vector3(origin.x, target.transform.position.y, origin.z);
        beam.SetPosition(0, adjOrigin); //set start of beam

        var player = GameObject.FindGameObjectWithTag("Player");
        beam.SetPosition(1, player.transform.position); // set end of beam

        var marine =  player.GetComponent<Marine>();
        if(marine == null)
        {
            Debug.Log("Marine Script not found");
        }
        marine.Damage(damage);

        iTween.ValueTo(target, iTween.Hash(
            "from", 5f,
            "to", .5f,
            "time", .4f,
            "onupdate", "OnTweenUpdate",
            "onupdatetarget", gameObject,
            "oncomplete", "OnTweenComplete",
            "oncompletetarget", gameObject
        ));
    }

    void OnTweenUpdate(float value)
    {
        glow.range = value;
        if(glow.range < 2f)
        {
            beam.enabled = false; // turn off beam early
        }
    }

    void OnTweenComplete()
    {
        
        target.SetActive(false); // hide the blaster
        
        if (shotsFired < 3)
        {
            Triggered(); // fire again
        }
        else
        {
            Reset(); // reset cooldown
            OnEnded.Invoke(); // fire event alert listeners
        }
    }
}
