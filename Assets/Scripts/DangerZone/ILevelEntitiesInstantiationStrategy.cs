using System;
using UnityEngine;
 
interface ILevelSpawner
{
    void instantiateEntities(GameObject bound);
    int Level { get; set; }
}
