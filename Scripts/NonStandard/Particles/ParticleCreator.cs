using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCreator : MonoBehaviour
{
    public ParticleSystem ps;
    public bool createOnStart;
    void Start() {
        if (createOnStart) {
            SpawnTemporary();
        }
    }
    public void SpawnTemporary() {
        GameObject go = Instantiate(ps.gameObject);
        Transform goT = go.transform;
        Transform t = transform;
        goT.position = t.position;
        goT.rotation = t.rotation;
        Destroy(go, ps.main.duration);
    }
}
