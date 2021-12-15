using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCounter : MonoBehaviour
{
    private int NumberOfEnemies;
    private int ActiveEnemies;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        List<GameObject> goList = new List<GameObject>();
        for (int i = 0; i < goArray.Length; i++)
        {
            if (goArray[i].layer == LayerMask.NameToLayer("whatIsEnemy")) { goList.Add(goArray[i]); }
        }
        //if (goList.Count == 0) { Debug.Log("Array vacía"); }
        //Debug.Log(goList.Count/*ToArray()*/);
        NumberOfEnemies = goList.Count;
        ActiveEnemies = NumberOfEnemies;
        gameObject.GetComponent<Text>().text = "Enemigos" + "\n" + ActiveEnemies.ToString() + " / " + NumberOfEnemies;
    }

    public void UpdateEnemies()
    {
        //FindGameObjectsWithLayer(LayerMask.NameToLayer("whatIsEnemy"));
        ActiveEnemies--;
        gameObject.GetComponent<Text>().text = "Enemigos" + "\n" + ActiveEnemies.ToString() + " / " + NumberOfEnemies;
        if(ActiveEnemies == 0) { GameObject.Find("HudEnabler").GetComponent<HudMenu>().EndDemo();}
    }
}
