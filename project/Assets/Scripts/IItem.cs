using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem {
    
    //Is the item toggleable or not.
    bool toggleable { get; }
    //Is the item activated immediatly on pickup
    bool useOnPickup { get; }
    //How many times the item can be used. -1 for infinite.
    int useCount { get; }

    //How powerfull the item is (weapon damage, heal amount, shield hitpoints etc);
    //TODO - this might not be needed in the interface?
    float power { get; }

    void activate(GameObject player);
    void deactivate(GameObject player);
}
