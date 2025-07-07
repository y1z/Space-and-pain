using System;
using UnityEngine;

namespace Util
{
    /// <summary>
    /// This only exist to create meta data about objects that are converted to json
    /// </summary>
    [Serializable]
    public sealed class MetaData
    {
        public MetaData(string _meta)
        {
            name = _meta;
        }

        public string toJson()
        {
            return JsonUtility.ToJson(this);
        }

        public string name;
    }
}
