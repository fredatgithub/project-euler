﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.ProjectEuler.Puzzles
{
    using System;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// A class representing the solution to <c>https://projecteuler.net/problem=10</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Puzzle010 : Puzzle
    {
        public override string Question => "Find the sum of all the primes below the specified value.";

        /// <inheritdoc />
        protected override int MinimumArguments => 1;

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            int max;

            if (!int.TryParse(args[0], NumberStyles.Integer & ~NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out max) ||
                max < 2)
            {
                Console.Error.WriteLine("The specified number is invalid.");
                return -1;
            }

            Answer = Enumerable.Range(2, max - 1)
                .Where((p) => Maths.IsPrime(p))
                .Select((p) => (long)p)
                .Sum();

            return 0;
        }
    }
}