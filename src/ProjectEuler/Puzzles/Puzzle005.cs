﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.ProjectEuler.Puzzles
{
    using System;
    using System.Linq;

    /// <summary>
    /// A class representing the solution to <c>https://projecteuler.net/problem=5</c>. This class cannot be inherited.
    /// </summary>
    internal sealed class Puzzle005 : Puzzle
    {
        /// <inheritdoc />
        public override string Question => "What is the smallest positive number that is evenly divisible by all of the numbers from 1 to the specified value?";

        /// <inheritdoc />
        protected override int MinimumArguments => 1;

        /// <inheritdoc />
        protected override int SolveCore(string[] args)
        {
            int max;

            if (!TryParseInt32(args[0], out max) || max < 2)
            {
                Console.WriteLine("The specified maximum number is invalid.");
                return -1;
            }

            // Do not need to bother testing for divisibility by 1, plus the
            // for loop below only tests even numbers so can also exclude 2.
            var divisors = Enumerable.Range(3, max - 2).ToList();

            for (int n = max % 2 == 0 ? max : max - 1; ; n += 2)
            {
                if (max >= 10)
                {
                    // Fast path for large numbers
                    if (n % 3 != 0 || n % 5 != 0 || n % 10 != 0)
                    {
                        continue;
                    }
                }

                if (divisors.All((p) => n % p == 0))
                {
                    Answer = n;
                    break;
                }
            }

            return 0;
        }
    }
}
