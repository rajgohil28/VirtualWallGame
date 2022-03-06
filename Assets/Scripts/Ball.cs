using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using TMPro;
public class Ball : MonoBehaviourPunCallbacks
{
    public float initalVelocity = 10;
    public float velocityMultiplier = 1.05f;
    public float randomDirectionMultiplier = 0.1f;
    public float maxRaycastDist = 1f;
    public float force = 0.1f;
    public GameObject gameOverPanel;
    public TMP_Text WinnersText;

    public float spinRadius = 2f;
    public float spinSpeed = 0.1f;
    public float spinTime = 4f;
    bool shouldSpin = true;
    float spinStartTime;
    bool GameStarted;
    int Turns;
    float ReturnMultiplier;

    Vector3 lastVelocity;
    Rigidbody rb;
    Vector3 initialPos;
    public static PhotonView PV;
    GameManager gm;
    // Start is called before the first frame update
    private void Awake()
    {
        PV = gameObject.GetComponent<PhotonView>();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPos = transform.position;
        GameStarted = false;
        Turns = 1;
        StartSpin();
        gm = GameManager.Instance;
    }

    private void Initialize()
    {
        if (!PV.IsMine)
            return;
        int multiplier = (Turns % 2 == 0) ? -1 : 1;
        rb.velocity = new Vector3(
            Random.Range(-randomDirectionMultiplier, randomDirectionMultiplier),
            Random.Range(-randomDirectionMultiplier, randomDirectionMultiplier),
            multiplier * initalVelocity);
        Turns++;
    }
    private void StartSpin()
    {
        if (!PV.IsMine)
            return;
        FindObjectOfType<AudioManager>().Play("StartSpin");
        transform.position = initialPos;
        shouldSpin = true;
        spinStartTime = Time.time;
    }
    // Update is called once per frame
    void Update()
    {
        lastVelocity = rb.velocity;
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.PlayerList.Length == 2)
        {
            if (PhotonNetwork.PlayerList[0].GetScore() > 5 || PhotonNetwork.PlayerList[1].GetScore() > 5)
            {
                GameOver();
            }
        }
        if (!PV.IsMine)
            return;
        if (PhotonNetwork.PlayerList.Length > 1)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, rb.velocity, out hit, maxRaycastDist))
            {
                if (hit.transform.tag == "Colliders2") //When The ball hit sidewalls
                {
                    Vector3 forceVect = Vector3.Normalize(new Vector3(-transform.position.x, -transform.position.y, 0));
                    rb.AddForce(forceVect * force);
                }
            }
            if (shouldSpin) //For Ball Spin
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

        Debug.Log(rb.velocity.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!PV.IsMine)
            return;
        if (collision.transform.name == "PlayerOneWall")
        {
            FindObjectOfType<AudioManager>().Play("GameOver");
            rb.velocity *= 0f;
            if (PV.IsMine)
            {
                gm.ScoreManager.IncreaseScorePlayerTwo();
            }
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

        if(collision.transform.name == "PlayerTwoWall")
        {
            FindObjectOfType<AudioManager>().Play("GameOver");
            rb.velocity *= 0f;

            if (PV.IsMine)
            {
                gm.ScoreManager.IncreaseScorePlayerOne();
            }
            StartCoroutine(WaitAfterOut());
            
                
        }
        if(collision.transform.tag == "Player")
        {
            /*Vector3 velocity = rb.velocity;
            Vector3 reflectVelocity = Vector3.Reflect(velocity, Vector3.back);
            rb.velocity = -reflectVelocity;*/
            Bounce(collision.contacts[0].normal);
            Debug.Log("Hit Player");
            //gm.ScoreManager.IncreaseScore();
            FindObjectOfType<AudioManager>().Play("PlayerHit");

        }
    }

    private void Bounce(Vector3 collisionNormal)
    {
        var speed = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized, collisionNormal);

        Debug.Log("Out Direction: " + direction);
        rb.velocity = direction * Mathf.Max(speed, 2f);
    }
    IEnumerator WaitAfterOut()
    {
        yield return new WaitForSeconds(2f);
        StartSpin();
    }

    public void GameOver()
    {
        if (PhotonNetwork.PlayerList[0].GetScore() > 5)
            WinnersText.text = "Player One Won";
        else
            WinnersText.text = "Player Two Won";
        gameOverPanel.SetActive(true);
        rb.velocity *= 0f;
        Debug.Log("GameOver");
    }

    public void Restart()
    {
        StartSpin();
        gameOverPanel.SetActive(false); 
        gm.ScoreManager.Reset();


    }

    [PunRPC]
    static void RPC_IncreasePlayerOneScore()
    {
        if (!PV.IsMine)
            return;
        else
            ScoreManager.m_ScoreOne++;
    }
    [PunRPC]
    static void RPC_IncreasePlayerTwoScore()
    {
        if (!PV.IsMine)
            return;
        else
            ScoreManager.m_ScoreTwo++;
    }
}
