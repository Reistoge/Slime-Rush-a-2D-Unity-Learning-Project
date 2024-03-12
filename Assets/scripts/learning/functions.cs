using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class functions : MonoBehaviour
{
   
    void Start()
    {
        // you can debug.log this functions because it return something, this means that converts into something or into a value, 
        // if the function just execute code will not convert into something it will just do things
        calculateTwoNumbers();
        Debug.Log(compareThreeNumbers(3,5,2));
        Debug.Log(calculateOtherTwoNumbers());

        void calculateTwoNumbers(){
            int a=3;
            int b=5;
            int c;
            c=a+b;
            // void means that not return something
            // if you execute this function the code will execute so the c will be assigned to a+b
            // 1. int a, b, c is created
            // 2. c equals a+b
            // just happen that, even if you call this function the values of c will not show because you only assign c to something.
        }
        
        int calculateOtherTwoNumbers(){
            int d=1;
            int e=7;
            int f=4;

            //YOU CAN also do this but is not practical.
            //int g= d-f*e;
            //return g;

            return d-f*e;

            // if you execute this function the code will execute but also the function will return the value of d-f*e
            // when it returns something, the function execute the code and returns something it converts into the thing that you wanna return.

            

        }

        bool compareThreeNumbers(float a, float b, float c){
            // function with parameters, this functions need variables after 
            //  be executed this variables will be used inside of the function, you can put whatever variable you want
            // also the function with parameters can return anything (if you dont put void )
            

            // i want a function that compare any three numbers and give/return me a true or false boolean  
            if(a+b < c){
                Debug.Log("GAME OVER");
                return true;
            }
            else{
                return false;
            }
            

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
