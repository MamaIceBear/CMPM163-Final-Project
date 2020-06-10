using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Kenny Doan
 * VFXManager is a singleton class that handles instanstiating effects from
 * calls form other scripts in scene
 */
public class VFXManager : MonoBehaviour
{
	public static VFXManager instance;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(this.gameObject);
		}
	}
	/**
  * Base function for VFX
  * name: the name of the effect in resources/effects
  * position: where it spawns
  * delay: how long until it plays
  * emitter: name of the other effect when it dies
  */
	public GameObject PlayEffect(string name, Vector3 position, float delay, string emitter, float duration)
	{
		GameObject inta = Resources.Load<GameObject>("Effects/" + name);
		GameObject effect = Instantiate(inta, position, inta.transform.rotation);
		Transform tEffect = effect.transform;
		VFXScript vfx = effect.GetComponent<VFXScript>();
		if (vfx != null && delay > 0)
		{
			vfx.delay = delay;
		}
		if (vfx != null && duration != 0)
		{
			vfx.duration = duration;
		}

		GameObject emit = Resources.Load<GameObject>("Effects/" + emitter);
		if (emit != null)
		{
			vfx.SetEmitter(emit);
		}

		return effect;
	}

	public GameObject PlayEffect(string name, Vector3 position, float delay)
	{
		return PlayEffect(name, position, delay, "", 0);
	}

	public GameObject PlayEffect(string name, Vector3 position)
	{
		return PlayEffect(name, position, 0, "", 0);
	}

	public GameObject PlayEffectReturn(string name, Vector3 position, float delay, string emitter)
	{
		return PlayEffect(name, position, delay, emitter, 0);
	}

	public GameObject PlayEffectForDuration(string name, Vector3 position, float delay, string emitter, float duration)
	{
		return PlayEffect(name, position, delay, emitter, duration);
	}

	public GameObject PlayEffectForDuration(string name, Vector3 position, float duration)
	{
		return PlayEffect(name, position, 0, "", duration);
	}
}
