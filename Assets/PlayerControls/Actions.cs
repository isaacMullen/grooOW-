using System;
using UnityEngine;

public static class Actions
{
    #region PlayerMovement
    public static Action<Vector2> OnMovePerformed;
    public static Action OnJumpPerformed;
    #endregion

    #region Coins
    public static Action OnCoinCollected;
    #endregion

    #region SceneLoaded
    public static Action OnLevelComplete;
    #endregion

    #region EnemyCollision
    public static Action OnEnemyCollision;
    #endregion

}
