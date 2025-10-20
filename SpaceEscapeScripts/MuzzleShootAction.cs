using System.Collections;
using UnityEngine;

public class MuzzleShootAction : CooldownAction
{
    public int damage = 3;
    public float range = 25f;
    Light glow;
    LineRenderer beam;
    Ray ray;
    RaycastHit hit;

    protected override void DoAwake()
    {
        beam = GetComponent<LineRenderer>();
        glow = GetComponent<Light>();
    }

    void EnableEffect(bool show)
    {
        try
        {
            beam.enabled = show;
            glow.enabled = show;
        }
        catch{ }
    }

    protected override void Triggered()
    {
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        //play sfx and vfx
        audioSource.Play();
        EnableEffect(true);

        var pos = transform.position;

        ray = new Ray();

        //start from muzzle

        ray.origin = new Vector3(pos.x, 1.35f, pos.z);
        beam.SetPosition(0, ray.origin);
        ray.direction = transform.forward;

        //perform the raycast

        if(Physics.Raycast(ray, out hit, range))
        {
            if((hit.collider.tag == "Foe"))
            {
                Turret turret = hit.collider.gameObject.GetComponent<Turret>();
                if(turret != null)
                {
                    turret.Damage(damage);
                }
                else
                {
                    Debug.Log("Turret is null");
                }
            }

            beam.SetPosition(1, hit.point);
        }
        else
        {
            beam.SetPosition(1, ray.origin + ray.direction * range);
        }
        yield return new WaitForSeconds(.2f);
        EnableEffect(false);
    }
}
