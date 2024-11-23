using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SerializableHashSet<TIndex, TValue> : HashSet<Tuple<TIndex, TValue>>, ISerializationCallbackReceiver
{

    [SerializeField] private List<TIndex> indexes = new List<TIndex>();
    [SerializeField] private List<TValue> values = new List<TValue>();
    public void OnBeforeSerialize() {
        indexes.Clear();
        values.Clear();
        foreach (Tuple<TIndex, TValue> pair in this) {
            indexes.Add(pair.Item1);
            values.Add(pair.Item2);
        }

    }

    public void OnAfterDeserialize() {
        this.Clear();
        if (indexes.Count != values.Count) {
            Debug.Log("SerializableHashSet: indexes.Count "+indexes.Count+" != values.Count "+values.Count);
        }
        for (int i = 0; i < indexes.Count; i++) {
            this.Add(new Tuple<TIndex, TValue>(indexes[i], values[i]));
        }
    }

}
