using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PipeControl : MonoBehaviour
{
    public GameObject pipePrefab;
    float time;
    public List<GameObject> gameObjectsList;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        if (GameManager.instance.isGamePlay)
        {
            time += Time.deltaTime;
            if (time > 1.5f)
            {
                time = 0;
                GameObject pipepre = Instantiate(pipePrefab, new Vector2(5, Random.Range(-2f, -0.5f)), Quaternion.identity);
                StartCoroutine(pipe(pipepre));
            }
        }

    }
    IEnumerator pipe(GameObject obj)
    {

        float randomValue = Random.Range(0f, 1f);


        if (randomValue <= 0.25f)
        {





            if (obj != null)
            {

                GameObject item = Instantiate(gameObjectsList[Random.Range(0, gameObjectsList.Count)]);

                item.transform.position = new Vector2(obj.transform.position.x - 1f, obj.transform.position.y);
                GameObject pipetr = obj.transform.Find("Pipe").gameObject;
                

                item.transform.SetParent(pipetr.transform);
            }
            else { }

           
        } 
        yield return null;
    }
}
