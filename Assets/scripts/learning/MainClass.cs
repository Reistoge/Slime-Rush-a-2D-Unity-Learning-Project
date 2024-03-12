using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Start is called before the first frame update
public class EnemyClass
{   

    int health;
    int strength;
    string name_data;
    string weapon;
   

    public EnemyClass()
    {
        // this is the default constructor, create a object with default values, this happens when you create an object without parameters.
        health = 1;
        strength = 1;
        name_data= string.Empty;
    }
    public EnemyClass(string name,int health, int strength, string weapon)
    {
        //"this" refers to the main variable and the variable that we assign is the parameter.
        this.health = health;
        this.strength = strength;  
        this.name_data= name;
        this.weapon = weapon;
        //Debug.Log("you create a Enemy called: "+name);
        //Debug.Log("the health is "+ health);
        //Debug.Log("the strength is " + strength);
        //Debug.Log("the enemy " + name + " have a " + weapon);
    }


    // we want to acces to a attribute of a class, to do this we have to create accesibility modifiers, to acces to private variables.
    //two forms to acces of data encapsulation:

    // you can set the private variable by two forms, one by a variable and the other by a method, both have to be the public.
    // can you create a method that adds an rigidbody a sprite renderer all that stuff ??
        
    // this happens because the attributes of the class enemy and also warrior(child class) are private so we cant acces and set to something by using a dot
    // we create a public variable that will set and call the private variable

    // in this case you make the attribute health of the class Enemy accesible by the Public variable Health

    // all this variable also exist in the child class so we can reuse it in the constructor.

    public int Health
    {
        get { return health; }
        set { health = value; }
    }
    public string Weapon
    {
        get { return weapon;}
        set { weapon = value; }
    }
    public string Name
    {
        get { return name_data; }
        set { name_data= value; }
    }
    public int Strength
    {
        get { return strength; }
        set { strength = value; }
    }
    // the second case you make the health attribute of the class Enemy accesible by the functions getHealth and setHealth
    public int getHealth()
    {
        return health;
    }
    public void setHealth(int value)
    {
        value= health;
         
    }

    public string getName()
    {
        return name_data;
    }
    public void setName(string value)
    {
        value = name_data;

    }

    public int getStrength()
    {
        return strength;
    }
    public void setStrength(int value)
    {
        value =strength;

    }

    public string getWeapon()
    {
        return weapon;
    }
    public void setWeapon(string value)
    {
        value = weapon;

    }


    // methods of a class, they are functions of the classes
    public void attack(string TargetName)
    {
        //Debug.Log("The Enemy " + name_data+ " is attacking with a "+Weapon+" to "+TargetName);
    }
    

    // override functions: a function that can  be changed in a inherited class

    public virtual void talk()
    {

        //Debug.Log("blah blah blah blah, ENEMY: " + name_data+ " is talking");
    }


     
}


