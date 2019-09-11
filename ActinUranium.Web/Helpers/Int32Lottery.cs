using System;

namespace ActinUranium.Web.Helpers
{
    internal sealed class Int32Lottery : Lottery<int>
    {
        /// <summary>
        /// Populates a new instance of the <see cref="Int32Lottery"/> class with numbers in the specified range.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the lottery numbers. <paramref name="maxValue"/> must
        /// be greater than or equal to 0.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxValue"/> is less than 0.</exception>
        public Int32Lottery(int maxValue)
        {
            if (maxValue < 0)
            {
                var message = $"'{nameof(maxValue)}' must be greater than 0.";
                throw new ArgumentOutOfRangeException(nameof(maxValue), message);
            }

            Seed(0, maxValue);
        }


        /// <summary>
        /// Populates a new instance of the <see cref="Int32Lottery"/> class with numbers in the specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the lottery numbers.</param>
        /// <param name="maxValue">The exclusive upper bound of the lottery numbers. <paramref name="maxValue"/> must
        /// be greater than or equal to <paramref name="minValue"/>.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="minValue"/> is greater than
        /// <paramref name="maxValue"/>.</exception>
        public Int32Lottery(int minValue, int maxValue)
        {
            if (minValue > maxValue)
            {
                var message = $"'{nameof(minValue)}' cannot be greater than '{nameof(maxValue)}'.";
                throw new ArgumentOutOfRangeException(nameof(minValue), message);
            }

            Seed(minValue, maxValue);
        }

        private void Seed(int minValue, int maxValue)
        {
            for (int number = minValue; number < maxValue; number++)
            {
                Add(number);
            }
        }
    }
}
