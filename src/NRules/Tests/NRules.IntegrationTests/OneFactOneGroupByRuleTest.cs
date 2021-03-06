﻿using System.Linq;
using NRules.IntegrationTests.TestAssets;
using NRules.IntegrationTests.TestRules;
using NUnit.Framework;

namespace NRules.IntegrationTests
{
    [TestFixture]
    public class OneFactOneGroupByRuleTest : BaseRuleTestFixture
    {
        [Test]
        public void Fire_NoMatchingFacts_DoesNotFire()
        {
            //Arrange - Act
            Session.Fire();

            //Assert
            AssertDidNotFire();
        }

        [Test]
        public void Fire_TwoFactsForOneGroupAndOneForAnother_FiresOnceWithTwoFactsInOneGroup()
        {
            //Arrange
            var fact1 = new FactType1 {TestProperty = "Valid Value Group1"};
            var fact2 = new FactType1 {TestProperty = "Valid Value Group1"};
            var fact3 = new FactType1 {TestProperty = "Valid Value Group2"};

            var facts = new[] {fact1, fact2, fact3};
            Session.InsertAll(facts);

            //Act
            Session.Fire();

            //Assert
            AssertFiredOnce();
            Assert.AreEqual(2, GetFiredFact<IGrouping<string, string>>().Count());
        }

        [Test]
        public void Fire_TwoFactsForOneGroupAndTwoForAnother_FiresTwiceWithTwoFactsInEachGroup()
        {
            //Arrange
            var fact1 = new FactType1 {TestProperty = "Valid Value Group1"};
            var fact2 = new FactType1 {TestProperty = "Valid Value Group1"};
            var fact3 = new FactType1 {TestProperty = "Valid Value Group2"};
            var fact4 = new FactType1 {TestProperty = "Valid Value Group2"};

            var facts = new[] {fact1, fact2, fact3, fact4};
            Session.InsertAll(facts);

            //Act
            Session.Fire();

            //Assert
            AssertFiredTwice();
            Assert.AreEqual(2, GetFiredFact<IGrouping<string, string>>(0).Count());
            Assert.AreEqual(2, GetFiredFact<IGrouping<string, string>>(1).Count());
        }

        [Test]
        public void Fire_TwoFactsForOneGroupAndTwoForAnotherOneRetracted_FiresOnceWithTwoFactsInOneGroup()
        {
            //Arrange
            var fact1 = new FactType1 {TestProperty = "Valid Value Group1"};
            var fact2 = new FactType1 {TestProperty = "Valid Value Group1"};
            var fact3 = new FactType1 {TestProperty = "Valid Value Group2"};
            var fact4 = new FactType1 {TestProperty = "Valid Value Group2"};

            var facts = new[] {fact1, fact2, fact3, fact4};
            Session.InsertAll(facts);

            Session.Retract(fact4);

            //Act
            Session.Fire();

            //Assert
            AssertFiredOnce();
            Assert.AreEqual(2, GetFiredFact<IGrouping<string, string>>().Count());
        }

        [Test]
        public void Fire_TwoFactsForOneGroupAndTwoForAnotherOneUpdatedToInvalid_FiresOnceWithTwoFactsInOneGroup()
        {
            //Arrange
            var fact1 = new FactType1 {TestProperty = "Valid Value Group1"};
            var fact2 = new FactType1 {TestProperty = "Valid Value Group1"};
            var fact3 = new FactType1 {TestProperty = "Valid Value Group2"};
            var fact4 = new FactType1 {TestProperty = "Valid Value Group2"};

            var facts = new[] {fact1, fact2, fact3, fact4};
            Session.InsertAll(facts);

            fact4.TestProperty = "Invalid Value";
            Session.Update(fact4);

            //Act
            Session.Fire();

            //Assert
            AssertFiredOnce();
            Assert.AreEqual(2, GetFiredFact<IGrouping<string, string>>().Count());
        }

        [Test]
        public void Fire_TwoFactsForOneGroupAndTwoForAnotherOneUpdatedToFirstGroup_FiresOnceWithThreeFactsInOneGroup()
        {
            //Arrange
            var fact1 = new FactType1 {TestProperty = "Valid Value Group1"};
            var fact2 = new FactType1 {TestProperty = "Valid Value Group1"};
            var fact3 = new FactType1 {TestProperty = "Valid Value Group2"};
            var fact4 = new FactType1 {TestProperty = "Valid Value Group2"};

            var facts = new[] {fact1, fact2, fact3, fact4};
            Session.InsertAll(facts);

            fact4.TestProperty = "Valid Value Group1";
            Session.Update(fact4);

            //Act
            Session.Fire();

            //Assert
            AssertFiredOnce();
            var actual = GetFiredFact<IGrouping<string, string>>().Count();
            Assert.AreEqual(3, actual);
        }

        [Test]
        public void Fire_TwoFactsForOneGroupAndOneForAnotherAndOneInvalidTheInvalidUpdatedToSecondGroup_FiresTwiceWithTwoFactsInEachGroup()
        {
            //Arrange
            var fact1 = new FactType1 {TestProperty = "Valid Value Group1"};
            var fact2 = new FactType1 {TestProperty = "Valid Value Group1"};
            var fact3 = new FactType1 {TestProperty = "Valid Value Group2"};
            var fact4 = new FactType1 {TestProperty = "Invalid Value"};

            var facts = new[] {fact1, fact2, fact3, fact4};
            Session.InsertAll(facts);

            fact4.TestProperty = "Valid Value Group2";
            Session.Update(fact4);

            //Act
            Session.Fire();

            //Assert
            AssertFiredTwice();
            Assert.AreEqual(2, GetFiredFact<IGrouping<string, string>>(0).Count());
            Assert.AreEqual(2, GetFiredFact<IGrouping<string, string>>(1).Count());
        }

        protected override void SetUpRules()
        {
            SetUpRule<OneFactOneGroupByRule>();
        }
    }
}
