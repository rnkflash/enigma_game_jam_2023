using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    public string toScene = null;
    public string toExit = null;

    public bool playerWasSprinting = false;
}
