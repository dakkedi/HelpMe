using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelUpController : MonoBehaviour
{
    [SerializeField] private float _attackCooldown = 0.9f;
    [SerializeField] private float _attackDamage = 1.2f;
    [SerializeField] private float _attackRange = 1.2f;
    [SerializeField] private float _attackSpeed = 1.2f;

    [SerializeField] private GameObject _levelUpCardTemplate;
    [SerializeField] private Transform[] _spawnPoints;

    private void Start()
    {
        PlayerController.Instance.OnPlayerLevelUp += PlayerController_OnPlayerLevelUp;
    }

    private void PlayerController_OnPlayerLevelUp(object sender, EventArgs e)
    {
        Debug.Log("Level up cards generating");

        Dictionary<string, float> choices = new Dictionary<string, float>();
        choices["Attack Damage"] = _attackDamage;
        choices["Attack Range"] = _attackRange;
        choices["Attack Speed"] = _attackSpeed;
        choices["Attack Cooldown"] = _attackCooldown;

        // pause game
        GameManager.Instance.HaltGame(true);
        for (int i = 0; i < 3; i++)
        {
            // generate a card
            int randomChoiceIndex = UnityEngine.Random.Range(0, choices.Keys.Count);
            // remove index from choices
            string choiceHeader = choices.Keys.ElementAt(randomChoiceIndex);
            float choiceValue = choices.Values.ElementAt(randomChoiceIndex);
            choices.Remove(choices.Keys.ElementAt(randomChoiceIndex));

            // spawn card
            GameObject card = Instantiate(_levelUpCardTemplate, _spawnPoints[i].position, Quaternion.identity, transform);
            LevelUpHeader choiceHeaderCard = card.GetComponentInChildren<LevelUpHeader>();
            LevelUpDescription choiceValueCard = card.GetComponentInChildren<LevelUpDescription>();

            if (choiceHeaderCard)
            {
                Debug.Log(choiceHeader);
                choiceHeaderCard.SetHeaderText(choiceHeader);
            }
            if (choiceValueCard) {
                Debug.Log(choiceValue.ToString());
                choiceValueCard.SetDescriptionText(choiceValue.ToString());
            }
        }
    }
}
