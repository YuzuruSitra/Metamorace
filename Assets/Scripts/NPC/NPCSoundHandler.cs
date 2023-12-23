using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSoundHandler : MonoBehaviour
{
   private SoundHandler _soundHandler;

    [SerializeField] 
    AudioClip jump, breakBlock, createBlock;
    // Start is called before the first frame update
    void Start()
    {
        _soundHandler = SoundHandler.InstanceSoundHandler;
    }

    public void PlayJumpSE()
    {
        _soundHandler.PlaySE(jump);
    }

    public void CreateBlockSE()
    {
        _soundHandler.PlaySE(createBlock);
    }

    public void BreakBlockSE()
    {
        _soundHandler.PlaySE(breakBlock);
    }
}
