using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{

	private enum LadderPart { complete, botom, top }
	[SerializeField] LadderPart part = LadderPart.complete;


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<PlayerController>())
		{
			PlayerController player = collision.GetComponent<PlayerController>();
			switch (part)
			{
				case LadderPart.complete:
					player.canClimb = true;
					player.ladder = this;
					break;
				case LadderPart.top:
					player.topLadder = true;
					break;
				case LadderPart.botom:
					player.bottomLadder = true;
					break;
				default:
					break;
			}
		}

	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.GetComponent<PlayerController>())
		{
			PlayerController player = collision.GetComponent<PlayerController>();
			switch (part)
			{
				case LadderPart.complete:
					player.canClimb = false;
					break;
				case LadderPart.top:
					player.topLadder = false;
					break;
				case LadderPart.botom:
					player.bottomLadder = false;
					break;
				default:
					break;
			}
		}
	}
}
