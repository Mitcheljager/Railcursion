using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class PlayersManager : NetworkBehaviour {
    public List<PlayerReference> players;
    public PlayerReference currentPlayer;
}
