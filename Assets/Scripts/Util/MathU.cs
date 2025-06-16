using UnityEngine;

namespace Util
{
    public static class MathU
    {

        /// <summary>
        /// Finds out much of a percentage _quantity2 is in comparison to _quantity2
        /// </summary>
        /// <param name="_percent1">The percentage of _quantity1 (100 means all)</param>
        /// <param name="_quantity1">How many you have a something</param>
        /// <param name="_quantity2">How many you have of something else or the same thing</param>
        /// <returns>The missing percentage of _quantity2 aka _percent2</returns>
        public static float RuleOfThreePercent(float _percent1, float _quantity1, float _quantity2)
        {
            /// Rule of 3 example
            /// 100% (_percent1) - 10 (_quantity1)
            ///    ? (don't know) - 1 (_quantity2)
            /// what is the value of ?
            /// just do the following (1 * 100) / 10  = 10%
            ///
            /// General form
            /// x1 - y1
            /// x2 - y2 
            /// 
            /// x2 = (y2 * x1) / y1
            return _quantity1 * _percent1 / _quantity2;
        }

        public static float RuleOfThreeQuantity(float _percent1, float _quantity1, float _percent2)
        {
            /// Rule of 3 example
            /// 100% (_percent1) - 5 (_quantity1)
            ///  10% (_percent2) - ? (don't know)
            /// what is the value of ?
            /// just do the following (10 * 5) / 100 = 0.5
            ///
            /// General form
            /// x1 - y1
            /// x2 - y2 
            /// 
            /// y2 = (x2 * y1) / x1
            return (_percent2 * _quantity1) / _percent2;

        }


    }

}
