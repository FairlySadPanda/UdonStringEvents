// Roll dice!
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class DiceRoller : UdonSharpBehaviour
{
    public UdonLogger logger;

    public void Roll1D20()
    {
        int result = Random.Range(1, 21);
        logger.Notice("A 1D20 was rolled! Result was " + result);
    }

}
