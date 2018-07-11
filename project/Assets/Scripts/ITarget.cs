using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITarget {
    void hit(IAmmunition ammution);
    Vector3 m_Move { get; }
    float hitPoints { get; set; }
    float armor { get; set; }
    bool invulnerable { get; set; }
    bool invisible { get; set; }
    bool dead { get; }
    float maxHealth { get; }
}
