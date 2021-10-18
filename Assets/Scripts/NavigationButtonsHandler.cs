using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using SimpleJSON;
using TMPro;

public class NavigationButtonsHandler : MonoBehaviour
{
    public static int selectionCounter = 0;
    public static int[] coloredImages = new int[50];
    public static int numOfSelectedCards;
    public static int[] deckCardsIndexes = new int[5];

    public RawImage leftArrow;
    public RawImage rightArrow;
    public RawImage pockeballCardBack;
    public RawImage[] deckImages;
    public int clickedImageIndex;
    public int clickedDeckImageIndex;
    public bool sorted = false;

    public void RightButtonClick(){
        Color rightCurrColor = rightArrow.color;
        Color leftCurrColor = leftArrow.color;
        GameObject LoadCards = GameObject.Find("LoadCards");

        if(NavigationButtonsHandler.selectionCounter != LoadCards.GetComponent<LoadCards>().numfOfDisplayedCards - 5){
            if(NavigationButtonsHandler.selectionCounter == 0){
                leftCurrColor.a = 1f;
                leftArrow.color = leftCurrColor;
            }
            NavigationButtonsHandler.selectionCounter += 5;
            UpdateSelectionCards(LoadCards);
            if(NavigationButtonsHandler.selectionCounter == LoadCards.GetComponent<LoadCards>().numfOfDisplayedCards - 5){
                rightCurrColor.a = 0.2f;
                rightArrow.color = rightCurrColor;
            }
        }
        
    }
    public void LeftButtonClick(){
        
        Color rightCurrColor = rightArrow.color;
        Color leftCurrColor = leftArrow.color;
        GameObject LoadCards = GameObject.Find("LoadCards");

        if(NavigationButtonsHandler.selectionCounter != 0){
            if(NavigationButtonsHandler.selectionCounter == LoadCards.GetComponent<LoadCards>().numfOfDisplayedCards - 5){
                rightCurrColor.a = 1f;
                rightArrow.color = rightCurrColor;            
            }
            
            NavigationButtonsHandler.selectionCounter -= 5;
            UpdateSelectionCards(LoadCards);
            if(NavigationButtonsHandler.selectionCounter == 0){
                leftCurrColor.a = 0.2f;
                leftArrow.color = leftCurrColor;
            }
        }
    }

    public void UpdateSelectionCards(GameObject LoadCards){
        Color currentColor;
        for (int i = 0; i < 5; i++) {
            LoadCards.GetComponent<LoadCards>().selectionImages[i].texture = LoadCards.GetComponent<LoadCards>().dowloadedImages[i+NavigationButtonsHandler.selectionCounter];
            if (NavigationButtonsHandler.coloredImages[NavigationButtonsHandler.selectionCounter+i] == 1){
                currentColor = LoadCards.GetComponent<LoadCards>().selectionImages[i].color;
                currentColor.a = 0.3f;
                LoadCards.GetComponent<LoadCards>().selectionImages[i].color = currentColor;
            }
            else{
                currentColor = LoadCards.GetComponent<LoadCards>().selectionImages[i].color;
                currentColor.a = 1f;
                LoadCards.GetComponent<LoadCards>().selectionImages[i].color = currentColor;
            }
        }
    }

    public void ClickSelectionImage(){

        if (NavigationButtonsHandler.coloredImages[clickedImageIndex+NavigationButtonsHandler.selectionCounter] != 1 && NavigationButtonsHandler.numOfSelectedCards < 5){
            GameObject LoadCards = GameObject.Find("LoadCards");
            NavigationButtonsHandler.coloredImages[clickedImageIndex+NavigationButtonsHandler.selectionCounter] = 1;
            // Debug.Log(clickedImageIndex+NavigationButtonsHandler.selectionCounter);
            UpdateSelectionCards(LoadCards);
            AddCardToDeck(LoadCards.GetComponent<LoadCards>().dowloadedImages[clickedImageIndex+NavigationButtonsHandler.selectionCounter] , clickedImageIndex+NavigationButtonsHandler.selectionCounter);
        }
    }
    public void AddCardToDeck(Texture2D newDeckCardTexture, int loadedCardIndex){
        for (int i = 0; i < 5; i++) {
            if (deckImages[i].texture == pockeballCardBack.texture){
                deckImages[i].texture = newDeckCardTexture;
                NavigationButtonsHandler.deckCardsIndexes[i] = loadedCardIndex;
                NavigationButtonsHandler.numOfSelectedCards += 1;
                break;
            }
        }
    }
    public void ClickDeckImage(){
        GameObject LoadCards = GameObject.Find("LoadCards");
        // Debug.Log(clickedDeckImageIndex);
        if (deckImages[clickedDeckImageIndex].texture != pockeballCardBack.texture){
            deckImages[clickedDeckImageIndex].texture = pockeballCardBack.texture;
            NavigationButtonsHandler.numOfSelectedCards -= 1;
            if(NavigationButtonsHandler.coloredImages[NavigationButtonsHandler.deckCardsIndexes[clickedDeckImageIndex]] == 0){
                Debug.Log("Card not dark, something is wrong");
            }
            NavigationButtonsHandler.coloredImages[NavigationButtonsHandler.deckCardsIndexes[clickedDeckImageIndex]] = 0;
            UpdateSelectionCards(LoadCards);
        }
        
    }
    public void sortDownloadedImages(){
        int[] sortedHpArrayIndexes = new int[50];
        GameObject LoadCards = GameObject.Find("LoadCards");
        JSONArray pokemonCards = LoadCards.GetComponent<LoadCards>().pokemonCards;
        if(sorted == false){
            int[] hpArray = new int[50];
            int maxHP;
            int maxHPindex;

            for (int i = 0; i < 50; i++) {
                hpArray[i] = (int)pokemonCards[i].AsObject["hp"]; 
            }
            for (int i = 0; i < 50; i++) {
                maxHP = 0;
                maxHPindex = 0;
                for (int j = 0; j < 50; j++) {

                    if(hpArray[j] >= maxHP){
                        maxHP = hpArray[j];
                        maxHPindex = j;
                    }
                }
                hpArray[maxHPindex] = -1;
                sortedHpArrayIndexes[i] = maxHPindex;
            }
            Texture2D[] sortedImages = new Texture2D[50];
            int[] sortedGreyImages = new int[50];

            for (int i = 0; i < 50; i++) {
                sortedImages[i] = LoadCards.GetComponent<LoadCards>().dowloadedImages[sortedHpArrayIndexes[i]];
                sortedGreyImages[i] = NavigationButtonsHandler.coloredImages[sortedHpArrayIndexes[i]];
            }
            for (int i = 0; i < 50; i++) {
                LoadCards.GetComponent<LoadCards>().dowloadedImages[i] = sortedImages[i];
                NavigationButtonsHandler.coloredImages[i] = sortedGreyImages[i];
            }
            NavigationButtonsHandler.selectionCounter = 0;
            sorted = true;
            UpdateSelectionCards(LoadCards);
            UpdateDeckCardsIndexes(sortedHpArrayIndexes);
        }
    }
    public void UpdateDeckCardsIndexes(int[] sortedHpArrayIndexes){
        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < 50; j++) {
                if( deckCardsIndexes[i] == sortedHpArrayIndexes[j]){
                    deckCardsIndexes[i] = j;
                }
            }
        }
    }
}
