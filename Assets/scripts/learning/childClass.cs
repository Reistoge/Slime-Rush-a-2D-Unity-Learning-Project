using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : EnemyClass
    //Warrior derives from the enemy class
{
     
    string armor;

    //why ?? the variable health exist in the parent class 
    //int health; ----> because health is private in enemy so in warrior it doesnt exist or you cant use it.
     
 
    
    // unity you cant create multiple inheritances this means that you only create childs classes not a class that is child of other.
    public string Armor //get&setter.
    {
        get { return armor; } set {armor = value;} 
    }
    
 
    // the warrior class has the same public variables but differents contructor so you have to create another one.
    public Warrior()
    {
        Name = string.Empty;
        Weapon = "BadSword";
        Health = 1;
        
    }
     public Warrior(string name, int health, int strength, string weapon, string armor)
    {
        Name = name;
        Weapon = weapon;
        Health= health;
        Strength= strength;
        this.armor = armor;
        //  this.health ----> error ---> why ?--->  you can only use health in enemy because is private
        // so we use the public variables of the parent to acces them an assign in the constructor.

    }
    public void DisplayArmor()
    {
        //Debug.Log("The armor of the Warrior " + Name + " is "+armor);
    }
    public override void talk()
    {

        //Debug.Log(Name + " is talking, also he wants to fight you!");

    }

}
