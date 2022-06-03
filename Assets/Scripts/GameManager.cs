using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static int population;
    public static int totalPopulation;
    [SerializeField] public GameObject explosion;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator disableGameObject(float waitSec, GameObject disableGameobject)
    {
        yield return new WaitForSeconds(waitSec);
        disableGameobject.SetActive(false);
    }

    public static int GetMoney()
    {
        totalPopulation = PlayerPrefs.GetInt("totalPopulation", 0);

        /*
         
        if (CanvasManager.instance != null)
            CanvasManager.instance.touristCounter.SetText(population.ToString());
        */

        return totalPopulation;
    }

    public static void SetMoney(int newMoney)
    {
        PlayerPrefs.SetInt("totalPopulation", newMoney);
        totalPopulation = GetMoney();
    }
}
