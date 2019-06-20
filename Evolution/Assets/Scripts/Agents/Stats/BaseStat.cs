namespace Evolution.Character
{
	public abstract class BaseStat<T> : IStat<T>
	{
		private readonly Agent owner;
		public BaseStat(Agent owner)
		{
			this.owner = owner;
		}
		public Agent Owner => owner;
		public abstract string Name { get; }
		public virtual T Percentage { get; } 
		public abstract T Value { get; protected set; }
		public virtual T RegenerationRate { get; }
		public virtual void ModifyValue(T amount)
		{
		}

		public T MinValue { get; protected set; }

		public T MaxValue { get; protected set; }

		public virtual void UpdateStat(float deltaTime)
		{
		}
	}
}
