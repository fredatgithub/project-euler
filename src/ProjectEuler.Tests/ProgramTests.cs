﻿// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.ProjectEuler
{
    using Xunit;

    /// <summary>
    /// A class containing tests for the <see cref="Program"/> class. This class cannot be inherited.
    /// </summary>
    public static class ProgramTests
    {
        [Fact(Skip = "No puzzles are implemented yet.")]
        public static void Program_Returns_Zero_If_Input_Valid()
        {
            // Arrange
            string[] args = new[] { string.Empty };

            // Act
            int actual = Program.Main(args);

            // Assert
            Assert.Equal(0, actual);
        }

        [Fact]
        public static void Program_Exits_If_Null_Arguments()
        {
            // Arrange
            string[] args = null;

            // Act
            int actual = Program.Main(args);

            // Assert
            Assert.Equal(-1, actual);
        }

        [Fact]
        public static void Program_Exits_If_No_Arguments()
        {
            // Arrange
            string[] args = new string[0];

            // Act
            int actual = Program.Main(args);

            // Assert
            Assert.Equal(-1, actual);
        }

        [Fact]
        public static void Program_Exits_If_Invalid_Day()
        {
            // Arrange
            string[] args = new[] { "a" };

            // Act
            int actual = Program.Main(args);

            // Assert
            Assert.Equal(-1, actual);
        }

        [Fact]
        public static void Program_Exits_If_Day_Too_Small()
        {
            // Arrange
            string[] args = new[] { "0" };

            // Act
            int actual = Program.Main(args);

            // Assert
            Assert.Equal(-1, actual);
        }

        [Fact]
        public static void Program_Exits_If_Day_Too_Large()
        {
            // Arrange
            string[] args = new[] { "999" };

            // Act
            int actual = Program.Main(args);

            // Assert
            Assert.Equal(-1, actual);
        }
    }
}
