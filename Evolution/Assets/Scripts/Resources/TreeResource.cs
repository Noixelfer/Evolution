using Evolution.Actions;
using Evolution.Character;
using Evolution.Items;
using System.Collections.Generic;
using UnityEngine;

namespace Evolution.Resourcess
{
	public class TreeResource : NaturalResource
	{
		public string Id = "";
		public override string ID => Id;
		private float appleYOffset = 0.85f;
		private readonly float appleGenerationRadius = 0.3f;
		private const int MAX_NUMBER_OF_APPLES = 7;
		private HashSet<GrowingResource> apples = new HashSet<GrowingResource>();
		private bool IsAppleTree = false;
		private float lastGenerateAppleTime = 0;

		public override List<IAction> GetPossibleActions(Agent agent)
		{
			if (!IsAppleTree || apples.Count == 0)
				return new List<IAction>();
			var harvestAction = new HarvestNaturalResource(agent, "Collecting apples", 4, Constants.COLLECT_APPLE_TIME, GetApple, IsResourceAvialbe);
			harvestAction.Effects = new HashSet<string>() { ActionEffects.OBTAINS_FOOD };
			return new List<IAction>() { harvestAction };
		}

		private void Start()
		{
			if (Random.Range(0f, 1f) < Constants.APPLE_TREE_AMMOUNT)
				IsAppleTree = true;
			if (IsAppleTree)
				GenerateApples();
		}

		private void Update()
		{
			if (IsAppleTree)
				if ((Time.time - lastGenerateAppleTime) * Constants.REAL_TIME_MULTIPLIER >= Constants.APPLE_GROW_TIME && apples.Count < MAX_NUMBER_OF_APPLES)
					GenerateApple();
		}

		private bool IsResourceAvialbe()
		{
			return apples.Count > 0;
		}

		private IItem GetApple()
		{
			if (apples.Count == 0)
				return null;
			GrowingResource collectedApple = null;
			foreach (var appleResource in apples)
			{
				if (appleResource != null)
				{
					collectedApple = appleResource;
					break;
				}
			}
			apples.Remove(collectedApple);
			collectedApple.gameObject.SetActive(false);
			var apple = new BaseEdibleItem(new ItemDefinition(0.2f, 0.1f), 25, ItemsUtils.APPLE_ID);
			apple.ItemDefinition.Name = "apple";
			return apple;
		}

		private void GenerateApples()
		{
			var numberOfApples = UnityEngine.Random.Range(3, MAX_NUMBER_OF_APPLES);
			for (int i = 0; i < MAX_NUMBER_OF_APPLES; i++)
			{
				GenerateApple();
			}
		}

		private void GenerateApple()
		{
			var validPosition = false;
			Vector3 newPosition = Vector3.zero;

			while (!validPosition)
			{
				validPosition = true;
				var randomPosition = UnityEngine.Random.insideUnitCircle * appleGenerationRadius;
				newPosition = new Vector3(randomPosition.x, randomPosition.y + appleYOffset, 0);
				foreach (var apple in apples)
					if ((apple.transform.position - newPosition).sqrMagnitude < 0.06f)
					{
						validPosition = false;
						break;
					}
			}
			var applePrefab = Game.Instance.PrefabsManager.GetPrefab<GrowingResource>("apple");
			var appleInstance = Instantiate(applePrefab, transform.position + newPosition, Quaternion.identity, transform);
			apples.Add(appleInstance);
			lastGenerateAppleTime = Time.time;
		}
	}
}
