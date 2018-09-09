using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BIStudio.Framework
{
    /// <summary>
    /// 可设置Key和Value的键值对
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [Serializable, DataContract]
    public class EditableKeyValuePair<TKey, TValue>
    {
        public EditableKeyValuePair()
        {
        }

        public EditableKeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public EditableKeyValuePair(KeyValuePair<TKey, TValue> pair)
        {
            Key = pair.Key;
            Value = pair.Value;
        }

        public KeyValuePair<TKey, TValue> ToKeyValuePair()
        {
            return new KeyValuePair<TKey, TValue>(Key, Value);
        }

        [DataMember(Name = "Key")]
        public TKey Key { get; set; }

        [DataMember(Name = "Value")]
        public TValue Value { get; set; }
    }
}
