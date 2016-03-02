using FftGuitarTuner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TunerTest
{


    /// <summary>
    ///This is a test class for NotesTest and is intended
    ///to contain all NotesTest Unit Tests
    ///</summary>
    [TestClass()]
    public class NotesTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        #region Additional test attributes

        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //

        #endregion


        /// <summary>
        ///A test for GetClosestNote
        ///</summary>
        [TestMethod()]
        public void GetClosestNoteTest()
        {
            Notes.GetClosestNote(1695);
            Notes.GetClosestNote2(1695);
        }

        [TestMethod()]
        public void GetClosestNoteTest2()
        {
            int Min = 0;
            int Max = 5000;
            Random randNum = new Random();
            var test2 = Enumerable
                .Repeat(0, 500000)
                .Select(i => randNum.Next(Min, Max))
                .ToList();

            foreach (var VARIABLE in test2)
            {
                var a = Notes.GetClosestNote2(VARIABLE);
                var b = Notes.GetClosestNote(VARIABLE);
                Assert.AreEqual(a,b, VARIABLE.ToString());
            }
        }
    }
}
