using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class CoinController : MonoBehaviour
{
    private bool _following = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            GameManager.Instance.coinCollected();
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (IsPlayerInReach() && !_following)
        {
            _following = true;
        }

        if (_following)
        {
            transform.position = Vector3.MoveTowards(transform.position, GameManager.Instance.GetPlayer().transform.position, Time.deltaTime * 5f);
        }
    }

    private bool IsPlayerInReach()
    {
        GameObject player = GameManager.Instance.GetPlayer();
        float radius = player.GetComponent<PlayerController>().GetPullRadius();
        Vector3 playerPos = player.transform.position;
        Vector3 coinPos = transform.position;
        float distance = Vector3.Distance(playerPos, coinPos);
        return distance <= radius;
    }
}
