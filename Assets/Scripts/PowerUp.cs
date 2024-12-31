using UnityEngine;

public class PowerUp : MonoBehaviour
{

    public SFXManager manager;
    public enum Type
    {
        Coin,
        ExtraLife,
        MagicMushroom,
        Starpower,
    }

    public Type type;

    private void Start()
    {
        manager = GameObject.Find("SFX").GetComponent<SFXManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Player player)) {
            Collect(player);
            manager.SetAudio(manager.coin);
        }
    }

    private void Collect(Player player)
    {
        switch (type)
        {
            case Type.Coin:
                GameManager.Instance.AddCoin();
                break;

            case Type.ExtraLife:
                GameManager.Instance.AddLife();
                break;

            case Type.MagicMushroom:
                player.Grow();
                break;

            case Type.Starpower:
                player.Starpower();
                break;
        }

        Destroy(gameObject);
    }

}
