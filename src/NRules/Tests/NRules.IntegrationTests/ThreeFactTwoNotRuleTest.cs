﻿using NRules.IntegrationTests.TestAssets;
using NRules.IntegrationTests.TestRules;
using NUnit.Framework;

namespace NRules.IntegrationTests
{
    [TestFixture]
    public class ThreeFactTwoNotRuleTest : BaseRuleTestFixture
    {
        [Test]
        public void Fire_NotMatchingNotPatternFacts_FiresOnce()
        {
            //Arrange
            var fact1 = new FactType1 {TestProperty = "Valid Value 1"};

            Session.Insert(fact1);

            //Act
            Session.Fire();

            //Assert
            AssertFiredOnce();
        }
        
        [Test]
        public void Fire_MatchingNotPatternFactsBothKind_DoesNotFire()
        {
            //Arrange
            var fact1 = new FactType1 {TestProperty = "Valid Value 1"};
            var fact2 = new FactType2 {TestProperty = "Valid Value 2", JoinProperty = fact1.TestProperty};
            var fact3 = new FactType3 {TestProperty = "Valid Value 3", JoinProperty = fact1.TestProperty};

            Session.Insert(fact1);
            Session.Insert(fact2);
            Session.Insert(fact3);

            //Act
            Session.Fire();

            //Assert
            AssertDidNotFire();
        }
        
        [Test]
        public void Fire_MatchingNotPatternFactsFirst_DoesNotFire()
        {
            //Arrange
            var fact1 = new FactType1 {TestProperty = "Valid Value 1"};
            var fact2 = new FactType2 {TestProperty = "Valid Value 2", JoinProperty = fact1.TestProperty};

            Session.Insert(fact1);
            Session.Insert(fact2);

            //Act
            Session.Fire();

            //Assert
            AssertDidNotFire();
        }

        [Test]
        public void Fire_MatchingNotPatternFactsSecondKind_DoesNotFire()
        {
            //Arrange
            var fact1 = new FactType1 {TestProperty = "Valid Value 1"};
            var fact3 = new FactType3 {TestProperty = "Valid Value 3", JoinProperty = fact1.TestProperty};

            Session.Insert(fact1);
            Session.Insert(fact3);

            //Act
            Session.Fire();

            //Assert
            AssertDidNotFire();
        }

        [Test]
        public void Fire_MatchingNotPatternFactsInsertedThenRetracted_FiresOnce()
        {
            //Arrange
            var fact1 = new FactType1 {TestProperty = "Valid Value 1"};
            var fact2 = new FactType2 {TestProperty = "Valid Value 2", JoinProperty = fact1.TestProperty};
            var fact3 = new FactType3 {TestProperty = "Valid Value 3", JoinProperty = fact1.TestProperty};

            Session.Insert(fact1);
            Session.Insert(fact2);
            Session.Insert(fact3);
            Session.RetractAll(new object[] {fact2, fact3});

            //Act
            Session.Fire();

            //Assert
            AssertFiredOnce();
        }

        [Test]
        public void Fire_MatchingNotPatternFactsTwoInsertedThenFirstRetracted_DoesNotFire()
        {
            //Arrange
            var fact1 = new FactType1 {TestProperty = "Valid Value 1"};
            var fact2 = new FactType2 {TestProperty = "Valid Value 2", JoinProperty = fact1.TestProperty};
            var fact3 = new FactType3 {TestProperty = "Valid Value 3", JoinProperty = fact1.TestProperty};

            Session.Insert(fact1);
            Session.Insert(fact2);
            Session.Insert(fact3);
            Session.Retract(fact2);

            //Act
            Session.Fire();

            //Assert
            AssertDidNotFire();
        }

        [Test]
        public void Fire_MatchingNotPatternFactsTwoInsertedThenSecondRetracted_DoesNotFire()
        {
            //Arrange
            var fact1 = new FactType1 {TestProperty = "Valid Value 1"};
            var fact2 = new FactType2 {TestProperty = "Valid Value 2", JoinProperty = fact1.TestProperty};
            var fact3 = new FactType3 {TestProperty = "Valid Value 3", JoinProperty = fact1.TestProperty};

            Session.Insert(fact1);
            Session.Insert(fact2);
            Session.Insert(fact3);
            Session.Retract(fact3);

            //Act
            Session.Fire();

            //Assert
            AssertDidNotFire();
        }

        [Test]
        public void Fire_InvalidNotPatternFactsInsertedThenUpdated_DoesNotFire()
        {
            //Arrange
            var fact1 = new FactType1 {TestProperty = "Valid Value 1"};
            var fact2 = new FactType2 {TestProperty = "Invalid Value 2", JoinProperty = fact1.TestProperty};
            var fact3 = new FactType3 {TestProperty = "Invalid Value 3", JoinProperty = fact1.TestProperty};

            Session.Insert(fact1);
            Session.Insert(fact2);
            Session.Insert(fact3);

            fact2.TestProperty = "Valid Value 2";
            Session.Update(fact2);

            fact3.TestProperty = "Valid Value 3";
            Session.Update(fact3);

            //Act
            Session.Fire();

            //Assert
            AssertDidNotFire();
        }

