using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAimCorrection : MonoBehaviour
{
    public Transform targetTransform;
    public Transform turretTransform;
    public Transform weaponTransform;

    private float f, d, x, y, h, b, a, weaponAngle, turnAngle;
    private void Start()
    {
        d = Vector2.Distance(new Vector2(targetTransform.position.x, targetTransform.position.y), new Vector2(turretTransform.position.x, turretTransform.position.y));
        x = Vector2.Distance(new Vector2(turretTransform.position.x, turretTransform.position.y), new Vector2(weaponTransform.position.x, weaponTransform.position.y));
        weaponAngle = weaponTransform.localEulerAngles.z;
        weaponAngle = weaponAngle * Mathf.Deg2Rad;
        y = Mathf.Abs(Mathf.Cos(weaponAngle) * x);
        f = Mathf.Abs(Mathf.Sin(weaponAngle) * x);
        b = Mathf.Rad2Deg * Mathf.Acos(y / d);
        a = Mathf.Rad2Deg * Mathf.Acos(y / x);
        Vector2 turnVector = new Vector2(turretTransform.position.x - targetTransform.position.x, turretTransform.position.y - targetTransform.position.y);
        //turn turret towards target
        turretTransform.up = targetTransform.position - turretTransform.position;
        //adjust for gun angle
        if (weaponTransform.localEulerAngles.z <180)
            turretTransform.Rotate(Vector3.forward, 90 - b - a);
        else
            turretTransform.Rotate(Vector3.forward, 90 - b + a);
        
        print(weaponTransform.name + "//" + weaponTransform.localRotation.z + "//" + weaponTransform.localEulerAngles.z + "//" + turnVector);
    }

    private void Update()
    {
        d = Vector2.Distance(new Vector2(targetTransform.position.x, targetTransform.position.y), new Vector2(turretTransform.position.x, turretTransform.position.y));
        x = Vector2.Distance(new Vector2(turretTransform.position.x, turretTransform.position.y), new Vector2(weaponTransform.position.x, weaponTransform.position.y));
        weaponAngle = weaponTransform.localEulerAngles.z;
        weaponAngle = weaponAngle * Mathf.Deg2Rad;
        y = Mathf.Abs(Mathf.Cos(weaponAngle) * x);
        f = Mathf.Abs(Mathf.Sin(weaponAngle) * x);
        b = Mathf.Rad2Deg * Mathf.Acos(y / d);
        a = Mathf.Rad2Deg * Mathf.Acos(y / x);
        Vector2 turnVector = new Vector2(turretTransform.position.x - targetTransform.position.x, turretTransform.position.y - targetTransform.position.y);
        //turn turret towards target
        turretTransform.up = targetTransform.position - turretTransform.position;
        //adjust for gun angle
        if (weaponTransform.localEulerAngles.z < 180)
            turretTransform.Rotate(Vector3.forward, 90 - b - a);
        else
            turretTransform.Rotate(Vector3.forward, 90 - b + a);

        print(weaponTransform.name + "//" + weaponTransform.localRotation.z + "//" + weaponTransform.localEulerAngles.z + "//" + turnVector);
    }
}
