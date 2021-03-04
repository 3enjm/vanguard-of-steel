using NUnit.Framework;


using VanguardOfSteel.Weapons;

namespace VanguardOfSteelTest.Weapon
{
    public class WeaponClipTest
    {
        [Test]
        public void Constructor_RegularTest()
        {
            var mag = new WeaponClip(1,
                new WeaponRound(Caliber.MM_50, Capability.AP));

            Assert.That(mag.RoundCount, Is.EqualTo(1),
                "Magazine should be at capacity");
        }

        [Test]
        public void Constructor_ExceptionTest()
        {
            var mag = new WeaponClip(5, 6,
                new WeaponRound( Caliber.MM_50, Capability.AP));

            Assert.That(mag.RoundCount, Is.Not.GreaterThan(mag.Capacity),
                "Magazine should not be instantiated with a round count higher than it's capacity.");
            Assert.That(mag.RoundCount, Is.EqualTo(mag.Capacity),
                "Magazine round count should cap at mag capacity if a higher round count is passed to constructor");
        }

        [Test]
        public void IsEmpty_RegularTest()
        {
            var mag = new WeaponClip(0,
                new WeaponRound(Caliber.MM_50, Capability.AP));

            Assert.That(mag.IsEmpty(), Is.True,
                "Magazine should be reported as empty");
        }

        [Test]
        public void DecrementOp_RegularTest()
        {
            var mag = new WeaponClip(1,
                new WeaponRound(Caliber.MM_50, Capability.AP));

            mag--;

            Assert.Zero(mag.RoundCount, "Magazine should have no rounds loaded");
        }

        [Test]
        public void DecrementOp_ExceptionTest()
        {
            var mag = new WeaponClip(0,
                new WeaponRound(Caliber.MM_50, Capability.AP));

            Assert.Throws<EmptyMagazineException>(() => mag--);
        }

        [Test]
        public void Refresh_RegularTest()
        {
            var mag = new WeaponClip(5, 0,
                new WeaponRound(Caliber.MM_50, Capability.AP));

            Assert.That(mag.Refresh(3), Is.EqualTo(0),
                "There should be no excess after refreshing under capacity");
            Assert.That(mag.RoundCount, Is.EqualTo(3),
                "Empty magazine should now equal what was put in");
        }

        [Test]
        public void Refresh_ExceptionTest()
        {
            var mag = new WeaponClip(0, 0,
                new WeaponRound(Caliber.MM_50, Capability.AP));

            Assert.That(mag.Refresh(5), Is.EqualTo(5),
                "The capacity is 0, we should get back what we put in");
        }

        [Test]
        public void Refresh_BoundryInTest()
        {
            var mag = new WeaponClip(5, 0,
                new WeaponRound(Caliber.MM_50, Capability.AP));

            Assert.That(mag.Refresh(5), Is.EqualTo(0),
                "We put in exactly what we needed, we shouldn't get anything back");
            Assert.That(mag.RoundCount, Is.EqualTo(mag.Capacity),
                "We should be at capacity based on what we put in");
        }

        [Test]
        public void Refresh_BoundryOutTest()
        {
            var mag = new WeaponClip(6, 5,
                new WeaponRound(Caliber.MM_50, Capability.AP));

            Assert.That(mag.Refresh(5), Is.EqualTo(4),
                "We have an almost full mag, so we should get back one less than what we put in");
            Assert.That(mag.RoundCount, Is.EqualTo(6),
                "The magazine should be at capacity because we had more then needed");
        }
    }
}