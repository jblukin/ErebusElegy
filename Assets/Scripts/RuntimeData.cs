using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create New RuntimeData")]

public class RuntimeData : ScriptableObject
{
    
    public GameplayState currentGameState;

    public PlayerMagicState currentPlayerMagic;

    public int currentPlayerCurrency;

}
