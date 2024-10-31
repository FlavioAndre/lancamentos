using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ControleLancamentos.Domain.Shared
{
    public class ValueObjectTest
    {
        private class TestValueObject : ValueObject
        {
            public int Value { get; }

            public TestValueObject(int value)
            {
                Value = value;
            }

            protected override IEnumerable<object> GetEqualityComponents()
            {
                yield return Value;
            }
        }

        [Fact]
        public void ValueObjects_WithSameValues_AreEqual()
        {
            var vo1 = new TestValueObject(1);
            var vo2 = new TestValueObject(1);

            Assert.Equal(vo1, vo2);
        }

        [Fact]
        public void ValueObjects_WithDifferentValues_AreNotEqual()
        {
            var vo1 = new TestValueObject(1);
            var vo2 = new TestValueObject(2);

            Assert.NotEqual(vo1, vo2);
        }

        [Fact]
        public void GetHashCode_ForEqualValueObjects_IsSame()
        {
            var vo1 = new TestValueObject(1);
            var vo2 = new TestValueObject(1);

            Assert.Equal(vo1.GetHashCode(), vo2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_ForDifferentValueObjects_IsDifferent()
        {
            var vo1 = new TestValueObject(1);
            var vo2 = new TestValueObject(2);

            Assert.NotEqual(vo1.GetHashCode(), vo2.GetHashCode());
        }
    }
}