using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    private SpriteRenderer faderGameplay;
    private GameObject menuAsteroid;
    private Image faderUI;
    private bool onMenu;
    [SerializeField]
    private float interval;
    private float taux = 0.01f;
    [SerializeField]
    private GameObject starSpawnLocation;
    [SerializeField]
    private GameObject shootingStar;

    public void FadeInGameplay()
    {
        StartCoroutine(FadeInGameplayCo());
    }

    public void FadeOutGameplay()
    {
        StartCoroutine(FadeOutGameplayCo());
    }
    public void FadeInUI()
    {
        StartCoroutine(FadeInUICo());
    }

    public void FadeOutUI()
    {
        StartCoroutine(FadeOutUICo());
    }

    void Start()
    {
        onMenu = false;
        GameObject fader = GameObject.Find("Fader");
        faderGameplay = fader.GetComponent<SpriteRenderer>();
        faderUI = fader.GetComponent<Image>();
        if (faderGameplay != null)
        {
            FadeInGameplay();
        }
        else if (faderUI != null)
        {            
            FadeInUI();
        }
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            onMenu = true;
            menuAsteroid = GameObject.Find("MenuAsteroid");
            StartCoroutine(ShootingStar());
        }
    }

    public async void StartGame()
    {
        FadeOutUI();
        await Task.Delay(1500);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public async void Menu()
    {
        FadeOutUI();
        await Task.Delay(1500);
        SceneManager.LoadScene(0);
    }

    public void FixedUpdate()
    {
        if (onMenu)
        {
            menuAsteroid.transform.Rotate(0, 0, 0.25f);
        }          
    }

    IEnumerator ShootingStar()
    {  
        while (true)
        {
            SpawnStar();
            yield return new WaitForSeconds(interval);
        }
    }

    public void SpawnStar()
    {
        RectTransform spawnRect = starSpawnLocation.GetComponent<RectTransform>();
        GameObject star = Instantiate(shootingStar, spawnRect.anchoredPosition, Quaternion.identity);
        star.transform.SetParent(GameObject.Find("Canvas").transform, false);
        Destroy(star, 6f);
    }

    IEnumerator FadeInGameplayCo()
    {
        Color colTemp = Color.black;
        faderGameplay.color = colTemp;
        while (faderGameplay.color.a > 0.0f)
        {
            colTemp.a -= taux;
            faderGameplay.color = colTemp;
            yield return new WaitForEndOfFrame(); 
        }
        //Pour ne pas avoir de alpha negatif
        colTemp.a = 0.0f;
        faderGameplay.color = colTemp;
    }

    IEnumerator FadeOutGameplayCo()
    {
        Color colTemp = Color.black;
        colTemp.a = 0.0f;
        faderGameplay.color = colTemp;
        while (faderGameplay.color.a < 1f)
        {
            colTemp.a += taux;
            faderGameplay.color = colTemp;
            yield return new WaitForEndOfFrame(); //a Chaque update
        }
        //Pour ne pas avoir de alpha plus que 1
        colTemp.a = 1f;
        faderGameplay.color = colTemp;
    }

    IEnumerator FadeInUICo()
    {
        Color colTemp = Color.black;
        faderUI.color = colTemp;
        while (faderUI.color.a > 0.0f)
        {
            colTemp.a -= taux;
            faderUI.color = colTemp;
            yield return new WaitForEndOfFrame();
        }
        //Pour ne pas avoir de alpha negatif
        colTemp.a = 0.0f;
        faderUI.color = colTemp;
    }

    IEnumerator FadeOutUICo()
    {
        Color colTemp = Color.black;
        colTemp.a = 0.0f;
        faderUI.color = colTemp;
        while (faderUI.color.a < 1f)
        {
            colTemp.a += taux;
            faderUI.color = colTemp;
            yield return new WaitForEndOfFrame(); //a Chaque update
        }
        //Pour ne pas avoir de alpha plus que 1
        colTemp.a = 1f;
        faderUI.color = colTemp;
    }
}