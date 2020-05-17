using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereController)), RequireComponent(typeof(ChuckSubInstance)), RequireComponent(typeof(AudioSource))]
public class SphereAudioController : MonoBehaviour
{
    private SphereController sc;
    private ChuckSubInstance chuck;
    private AudioSource source;

    void Awake()
    {
        sc = GetComponent<SphereController>();
        chuck = GetComponent<ChuckSubInstance>();
        source = GetComponent<AudioSource>();
    }

    public void Play()
    {
        source.volume = 1f;
        chuck.RunCode(@"
            SndBuf textureBuf => dac;
            me.dir() + ""texture01.wav"" => textureBuf.read;

            // start at the beginning of the clip
            0 => textureBuf.pos;

            // pass time so that the file plays
            textureBuf.length() / textureBuf.rate() => now;
        ");

        // chuck.RunCode(@"
        // 	TriOsc spatialOsc => dac;
        // 	while( true )
        // 	{
        // 		Math.random2f( 300, 1000 ) => spatialOsc.freq;
        // 		50::ms => now;
        // 	}
        // ");
    }

    public void Stop()
    {
        source.volume = 0f;
    }
}
