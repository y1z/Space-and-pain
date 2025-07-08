using System;
using UnityEngine;

namespace interfaces
{
    public interface ILoadGameData
    {
        public void loadData(string data);
        public void loadData(StandardEntitySaveData data);
    }

    [Serializable]
    public struct StandardEntitySaveData
    {
        public StandardEntitySaveData(Vector2 _position, Vector2 _speed, Vector2 _direction, string _prefabName)
        {
            position = _position;
            speed = _speed;
            prefabName = _prefabName;
            direction = _direction;
        }

        public static StandardEntitySaveData create(Vector2 _position, Vector2 _speed, Vector2 _direction, string _prefabName)
        {
            StandardEntitySaveData result;
            result.position = _position;
            result.speed = _speed;
            result.prefabName = _prefabName;
            result.direction = _direction;
            return result;
        }

        public Vector2 position;
        public Vector2 speed;
        public Vector2 direction;
        public string prefabName;
    }
}
