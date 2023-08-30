using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{

    private float metreScore;
    private GameObject k_metreText;
    private GameObject b_metreText;


    void Start()
    {
        #region AÇIKLAMA
        /*
         Oyunda iki tane tabelamız var.
         b_metreText sabit bir text'e sahip tabela, k_metreText ise mouse'a basıldığında artan metreScore'u yazdırdığımız tabela
        */
        #endregion
        b_metreText = GameObject.Find("B_ScoreText");
        k_metreText = GameObject.Find("Score_Text");
        metreScore = 200;

        b_metreText.GetComponent<TextMesh>().text = "1400m";
        k_metreText.GetComponent<TextMesh>().text = metreScore+"m";
    }

    #region AÇIKLAMA
    /*
     Mouse'a basıldığında metreScore artacak ve bu k_metreText tabelasına yazdırılıcak.
     (mouse'a basıldığında karakter merdiven çıkıyor ve biz kaç metre çıktığını hesaplamak için burada da mouse'a basıldığı sürece metreScore değişkenine
     arttırma işlemi yaptık.)
    */
    #endregion
    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            metreScore +=1f;
            k_metreText.GetComponent<TextMesh>().text = metreScore + "m";
        }

    }
}
