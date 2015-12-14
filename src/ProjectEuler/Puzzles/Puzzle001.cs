﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.ProjectEuler.Puzzles
{
    using System;
    using System.Globalization;

    /// <summary>
    /// A class representing the solution to <c>https://projecteuler.net/problem=1</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Puzzle001 : Puzzle
    {
        public override string Question => "What is the sum of all the multiples of 3 or 5 below the given value?";

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

            int sum = 0;

            for (int i = 1; i < max; i++)
            {
                if (i % 3 == 0 || i % 5 == 0)
                {
                    sum += i;
                }
            }

            Answer = sum;

            return 0;
        }
    }
}