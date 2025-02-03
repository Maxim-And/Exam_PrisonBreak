using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using Newtonsoft.Json;

public class ClickButton : MonoBehaviour
{
    [SerializeField] private Image characterImage;
    [SerializeField] private TMP_Text characterBio;
    [SerializeField] private RawImage videoRawImage;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private Button playVideoButton;

    private Character[] characters;
    private bool isVideoPlaying = false;

    void Start()
    {

        playVideoButton.onClick.AddListener(ToggleVideo);
        LoadCharacterData();
        videoRawImage.gameObject.SetActive(false);
        videoPlayer.gameObject.SetActive(false);
        videoPlayer.playOnAwake = false;
        characterBio.gameObject.SetActive(false);
    }
    private void LoadCharacterData()
    {
        Resources.UnloadUnusedAssets();

        TextAsset jsonFile = Resources.Load<TextAsset>("characternew");

        if (jsonFile != null)
        {
            characters = JsonConvert.DeserializeObject<Character[]>(jsonFile.text);
        }
        else
        {
            Debug.LogError("Charakterdaten nicht gefunden!");
        }
    }

    public void DisplayCharacter(string name)
{
    var character = System.Array.Find(characters, c => c.name == name);
    if (character != null)
    {
        characterImage.sprite = Resources.Load<Sprite>(character.imagePath);

        characterBio.text = $"{character.name}, {character.age} Jahre alt\n{character.bio}";
        characterBio.gameObject.SetActive(true);

        StopVideo();
    }
    else
    {
        characterBio.gameObject.SetActive(false);
        Debug.LogError("Charakter nicht gefunden!");
    }
}

    private void ToggleVideo()
    {
        if (isVideoPlaying)
        {
            PauseVideo();
        }
        else
        {
            PlayVideo();
        }
    }
    private void PlayVideo()
    {
        var videoClip = Resources.Load<VideoClip>("prison_break_intro");
        if (videoClip != null)
        {
            videoPlayer.clip = videoClip;

            var renderTexture = new RenderTexture(1920, 1080, 24);
            videoPlayer.targetTexture = renderTexture;

            videoRawImage.texture = renderTexture;
            videoRawImage.gameObject.SetActive(true);
            characterImage.gameObject.SetActive(false);

            videoPlayer.gameObject.SetActive(true);
            videoPlayer.Play();
            isVideoPlaying = true;
        }
        else
        {
            Debug.LogError("Video nicht gefunden!");
        }
    }
    private void PauseVideo()
    {
        videoPlayer.Pause();
        isVideoPlaying = false;
    }
    private void StopVideo()
    {
        videoPlayer.Stop();
        videoRawImage.gameObject.SetActive(false);
        characterImage.gameObject.SetActive(true);
        videoPlayer.gameObject.SetActive(false);
        isVideoPlaying = false;
    }
}

[System.Serializable]
public class Character
{
    public string name;
    public int age;
    public string imagePath;
    public string bio;
}
