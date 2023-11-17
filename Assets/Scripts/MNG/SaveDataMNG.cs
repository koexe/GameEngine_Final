using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataMNG : MonoBehaviour
{
    public List<Trigger> g_PlayerTriggerList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public class Trigger
    {
        public string name;
        public bool isTriggered;
    }


}
