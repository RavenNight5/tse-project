using Unity.Netcode.Components;
using UnityEditor.Tilemaps;
using UnityEngine;

public class ClientNetworkTransform : NetworkTransform
{
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
