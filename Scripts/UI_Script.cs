using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class UI_Script : MonoBehaviour
{
    public static float moneyScore;
    public static float sLiderScore;
    public static bool isCharacterDestroy;
    public static bool isFinish;


    [SerializeField]  GameObject m_text;
    [SerializeField]  Slider slider;
    [SerializeField]  Text moneyText;


    private GameObject _money;
    private GameObject _level;
    private GameObject _panel1;
    private GameObject _panel2;
    private GameObject _pauseMenu;
    private GameObject _pauseButton;
    private GameObject _startButton;

   

    void Start()
    {

        Time.timeScale = 0;


        _pauseButton = GameObject.Find("Pause");
        _pauseMenu = GameObject.Find("PausePanel");
        _money = GameObject.Find("Money");
        _level = GameObject.Find("Level");
        _panel1 = GameObject.Find("Panel");
        _panel2 = GameObject.Find("FinishPanel");
        _startButton = GameObject.Find("TapToStartPanel");

        _startButton.SetActive(true);
        _pauseMenu.SetActive(false);
        _panel1.SetActive(false);
        _panel2.SetActive(false);

        isCharacterDestroy = false;
        isFinish = false;


        sLiderScore = 0;
        moneyScore = 70;
        moneyText.text = "$" + moneyScore;


        slider.GetComponent<Slider>().maxValue = 1400f;

        m_text.GetComponentInChildren<TMP_Text>().text = "$0.5";

    }



    void Update()
    {
        #region AÇIKLAMA
        // slider.value'muzu static bir değişken olan sliderScore'a eşitledik ve sliderScore'u characterController sınıfında mouse basıldığı süre boyunca arttırdık.
        #endregion
        slider.value = sLiderScore;
        moneyText.text = "$" + moneyScore;


        #region AÇIKLAMA
        /*
        Karakter destroy olduğunda,bool değişkeni olan isCharacterDestroy'u CharacterController sınıfındaki CharacterDestroy metodu'nun içinde true yaptık.
        Oyun ekranındaki bulunan UI Component'lerini (Slider,Pause Button,Level Text vs.) SetActive(false) yaptık ve Try Again button'un bulunduğu _panel1'i
        SetActive(true) yaptık.
        */
        #endregion
        if (isCharacterDestroy == true)
        {
            _money.gameObject.SetActive(false);
            _level.gameObject.SetActive(false);
            slider.gameObject.SetActive(false);
            moneyText.gameObject.SetActive(false);
            _pauseButton.gameObject.SetActive(false);

            _panel1.SetActive(true);
        }


        #region AÇIKLAMA
        /*
        isFinish == true olduğunda oyun ekranında bulunan UI Component'lerini (Slider, Pause Button,Level Text vs.) SetActive(false) yaptık.
        Bir sonra ki level'e geçmek için Next button'un bulunduğu panel'i SetActive(true)yaptık.
        NOT: Tek bir sahne yaptığım için Next button'una tıklandığında yine aynı "SampleScene" yükeniyor.
        */
        #endregion
        if (isFinish == true)
        {
            _money.gameObject.SetActive(false);
            _level.gameObject.SetActive(false);
            slider.gameObject.SetActive(false);
            _pauseButton.gameObject.SetActive(false);

            _panel2.SetActive(true);
        }

    }

    #region AÇIKLAMA
    /*
     Oyun ekranımızdaki Pause button'una tıkladığımızda Restart ve Resume button'larının olduğu _pauseMenu panel'i açılıyor.
     Panel'de bulunan Restart button'una tıkladığımızda da  _pauseMenu panel'i SetActive(false) yapıyoruz ve oyun sahnemizi yeniden yüklüyoruz.
    */
    #endregion
    public void Restart()
    {
       _pauseMenu.SetActive(false);
        SceneManager.LoadScene("SampleScene");
    }


    #region AÇIKLAMA
    /*
    Karakter destroy olduğunda TryAgain button'un olduğu panel açılıyor. TryAgain button'una tıklandığında sahne yeniden yükleniyor.
    static bir değişken olan _startTime'ı 0'a eşitledik.
    */
    #endregion
    public void TryAgain()
    {
        CharacterController._startTime = 0;
        SceneManager.LoadScene("SampleScene");
    }


    #region AÇIKLAMA
    /*
     Oyun ekranımızdaki Pause button'una tıkladığımızda oyun duruyor. Time.timeScale'i 0'a eşitleyerek oyunu durduruyoruz ve
    _pauseMenu.SetActive(true) yaparak Resart ve Resume button'larının olduğu panel açılıyor.
    */
    #endregion
    public void Pause()
    {
        _pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }


    #region AÇIKLAMA
    /*
     Oyun ekranımızdaki Pause button'una tıkladığımızda oyun duruyor(Time.timeScale = 0) Resart ve Resume olduğu panel açılıyor.
     _pauseMenu panelinde bulunan Resume button'una tıkladığımızda da _pauseMenu panel'i SetActive(false) yapıyoruz.
     Time.timeScale'i 1'e eşitleyerek oyuna kaldığımız yerden devam ediyoruz.
    */
    #endregion
    public void Resume()
    {
       _pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }


    #region AÇIKLAMA
    /*
    Oyun başladığında Start() metodunda Time.timeScale = 0'a eşitlemiştik ve Tap to start button'unun bulunduğu  _startButton panelini SetActive(true) yapmıştık.
    Tap to start button'una basıldığında Time.timeScale = 1'e eşit yaptık ve oyunu başlatmış olduk ve paneli SetActive(false) yaptık.
    */
    #endregion
    public void TaptoStart()
    {
        _startButton.SetActive(false);
        Time.timeScale = 1;
    }


    #region AÇIKLAMA
    /*
     Oyun başladığında Start() metodunda Time.timeScale = 0'a eşitlemiştik ve Stamina button'unun bulunduğu _startButton panelini SetActive(true) yapmıştık.
     Stamina button'una basıldığında Time.timeScale = 1'e eşit yaptık ve oyunu başlatmış olduk ve panel'i SetActive(false) yaptık. Ayrıca Stamina button'una
     basıldığında moneyScore'u 40 azalttık ve StaminaTime Coroutine'ni çalıştırdık.
    */
    #endregion
    public void Stamina()
    {
        _startButton.SetActive(false);
        Time.timeScale = 1;
        StartCoroutine(StaminaTime());
        moneyScore -= 40;

    }

    #region AÇIKLAMA
    /*
     Oyun başladığında Start() metodunda Time.timeScale = 0'a eşitlemiştik ve Speed button'unun bulunduğu _startButton panelini SetActive(true) yapmıştık.
     Speed button'una basıldığında Time.timeScale = 1'e eşit yaptık ve oyunu başlatmış olduk ve panel'i SetActive(false) yaptık. Ayrıca Speed button'una
     basıldığında moneyScore'u 40 azalttık ve SpeedTime Coroutine'ni çalıştırdık.
    */
    #endregion
    public void Speed()
    {
        _startButton.SetActive(false);
        Time.timeScale = 1;
        StartCoroutine(SpeedTime());
        moneyScore -= 40;
    }

    #region AÇIKLAMA
    /*
     Oyun başladığında Start() metodunda Time.timeScale = 0'a eşitlemiştik ve Income button'unun bulunduğu _startButton panelini SetActive(true) yapmıştık.
     Income button'una basıldığında Time.timeScale = 1'e eşit yaptık ve oyunu başlatmış olduk ve panel'i SetActive(false) yaptık. Ayrıca Income button'una
     basıldığında moneyScore'u 40 azalttık ve m_text'i değiştirdik.
    */
    #endregion
    public void Income()
    {
        _startButton.SetActive(false);
        Time.timeScale = 1;
        m_text.GetComponentInChildren<TMP_Text>().text = "$2.5";
        moneyScore -= 40;

    }


    IEnumerator StaminaTime()
    {
        CharacterController._startTime=Time.deltaTime-20;
        yield return new WaitForSeconds(4f);
        CharacterController._startTime = Time.deltaTime; ;

    }


    IEnumerator SpeedTime()
    {
        CharacterController._speed = 1f;
        yield return new WaitForSeconds(3f);
        CharacterController._speed = 0.5f;

    }
}
