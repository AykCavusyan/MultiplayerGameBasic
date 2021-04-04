using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using System.Xml.Serialization;

public class MyNetworkPlayer : NetworkBehaviour
{

    [SerializeField] private TMP_Text Text_DisplayName = null;
    [SerializeField] private Renderer DisplayColorRenderer = null;


    [SyncVar(hook =nameof(HandleDisplayName))]
    [SerializeField]
    private string displayName = "Missing Name";

    [SyncVar(hook = nameof(HandleDisplayColorUpdated))]
    [SerializeField]
    private Color displayColor;


    #region Client
    [ContextMenu("Set My Name")]
    private void SetMyName()
    {
        CmdSetDisplayName("M");
    }

    [ClientRpc]
    private void RpcLogNewName(string newDisplayName)
    {
        Debug.Log(newDisplayName);
    }

    #endregion



    #region Server
    [Server]
    public void SetDisplayName(string newDisplayName)
    {
        displayName = newDisplayName;
    }

    [Server]
    public void SetDisplayColor(Color newDisplaycolor)
    {
        displayColor = newDisplaycolor;
    }
    [Command]
    private void CmdSetDisplayName(string newDisplayName)
    {
        if(newDisplayName.Length < 2 || newDisplayName.Length > 20)
        {
            return;
        }

        RpcLogNewName(newDisplayName);
        SetDisplayName(newDisplayName);
    }

    #endregion


    private void HandleDisplayColorUpdated(Color oldDisplaycolor, Color newDisplayColor )
    {
        DisplayColorRenderer.material.SetColor("_BaseColor", newDisplayColor);
    }

    private void HandleDisplayName(string oldDisplayName, string newDisplayName)
    {
        Text_DisplayName.text = newDisplayName;
    }
}
