using Evolution.Character;
using UnityEngine;

namespace Evolution.Actions
{
	public class Breed : BaseAction
	{
		public override string ID => "Breed";
		private Agent requester;
		private Agent receiver;

		public Breed(Agent requester, Agent receiver)
		{
			this.requester = requester;
			this.receiver = receiver;
		}

		public override void OnStart()
		{
			base.OnStart();
			receiver.RequesterArrived(requester, null);
		}

		public override ActionStatus OnUpdate(float time)
		{
			var agent = MonoBehaviour.Instantiate(Game.Instance.PrefabsManager.GetPrefab<Agent>("female_agent"));
			int posX = (int)(Mathf.Round(requester.Transform.position.x));
			int posY = (int)(Mathf.Round(requester.Transform.position.y));
			agent.transform.position = new Vector3(posX, posY, 0);
			agent.transform.parent = GameObject.Find("Agents Container").transform;
			agent.CharacterTraits = Traits.GetTraitsFromParents(requester, receiver);
			Game.Instance.UIManager.UILog.AddLog(agent.AGENT_ID + " was born from " + requester.AGENT_ID + " and " + receiver.AGENT_ID, UI.Event.BREED);
			requester.BreedController.OnBreed();
			receiver.BreedController.OnBreed();
			return ActionStatus.SUCCESSFULLY_EXECUTED;
		}

		public override float GetScoreBasedOnTraits()
		{
			var breedScore = GetBreedScore();
			var lustScore = Mathf.Pow(requester.CharacterTraits[Traits.LUST_TRAIT].Percentage, 2.25f);
			var compatibilityScore = requester.BreedController.Compability(receiver);
			if (breedScore != 0)
			{
				breedScore = Mathf.Clamp(breedScore * 0.4f + 0.3f * lustScore + 0.3f * compatibilityScore, 0, 1);
			}
			return breedScore;
		}

		private float GetBreedScore()
		{
			var currentAgeInYears = requester.StatsManager.Age.GetAge().Years;
			if (currentAgeInYears <= Constants.MINIMUM_BREED_AGE || currentAgeInYears >= Constants.MAXIMUM_BREED_AGE)
				return -1;
			var x = Mathf.Clamp(currentAgeInYears / (Constants.MAXIMUM_BREED_AGE - Constants.MINIMUM_BREED_AGE), 0, 1);
			if (x <= 0.1f)
				return Mathf.Clamp(0.5f + Mathf.Pow(x, 0.3f), 0, 1);
			else
				return Mathf.Clamp(-Mathf.Pow(x, 2) + 1.1f, 0, 1);
		}
	}
}
