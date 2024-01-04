using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class FlappyControl : MonoBehaviour
{
    
    public Rigidbody2D rigibody2D;
    public Animator animator;
    public float force;
    public AudioSource audioSource;
    public AudioClip flyClip, pingClip, dieClip;
    public bool isAlive;
    public bool isFade=false;
    public int score;
    private Vector3 originalScale;
    private bool issmall = false;
    private bool isbig = false;
    private GameObject objectToFade;
    private GameObject[] scoreobj;
    public GameObject scoreob;
    public float fadeDuration = 2.0f;
    public float displayDuration = 5.0f;

    // Start is called before the first frame update
    public static FlappyControl Instance;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        

        objectToFade = gameObject;
        originalScale = transform.localScale;
        isAlive = false;
        rigibody2D.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        scoreobj = GameObject.FindGameObjectsWithTag("Score");
        if (scoreobj.Length > 0)
        {
            Vector2 scalegoc = new Vector2(scoreob.transform.localScale.x, scoreob.transform.localScale.y);

            if (isFade)
            {


                foreach (GameObject scoreobj in scoreobj)
                {
                    scoreobj.transform.localScale = new Vector2(scalegoc.x, scalegoc.y * 4);
                }
            }
            else
            {
                foreach (GameObject scoreobj in scoreobj)
                {
                    scoreobj.transform.localScale = scalegoc;
                }
            }
        }
        
        if(isAlive && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) ))
        {
            rigibody2D.AddForce(Vector2.up * force);
            audioSource.PlayOneShot(flyClip);
        }
        if(rigibody2D.velocity.y > 0)
        {
            float angle = Mathf.Lerp(0, 90, rigibody2D.velocity.y / 7);
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            float angle = Mathf.Lerp(0, -90, -rigibody2D.velocity.y / 7);
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        
        
    }
    public void  OnInit()
    {
        isAlive = true;
        score = 0;
        rigibody2D.gravityScale = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Score")
        {
            
            audioSource.PlayOneShot(pingClip);
            score++;
            UIManager.instance.scoreUI.text = score.ToString();
            


        }
       

        if (collision.tag == "buffsmall")
        {
            issmall = true;
            StartCoroutine(ScaleDownAndRestore());


        }
        if (collision.tag == "buffbig")
        {
            isbig = true;
            StartCoroutine(ScaleUpAndRestore());


        }
        if (collision.tag == "bufffade")
        {
            isFade = true;
            
            StartCoroutine(Fade());
           
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(isAlive && (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Pipe") )
        {
            isAlive = false;
            audioSource.PlayOneShot(dieClip);
            animator.SetTrigger("die");
            GameManager.instance.isGamePlay = false;
            UIManager.instance.FinishGame();
        }
       
    }
    IEnumerator ScaleDownAndRestore()
    {
        while (issmall)
        {
            
            while (transform.localScale.x > originalScale.x / 2 && issmall)
            {
                transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                yield return new WaitForSeconds(0.1f);
            }

           
            yield return new WaitForSeconds(5f);

            
            while (transform.localScale.x < originalScale.x && issmall)
            {
                transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
                yield return new WaitForSeconds(0.1f);
               ;
            }
            issmall = false;
        }
        
    }
    IEnumerator ScaleUpAndRestore()
    {
        while (isbig)
        {

            while (transform.localScale.x < originalScale.x * 1.5 && isbig)
            {
                transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
                yield return new WaitForSeconds(0.1f);
            }


            yield return new WaitForSeconds(5f);


            while (transform.localScale.x > originalScale.x && isbig)
            {
                transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                yield return new WaitForSeconds(0.1f);
                ;
            }
            isbig = false;
        }

    }
    IEnumerator Fade()
    {
        
        gameObject.layer = LayerMask.NameToLayer("Water");


        

        
       
        float startTime = Time.time;
        Color originalColor = objectToFade.GetComponent<Renderer>().material.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
        while (Time.time - startTime < fadeDuration)
        {
            float t = (Time.time - startTime) / fadeDuration;
            objectToFade.GetComponent<Renderer>().material.color = Color.Lerp(originalColor, targetColor, t);
            yield return null;
        }

        
        yield return new WaitForSeconds(displayDuration);

       
        startTime = Time.time;
        while (Time.time - startTime < fadeDuration)
        {
            float t = (Time.time - startTime) / fadeDuration;
            objectToFade.GetComponent<Renderer>().material.color = Color.Lerp(targetColor, originalColor, t);
            yield return null;
        }

        gameObject.layer = LayerMask.NameToLayer("Default");
        isFade = false;
    }

}


