using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PlainCore.Test
{
    public class ArrayBufferListTest
    {
        [Fact]
        public void AddingItems()
        {
            var list = new ArrayBufferList<string>();
            list.Add("test");
            list.Add("test2");
            list.Add("test3");
            list.Add("test4");
            Assert.Equal(4, list.Count);
            list.Add("test5");
            Assert.Equal(5, list.Count);
            list.Clear();
            Assert.Empty(list);
            list.Add("test0");
            list[0] = "test_test";
            Assert.Single(list);
            Assert.Equal("test_test", list[0]);
        }

        [Fact]
        public void Resizing()
        {
            var list = new ArrayBufferList<string>(4);
            Assert.Equal(4, list.Capacity);
            Assert.Equal(4, list.Buffer.Length);

            for (int i = 0; i < 12; i++)
            {
                list.Add($"test{i}");
            }

            Assert.Equal(12, list.Count);
            Assert.True(list.Capacity >= 12);
            Assert.True(list.Buffer.Length >= 12);
            list.Clear();
            Assert.True(list.Capacity >= 12);
        }

        [Fact]
        public void Iterating()
        {
            var testList = new ArrayBufferList<string>();
            var checkList = new List<string>();

            for (int i = 0; i < 13; i++)
            {
                var str = $"test{i}";
                testList.Add(str);
                checkList.Add(str);
            }

            foreach (var s in testList)
            {
                Assert.Contains(s, checkList);
            }

            //Check if elements are identical
            Assert.True(
                testList
                .Zip(checkList, (a, b) => a == b)
                .All(x => x));
        }
    }
}
