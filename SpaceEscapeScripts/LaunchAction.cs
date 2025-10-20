using System.Collections;
using UnityEngine;

public class LaunchAction : CooldownAction
{
    public GameObject missilePrefab;
    public GameObject lid;
    public AudioClip lidSFX;
    private float lidLiftTime = 0.93f;
    private GameObject missile;
    private Missile missileScript;

    protected override void Triggered()
    {
        if(target == null)
        {
            Debug.LogWarning("No target assigned for LaunchAction");
            return;
        }
        //create missile at spawn
        missile = Instantiate(missilePrefab, target.transform);
        RaiseLid();
        CreateMissile();
    }

    void CreateMissile()
    {
        missile = Instantiate(missilePrefab, target.transform);
        missileScript = missile.GetComponent<Missile>();
    }

    

    IEnumerator OnLaunchComplete()
    {
        yield return new WaitForSeconds(2f);
        LowerLid();
    }


    void RaiseLid()
    {
        PlaySFX(lidSFX); // lid up
        iTween.MoveBy(lid, iTween.Hash(
            "y", .25,
            "time", lidLiftTime,
            "oncomplete", "OnRaiseComplete",
            "oncompletetarget", gameObject
            ));
    }

    void OnRaiseComplete()
    {
        //launch a missile
        LaunchMissile();
    }

    void LaunchMissile()
    {
        PlaySFX(actionSFX); // launch sfx

        //locate the player to have target
        var player = GameObject.FindGameObjectWithTag("Muzzle");
        missileScript.Launch(player);
        StartCoroutine(OnLaunchComplete());

        //iTween.MoveTo(missile, iTween.Hash(
        //    "position", player.transform,
        //    "time", 1f,
        //    "oncomplete", "OnMissileComplete",
        //    "oncompletetarget", gameObject
        //    ));
    }

    void OnMissileComplete()
    {
        LowerLid();
    }

    void LowerLid()
    {
        PlaySFX(lidSFX); // lid up
        iTween.MoveBy(lid, iTween.Hash(
            "y", -.25,
            "time", lidLiftTime,
            "oncomplete", "OnLowerComplete",
            "oncompletetarget", gameObject
            ));
    }
    void OnLowerComplete()
    {
        Reset(); // reset cooldown
        OnEnded.Invoke(); //fire event alert listeners
    }

}
