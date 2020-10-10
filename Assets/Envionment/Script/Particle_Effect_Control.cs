using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Effect_Control : MonoBehaviour
{
    public ParticleSystem myParticle;

    
    // Start is called before the first frame update
    void Start()
    {
        myParticle = GetComponent<ParticleSystem>();
        var par = myParticle.emission;

        par.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
