using Evolution.Character;
using UnityEngine;

namespace Evolution.Actions
{
	public class MoveToAnotherArea : BaseAction
	{
		public override string ID => "NoveToAnotherArea";
		public override string Description { get => base.Description; protected set => base.Description = value; }
		private Agent agent;
		private bool hasFoundValdPosition = false;
		private Vector2 destination = Vector3.zero;
		private MoveAction MoveAction;

		public MoveToAnotherArea(Agent agent)
		{
			this.agent = agent;
		}

		public override void OnStart()
		{
			Description = "Moving to another area";
			base.OnStart();
			destination = GetValidPosition(out hasFoundValdPosition);
			if (hasFoundValdPosition)
			{
				MoveAction = new MoveAction(agent, destination);
				MoveAction.Initialize();
			}
		}

		public override ActionStatus OnUpdate(float time)
		{
			if (!hasFoundValdPosition)
				return ActionStatus.FAILED;
			return MoveAction.OnUpdate(0);
		}

		private Vector2 GetValidPosition(out bool validPosition)
		{
			float maxTries = 5;
			validPosition = false;
			Vector2 foundPosition = Vector2.zero;
			int i = 0;
			while (i < maxTries && !validPosition)
			{
				int xOffset = (int)(Random.Range(3, 9) * Mathf.Pow(-1, Random.Range(1, 3)));
				int yOffset = (int)(Random.Range(3, 9) * Mathf.Pow(-1, Random.Range(1, 3)));

				int posX = (int)(Mathf.Round(agent.Transform.position.x));
				int posY = (int)(Mathf.Round(agent.Transform.position.y));

				//search for valid position around that area
				var node = Game.Instance.MapManager.MapGraph.GetNode(posX + xOffset, posY + yOffset);
				if (node != null)
				{
					validPosition = true;
					foundPosition = new Vector3(posX + xOffset, posY + yOffset);
				}
				i++;
			}
			return foundPosition;
		}

		public override float GetScoreBasedOnTraits()
		{
			return 0.2f * agent.CharacterTraits[Traits.CURIOUS_TRAIT].Percentage;
		}
	}
}
