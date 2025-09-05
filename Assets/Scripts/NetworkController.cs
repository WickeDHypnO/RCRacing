using System;
using System.Linq;
using Fusion;
using UnityEngine;

public class NetworkController : SimulationBehaviour, IPlayerJoined, ISceneLoadDone
{
    public GameObject playerPrefab;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        FindFirstObjectByType<FusionBootstrap>().DefaultRoomName = "MainRoom";
        FindFirstObjectByType<FusionBootstrap>().StartSharedClient();
    }

    public void PlayerJoined(PlayerRef player)
    {
        if (Runner.ActivePlayers.Count() == 2)
        {
            Runner.LoadScene("Demo");
        }
    }

    public void SceneLoadDone(in SceneLoadDoneArgs sceneInfo)
    {
        if (sceneInfo.Scene.name == "Demo")
        {
            Runner.Spawn(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
}
