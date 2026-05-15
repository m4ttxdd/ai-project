using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCharacter : Character
{
    public static PlayerCharacter Instance { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    public override void Die()
    {
        base.Die();

        GetComponent<MeshRenderer>().enabled = false;
        Invoke(nameof(RestartGame), 3f);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
