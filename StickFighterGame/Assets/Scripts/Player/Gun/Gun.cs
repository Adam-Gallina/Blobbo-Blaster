using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Gun
{
    public string gunName;
    protected float damage;// = 23;
    public float reloadTime;//  = 2f;
    protected float fireRate;// = 0.3f;
    public int clipSize;//  = 16;
    protected bool isAutomatic;//  = false;
    public float lastShot = 0;
    public int clip;
    public bool reloading = false;
    public float accuracy;
    public GameObject ShotTracer;
    public int price;
    public GameObject lastTracer;
    public float damageDone = 0;
    public void setVals(string _gunName, float _damage, float _reloadTime, float _fireRate, int _clipSize, bool _isAutomatic, float _accuracy, GameObject _ShotTracer, int _price)
    {
        gunName = _gunName;
        damage = _damage;
        reloadTime = _reloadTime;
        fireRate = _fireRate;
        clipSize = _clipSize;
        isAutomatic = _isAutomatic;
        clip = clipSize;
        accuracy = _accuracy;
        ShotTracer = _ShotTracer;
        price = _price;
    }
    public void setVals(Gun other)
    {
        gunName = other.gunName;
        damage = other.damage;
        reloadTime = other.reloadTime;
        fireRate = other.fireRate;
        clipSize = other.clipSize;
        isAutomatic = other.isAutomatic;
        clip = clipSize;
        accuracy = other.accuracy;
        ShotTracer = other.ShotTracer;
        price = other.price;
    }
    public virtual void ShootGun(Vector2 playerCoords, Vector2 gunshot, bool clickFire, bool autoFire)
    {
        Debug.Log("wrong one");
    }
    public void reload()
    {
        if (clip != clipSize)
        {
            reloading = true;
            clip = clipSize;
            lastShot = Time.time;
        }
    }
}
public class Hitscan : Gun
{

    public override void ShootGun(Vector2 shotOrigin, Vector2 gunshot, bool clickFire, bool autoFire)
    {
        if (clickFire == false && autoFire == false)
        {
            return;
        }
        if (isAutomatic == false && clickFire == false)
        {
            return;
        }
        if (reloading == true)
        {
            if (Time.time - lastShot < reloadTime)
            {
                return;
            }
            reloading = false;
        }
        if (Time.time - lastShot < (1f / fireRate))
        {
            return;
        }

        Vector2 direction = new Vector2(gunshot.x - shotOrigin.x, gunshot.y - shotOrigin.y);
        direction.Normalize();
        Vector2 spread = Random.insideUnitCircle * accuracy;
        direction += spread;
        RaycastHit2D hit = Physics2D.Raycast(shotOrigin, direction, Mathf.Infinity, LayerMask.GetMask("Platform", "Enemies"));
        //Debug.DrawRay(playerCoords, direction, Color.red, 1f);
        if (hit.collider != null)
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemies"))
            {
                EnemyController enemy = hit.transform.gameObject.GetComponent<EnemyController>();
                enemy.TakeDamage(damage);
            }
        }
        GameObject tracer = GameObject.Instantiate(ShotTracer);
        LineRenderer shotTracer = tracer.GetComponent<LineRenderer>();
        Vector3[] positions = { gunshot, hit.point };
        //Debug.Log(mousePos);
        shotTracer.startWidth = 0.05f;
        shotTracer.endWidth = 0.05f;
        shotTracer.SetPositions(positions);
        lastShot = Time.time;
        clip--;
        GameObject.Destroy(tracer, 0.05f);
        if (clip <= 0)
        {
            reload();
        }
    }
}
public class Beam : Hitscan
{
    float ammoLoss;
    public override void ShootGun(Vector2 shotOrigin, Vector2 gunshot, bool clickFire, bool autoFire)
    {
        if (lastTracer != null)
        {
            GameObject.Destroy(lastTracer);
        }
        if (clickFire == false && autoFire == false)
        {
            return;
        }
        if (isAutomatic == false && clickFire == false)
        {
            return;
        }
        if (reloading == true)
        {
            if (Time.time - lastShot < reloadTime)
            {
                return;
            }
            reloading = false;
        }
        Vector2 direction = new Vector2(gunshot.x - shotOrigin.x, gunshot.y - shotOrigin.y);
        RaycastHit2D hit = Physics2D.Raycast(shotOrigin, direction, Mathf.Infinity, LayerMask.GetMask("Platform", "Enemies"));
        //Debug.DrawRay(playerCoords, direction, Color.red, 1f);
        if (hit.collider != null)
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemies"))
            {
                EnemyController enemy = hit.transform.gameObject.GetComponent<EnemyController>();
                enemy.TakeDamage(Time.deltaTime * damage);
                damageDone += Time.deltaTime * damage;
            }
        }
        GameObject tracer = GameObject.Instantiate(ShotTracer);
        LineRenderer shotTracer = tracer.GetComponent<LineRenderer>();
        Vector3[] positions = { gunshot, hit.point };
        //Debug.Log(mousePos);
        shotTracer.startWidth = 0.1f;
        shotTracer.endWidth = 0.1f;
        shotTracer.SetPositions(positions);
        if (gunName.Contains("Heal"))
        {
            shotTracer.material.color = new Color32(124, 255, 0, 255);
        }
        else
        {
            shotTracer.material.color = new Color32(0, 155, 255, 255);
        }
        lastShot = Time.time;
        ammoLoss += Time.deltaTime * 100;
        clip -= (int)ammoLoss;
        ammoLoss -= (int)ammoLoss;
        lastTracer = tracer;
        if (clip <= 0)
        {
            reload();
        }
    }
}
public class Projectile : Gun
{

}
public class Shotgun : Hitscan
{

    public override void ShootGun(Vector2 shotOrigin, Vector2 gunshot, bool clickFire, bool autoFire)
    {
        if (clickFire == false && autoFire == false)
        {
            return;
        }
        if (isAutomatic == false && clickFire == false)
        {
            return;
        }
        if (reloading == true)
        {
            if (Time.time - lastShot < reloadTime)
            {
                return;
            }
            reloading = false;
        }
        if (Time.time - lastShot < (1f / fireRate))
        {
            return;
        }

        for (int i = 0; i < 10; i++)
        {
            Vector2 direction = new Vector2(gunshot.x - shotOrigin.x, gunshot.y - shotOrigin.y);
            direction.Normalize();
            Vector2 spread = Random.insideUnitCircle * accuracy;
            direction += spread;
            RaycastHit2D hit = Physics2D.Raycast(shotOrigin, direction, Mathf.Infinity, LayerMask.GetMask("Platform", "Enemies"));
            //Debug.DrawRay(playerCoords, direction, Color.red, 1f);
            if (hit.collider != null)
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemies"))
                {
                    EnemyController enemy = hit.transform.gameObject.GetComponent<EnemyController>();
                    enemy.TakeDamage(damage);
                }
            }
            GameObject tracer = GameObject.Instantiate(ShotTracer);
            LineRenderer shotTracer = tracer.GetComponent<LineRenderer>();
            Vector3[] positions = { gunshot, hit.point };
            //Debug.Log(mousePos);
            shotTracer.startWidth = 0.05f;
            shotTracer.endWidth = 0.05f;
            shotTracer.SetPositions(positions);
            GameObject.Destroy(tracer, 0.05f);
        }
        lastShot = Time.time;
        clip--;
        if (clip <= 0)
        {
            reload();
        }
    }

}