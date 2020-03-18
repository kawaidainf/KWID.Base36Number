using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KWID.Common.Test
{
    [TestClass]
    public class Base36NumberTest
    {
        [TestMethod]
        [Description("数値から36進数へ変換するテスト")]
        public void ToStringTest()
        {
            Base36Number val;

            val = new Base36Number(0);
            Assert.AreEqual("0", val.ToString());

            val = new Base36Number(99);
            Assert.AreEqual("2R", val.ToString());

            val = new Base36Number(999);
            Assert.AreEqual("RR", val.ToString());

            val = new Base36Number(9999);
            Assert.AreEqual("7PR", val.ToString());

            val = new Base36Number(99999);
            Assert.AreEqual("255R", val.ToString());

            val = new Base36Number(999999);
            Assert.AreEqual("LFLR", val.ToString());

            val = new Base36Number(9999999);
            Assert.AreEqual("5YC1R", val.ToString());

            val = new Base36Number(99999999);
            Assert.AreEqual("1NJCHR", val.ToString());

            val = new Base36Number(999999999);
            Assert.AreEqual("GJDGXR", val.ToString());

            val = new Base36Number(9999999999);
            Assert.AreEqual("4LDQPDR", val.ToString());

            val = new Base36Number(int.MaxValue);
            Assert.AreEqual("ZIK0ZJ", val.ToString());

            val = new Base36Number(long.MaxValue);
            Assert.AreEqual("1Y2P0IJ32E8E7", val.ToString());
        }

        [TestMethod]
        [Description("36進数から数値へ変換するテスト")]
        public void ParseTest()
        {
            Base36Number val;

            val = Base36Number.Parse("0");
            Assert.AreEqual(0, val.Value);

            val = Base36Number.Parse("2R");
            Assert.AreEqual(99, val.Value);

            val = Base36Number.Parse("RR");
            Assert.AreEqual(999, val.Value);

            val = Base36Number.Parse("7PR");
            Assert.AreEqual(9999, val.Value);

            val = Base36Number.Parse("255R");
            Assert.AreEqual(99999, val.Value);

            val = Base36Number.Parse("LFLR");
            Assert.AreEqual(999999, val.Value);

            val = Base36Number.Parse("5YC1R");
            Assert.AreEqual(9999999, val.Value);

            val = Base36Number.Parse("1NJCHR");
            Assert.AreEqual(99999999, val.Value);

            val = Base36Number.Parse("GJDGXR");
            Assert.AreEqual(999999999, val.Value);

            val = Base36Number.Parse("4LDQPDR");
            Assert.AreEqual(9999999999, val.Value);

            val = Base36Number.Parse("ZIK0ZJ");
            Assert.AreEqual(int.MaxValue, val.Value);

            val = Base36Number.Parse("1Y2P0IJ32E8E7");
            Assert.AreEqual(long.MaxValue, val.Value);
        }

        [TestMethod]
        [Description("36進数から数値へ変換できるかを判定するメソッドのテスト")]
        public void TryParseTest()
        {
            bool val;
            Base36Number result;

            val = Base36Number.TryParse("0", out result);
            Assert.AreEqual(true, val);
            Assert.AreEqual(new Base36Number(0), result);

            val = Base36Number.TryParse("1Y2P0IJ32E8E7", out result);
            Assert.AreEqual(true, val);
            Assert.AreEqual(new Base36Number(long.MaxValue), result);

            val = Base36Number.TryParse("001JK", out result);
            Assert.AreEqual(true, val);
            Assert.AreEqual(new Base36Number(2000), result);


            val = Base36Number.TryParse("frhua!##", out result);
            Assert.AreEqual(false, val);
            Assert.AreEqual(Base36Number.MinValue, result);

            val = Base36Number.TryParse("609DOLZQ7PSG9W15", out result);
            Assert.AreEqual(false, val);
        }
    }
}