        [Test]
        public void Fire_MatchingNotPatternFactsInsertedThenUpdated_FiresOnce()
        {
            //Arrange
            var fact1 = new FactType1 {TestProperty = "Valid Value 1"};
            var fact2 = new FactType2 {TestProperty = "Valid Value 2", JoinProperty = fact1.TestProperty};
            var fact3 = new FactType3 {TestProperty = "Valid Value 3", JoinProperty = fact1.TestProperty};

            Session.Insert(fact1);
            Session.Insert(fact2);
            Session.Insert(fact3);

            fact2.TestProperty = "Invalid Value 2";
            Session.Update(fact2);

            fact3.TestProperty = "Invalid Value 3";
            Session.Update(fact3);

            //Act
            Session.Fire();

            //Assert
            AssertFiredOnce();
        }

        [Test]
        public void Fire_MatchingNotPatternFactsTwoInsertedThenFirstUpdated_DoesNotFire()
        {
            //Arrange
            var fact1 = new FactType1 {TestProperty = "Valid Value 1"};
            var fact2 = new FactType2 {TestProperty = "Valid Value 2", JoinProperty = fact1.TestProperty};
            var fact3 = new FactType3 {TestProperty = "Valid Value 3", JoinProperty = fact1.TestProperty};

            Session.Insert(fact1);
            Session.Insert(fact2);
            Session.Insert(fact3);

            fact2.TestProperty = "Invalid Value 2";
            Session.Update(fact2);

            //Act
            Session.Fire();

            //Assert
            AssertDidNotFire();
        }

        [Test]
        public void Fire_MatchingNotPatternFactsTwoInsertedThenSecondUpdated_DoesNotFire()
        {
            //Arrange
            var fact1 = new FactType1 {TestProperty = "Valid Value 1"};
            var fact2 = new FactType2 {TestProperty = "Valid Value 2", JoinProperty = fact1.TestProperty};
            var fact3 = new FactType3 {TestProperty = "Valid Value 3", JoinProperty = fact1.TestProperty};

            Session.Insert(fact1);
            Session.Insert(fact2);
            Session.Insert(fact3);

            fact3.TestProperty = "Invalid Value 3";
            Session.Update(fact3);

            //Act
            Session.Fire();

            //Assert
            AssertDidNotFire();
        }

        [Test]
        public void Fire_MatchingNotPatternFactsInsertedThenRetractSomeOfThemButNotAll_StillDoesNotFire()
        {
            //Arrange
            var fact1 = new FactType1 {TestProperty = "Valid Value 1"};

            var fact2 = new FactType2 {TestProperty = "Valid Value 2", JoinProperty = fact1.TestProperty};
            var fact3 = new FactType2 {TestProperty = "Valid Value 3", JoinProperty = fact1.TestProperty};
            var fact4 = new FactType2 {TestProperty = "Valid Value 4", JoinProperty = fact1.TestProperty};

            var fact5 = new FactType3 {TestProperty = "Valid Value 5", JoinProperty = fact1.TestProperty};
            var fact6 = new FactType3 {TestProperty = "Valid Value 6", JoinProperty = fact1.TestProperty};

            var allFacts = new object[] {fact1, fact2, fact3, fact4, fact5, fact6};
            Session.InsertAll(allFacts);
            Session.RetractAll(new object[] {fact3, fact4, fact5});

            //Act
            Session.Fire();

            //Assert
            AssertDidNotFire();
        }

        [Test]
        public void Fire_MatchingNotPatternFactsInsertedTheUpdatesSomeOfThemButNotAll_StillDoesNotFire()
        {
            //Arrange
            var fact1 = new FactType1 {TestProperty = "Valid Value 1"};

            var fact2 = new FactType2 {TestProperty = "Valid Value 2", JoinProperty = fact1.TestProperty};
            var fact3 = new FactType2 {TestProperty = "Valid Value 3", JoinProperty = fact1.TestProperty};
            var fact4 = new FactType2 {TestProperty = "Valid Value 4", JoinProperty = fact1.TestProperty};

            var fact5 = new FactType3 {TestProperty = "Valid Value 5", JoinProperty = fact1.TestProperty};
            var fact6 = new FactType3 {TestProperty = "Valid Value 6", JoinProperty = fact1.TestProperty};

            var allFacts = new object[] {fact1, fact2, fact3, fact4, fact5, fact6};
            Session.InsertAll(allFacts);

            fact3.JoinProperty = "FakeJoinProp";
            fact4.JoinProperty = "FakeJoinProp";
            fact5.JoinProperty = "FakeJoinProp";
            var facts = new object[] {fact3, fact4, fact5};
            Session.UpdateAll(facts);

            //Act
            Session.Fire();

            //Assert
            AssertDidNotFire();
        }

        protected override void SetUpRules()
        {
            SetUpRule<ThreeFactTwoNotRule>();
        }
    }
}