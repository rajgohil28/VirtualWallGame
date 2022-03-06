using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BallSinglePlayer : MonoBehaviour
{
    public float initalVelocity = 10;
    public float velocityMultiplier = 1.05f;
    public float randomDirectionMultiplier = 0.1f;
    public float maxRaycastDist = 1f;
    public float force = 0.1f;
    public GameObject gameOverPanel;
    private XRInteractorLineVisual[] lineVisualiser;


    public float spinRadius = 2f;
    public float spinSpeed = 0.1f;
    public float spinTime = 4f;
    bool shouldSpin = true;
    float spinStartTime;

    Rigidbody rb;
    Vector3 initialPos;
    GameManagerSinglePlayer gm;
    // Start is called before the first frame update
    void Start()
    {
        lineVisualiser = FindObjectsOfType<XRInteractorLineVisual>();
        foreach (XRInteractorLineVisual visualizer in lineVisualiser)
        {
            visualizer.enabled = false;
        }
        rb = GetComponent<Rigidbody>();
        initialPos = transform.position;
        StartSpin();
        gm = GameManagerSinglePlayer.Instance;

    }

    private void Initialize()
    {
        rb.velocity = new Vector3(
            Random.Range(-randomDirectionMultiplier, randomDirectionMultiplier),
            Random.Range(-randomDirectionMultiplier, randomDirectionMultiplier),
            initalVelocity);
    }
    private void StartSpin()
    {
        FindObjectOfType<AudioManager>().Play("StartSpin");
        transform.position = initialPos;
        shouldSpin = true;
        spinStartTime = Time.time;

    }
    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, rb.velocity, out hit, maxRaycastDist))
        {
            if (hit.transform.tag == "Colliders2")
            {
                Vector3 forceVect = Vector3.Normalize(new Vector3(-transform.position.x, -transform.position.y, 0));
                rb.AddForce(forceVect * force);
            }
        }
        if (shouldSpin)
        {
            transform.position = new Vector3(Mathf.Sin(Time.time * spinSpeed) * spinRadius, Mathf.Cos(Time.time * spinSpeed) * spinRadius, initialPos.z);
            if (Time.time - spinStartTime > spinTime)
            {
                shouldSpin = false;
                FindObjectOfType<AudioManager>().Play("BallGo");
                Initialize();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "GameOverWall")
        {
            //GameOver();
            FindObjectOfType<AudioManager>().Play("GameOver");
            rb.velocity *= 0f;
            gm.ScoreManager.DecreaseLive();
            if (gm.ScoreManager.Lives == 0)
                GameOver();
            else
                StartCoroutine(WaitAfterOut());

        }
        if (collision.transform.tag == "Colliders")
        {
            rb.velocity = new Vector3(
                Random.Range(-randomDirectionMultiplier, randomDirectionMultiplier) + rb.velocity.x,
                Random.Range(-randomDirectionMultiplier, randomDirectionMultiplier) + rb.velocity.y,
                velocityMultiplier * rb.velocity.z);
            FindObjectOfType<AudioManager>().Play("Bounce");

        }

        if (collision.transform.tag == "Player")
        {
            Vector3 velocity = rb.velocity;
            Vector3 reflectVelocity = Vector3.Reflect(velocity, Vector3.back);
            rb.velocity = -reflectVelocity;
            gm.ScoreManager.IncreaseScore();
            FindObjectOfType<AudioManager>().Play("PlayerHit");

        }
    }
    IEnumerator WaitAfterOut()
    {
        yield return new WaitForSeconds(2f);
        StartSpin();
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        rb.velocity *= 0f;
        foreach (XRInteractorLineVisual visualizer in lineVisualiser)
        {
            visualizer.enabled = true;
        }
        Debug.Log("GameOver");
    }

    public void Restart()
    {
        foreach (XRInteractorLineVisual visualizer in lineVisualiser)
        {
            visualizer.enabled = false;
        }
        StartSpin();
        gameOverPanel.SetActive(false);
        gm.ScoreManager.Reset();

    }
}
