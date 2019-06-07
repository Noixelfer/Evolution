namespace Evolution.Character
{
	public interface IStat<T>
	{
		Agent Owner { get; }
		string Name { get; }
		T Value { get; }
		T RegenerationRate { get; }
		T MinValue { get; }
		T MaxValue { get; }

		void UpdateStat(float deltaTime);
	}
}