using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModifier  {
    int minimumPlayers { get; }
    int maximumPlayers { get; }
    string description { get; }

    void apply();
    

}
