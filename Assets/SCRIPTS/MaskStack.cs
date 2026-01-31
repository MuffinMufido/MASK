using UnityEngine;

using System.Collections.Generic;


public class MaskStack : MonoBehaviour

{

    [Header("Maske Objeleri (Sýrasýyla 1-6)")]

    public List<GameObject> maskObjects;


    [Header("Sabit Pozisyon Slotlarý (Sýrasýyla 1-6)")]

    public List<Transform> slotPositions;


    // Þu an takýlmýþ olan maskelerin listesi (Takýlma sýrasýna göre)

    private List<GameObject> equippedMasks = new List<GameObject>();


    void Start()

    {

        // Baþlangýçta tüm maskeleri kapat

        foreach (GameObject mask in maskObjects)

        {

            if (mask != null) mask.SetActive(false);

        }

    }


    void Update()

    {

        // 1'den 6'ya kadar tuþlarý kontrol et

        for (int i = 0; i < maskObjects.Count; i++)

        {

            if (Input.GetKeyDown(KeyCode.Alpha1 + i))

            {

                ToggleMask(maskObjects[i]);

            }

        }

    }


    void ToggleMask(GameObject selectedMask)

    {

        // EÐER MASKE ZATEN TAKILIYSA: Çýkar

        if (equippedMasks.Contains(selectedMask))

        {

            selectedMask.SetActive(false);

            equippedMasks.Remove(selectedMask);

        }

        // EÐER MASKE TAKILI DEÐÝLSE: Listeye ekle ve aç

        else

        {

            selectedMask.SetActive(true);

            equippedMasks.Add(selectedMask);

        }


        // HER DURUMDA: Maskeleri slotlara göre yeniden diz

        RearrangeMasks();

    }


    void RearrangeMasks()

    {

        for (int i = 0; i < equippedMasks.Count; i++)

        {

            // Takýlý olan i. maskeyi, sahnedeki i. slot pozisyonuna ýþýnla

            equippedMasks[i].transform.position = slotPositions[i].position;

            equippedMasks[i].transform.rotation = slotPositions[i].rotation;


            // Eðer maskeleri bir objeye baðlamak istersen:

            // equippedMasks[i].transform.SetParent(slotPositions[i]);

        }

    }

}

