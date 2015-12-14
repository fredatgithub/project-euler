﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.ProjectEuler.Puzzles
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// A class representing the solution to <c>https://projecteuler.net/problem=2</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Puzzle002 : Puzzle
    {
        public override string Question => "By considering the terms in the Fibonacci sequence starting with 1 and 2 whose values do not exceed the specified value, what is the sum of the even-valued terms?";

        /// <inheritdoc />
        protected override int MinimumArguments => 1;

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            int max;

            if (!int.TryParse(args[0], NumberStyles.Integer & ~NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out max) ||
                max < 1)
            {
                Console.Error.WriteLine("The specified maximum value is invalid.");
                return -1;
            }

            IList<int> fibonacciValues = new List<int>() { 1, 2 };

            while (true)
            {
                int next = fibonacciValues.Last() + fibonacciValues[fibonacciValues.Count - 2];

                if (next > max)
                {
                    break;
                }

                fibonacciValues.Add(next);
            }

            Answer = fibonacciValues
                .Where((p) => p % 2 == 0)
                .Sum();

            return 0;
        }
    }
}