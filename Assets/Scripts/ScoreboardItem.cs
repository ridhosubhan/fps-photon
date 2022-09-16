using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class ScoreboardItem : MonoBehaviourPunCallbacks
{
    public TMP_Text textUsername;
    public TMP_Text textKill;
    public TMP_Text textDeath;

    Player player;

    public void Initialized(Player player)
    {
        this.player = player;

        textUsername.text = player.NickName;
        UpdateStats();
    }

    void UpdateStats()
    {
        if (player.CustomProperties.TryGetValue("kills", out object kills))
        {
            textKill.text = kills.ToString();
        }
        if (player.CustomProperties.TryGetValue("deaths", out object deaths))
        {
            textDeath.text = deaths.ToString();
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer == player)
        {
            if (changedProps.ContainsKey("kills") || changedProps.ContainsKey("deaths"))
            {
                UpdateStats();
            }
        }
    }
}
