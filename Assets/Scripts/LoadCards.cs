using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using SimpleJSON;
using TMPro;

public class LoadCards : MonoBehaviour
{
    public JSONArray pokemonCards;
    public Texture2D[] dowloadedImages = new Texture2D[50];
    public RawImage[] selectionImages;
    public TextMeshProUGUI pokeNameText;
    public GameObject deckBuilder;
    public GameObject loadingBackground;
    public int numfOfDisplayedCards = 50;

    // public Button rightButton;
    // public Button leftButton;
    // public int selectionCounter = 0;

    private readonly string basePokeURL = "https://api.pokemontcg.io/v2/cards/";

    public void Start(){

        // Button btn = rightButton.GetComponent<Button>();
		// btn.onClick.AddListener(RightButtonClick);
        
        //total cards are 14058 that means that we have 282 pages


        //todo kapou edo na mpei ena loading page

        StartCoroutine(LoadCardList(Random.Range(1, 282+1)));


    }

    private IEnumerator LoadCardList(int page) {
        //https://api.pokemontcg.io/v2/cards?page=282&pageSize=1
        deckBuilder.SetActive(false);
        loadingBackground.SetActive(true);

        string pokemonURL = basePokeURL + "?page="  + page + "&pageSize=50";
        // Debug.Log(pokemonURL);

        UnityWebRequest pokeInfoRequest = UnityWebRequest.Get(pokemonURL);
        
        yield return pokeInfoRequest.SendWebRequest();


        if (pokeInfoRequest.result == UnityWebRequest.Result.ConnectionError || pokeInfoRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(pokeInfoRequest.error);
            yield break;
        }

        JSONNode pokeInfo = JSON.Parse(pokeInfoRequest.downloadHandler.text);

        // Debug.Log(pokeInfo);

        //opote edo exoume to json me ti pliroforia. theloume mia lista mono me tis kartes
        //oi kartes einai sto data
        pokemonCards = pokeInfo["data"].AsArray;
        
        yield return DownloadImages(pokemonCards);

        for (int i = 0; i < 5; i++) {
            selectionImages[i].texture = dowloadedImages[i];
        }
        deckBuilder.SetActive(true);
        loadingBackground.SetActive(false);
    }
// 
    private IEnumerator DownloadImages(JSONArray pokemonCards){ 
        for (int i = 0; i < numfOfDisplayedCards; i++) {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(pokemonCards[i].AsObject["images"]["small"]);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError){
                Debug.Log(request.error);
            }
            else{
                dowloadedImages[i] = DownloadHandlerTexture.GetContent(request);
            }
        }
    }
  
}
