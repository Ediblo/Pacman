using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Ghost[] ghosts; 

    public Pacman pacman;

    public Transform pellets;

    public int ghostMultiplier { get; private set; } = 1;

    public int score { get; set; }

    public int highestscore { get; set; }
    
    public int lives { get; private set; }

    public Text scoreText;

    public Text highestScoreText;

    public Text livesText;

    public Text gameOverText;

    public GameObject pauseMenu;
    
    [SerializeField] public AudioSource winSound;

    private void Start(){
        NewGame();
        Load();
        scoreText.text = score.ToString();
        highestScoreText.text = highestscore.ToString();
    }

    private void Update(){
        if(this.lives <= 0 && Input.anyKeyDown){
            NewGame();
        }
    }

    public void NewGame(){
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound(){
        gameOverText.enabled = false;
        foreach (Transform pellet in this.pellets)
        {
            pellet.gameObject.SetActive(true);
        }

        for(int i = 0; i < this.ghosts.Length; i++){
            this.ghosts[i].ResetState();
        }

        
        this.pacman.ResetState();
    }

    private void ResetState(){
        ResetGhostMultiplier();

        for(int i = 0; i < this.ghosts.Length; i++){
            this.ghosts[i].ResetState();
        }

        this.pacman.ResetState();
    }

    private void GameOver(){
        gameOverText.enabled = true;
        Save();
        for(int i = 0; i < this.ghosts.Length; i++){
            this.ghosts[i].gameObject.SetActive(false);
        }
        this.pacman.gameObject.SetActive(false);
    }

    private void SetScore(int score){
        this.score = score;
        scoreText.text = score.ToString();
        if(this.score > this.highestscore){
            this.highestscore = this.score;
            highestScoreText.text = highestscore.ToString();
        }
    }

    private void SetLives(int lives){
        this.lives = lives;
        livesText.text = "x" + lives.ToString();
    }

    public void GhostEaten(Ghost ghost){
        int points = ghost.points * this.ghostMultiplier;

        SetScore(this.score + points);

        this.ghostMultiplier++; 
    }

    public void PacmanEaten(){
        this.pacman.DeathSequence();

        for(int i = 0; i < this.ghosts.Length; i++){
            this.ghosts[i].GhostEatPacman();
        }

        SetLives(this.lives - 1);

        if(this.lives > 0){
            Invoke(nameof(ResetState), 3.0f);
        }
        else{
            Invoke(nameof(GameOver), 3.0f);
        }
    }

    public void PelletEaten(Pellet pellet){
        pellet.gameObject.SetActive(false);
        
        SetScore(this.score + pellet.points);

        if(!HasRemainingPellets()){
            this.pacman.gameObject.SetActive(false);
            winSound.Play();
            for(int i = 0; i < this.ghosts.Length; i++){
                this.ghosts[i].gameObject.SetActive(false);
        }
            Invoke(nameof(NewRound), 3.0f);
        }
    }

    public void PowerPelletEaten(PowerPellet pellet){

        for(int i = 0; i < this.ghosts.Length; i++){
            this.ghosts[i].frightened.Enable(pellet.duration);
        }

        CancelInvoke(); 

        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
        
        PelletEaten(pellet);


    }

    private bool HasRemainingPellets(){
        foreach(Transform pellet in this.pellets){
            if(pellet.gameObject.activeSelf){
                return true;
            }
        }
        return false;
    }

    private void ResetGhostMultiplier(){
        this.ghostMultiplier = 1;
    }

    public void Save(){
        if(this.score >= this.highestscore){
            PlayerPrefs.SetInt("highestscore", this.highestscore);
            PlayerPrefs.SetInt("score", this.score);
        }else if(this.score < this.highestscore){
            PlayerPrefs.SetInt("score", this.score);
        }else{
            PlayerPrefs.SetInt("highestscore", this.highestscore);
        }
    }

    public void Load(){
        this.score = PlayerPrefs.GetInt("score");
        this.highestscore = PlayerPrefs.GetInt("highestscore");
    }
}
