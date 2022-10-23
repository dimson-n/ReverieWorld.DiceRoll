namespace RP.ReverieWorld.DiceRoll
{
    public sealed class AutoRoller
    {
        private readonly IRandomProvider randomProvider;
        private readonly IParameters defaultParameters;
        private readonly IDiceRemoveStrategy diceRemoveStrategy;

        /// <summary>
        /// </summary>
        /// <param name="randomProvider">Implementation of <see cref="IRandomProvider"/> interface.</param>
        /// <param name="defaultParameters">Custom implementation of <see cref="IParameters"/> interface or <see cref="Parameters"/> (default).</param>
        /// <param name="diceRemoveStrategy">Custom implementation of <see cref="IDiceRemoveStrategy"/> interface or <see cref="DefaultDiceRemoveStrategy"/> (default).</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="randomProvider"/> is <see langword="null"/>.</exception>
        public AutoRoller(IRandomProvider randomProvider, IParameters? defaultParameters = null, IDiceRemoveStrategy? diceRemoveStrategy = null)
        {
            ArgumentNullException.ThrowIfNull(randomProvider);

            defaultParameters ??= Parameters.Default;
            Parameters.Validate(defaultParameters);

            this.randomProvider = randomProvider;
            this.defaultParameters = defaultParameters;
            this.diceRemoveStrategy = diceRemoveStrategy ?? new DefaultDiceRemoveStrategy();
        }

        /// <summary>
        /// </summary>
        /// <param name="randomProvider">Implementation of <see cref="IRandomProvider"/> interface.</param>
        /// <param name="diceRemoveStrategy">Custom implementation of <see cref="IDiceRemoveStrategy"/> interface or <see cref="DefaultDiceRemoveStrategy"/> (default).</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="randomProvider"/> is <see langword="null"/>.</exception>
        public AutoRoller(IRandomProvider randomProvider, IDiceRemoveStrategy? diceRemoveStrategy) :
            this(randomProvider, null, diceRemoveStrategy)
        {
        }

        public Result Roll(IParameters? parameters = null, IDiceRemoveStrategy? diceRemoveStrategy = null)
        {
            parameters ??= defaultParameters;
            Parameters.Validate(parameters);

            diceRemoveStrategy ??= this.diceRemoveStrategy;

            var current = new InteractiveRoller(randomProvider, parameters).Begin();

            if (parameters.AdditionalDicesCount != 0)
            {
                current.RemoveDices(diceRemoveStrategy.Select(current.Values, parameters.AdditionalDicesCount, parameters));
            }

            return current.Result();
        }

        public Result Roll(IDiceRemoveStrategy? diceRemoveStrategy)
        {
            return Roll(null, diceRemoveStrategy);
        }
    }
}
