using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private CapsuleCollider col;
    private Animator anim;
    private Score score;
    private Vector3 directoin;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;
    [SerializeField] private int coins;
    [SerializeField] private GameObject scoreText;
    [SerializeField] private GameObject losePanle;
    [SerializeField] private Text coinsText;
    [SerializeField] private Score scoreScript;
    [SerializeField] private GameObject effect;
    [SerializeField] private GameObject boomEffect;



    private bool isSliding;
    private bool isImmortal;

    private int lineToMove = 1;//0 - left, 1 - mid, 2 - right;
    [SerializeField] private float lineDistance = 4;// distance between lines;
    private float maxSpeed = 110;

    void Start()
    {
        
        controller = GetComponent<CharacterController>();
        col = GetComponent<CapsuleCollider>();
        score = scoreText.GetComponent<Score>();
        anim = GetComponentInChildren<Animator>();
        score.scoreMultiplayer = 1;
        Time.timeScale = 1;
        coins = PlayerPrefs.GetInt("coins");
        //≈сли нужно чтобы отображало сколько монет сейчас пр€мо в игре
        coinsText.text = coins.ToString();
        StartCoroutine(SpeedIncrease());
        isImmortal = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (SwipeController.swipeRight)
        {
            if (lineToMove < 2)
            {
                lineToMove++;
                AudioManager.Instance.PlaySwipeSound();
            }

        }
        if (SwipeController.swipeLeft)
        {
            if (lineToMove > 0)
            {
                lineToMove--;
                AudioManager.Instance.PlaySwipeSound();
            }
        }
        if (SwipeController.swipeUp)
        {
            if(controller.isGrounded)
                Jump();
        }
        if (SwipeController.swipeDown)
        {
            StartCoroutine(Slide());
        }

        if (controller.isGrounded && !isSliding)
            anim.SetBool("Run", true);
        else
            anim.SetBool("Run", false);

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if (lineToMove == 0)
            targetPosition += Vector3.left * lineDistance;
        else if (lineToMove == 2)
            targetPosition += Vector3.right * lineDistance;

        if (transform.position == targetPosition)
            return;
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(moveDir);
        else
            controller.Move(diff);

        
        //speed += 0.1f * Time.deltaTime;
    }

    private void Jump()
    {
        directoin.y = jumpForce;
        anim.SetTrigger("Jump");
        AudioManager.Instance.PlayJumpSound();
    }

    private void FixedUpdate()
    {
        directoin.z = speed;
        directoin.y += gravity * Time.fixedDeltaTime;
        controller.Move(directoin * Time.fixedDeltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {

        if (hit.gameObject.tag == "obstacle")
        {
            if (isImmortal)
            {
                Instantiate(boomEffect, transform.position, Quaternion.identity);
                AudioManager.Instance.PlayExplosionSound();
                Destroy(hit.gameObject);
            }

            else
            {
                AudioManager.Instance.PlayEndSound();
                losePanle.SetActive(true);
                int lastRunscore = int.Parse(scoreScript.scoreText.text.ToString());
                PlayerPrefs.SetInt("lastRunScore", lastRunscore);
                Time.timeScale = 0;
            }
        }
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin")
        {
            coins++;
            PlayerPrefs.SetInt("coins", coins);
            coinsText.text = coins.ToString();
            AudioManager.Instance.PlayPickUpCoinSound();
            Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Star")
        {
            StartCoroutine(StarBonus());
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Shield")
        {
            StartCoroutine(Shield());
            Destroy(other.gameObject);
        }
    }
    // ”велечение скорости
    private IEnumerator SpeedIncrease()
    {
        yield return new WaitForSeconds(4);
        if (speed < maxSpeed)
        {
            speed += 3;
            StartCoroutine(SpeedIncrease());
        }
    }
    // »зменение размеров коллайдера дл€ кувырка 
    private IEnumerator Slide()
    {
        col.center = new Vector3(0, 0.4981972f, 0);
        col.height = 1.894737f;
        isSliding = true;
        anim.SetTrigger("Roll");
        AudioManager.Instance.PlayRollSound();
        yield return new WaitForSeconds(1);

        col.center = new Vector3(0, 2.891818f, 0);
        col.height = col.height = 5.283636f;
        isSliding = false;
    }

    private IEnumerator StarBonus()
    {
        score.scoreMultiplayer = 2;

        yield return new WaitForSeconds(5);

        score.scoreMultiplayer = 1;
    }

    private IEnumerator Shield()
    {
        isImmortal = true;

        yield return new WaitForSeconds(5);

        isImmortal = false;
    }
}
