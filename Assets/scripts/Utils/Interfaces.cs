using System.Collections;
using UnityEngine;

 
public interface IDamageable
{
    int Hp { get; set; }
    int MaxHp { get; set; }
    bool CanTakeDamage { get; set; }



    void takeDamage(int d);
    void die();
 

}
public interface IEnemyBehaviour 
{
    int Damage {  get; set; }
    void dealDamage(GameObject o);


}
public interface IStickable
{
    GameObject Sticked { get; set; } 
    float TimeStick {  get; set; }

    public void stickObject(GameObject collision);
    public void deStickObject();
     
}
public interface IRotable
{

}
public interface IBreakable{

    public void breakObject();
    public void repairObject();
}
public interface ILootable
{
    int LootCoins { get; set; }
    public void throwLoot();
    
}
 
