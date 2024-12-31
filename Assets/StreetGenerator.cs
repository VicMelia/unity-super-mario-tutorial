using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StreetGenerator : MonoBehaviour
{
    public GameObject pearlPrefab;
    public Transform[] pearlPositions;
    public GameObject[] streets;
    public GameObject policeEnemy;
    public Transform[] policePositions;
    public int counter = 0;

    public List<GameObject> streetList;

    public CarPlayer carPlayer;
    bool last3 = false;
    bool lastPearl = true;
    public float countdownTimer = 90f;
    public TextMeshProUGUI timerText;
    public UIPanelFadeIn fadeIn;

    public enum State { Playing, Ending};
    public State currentState = State.Playing;


    private void Update()
    {
        if(currentState == State.Playing)
        {
            if (streetList.Count < 2)
            {
                GenerateStreet();
            }

            countdownTimer -= Time.deltaTime;
            if (countdownTimer <= 0f)
            {
                carPlayer.win = true;
                if (streetList.Count < 2)
                {
                    GenerateStreet();
                }
                countdownTimer = 0f; // Clamp the timer to 0
                currentState = State.Ending;
                EndGame();
            }


            UpdateTimerDisplay();
        }
        
    }

    public void GenerateStreet()
    {
        if (counter >= streets.Length)
        {
            counter = 0;
        }

        GameObject a = Instantiate(streets[counter], transform.position, Quaternion.identity);
        
        streetList.Add(a);
        

        CalculatePolice();
    }

    private void CalculatePolice()
    {
        int x = Random.Range(0, 10);
        int pearl = Random.Range(0, 10);
        if(!lastPearl && !carPlayer.hasPearl && pearl < 3)
        {
            int pos = Random.Range(0, 2);
            Instantiate(pearlPrefab, pearlPositions[pos].position, Quaternion.identity);
            lastPearl = true;
        }
        if(x < 7) //Police spawn prob
        {
            int policeNums;
            if (carPlayer.hasPearl && !last3) policeNums = Random.Range(1, 4); //Can be 1-3
            else policeNums = Random.Range(1, 2); //Can be one or two
          
            int f = carPlayer.actualPosition;
            GameObject p1 = Instantiate(policeEnemy, policePositions[f].position, Quaternion.identity);
            
            if(policeNums == 3)
            {
                int s = 0;
                int t = 0;
                while (s == f)
                {
                    s = Random.Range(0, 3);
                }
                while (t == f || t == s)
                {
                    t = Random.Range(0, 3);
                }
                GameObject p2 = Instantiate(policeEnemy, policePositions[s].position, Quaternion.identity);
                GameObject p3 = Instantiate(policeEnemy, policePositions[t].position, Quaternion.identity);

                last3 = true;
            }

            else if (policeNums == 2) //Two police
            {
                int s = 0;
                while (s == f)
                {
                    s = Random.Range(0, 3);
                }

                GameObject p2 = Instantiate(policeEnemy, policePositions[s].position, Quaternion.identity);
                last3 = false;

            }

            else last3 = false;


        }
        lastPearl = !lastPearl;

        counter++;
    }

    void EndGame()
    {
        StartCoroutine(fadeIn.FadeIn(true));
    }

    void UpdateTimerDisplay()
    {
        timerText.text = Mathf.CeilToInt(countdownTimer).ToString(); // Display as whole seconds
    }
}
