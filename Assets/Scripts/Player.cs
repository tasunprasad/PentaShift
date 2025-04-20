using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public float jumpForce = 10f;

    public GameObject particle;

	public Rigidbody2D rb;
	public SpriteRenderer sr;
    private bool playStarted;
    public GameObject lostPrompt;
    public static int score;
    public static Text scoreBox;
    public static bool isAlive;
    public static int highScore;
    public Text highScoreBox;
    public Text scoreBoxPrompt;
    public GameObject highScoreCongratulate;

    public Color[] color;
    public string[] colorName;
    public int currentColor;

	void Start () {
        highScore = PlayerPrefs.GetInt("score", 0);
        isAlive = true;
        score = 0;
        scoreBox = GameObject.Find("ScoreBox").GetComponent<Text>();
		SetRandomColor();
        lostPrompt.SetActive(false);
        playStarted = false;
	}
	
	void Update () {
        //at the beginning of scene, till screen is touched, don't wake up player, to remove idle deaths
        if (!playStarted) {
            rb.Sleep();
            if (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0)) {
                rb.WakeUp();
                rb.velocity = Vector2.up * jumpForce;
                playStarted = true;
            }
        }
        //move player if he is alive and then screen is touched
        if (isAlive) {
            if (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0)) {                  //Input.GetButtonDown("Jump") || <- for development
                rb.velocity = Vector2.up * jumpForce;
            }
        }
        
	}
	void OnTriggerEnter2D (Collider2D col) {
        //If player goes below screen, game over
        if (col.tag == "MainCamera" && isAlive) {
            Lost();
            return;
        }
        if (col.tag == "Star") {
            return;
        }
        //If player touches ColorChanger, change color and then consume it
        if (col.tag == "ColorChanger") {
			IncrementColor();
			Destroy(col.gameObject);
			return;
		}
        //If player touches wrong color, game over
        if (/*col.tag != colorName[currentColor] &&*/ isAlive) {
            Lost();
            return;
        }
    }
    //When encountering a colorChange object, change color to next one
    void IncrementColor() {
        currentColor = (currentColor + 1) % 4;
        sr.color = color[currentColor];
         
    }
    //if player loses then show prompt, set values in textBoxes
    public void Lost() {
        highScoreCongratulate.SetActive(false);
        if (score > highScore) {
            highScore = score;
            PlayerPrefs.SetInt("score", score);
            highScoreCongratulate.SetActive(true);
        }
        highScoreBox.text = "High Score : " + highScore;
        scoreBoxPrompt.text = "Your score: " + score;
        GameObject ps = Instantiate(particle, gameObject.transform.transform.position, Quaternion.identity);
        ParticleSystem.MainModule main = ps.GetComponent<ParticleSystem>().main;
        main.startColor = color[currentColor];
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        rb.Sleep();
        
        isAlive = false;
        StartCoroutine(Prompt());
    }
    //show prompt after a short delay
    private IEnumerator Prompt() {
        yield return new WaitForSeconds(.7f);
        lostPrompt.SetActive(true);
    }
    //Send score to other apps
    public void ShareScore() {
        ScoreSubmission.SendScore(score);
    }
    //Start new game
    public void NewGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    //Function to change color of player at load scene
	void SetRandomColor () {
		int index = Random.Range(0, 4);
		switch (index) {
			case 0:
				currentColor = 0;
				sr.color = color[currentColor];
				break;
			case 1:
				currentColor = 1;
				sr.color = color[currentColor];
				break;
			case 2:
				currentColor = 2;
				sr.color = color[currentColor];
				break;
			case 3:
				currentColor = 3;
				sr.color = color[currentColor];
				break;
		}
	}
}
