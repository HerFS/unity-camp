using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [Header("Hair")]
    public Transform HairGroup;
    public List<GameObject> HairList = new List<GameObject>();
    int _currentHairNum = 0;
    public Button HairRightBtn;
    public Button HairLeftBtn;

    [Header("Shirt")]
    public Transform ShirtGroup;
    public List<GameObject> ShirtList = new List<GameObject>();
    int _currentShirtNum = 0;
    public Button ShirtRightBtn;
    public Button ShirtLeftBtn;

    [Header("Pants")]
    public Transform PantsGroup;
    public List<GameObject> PantsList = new List<GameObject>();
    int _currentPantsNum = 0;
    public Button PantsRightBtn;
    public Button PantsLeftBtn;

    Animator _animator;
    public Button AnimeBtn;
    int _aniNum;
    // Start is called before the first frame update
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        InitClothes();
        ChangeAnimation();
    }

    void InitClothes()
    {
        //hairGroup = transform.Find("hairGroup"); // 부모가 자식 오브젝트에 접근

        SetClothesList(HairGroup, HairList);
        //ShowDrees(HairList, _currentHairNum);
        SetClothesList(ShirtGroup, ShirtList);
        //ShowDrees(ShirtList, _currentShirtNum);
        SetClothesList(PantsGroup, PantsList);
        //ShowDrees(PantsList, _currentPantsNum);
        ChangeClothes(_currentHairNum, HairLeftBtn, HairRightBtn, HairList);
        ChangeClothes(_currentShirtNum, ShirtLeftBtn, ShirtRightBtn, ShirtList);
        ChangeClothes(_currentPantsNum, PantsLeftBtn, PantsRightBtn, PantsList);
    }

    void SetClothesList(Transform clothesGroup, List<GameObject> clothesList)
    {
        foreach(Transform clothes in clothesGroup)
        {
            clothes.gameObject.SetActive(false);
            clothesList.Add(clothes.gameObject);
        }
        //ShowDrees(clothesList, 1);
        clothesList[0].SetActive(true);
    }

    //void ShowDrees(List<GameObject> clothesList, int dressNumber) // list.Count == array.Length
    //{
    //    for (int i = 0; i < clothesList.Count; ++i)
    //    {
    //        clothesList[i].SetActive(false);
    //    }
    //    clothesList[dressNumber].SetActive(true);
    //}

    public void ChangeClothes(int value, Button leftButton, Button rightButton, List<GameObject> clothesList)
    {
        int listMaxNum = clothesList.Count - 1;
        leftButton.onClick.AddListener(() =>
        {
            clothesList[value].SetActive(false);
            --value;
            if (value < 0)
            {
                value = listMaxNum;
            }
            clothesList[value].SetActive(true);
        });
        rightButton.onClick.AddListener(() =>
        {
            clothesList[value].SetActive(false);
            ++value;
            if (listMaxNum < value)
            {
                value = 0;
            }
            clothesList[value].SetActive(true);
        });
    }

    public void ChangeAnimation() // animation 바꾸기
    {
        _aniNum = 0;
        AnimeBtn.onClick.AddListener(() => {
            _aniNum = Random.Range(0, 4);
            _animator.SetInteger("Anim", _aniNum);
        });
    }
}
