using System;
using UnityEngine;
 
interface ILevelEntitiesInstantiationStrategy
{
    void instantiateEntities(GameObject bound);
    int Level { get; set; }
}
