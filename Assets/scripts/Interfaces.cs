using System.Collections;
using UnityEngine;

public interface IRotable
{

    float RotAngle { get; set; }
    float RotSpeed { get; set; }
    float RotDelay { get; set; }

    IEnumerator RotateAngles(float angle, float rotSpeed);
    IEnumerator RotateBetweenAngle(float angle, float rotSpeed);
    IEnumerator RotateFor(GameObject Player, float time, float RotVel);








}
public interface IDamageable
{
    int Hp { get; set; }
    int MaxHp { get; set; }



    void takeDamage(int d);
    void die();
 


}