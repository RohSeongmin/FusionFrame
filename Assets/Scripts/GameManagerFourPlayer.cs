using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManagerFourPlayer : MonoBehaviour
{
    [Header("Player Settings")]
    public PlayerController[] players; // Expecting 4 players

    [Header("Goal Check")]
    public TileType goalType = TileType.Goal;

    [Header("Death Menu Objects (multiple)")]
    public GameObject[] deathMenuObjects;

    [Header("Victory Menu Objects (multiple, fade-in)")]
    public GameObject[] victoryMenuObjects;

    [Header("Victory Sprite Objects (multiple)")]
    public GameObject[] victorySpriteObjects; // Now supports multiple temporary sprites

    [Header("Level Transition")]
    public string nextLevelName;
    public float fadeDuration = 1f;
    public float victorySpriteDuration = 1f;

    private bool victoryTriggered = false;
    private bool deathTriggered = false;

    private bool[] playerLocked = new bool[4];
    private int totalLocked = 0;

    void Start()
    {
        playerLocked = new bool[players.Length];

        // Hide menus initially
        SetObjectsActive(deathMenuObjects, false);
        SetButtonsInteractable(deathMenuObjects, false);

        SetObjectsActive(victoryMenuObjects, false);
        SetButtonsInteractable(victoryMenuObjects, false);

        if (victorySpriteObjects != null)
        {
            foreach (var obj in victorySpriteObjects)
            {
                if (obj != null) obj.SetActive(false);
            }
        }
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
        // Count players standing on goals right now
        int currentlyOnGoals = 0;
        bool[] onGoalNow = new bool[players.Length];

        for (int i = 0; i < players.Length; i++)
        {
            Tile tile = players[i].CurrentTile();
            if (tile != null && tile.GetTileType() == goalType)
            {
                currentlyOnGoals++;
                onGoalNow[i] = true;
            }
        }

        // Case: exactly 2 players on goals → lock those 2
        if (currentlyOnGoals == 2 && totalLocked < 2)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (onGoalNow[i] && !playerLocked[i])
                {
                    playerLocked[i] = true;
                    players[i].canMove = false;
                    totalLocked++;
                }
            }
        }

        // Case: all 4 players on goals → lock remaining 2 + trigger victory
        if (currentlyOnGoals == 4 && totalLocked < 4)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (onGoalNow[i] && !playerLocked[i])
                {
                    playerLocked[i] = true;
                    players[i].canMove = false;
                    totalLocked++;
                }
            }

            if (!victoryTriggered)
            {
                victoryTriggered = true;
                StartCoroutine(VictorySequence());
            }
        }
    }

    private IEnumerator VictorySequence()
    {
        if (victoryMenuObjects.Length == 0) yield break;

        // Fade in all victory menu objects first
        for (int i = 0; i < victoryMenuObjects.Length; i++)
        {
            yield return StartCoroutine(FadeInObject(victoryMenuObjects[i]));
        }

        // After all menus fade in, swap sprites temporarily for all players
        if (players.Length > 0 && victorySpriteObjects != null && victorySpriteObjects.Length > 0)
        {
            for (int i = 0; i < players.Length && i < victorySpriteObjects.Length; i++)
            {
                if (players[i] != null && victorySpriteObjects[i] != null)
                {
                    yield return StartCoroutine(SwapPlayerSpriteTemporarily(players[i], victorySpriteObjects[i], victorySpriteDuration));
                }
            }
        }

        // Wait briefly
        yield return new WaitForSeconds(1f);

        // ==== PlayerPrefs Level Progression ====
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        PlayerPrefs.SetInt("CurrentLevel", currentLevel + 1); // Increment current level

        if (PlayerPrefs.GetInt("PlayerMaxLevel", 1) < currentLevel)
        {
            PlayerPrefs.SetInt("PlayerMaxLevel", currentLevel); // Update max level if needed
        }

        PlayerPrefs.Save();
        // =====================================

        // Load next level as specified
        if (!string.IsNullOrEmpty(nextLevelName))
        {
            SceneManager.LoadScene(nextLevelName);
        }
    }

    private IEnumerator SwapPlayerSpriteTemporarily(PlayerController player, GameObject newSpriteObj, float duration)
    {
        if (player == null || newSpriteObj == null) yield break;

        SpriteRenderer playerSr = player.GetComponent<SpriteRenderer>();
        if (playerSr != null)
            playerSr.enabled = false;

        newSpriteObj.SetActive(true);

        yield return new WaitForSeconds(duration);

        if (playerSr != null)
            playerSr.enabled = true;

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
