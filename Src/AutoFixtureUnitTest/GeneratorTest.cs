﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Xunit;
using Xunit.Extensions;

namespace Ploeh.AutoFixtureUnitTest
{
    public class GeneratorTestOfObject : GeneratorTest<object> { }
    public class GeneratorTestOfString : GeneratorTest<string> { }
    public class GeneratorTestOfInt32 : GeneratorTest<int> { }
    public class GeneratorTestOfGuid : GeneratorTest<Guid> { }
    public class GeneratorTestOfNetPipeStyleUriParser : GeneratorTest<NetPipeStyleUriParser> { }
    public abstract class GeneratorTest<T>
    {
        [Fact]
        public void SutIsEnumerable()
        {
            // Fixture setup
            ISpecimenBuilderComposer composer = new Fixture();
            var sut = new Generator<T>(composer);
            // Exercise system
            // Verify outcome
            Assert.IsAssignableFrom<IEnumerable<T>>(sut);
            // Teardown
        }

        [Theory, ClassData(typeof(CountTestCases))]
        public void StronglyTypedEnumerationYieldsNonDefaultValues(int count)
        {
            // Fixture setup
            var sut = new Generator<T>(new Fixture());
            // Exercise system
            var actual = sut.Take(count);
            // Verify outcome
            Assert.Equal(count, actual.Count(x => !object.Equals(default(T), x)));
            // Teardown
        }

        [Theory, ClassData(typeof(CountTestCases))]
        public void StronglyTypedEnumerationYieldsUniqueValues(int count)
        {
            // Fixture setup
            var sut = new Generator<T>(new Fixture());
            // Exercise system
            var actual = sut.Take(count);
            // Verify outcome
            Assert.Equal(count, actual.Distinct().Count());
            // Teardown
        }

        [Theory, ClassData(typeof(CountTestCases))]
        public void WeaklyTypedEnumerationYieldsNonDefaultValues(int count)
        {
            // Fixture setup
            IEnumerable sut = new Generator<T>(new Fixture());
            // Exercise system
            var actual = sut.OfType<T>().Take(count);
            // Verify outcome
            Assert.Equal(count, actual.Count(x => !object.Equals(default(T), x)));
            // Teardown
        }

        [Theory, ClassData(typeof(CountTestCases))]
        public void WeaklyTypedEnumerationYieldsUniqueValues(int count)
        {
            // Fixture setup
            IEnumerable sut = new Generator<T>(new Fixture());
            // Exercise system
            var actual = sut.OfType<T>().Take(count);
            // Verify outcome
            Assert.Equal(count, actual.Distinct().Count());
            // Teardown
        }
    }

    internal class CountTestCases : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 0 };
            yield return new object[] { 1 };
            yield return new object[] { 2 };
            yield return new object[] { 3 };
            yield return new object[] { 20 };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
