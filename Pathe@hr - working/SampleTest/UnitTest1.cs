using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SampleTest;

namespace SampleTest
{
    [TestClass]
    public class ChairSelectionTests
    {
        private Stoel[,] stoelArray;
        private List<Stoel> chairs;

        [TestInitialize]
        public void Setup()
        {
            int rows = 3;
            int columns = 3;
            chairs = new List<Stoel>();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    chairs.Add(new Stoel(i, j, true, false, columns));
                }
            }

            stoelArray = CreateStoelArray(rows, columns);
            stoelArray[2, 2].selected = true;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Assert.IsNotNull(stoelArray[i, j], $"stoelArray[{i},{j}] is null.");
                }
            }
        }

        private Stoel[,] CreateStoelArray(int rows, int columns)
        {
            Stoel[,] array = new Stoel[rows, columns];

            int index = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    array[i, j] = chairs[index++];
                }
            }

            return array;
        }

        private (int, int) findSelectedChair()
        {
            int rows = stoelArray.GetLength(0);
            int columns = stoelArray.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (stoelArray[i, j] != null && stoelArray[i, j].isCurrentChair)
                    {
                        return (i, j);
                    }
                }
            }
            return (0, 0);
        }

        [TestMethod]
        public void Test_ChairNotIsCurrent()
        {
            var result = findSelectedChair();
            Assert.AreEqual((0, 0), result);
        }

        [TestMethod]
        public void Test_ChairIsCurrent()
        {
            stoelArray[1, 1].isCurrentChair = true;
            var result = findSelectedChair();
            Assert.AreEqual((1, 1), result);
        }

        [TestMethod]
        public void Test_ChairIsCurrentAtDifferentLocation()
        {
            stoelArray[2, 2].isCurrentChair = true;
            var result = findSelectedChair();
            Assert.AreEqual((2, 2), result);
        }
    }
}