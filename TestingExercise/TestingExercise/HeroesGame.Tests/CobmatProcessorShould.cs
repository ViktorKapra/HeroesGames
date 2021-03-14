using System;
using System.Collections.Generic;
using System.Text;
using HeroesGame.Implementation.Hero;
using NUnit.Framework;
using HeroesGame.Contract;
using HeroesGame.Implementation.Monster;
using HeroesGame.Implementation.Hero;
using HeroesGame.Constant;

namespace HeroesGame.Tests
{
  public  class CobmatProcessorShould
    {
       private CombatProcessor cp;

        [SetUp]
        public void Setup()
        {
            cp = new CombatProcessor(new Hunter());
        }
        [Test]
        public void InitializeCorrectly()
        {
            //Assert
            Assert.That(cp.Hero, Is.Not.Null);
            Assert.That(cp.Logger, Is.Not.Null);
            Assert.That(cp.Logger, Is.Empty);
        }
        [Test]
        public void FightCorrectly_WeakerEnemy()
        {
            //Arrange
            IMonster monster = new MedusaTheGorgon(level: 1);
            LevelUpHero(50);

            //Act
            cp.Fight(monster);
            List<string> logger = cp.Logger;

            //Assert - expected monster to die in 1 hit
            Assert.That(logger.Count, Is.EqualTo(2));
            Assert.That(logger, Does.Contain("The Hunter hits the MedusaTheGorgon dealing 510 damage to it.")
                .And.Contains("The monster dies. (4 XP gained.)"));
        }
        [Test]
        public void FightCorrectly_StrongerEnemy()
        {
            //Arrange
            IMonster monster = new MedusaTheGorgon(level: 50);

            //Act
            cp.Fight(monster);
            List<string> logger = cp.Logger;

            //Assert - expected hero to die in 1 hit but heal 3 times which gives him an extra turn
            Assert.That(logger, Has.Count.EqualTo(12));
            Assert.That(logger, Does.Contain("The hero dies on level 1 after 4 fights."));
        }
        private void LevelUpHero(int levels)
        {
            for (int i = 0; i < levels; i++)
            {
                cp.Hero.GainExperience(HeroConstants.MaximumExperience);
            }
        }
    }
}
