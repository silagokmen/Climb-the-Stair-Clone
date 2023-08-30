using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

   [SerializeField]  Transform[] target;
   [SerializeField]  GameObject wood;
   [SerializeField]  GameObject M_Score;
   [SerializeField]  GameObject MoneyPopupText;
   [SerializeField]  ParticleSystem sparkExplosionBlue;
   [SerializeField]  ParticleSystem bloodExplosion;
   [SerializeField]  ParticleSystem Partical1;


    public static float _startTime;
    public static float _speed = 0.5f;


    private int _current;
    private float _Yaxis = 0.072f;
    private float _woodInc;
    private float _limit = 2.0f;
    private bool aa = true;
    private bool bb = true;
    private GameObject _woodInstantiate;
    private GameObject _MoneyPopupInstantiate;
    private Animator _anim;
    private SkinnedMeshRenderer _skinnedMesh;
    private Color _color = new Color(0.8679245f, 0.241545f, 0.241545f);





    void Start()
    {

        Partical1.Stop();
        bloodExplosion.Stop();
        sparkExplosionBlue.Stop();


        _anim = GetComponent<Animator>();
        _skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
    }




    void FixedUpdate()
    {

        if (Input.GetMouseButton(0))
        {
           
            _anim.SetBool("isUpStairs", true);

            _startTime += Time.deltaTime;

            CharacterWayPoint();

            _anim.SetBool("isBreathe", false);

            UI_Script.sLiderScore += 0.5f;

            _woodInc += 0.001f;

            _woodInstantiate = Instantiate(wood, new Vector3(0.2964837f, (_Yaxis + _woodInc) / 1.7f, -3.42f), wood.transform.rotation);

            M_Score.transform.position = new Vector3(_woodInstantiate.transform.position.x, _woodInstantiate.transform.position.y + 0.25f, _woodInstantiate.transform.position.z);
        }

        #region AÇIKLAMA
        /*
          GetMouseButtonUp(0) olduğunda isUpStairs animasyonunu false yapıp çalışmasını durdurduk, _startTime' 0'a eşitledik ki GetMouseButton(0) olduğunda
          tekrar _startTime 0'dan saymaya başlasın.
        */
        #endregion
        if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(Breathe());
            _anim.SetBool("isUpStairs", false);
            _startTime = 0;

            bb = true;
        }


        CharacterColorChange();

    }



    private void CharacterWayPoint()
    {

        #region AÇIKLAMA
        //Karakter yukarı çıktıkça rotasyonun X'in ve Z'nin açısı değişiyordu o yüzden X ve Z'yi sıfıra eşitledik.
        #endregion
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, eulerRotation.y, 0);


        #region AÇIKLAMA
        //Karakteri waypoint objesine ulaştırana kadar hareket ettiriyoruz.
        #endregion
        if (transform.position != target[_current].position)
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, target[_current].position, _speed * Time.deltaTime);
            Mathf.Lerp(transform.rotation.y, target[_current].position.z, 0.5f);
            GetComponent<Rigidbody>().MovePosition(pos);
            _Yaxis += 0.005f;
        }

        #region AÇIKLAMA
        //Karakteri sonraki waypoint objesine hareket ettiriyoruz.
        #endregion
        else
        {
            _current = (_current + 1) % target.Length;
        }


        var rotation = Quaternion.LookRotation(target[_current].position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);
    }



    private void CharacterColorChange()
    {
        #region AÇIKLAMA
        /*
         _startTime _limit'ten büyük olduğunda isBreathe animasyonunu true yapıp çalıştırıyoruz.
         Karakterin SkinnedMeshRenderer'ından material'ini beyazdan kırmızıya doğru değiştiriyoruz
        */
        #endregion
        if (_startTime >= _limit)
        {
            #region AÇIKLAMA
            //bb değişkeni sparkExplosionBlue partical'ının bir kere çalışmasını sağlmak için yazdığımız bool değişken.
            #endregion
            if (_startTime > _limit && bb == true)
            {
                sparkExplosionBlue.Play();
                bb = false;
            }

            _anim.SetBool("isBreathe", true);
            _skinnedMesh.material.color = Color.Lerp(_skinnedMesh.material.color, _color, 3f * Time.deltaTime);

        }

        #region AÇIKLAMA
        /*
         aa değişkeni CharacterDestroy Coroutine'in içindeki bloodExplosion partical'ın bir kere çalışması için yazdığımız bool değişken.
         Karakterin material.color'ı _color değişkenindeki renge eşit olduğunda bu fonsiyon çalışıyor ve karakter kırmızılaşıp patlıyor.
        */
        #endregion
        if (_skinnedMesh.material.color == _color && aa == true)
        {
            StartCoroutine(CharacterDestroy());
            aa = false;
        }

        #region AÇIKLAMA
        /*
         GetMouseButtonUp(0) olduğunda _startTime'ı 0'a eşitliyoruz ve karakterin SkinnedMeshRenderer'dan material.color'ını.
         Color.white yapıyoruz yani burada kırmızıdan tekrar eski rengi olan beyaza dönüyor
        */
        #endregion
        if (_startTime == 0)
        {
            _skinnedMesh.material.color = Color.Lerp(_skinnedMesh.material.color, Color.white, 0.5f * Time.deltaTime);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        #region AÇIKLAMA
        /*
         Tag'ı Stairs olan basamakların trigger'ına karakter değdiğinde basanakların MeshRenderer'ını enabled=true yaptık ve karakterin değdiği basamaklar görünür oldu.
         MoneyPopup Coroutine'i başlattık bu Coroutine'de karakter Stairs tag'lı basamakların trigger'ına çarptığında moneyPopup text'i instantiate ediliyor
         ve UI scriptindeki moneyScrore değişkeni arttırılıyor.
        */
        #endregion
        if (other.gameObject.tag == "Stairs")
        {
            other.GetComponent<MeshRenderer>().enabled = true;
            StartCoroutine(MoneyPopup());
        }

        #region AÇIKLAMA
        /*
         Karakter Finish tag'ına sahip gameObject'in trigger değdiğinde karakterin transform.position'u verdiğimiz Vector3 değerine eşitleniyor.
         Finish Coroutin çalıyor ve partical effect başlıyor.
        */
        #endregion
        if (other.gameObject.tag == "Finish")
        {
            transform.position = new Vector3(0.439999998f, 11.0799999f, -6.57999992f);
            Partical1.Play();
            StartCoroutine(Finish());
        }

    }

    #region AÇIKLAMA
    //3 saniye sonra partical effect duruyor ve isBreathe false oluyor.
    #endregion
    IEnumerator Breathe()
    {
        yield return new WaitForSeconds(3);
        sparkExplosionBlue.Stop();
        _anim.SetBool("isBreathe", false);
    }

    #region AÇIKLAMA
    /*
     bloodExplosion partical effect'i çalışıyor ve 0.1f saniye sonra karakter SetActive(false) oluyor ve UI_Script'in içinde isCharacterDestroy bool değişkenini true yapıyoruz.
     UI_Script.isCharacterDestroy = true olduğunda bize "try again" button'unun olduğu panel açılıyor.
    */
    #endregion
    IEnumerator CharacterDestroy()
    {
        bloodExplosion.Play();
        yield return new WaitForSeconds(.1f);
        this.gameObject.SetActive(false);
        UI_Script.isCharacterDestroy = true;
    }

    #region AÇIKLAMA
    /*
     MoneyPopupText gameObject'inin instantiate yapıyoruz ve instantiate yaptığımız MoneyPopupText'i 0.5f saniye sonra destroy ediyoruz ve
     UI_Script sınıfındaki moneyScore değişkenini arttırarak moneyText.text'e yazdırıyoruz.
    */
    #endregion
    IEnumerator MoneyPopup()
    {
        yield return new WaitForSeconds(0.01f);
        _MoneyPopupInstantiate = Instantiate(MoneyPopupText, _woodInstantiate.transform.position, Quaternion.identity);
        Destroy(_MoneyPopupInstantiate, 0.5f);
        UI_Script.moneyScore++;
    }


    IEnumerator Finish()
    {
        yield return new WaitForSeconds(1f);
        UI_Script.isFinish = true;
    }
}
