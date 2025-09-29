using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Player Settings")]
    public PlayerController[] players;

    [Header("Goal Check")]
    public TileType goalType = TileType.Goal;

    [Header("Death Menu Objects (multiple)")]
    public GameObject[] deathMenuObjects;

    [Header("Victory Menu Objects (multiple, fade-in)")]
    public GameObject[] victoryMenuObjects;

    [Header("Victory Sprite Object")]
    public GameObject victorySpriteObject; // This will appear temporarily

    [Header("Level Transition")]
    public string nextLevelName;
    public float fadeDuration = 1f;
    public float victorySpriteDuration = 1f;

    private bool victoryTriggered = false;
    private bool deathTriggered = false;

    void Start()
    {
        // Hide all death and victory objects initially
        SetObjectsActive(deathMenuObjects, false);
        SetButtonsInteractable(deathMenuObjects, false);

        SetObjectsActive(victoryMenuObjects, false);
        SetButtonsInteractable(victoryMenuObjects, false);

        if (victorySpriteObject)
            victorySpriteObject.SetActive(false);
    }

    void Update()
    {
        if (!deathTriggered)
            CheckForDeath();

        if (!victoryTriggered)
            CheckForVictory();
    }

    #region Death Handling
    void CheckForDeath()
    {
        foreach (var player in players)
        {
            Tile tile = player.CurrentTile();
            if (tile != null && tile.GetTileType() == TileType.Spike)
            {
                deathTriggered = true;
                ShowDeathMenu();
                break;
            }
        }
    }

    void ShowDeathMenu()
    {
        SetObjectsActive(deathMenuObjects, true);
        SetButtonsInteractable(deathMenuObjects, true);
    }
    #endregion

    #region Victory Handling
    void CheckForVictory()
    {
        bool allOnGoal = true;
        foreach (var player in players)
        {
            Tile tile = player.CurrentTile();
            if (tile == null || tile.GetTileType() != goalType)
            {
                allOnGoal = false;
                break;
            }
        }

        if (allOnGoal)
        {
            victoryTriggered = true;
            StartCoroutine(VictorySequence());
        }
    }

    private IEnumerator VictorySequence()
    {
        if (victoryMenuObjects.Length == 0) yield break;

        // Disable all players
        foreach (var player in players)
        {
            player.canMove = false;
        }



        // Fade in first object
        yield return StartCoroutine(FadeInObject(victoryMenuObjects[0]));

        // Swap first player's sprite temporarily
        if (players.Length > 0 && victorySpriteObject != null)
        {
            yield return StartCoroutine(SwapPlayerSpriteTemporarily(players[0], victorySpriteObject, victorySpriteDuration));
        }




        // Fade in remaining victory objects
        for (int i = 1; i < victoryMenuObjects.Length; i++)
        {
            yield return StartCoroutine(FadeInObject(victoryMenuObjects[i]));
        }

        // Wait briefly, then load next level
        // Wait briefly, then load next level
        yield return new WaitForSeconds(1f);

        // Get the current level (default to 1 if not set)
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel");

        // Update PlayerMaxLevel if this is the highest so far
        if (PlayerPrefs.GetInt("PlayerMaxLevel") < currentLevel)
        {
            PlayerPrefs.SetInt("PlayerMaxLevel", currentLevel);
        }

        // Increment current level
        PlayerPrefs.SetInt("CurrentLevel", currentLevel + 1);

        // Save PlayerPrefs just to be safe
        PlayerPrefs.Save();

        // Load your LevelScene (instead of nextLevelName)
      //  SceneManager.LoadScene("LevelScene");

        if (!string.IsNullOrEmpty(nextLevelName))
        {
            SceneManager.LoadScene(nextLevelName);
        }
    }
    private IEnumerator SwapPlayerSpriteTemporarily(PlayerController player, GameObject newSpriteObj, float duration)
    {
        if (player == null || newSpriteObj == null) yield break;

        // Disable the player's current sprite
        SpriteRenderer playerSr = player.GetComponent<SpriteRenderer>();
        if (playerSr != null)
            playerSr.enabled = false;

        // Enable the new sprite object
        newSpriteObj.SetActive(true);

        // Wait for duration
        yield return new WaitForSeconds(duration);

        // Re-enable original sprite
        if (playerSr != null)
            playerSr.enabled = true;

        // Hide the temporary sprite
        newSpriteObj.SetActive(false);
    }

    private IEnumerator FadeInObject(GameObject obj)
    {
        if (obj == null) yield break;

        obj.SetActive(true);
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        CanvasRenderer cr = obj.GetComponent<CanvasRenderer>();

        float timer = 0f;

        if (sr != null)
        {
            Color c = sr.color;
            c.a = 0f;
            sr.color = c;

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                c.a = Mathf.Clamp01(timer / fadeDuration);
                sr.color = c;
                yield return null;
            }
            c.a = 1f;
            sr.color = c;
        }
        else if (cr != null)
        {
            cr.SetAlpha(0f);
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                cr.SetAlpha(Mathf.Clamp01(timer / fadeDuration));
                yield return null;
            }
            cr.SetAlpha(1f);
        }
    }
    #endregion

    #region Helper Methods
    void SetObjectsActive(GameObject[] objs, bool active)
    {
        foreach (GameObject obj in objs)
        {
            if (obj != null) obj.SetActive(active);
        }
    }

    void SetButtonsInteractable(GameObject[] objs, bool interactable)
    {
        foreach (GameObject obj in objs)
        {
            if (obj == null) continue;

            Button[] buttons = obj.GetComponentsInChildren<Button>(true);
            foreach (Button b in buttons)
            {
                b.interactable = interactable;
            }
        }
    }
    #endregion
}
