using UnityEngine;

public class ignoreThisScriptPlease : MonoBehaviour
{

    void Start()
    {
        //spawn object
        GameObject objToSpawn = new GameObject("Cool GameObject made from Code");
        //Add Components
        objToSpawn.AddComponent<Rigidbody>();
        objToSpawn.AddComponent<MeshFilter>();
        objToSpawn.AddComponent<BoxCollider>();
        objToSpawn.AddComponent<MeshRenderer>();
        objToSpawn.GetComponent<Rigidbody>().useGravity = false;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject objToSpawn = new GameObject("random object spawned after pressing 'p'");
        }
    }

}
