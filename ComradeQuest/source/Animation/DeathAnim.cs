using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Animator))]
public class DeathAnim : MonoBehaviour {
/// <summary>
/// This class is used as a parent only.
/// </summary>

	protected int deathFrames = 0; // Frame counter
	protected int deathCount = 1; // How many frames 'til death
	private AnimatorStateInfo state; // Determines length of death animation for the purposes of deathCount
	//public Animator deathAnimator; // How cool do I look in my last moments? This is defined in child classes.
	
	public int GetDeathFrames()
	{
		return deathFrames;
	}
	public int GetDeathCount()
	{
		return deathCount;
	}
	public void SetDeathFrames(int x)
	{
		deathFrames = x;
	}
	public void SetDeathCount(int x)
	{
		deathCount = x;
	}
}
