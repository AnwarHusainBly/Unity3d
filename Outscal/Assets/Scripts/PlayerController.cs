using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public Text redCubeScoreText;	//Disply red cube score 
    public Text redStreakText;	//Disply red Streak
    public Text blueCubeScoreText;	//Disply blue cube score
    public Text blueStreakText;	//Disply blue Streak
    public Text totalScore;	//Disply blue cube score
    public Text bestScore;	// Display Total Score
    public Text countdown; 	//UI Text Object
    public Text winText; 	//UI Text Object
    public float speed;	// Player movement speed
    public GameObject winPopUp;	//Game Win PopUp 
    public int timeLeft = 60; 	//Seconds Overall


    private Rigidbody rigidbody;
    private ItemLoader _itemLoader; // get the Item Loader scripts
    private int countSpawnpoints = 0; // total count to spwan point 
    private int scoreStreakRedCount = 0; //red cube score count 
    private int scoreStreakBlueCount = 0; // blue cube score count
    private int scoreRed, scoreBlue, TotalScore;


    internal enum GameState
    {
        GamePlay,
        GamePause,

    }
    internal GameState _gameState = GameState.GamePlay;



    void Start()
    {
        _gameState = GameState.GamePlay;
        StartCoroutine("UpdateTimer");
        bestScore.text = "Best Score : "+PlayerPrefs.GetInt( "High Score: ",0).ToString();
        rigidbody = GetComponent<Rigidbody>();
        _itemLoader = GameObject.FindObjectOfType<ItemLoader>();
        Invoke("Delay",0.2f);
    }

    /// <summary>
    /// This Delay Method is called to Wait For The CountSpawnPoints For Next After load ItemLoader Scripts..
    /// </summary>
    void Delay()
    {
        if (_itemLoader.numberOfCube != null)
            countSpawnpoints = _itemLoader.numberOfCube;
    }

    private void Update()
    {
        if (_gameState == GameState.GamePause)
            return;
        countdown.text = ("Timer : " + timeLeft); //Showing the Timer
    }
    // Before physics calculations
    void FixedUpdate()
    {
        if (_gameState == GameState.GamePause)
            return;

        if (TotalScore > PlayerPrefs.GetInt("High Score: ", 0))
        {
            PlayerPrefs.SetInt("High Score: ", TotalScore);
            bestScore.text = "Best Score : " + PlayerPrefs.GetInt("High Score: ", 0).ToString();
        }

		#if UNITY_EDITOR
        float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");
		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
		rigidbody.AddForce(movement * speed * Time.deltaTime);
		#elif
		speed = 250f;
		Vector3 acc = Input.acceleration;
		rigidbody.AddForce(acc.x * speed * Time.deltaTime, 0, acc.y * speed * Time.deltaTime);
		#endif
	}
   
	void OnTriggerEnter(Collider other)
    {
		Debug.Log ("Cube name = " + other.gameObject.name);
        if (other.gameObject.name.Equals("RedCube"))
        {
            scoreStreakBlueCount = 0;
            blueStreakText.text = "Blue Streak : " + scoreStreakBlueCount;
            countSpawnpoints--;
            other.gameObject.SetActive(false);
            scoreStreakRedCount++;
//            Debug.Log("scoreStreakRedCount = " + scoreStreakRedCount + ", " + _itemLoader.ScoreUpdateRed);
            scoreRed += (_itemLoader.ScoreUpdateRed * scoreStreakRedCount);
            
            redCubeScoreText.text = "RedCubeScore : " + scoreRed.ToString();
            redStreakText.text = "Red Streak : " + scoreStreakRedCount;
            TotalScore += scoreRed;
            CheckForGameWin();
        }
        else if (other.gameObject.name.Equals("BlueCube"))
        {
            scoreStreakRedCount = 0;
            redStreakText.text = "Red Streak : " + scoreStreakRedCount;
            other.gameObject.SetActive(false);
            countSpawnpoints--;
            scoreStreakBlueCount++;
//            Debug.Log("BlueCubeScore = " + scoreStreakBlueCount + ", " + _itemLoader.ScoreUpdateBlue);
            scoreBlue += (_itemLoader.ScoreUpdateBlue * scoreStreakBlueCount);
            
            
            blueCubeScoreText.text = "BlueCubeScore : " + scoreBlue.ToString();
            blueStreakText.text = "Blue Streak : " + scoreStreakBlueCount;
            TotalScore += scoreBlue;
            CheckForGameWin();
        }
	}

    IEnumerator UpdateTimer()
    {
        while (true)
        {
           
            yield return new WaitForSeconds(1);
			if (timeLeft > 0)
			{
				timeLeft--;               
			} 
			else
			{
				StopAllCoroutines ();
				winPopUp.SetActive (true);
				winText.text = "Congratulations you gained " + (scoreRed + scoreBlue + timeLeft).ToString () + " scores";
				_gameState = GameState.GamePause;
			}
        }
    }

    void CheckForGameWin()
    {
		if (countSpawnpoints == 0)
		{
			winPopUp.SetActive (true);
			StopAllCoroutines ();
			winText.text = "Congratulations you gained " + (scoreRed + scoreBlue + timeLeft).ToString () + " scores";
			_gameState = GameState.GamePause;
		}
    }
}