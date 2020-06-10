using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSplash : MonoBehaviour
{
	public GameObject SplashDrops;
	public GameObject Waves;

	public void SetDrops(int count)
	{
		var ps = SplashDrops.GetComponent<ParticleSystem>();
		var em = ps.emission;
		em.burstCount = count;
	}

	public void SetWaves(int count)
	{
		var ps = Waves.GetComponent<ParticleSystem>();
		var em = ps.emission;
		em.burstCount = count;
	}
}
