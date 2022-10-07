﻿using System.Collections.Immutable;

namespace RP.ReverieWorld.DiceRoll
{
    public sealed partial class AutoRoller
    {
        private readonly IRandomProvider randomProvider;
        private readonly IParameters defaultParameters;
        private readonly IDiceRemovingSelector diceRemovingSelector;

        /// <summary>
        /// Strategy for "add then remove" dice mechanic.
        /// </summary>
        public interface IDiceRemovingSelector
        {
            /// <summary>
            /// </summary>
            /// <param name="dices">List of current dices values.</param>
            /// <param name="count">Count of dices to remove.</param>
            /// <param name="parameters">Parameters of current roll.</param>
            /// <returns>Set of indices that must be removed with exactly <paramref name="count"/> elements.</returns>
            IReadOnlySet<int> Select(IReadOnlyList<int> dices, int count, IParameters parameters);
        }

        /// <summary>
        /// </summary>
        /// <param name="randomProvider">Implementation of <see cref="IRandomProvider"/> interface.</param>
        /// <param name="defaultParameters">Custom implementation of <see cref="IParameters"/> interface or <see cref="Parameters"/> (default).</param>
        /// <param name="diceRemovingSelector">Custom implementation of <see cref="IDiceRemovingSelector"/> interface or <see cref="DefaultDiceRemovingSelector"/> (default).</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="randomProvider"/> is <see langword="null"/>.</exception>
        public AutoRoller(IRandomProvider randomProvider, IParameters? defaultParameters = null, IDiceRemovingSelector? diceRemovingSelector = null)
        {
            ArgumentNullException.ThrowIfNull(randomProvider);

            defaultParameters ??= new Parameters();
            Parameters.Validate(defaultParameters);

            this.randomProvider = randomProvider;
            this.defaultParameters = defaultParameters;
            this.diceRemovingSelector = diceRemovingSelector ?? new DefaultDiceRemovingSelector();
        }

        /// <summary>
        /// </summary>
        /// <param name="randomProvider">Implementation of <see cref="IRandomProvider"/> interface.</param>
        /// <param name="diceRemovingSelector">Custom implementation of <see cref="IDiceRemovingSelector"/> interface or <see cref="DefaultDiceRemovingSelector"/> (default).</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="randomProvider"/> is <see langword="null"/>.</exception>
        public AutoRoller(IRandomProvider randomProvider, IDiceRemovingSelector? diceRemovingSelector) :
            this(randomProvider, null, diceRemovingSelector)
        {
        }

        public Result Roll(IParameters? parameters = null, IDiceRemovingSelector? diceRemovingSelector = null)
        {
            parameters ??= defaultParameters;
            Parameters.Validate(parameters);

            diceRemovingSelector ??= this.diceRemovingSelector;

            var current = new InteractiveRoller(randomProvider, parameters).Begin();

            current.RemoveDices(diceRemovingSelector.Select(current.Values.Select(d => d.Value).ToImmutableList(), parameters.AdditionalDicesCount, parameters));

            return current.Result();
        }

        public Result Roll(IDiceRemovingSelector? diceRemovingSelector)
        {
            return Roll(null, diceRemovingSelector);
        }
    }
}