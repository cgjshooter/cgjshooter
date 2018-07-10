using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITarget {
    void hit(IAmmunition ammution);
    Vector3 m_Move { get; }
    float hitPoints { get; set; }
    bool dead { get; }
}
