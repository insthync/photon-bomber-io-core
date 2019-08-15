﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class UIMainMenu : MonoBehaviour
{
    public Text textSelectCharacter;
    public Text textSelectHead;
    public Text textSelectBomb;
    public InputField inputName;
    public Transform characterModelTransform;
    public Transform bombEntityTransform;
    private int selectCharacter = 0;
    private int selectHead = 0;
    private int selectBomb = 0;
    // Showing character / items
    private CharacterModel characterModel;
    private BombEntity bombEntity;
    private HeadData headData;

    public int SelectCharacter
    {
        get { return selectCharacter; }
        set
        {
            selectCharacter = value;
            if (selectCharacter < 0)
                selectCharacter = MaxCharacter;
            if (selectCharacter > MaxCharacter)
                selectCharacter = 0;
            UpdateCharacter();
        }
    }

    public int SelectHead
    {
        get { return selectHead; }
        set
        {
            selectHead = value;
            if (selectHead < 0)
                selectHead = MaxHead;
            if (selectHead > MaxHead)
                selectHead = 0;
            UpdateHead();
        }
    }

    public int SelectBomb
    {
        get { return selectBomb; }
        set
        {
            selectBomb = value;
            if (selectBomb < 0)
                selectBomb = MaxBomb;
            if (selectBomb > MaxBomb)
                selectBomb = 0;
            UpdateBomb();
        }
    }

    public int MaxHead
    {
        get { return GameInstance.AvailableHeads.Count - 1; }
    }

    public int MaxCharacter
    {
        get { return GameInstance.AvailableCharacters.Count - 1; }
    }

    public int MaxBomb
    {
        get { return GameInstance.AvailableBombs.Count - 1; }
    }

    private void Start()
    {
        inputName.text = PlayerSave.GetPlayerName();
        SelectHead = PlayerSave.GetHead();
        SelectCharacter = PlayerSave.GetCharacter();
        SelectBomb = PlayerSave.GetBomb();
    }

    private void Update()
    {
        textSelectCharacter.text = (SelectCharacter + 1) + "/" + (MaxCharacter + 1);
        textSelectHead.text = (SelectHead + 1) + "/" + (MaxHead + 1);
        textSelectBomb.text = (SelectBomb + 1) + "/" + (MaxBomb + 1);
    }

    private void UpdateCharacter()
    {
        if (characterModel != null)
            Destroy(characterModel.gameObject);
        var characterData = GameInstance.GetAvailableCharacter(SelectCharacter);
        if (characterData == null || characterData.modelObject == null)
            return;
        characterModel = Instantiate(characterData.modelObject, characterModelTransform);
        characterModel.transform.localPosition = Vector3.zero;
        characterModel.transform.localEulerAngles = Vector3.zero;
        characterModel.transform.localScale = Vector3.one;
        if (headData != null)
            characterModel.SetHeadModel(headData.modelObject);
        characterModel.gameObject.SetActive(true);
    }

    private void UpdateHead()
    {
        headData = GameInstance.GetAvailableHead(SelectHead);
        if (characterModel != null && headData != null)
            characterModel.SetHeadModel(headData.modelObject);
    }

    private void UpdateBomb()
    {
        if (bombEntity != null)
            Destroy(bombEntity.gameObject);
        var bombData = GameInstance.GetAvailableBomb(SelectHead);
        if (bombData == null || bombData.bombPrefab == null)
            return;
        bombEntity = Instantiate(bombData.bombPrefab, bombEntityTransform);
        bombEntity.transform.localPosition = Vector3.zero;
        bombEntity.transform.localEulerAngles = Vector3.zero;
        bombEntity.transform.localScale = Vector3.one;
        bombEntity.gameObject.SetActive(true);
    }

    public void OnClickBackCharacter()
    {
        --SelectCharacter;
    }

    public void OnClickNextCharacter()
    {
        ++SelectCharacter;
    }

    public void OnClickBackHead()
    {
        --SelectHead;
    }

    public void OnClickNextHead()
    {
        ++SelectHead;
    }

    public void OnClickBackBomb()
    {
        --SelectBomb;
    }

    public void OnClickNextBomb()
    {
        ++SelectBomb;
    }

    public void OnInputNameChanged(string eventInput)
    {
        PlayerSave.SetPlayerName(inputName.text);
    }

    public void OnClickSaveData()
    {
        PlayerSave.SetCharacter(SelectCharacter);
        PlayerSave.SetHead(SelectHead);
        PlayerSave.SetBomb(SelectBomb);
        PlayerSave.SetPlayerName(inputName.text);
        PhotonNetwork.LocalPlayer.NickName = PlayerSave.GetPlayerName();
    }

    public void UpdateAvailableItems()
    {
        GameInstance.Singleton.UpdateAvailableItems();
    }
}
