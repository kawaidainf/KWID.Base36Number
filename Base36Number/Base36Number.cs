using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace KWID.Common
{
    /// <summary>
    /// 36進数を実現するクラス
    /// </summary>
    public struct Base36Number
    {
        /// <summary>
        /// 36進数で使用されている数値と文字一覧
        /// </summary>
        public static readonly char[] Base36NumberChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        #region public static properties

        /// <summary>
        /// 最小値を取得。
        /// この値を計算に使用することは推奨しません。
        /// </summary>
        public static readonly Base36Number MinValue = new Base36Number(true);

        #endregion

        #region public fields

        /// <summary>
        /// 実値
        /// </summary>
        public long Value
        {
            get => __value;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "36進数は0以下の負の数は使用できません。");
                __value = value;
            }
        }

        private long __value;

        #endregion

        #region getter

        /// <summary>
        /// 10進数値文字列
        /// </summary>
        public string DecimalValue { get => Value.ToString(); }

        /// <summary>
        /// 2進数値文字列
        /// </summary>
        public string BinaryValue { get => Convert.ToString(Value, 2); }

        /// <summary>
        /// 8進数値文字列
        /// </summary>
        public string OctalValue { get => Convert.ToString(Value, 8); }

        /// <summary>
        /// 16進数値文字列
        /// </summary>
        public string HexaDecimalValue { get => Convert.ToString(Value, 16); }

        #endregion

        #region constructor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="val">値を指定</param>
        public Base36Number(long val)
        {
            if (val < 0)
                throw new ArgumentOutOfRangeException(nameof(val), "36進数は負数は使用できません。");

            __value = val;
        }

        /// <summary>
        /// MinValue生成用コンストラクタ
        /// </summary>
        /// <param name="createMinValue"></param>
        private Base36Number(bool createMinValue)
        {
            __value = long.MinValue;
        }

        #endregion

        #region object override method and operator

        /// <summary>
        /// この構造体と指定されたオブジェクトが同一かどうかを検証します。
        /// </summary>
        /// <param name="obj">比較対象オブジェクト</param>
        /// <returns>同一ならTrue</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
                return false;

            var castedObj = (Base36Number)obj;
            return Value == castedObj.Value;
        }

        /// <summary>
        /// Value の値のハッシュコードを返す。
        /// </summary>
        /// <returns>このインスタンスのハッシュ コードである 32 ビット符号付き整数。</returns>
        public override int GetHashCode()
            => Value.GetHashCode();

        public static bool operator ==(Base36Number left, Base36Number right)
            => left.Equals(right);

        public static bool operator !=(Base36Number left, Base36Number right)
            => !left.Equals(right);

        #endregion

        #region override operator

        #region same type

        public static Base36Number operator +(Base36Number left, Base36Number right)
            => new Base36Number(left.Value + right.Value);

        public static Base36Number operator -(Base36Number left, Base36Number right)
            => new Base36Number(left.Value - right.Value);

        public static Base36Number operator *(Base36Number left, Base36Number right)
            => new Base36Number(left.Value * right.Value);

        public static Base36Number operator /(Base36Number left, Base36Number right)
            => new Base36Number(left.Value / right.Value);

        public static Base36Number operator %(Base36Number left, Base36Number right)
            => new Base36Number(left.Value % right.Value);

        #endregion

        #region long type

        public static Base36Number operator +(Base36Number left, long right)
            => new Base36Number(left.Value + right);

        public static Base36Number operator +(long left, Base36Number right)
            => new Base36Number(left + right.Value);

        public static Base36Number operator -(Base36Number left, long right)
            => new Base36Number(left.Value - right);

        public static Base36Number operator -(long left, Base36Number right)
            => new Base36Number(left - right.Value);

        public static Base36Number operator *(Base36Number left, long right)
            => new Base36Number(left.Value * right);

        public static Base36Number operator *(long left, Base36Number right)
            => new Base36Number(left * right.Value);

        public static Base36Number operator /(Base36Number left, long right)
            => new Base36Number(left.Value / right);

        public static Base36Number operator /(long left, Base36Number right)
            => new Base36Number(left / right.Value);

        public static Base36Number operator %(Base36Number left, long right)
            => new Base36Number(left.Value % right);

        public static Base36Number operator %(long left, Base36Number right)
            => new Base36Number(left % right.Value);

        #endregion

        #endregion

        #region public method

        /// <summary>
        /// 加算します。
        /// </summary>
        /// <param name="val">加算したい値</param>
        /// <returns>加算された値</returns>
        public Base36Number AddValue(long val)
        {
            Value += val;
            return this;
        }

        /// <summary>
        /// 36進数の文字列に変換します。
        /// </summary>
        /// <returns>変換後の36進数文字列</returns>
        public override string ToString()
        {
            // if Value is between 0 and 35, return it.
            if (Value < Base36NumberChars.Length)
                return Base36NumberChars[Value].ToString();

            long val = Value;
            var sb = new StringBuilder();

            // save remainder.
            do
            {
                int mod = Convert.ToInt32(val % Base36NumberChars.Length);

                // convert remainder to base36 and save it.
                sb.Insert(0, Base36NumberChars[mod]);

                val /= Base36NumberChars.Length;
            } while (val >= Base36NumberChars.Length);

            // save last value
            sb.Insert(0, Base36NumberChars[(int)val]);

            return sb.ToString();
        }

        /// <summary>
        /// 文字列に変換します。
        /// </summary>
        /// <param name="format">指定した進数に変換する。2, 8, 10, 16, 36 のみ指定可能</param>
        /// <returns>指定した進数変換を行った結果文字列</returns>
        public string ToString(int format)
        {
            switch(format)
            {
                case 2: return BinaryValue;
                case 8: return OctalValue;
                case 10: return Value.ToString();
                case 16: return HexaDecimalValue;
                case 36: return ToString();
                default: throw new ArgumentOutOfRangeException(nameof(format), "指定できるフォーマットは 2, 8, 10, 16, 36 のみです。");
            }
        }

        #endregion

        #region static method

        /// <summary>
        /// 文字列を36進数に変換します。
        /// </summary>
        /// <param name="base36Value">変換する36進数文字列</param>
        /// <returns>変換後の36進数オブジェクト</returns>
        public static Base36Number Parse(string base36Value)
        {
            if (base36Value == null || string.IsNullOrEmpty(base36Value.Trim()))
                throw new ArgumentNullException(nameof(base36Value));

            if (!IsBase36Number(base36Value))
                throw new ArgumentException("Base36Number に変換できない文字が含まれています。", nameof(base36Value));

            string val = base36Value.ToUpper();

            // base36 array to number array
            var numberArray = val.Select(c => Array.IndexOf(Base36NumberChars, c)).ToArray();

            // calc number to base10
            long sum = 0;
            int multiplier = 0;
            for (int i = numberArray.Length - 1; i >= 0; i--)
            {
                int num = numberArray[i];

                // calc [number * 36 ^ (digit - 1)] and calc sum
                sum += (long)(num * Math.Pow(Base36NumberChars.Length, multiplier));

                // countup multiplier
                multiplier++;
            }

            return new Base36Number(sum);
        }

        /// <summary>
        /// 文字列が36進数オブジェクトに変換できるかどうかを検証します。
        /// </summary>
        /// <param name="base36Value">変換する36進数文字列</param>
        /// <param name="result">変換後の36進数オブジェクト。失敗した場合はMinValueが返却される。</param>
        /// <returns>変換できればTrue</returns>
        public static bool TryParse(string base36Value, out Base36Number result)
        {
            try
            {
                if (!IsBase36Number(base36Value))
                {
                    result = MinValue;
                    return false;
                }

                result = Parse(base36Value);
                return true;
            }
            catch
            {
                result = MinValue;
                return false;
            }
        }

        /// <summary>
        /// 文字列が36進数で使用される文字のみで構成されているかどうかを検証します。
        /// </summary>
        /// <param name="base36Value">検査対象文字列</param>
        /// <returns>36進数で使用される文字のみで構成されていればTrue</returns>
        private static bool IsBase36Number(string base36Value)
            => base36Value != null && !string.IsNullOrEmpty(base36Value.Trim()) &&
               Regex.IsMatch(base36Value, @"^[A-Z0-9]+$", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        #endregion
    }
}
