using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private float _offset;
    private GameObject _player;



    #region AÇIKLAMA
    //_player değişkenine karakterimizi atadık ve karakterin transform'una eriştik.
    //Cameranın transform.position.y'sinden, _player'in transform.position.y'sini çıkartarak y eksininde aralarındaki farkı bulduk ve bunu offset değişkenine atadık.
    #endregion
    void Start()
    {
        _player = GameObject.Find("Character"); 
        _player.GetComponent<Transform>();
        _offset = transform.position.y - _player.transform.position.y;
    }

    #region AÇIKLAMA
    //Camera'yı  _player'ın y eksenindeki posizyonuna ayarladık._player y ekseninde yukarı doğru hareket ettiğinde kamerada y ekseninde _player kadar hareket edip onu takip edicek.
    #endregion
    void Update()
    {
        transform.position = new Vector3(transform.position.x, _player.transform.position.y + _offset, 1.5f);
    }
}
