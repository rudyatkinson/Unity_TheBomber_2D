using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelDesigner : MonoBehaviour
{
    //CameraSize used to set some of attributes dynamically.
    private float cameraSize;

    //GameObject used to take last module position to build next one.
    public GameObject lastModuleGO;
    //ScriptableObject contains radius, module gameObject and usable module SO informations for next iteration.
    public Module lastModuleSO;

    //This transform used to sort modules in hierarchy.
    public Transform instantiatedModulesParent;

    private void Awake()
    {
        //Event Subscribe
        GEvents.BuildNextGeneration.AddListener(SpawnNext);

        //Declaring the main camera size.
        cameraSize = Camera.main.orthographicSize;

        //Instantiating first module.
        lastModuleGO = Instantiate(lastModuleSO.module, instantiatedModulesParent);
        lastModuleGO.transform.position = new Vector3(-2, -cameraSize);
    }

    /// <summary>
    /// This method spawn one of the next usable modules when the camera close to end point.
    /// </summary>
    private void SpawnNext()
    {
        //Pick random module inside of last module's scriptable object.
        var randomModule = UnityEngine.Random.Range(0, lastModuleSO.nextUsableModules.Length);
        Module newModule = lastModuleSO.nextUsableModules[randomModule];

        //Instantiating next module as gameObject and setting up its parent.
        GameObject go = Instantiate(newModule.module, instantiatedModulesParent);

        //Calculating end point of last module and center position of next module to set next module position.
        float endPointOfLastModule = lastModuleGO.transform.position.x + lastModuleSO.radius;
        float spawnPoint = endPointOfLastModule + newModule.radius;
        Vector3 spawnVector = new Vector3(spawnPoint, -cameraSize);
        go.transform.position = spawnVector;

        //Declaring new module scriptable object and game object as last module.
        lastModuleSO = newModule;
        lastModuleGO = go;
    }

    private void OnDestroy()
    {
        //Event remove if level designer has to destroyed.
        GEvents.BuildNextGeneration.RemoveListener(SpawnNext);

        if(AchievementManager._Instance.currentGameState == GameState.Playing)
        GEvents.GameOver.Invoke();
    }

}
