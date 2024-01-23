using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Transform hairGroup;
    public List<GameObject> hairList = new List<GameObject>();
    public Transform shirtGroup;
    public List<GameObject> shirtList = new List<GameObject>();
    public Transform pantGroup;
    public List<GameObject> pantList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        //hairGroup = transform.Find("hairGroup"); // 부모가 자식 오브젝트에 접근

        InitClothes(hairGroup, hairList);
        InitClothes(shirtGroup, shirtList);
        InitClothes(pantGroup, pantList);
        //foreach (Transform hair in hairGroup)
        //{
        //    hair.gameObject.SetActive(false);
        //    hairs.Add(hair.gameObject);
        //}
        //hairs[0].SetActive(true);

        //foreach (Transform shirt in shirtGroup)
        //{
        //    shirt.gameObject.SetActive(false);
        //    shirts.Add(shirt.gameObject);
        //}
        //shirts[0].SetActive(true);

        //foreach (Transform pant in pantGroup)
        //{
        //    pant.gameObject.SetActive(false);
        //    pants.Add(pant.gameObject);
        //}
        //pants[0].SetActive(true);
    }

    void InitClothes(Transform clothesGroup, List<GameObject> clothesList)
    {
        foreach(Transform clothes in clothesGroup)
        {
            clothes.gameObject.SetActive(false);
            clothesList.Add(clothes.gameObject);
        }
        clothesList[0].SetActive(true);
    }
}
