using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RouletteManager : MonoBehaviour
{
    public UIPanelFadeIn UIPanelFadeIn;
    //Audio
    public AudioSource audioSource; // Reference to the AudioSource
    public AudioClip startGameAudio; // Audio clip to play at the start

    int nextBet = 0;
    bool asigned = false;

    public GameObject[] bets;
    public GameObject actualBet;

    //dinero
    public int money = 10;
    int roundSumBlack = 0;
    int roundSumRed = 0;
    public TextMeshProUGUI moneyText;

    //winner
    string winner = "";

    //posiciones spawn
    public Transform PosRoja;
    public Transform PosNegra;

    public CanvasGroup betButtons;
    public CanvasGroup timerCanvasGroup;
    public CanvasGroup canvasBets;
    public CanvasGroup numeroCanvas;
    public TextMeshProUGUI timerText;

    //Apuesta ganadora
    public Image apuestaGan;
    public Sprite numeroVerde;
    public Sprite numeroRojo;
    public Sprite numeroNegro;
    public TextMeshProUGUI apuestaText;
    bool numActualRojo = false;

    //Lista de instanciados
    List<GameObject> instancedBets = new List<GameObject>();

    // Timer and game state
    private float countdownTimer = 10f; // 15 seconds for betting phase
    private enum GameState { Starting, Betting, EndBetting, Playing, EndPlaying }
    private GameState currentState = GameState.Starting;

    void Start()
    {
        StartGame();
    }


    private void Update()
    {
        if (currentState == GameState.Betting)
        {
            countdownTimer -= Time.deltaTime;
            if (countdownTimer <= 0f)
            {
                countdownTimer = 0f; // Clamp the timer to 0
                currentState = GameState.EndBetting;
                EndBettingPhase();
            }

            
            UpdateTimerDisplay();
        }
    }
       
    

    public void Bet1()
    {
        if(currentState == GameState.Betting && money >= 1f)
        {
            nextBet = 1;
            actualBet = bets[0];
            betButtons.alpha = 1;
            betButtons.interactable = true;
        }
        
    }

    public void Bet5()
    {
        if (currentState == GameState.Betting && money >= 5f)
        {
            nextBet = 5;
            actualBet = bets[1];
            betButtons.alpha = 1;
            betButtons.interactable = true;
        }
            
    }

    public void Bet10()
    {
        if(currentState == GameState.Betting && money >= 10f)
        {
            nextBet = 10;
            actualBet = bets[2];
            betButtons.alpha = 1;
            betButtons.interactable = true;
        }
        
    }

    public void ApostarRojo()
    {
        GameObject a = Instantiate(actualBet, PosRoja.position, Quaternion.identity);
        instancedBets.Add(a);
        money -= actualBet.GetComponent<Chip>().value;
        UpdateMoneyDisplay();
        roundSumRed += actualBet.GetComponent<Chip>().value;
        betButtons.alpha = 0;
        betButtons.interactable = false;


    }

    public void ApostarNegro()
    {
        GameObject a = Instantiate(actualBet, PosNegra.position, Quaternion.identity);
        instancedBets.Add(a);
        money -= actualBet.GetComponent<Chip>().value;
        UpdateMoneyDisplay();
        roundSumBlack += actualBet.GetComponent<Chip>().value;
        betButtons.alpha = 0;
        betButtons.interactable = false;


    }

    void CalculateWinner()
    {
        if(winner == "Black")
        {
            money += roundSumBlack * 2;
        }

        else if(winner == "Red")
        {
            money += roundSumRed * 2;
        }

        UpdateMoneyDisplay();

    }

    public void StartGame()
    {
        StartCoroutine(PlayAudioAndStartBetting());
    }

    private IEnumerator PlayAudioAndStartBetting()
    {
        if (startGameAudio != null)
        {
            // Play the audio
            audioSource.clip = startGameAudio;
            audioSource.Play();

            // Wait for the audio to finish
            yield return new WaitForSeconds(startGameAudio.length);
        }

        // Set the game state to Betting
        currentState = GameState.Betting;
        canvasBets.alpha = 1;
        canvasBets.interactable = true;
        timerCanvasGroup.alpha = 1;
        countdownTimer = 10f; // Reset the timer
    }



    void EndBettingPhase()
    {
        betButtons.alpha = 0;
        betButtons.interactable = false;
        //canvas group of timer also goes alpha 0
        timerCanvasGroup.alpha = 0;
        timerCanvasGroup.interactable = false;
        numeroCanvas.alpha = 1;
        currentState = GameState.Playing;
        StartCoroutine(CalculateNumber());
    }

    void UpdateTimerDisplay()
    {
        // Update the timer text to display the remaining time
        timerText.text = Mathf.CeilToInt(countdownTimer).ToString(); // Display as whole seconds
        //NOTA: En los ultimos 3 segundos, poner sonido de pip pip
    }

    IEnumerator CalculateNumber()
    {
        int x = Random.Range(0, 36);
        float delayTiempo = 0.1f;


        for(int j = 0; j < 2; j++) //5 veces se repite
        {
            apuestaGan.sprite = numeroVerde;
            apuestaText.text = "0";
            yield return new WaitForSeconds(0.05f);

            for (int i = 1; i < 37; i++)
            {
                
                numActualRojo = !numActualRojo;
                if (numActualRojo) apuestaGan.sprite = numeroRojo;
                else apuestaGan.sprite = numeroNegro;
                apuestaText.text = i.ToString();
                yield return new WaitForSeconds(0.05f);

            }
        }

        apuestaGan.sprite = numeroVerde;
        apuestaText.text = "0";
        yield return new WaitForSeconds(0.05f);

        for (int i = 0; i < x+1; i++) //Hasta el ganador
        {
            numActualRojo = !numActualRojo;
            if (numActualRojo) apuestaGan.sprite = numeroRojo;
            else apuestaGan.sprite = numeroNegro;
            apuestaText.text = i.ToString();
            if(x-i <= 5) //Los ultimos 5 son mas lentos
            {
                yield return new WaitForSeconds(delayTiempo + 0.2f);
                delayTiempo += 0.2f;
            }
            else yield return new WaitForSeconds(0.05f);
        }

        if (numActualRojo) winner = "Red";
        else if (!numActualRojo && x != 0) winner = "Black";
        else winner = "Green";
        Debug.Log(winner);
        yield return new WaitForSeconds(2f);
        CalculateWinner();
        if (money >= 20f) Ganar();
        else if (money <= 0f) Perder();
        else
        {
            EndPlayingPhase();
        }
    
        
    }

    void EndPlayingPhase()
    {
        currentState = GameState.EndPlaying;
        winner = "";
        numeroCanvas.alpha = 0;
        roundSumBlack = 0;
        roundSumRed = 0;
        DeleteInstanced();

        //Empieza la nueva ronda
        currentState = GameState.Betting;
        timerCanvasGroup.alpha = 1;
        timerCanvasGroup.interactable = false;
        countdownTimer = 10f;

    }

    void Ganar()
    {
        UIPanelFadeIn.StartCoroutine(UIPanelFadeIn.FadeIn(true));
    }

    void Perder()
    {
        UIPanelFadeIn.StartCoroutine(UIPanelFadeIn.FadeIn(false));
    }

    void DeleteInstanced()
    {
        // Loop through each GameObject in the list
        foreach (GameObject bet in instancedBets)
        {
            if (bet != null)
            {
                Destroy(bet); // Destroy the GameObject
            }
        }

        // Clear the list to remove all references
        instancedBets.Clear();
    }

    void UpdateMoneyDisplay()
    {
        moneyText.text = money.ToString() + " $";
    }
}
