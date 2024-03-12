using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class learnHowToProgram : MonoBehaviour
{
    
    




        //variables for "if conditions" section
        public int HP;
        public int coins;

    void Start()
    {

        // iterations: help us to repeat code.
        int iterations=10;
        // start; condition(if is true continue, if is false the statement stop); steps
        for(int i=0;i<iterations;i++){
            Debug.Log("value of i is"+" "+ i);
            // the code will execute 10 times
        }

        //while loop, for some reason you cant create variables inside of them
        int i2=0;
        while(i2<10){
            Debug.Log("the value of i2 is" + " " +i2);
            i2++;
            
            
        }





        // if conditions
        switch(coins){
            case 0:
                Debug.Log("you have 0 coins");
                break;
            case <20:
                Debug.Log("you have less than 20 coins");    
                break;
            case >=20:
                Debug.Log("you have 20 or more coins");
                break;
        
        }

        if(HP<50){
            Debug.Log("You have less than 50 HP ");

        }
        if(HP>=50){
            Debug.Log("You have 50 or more points of HP ");
            
        }
        if(HP==0){
            Debug.Log("You ded bro ");
            
        }

    }

       
    

    // Update is called once per frame
    void Update()
    {




    }
}

