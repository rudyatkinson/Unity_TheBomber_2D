using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Module", menuName = "Generation/Module")]
public class Module : ScriptableObject
{
    public float radius;
    public GameObject module;
    public Module[] nextUsableModules;
}
