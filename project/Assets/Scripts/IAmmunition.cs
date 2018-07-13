using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAmmunition {

    //Direction of the ammunition
    Vector3 direction { get; set; }
    //Damage of the ammunition
    float damage { get; set; }
    //Radius of the ammunition effect.
    float effectRadius { get; set; }

    float bulletToShotRatio { get; set; }

    GameObject shooter { get; set; }

    void affect(GameObject target);


}
