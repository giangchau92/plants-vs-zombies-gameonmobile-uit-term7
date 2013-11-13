using PlantVsZombie.GameComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSCEngine.Utils.GameObject.Component;
using Microsoft.Xna.Framework;

namespace PlantVsZombie.GameCore
{
    public class PZObjectManager
    {
        private static PZObjectManager _instance = null;
        public static PZObjectManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PZObjectManager();
                return _instance;
            }
        }

        private ulong _nextObjectId = 1;
        private Dictionary<ulong, ObjectEntity> _listObject = new Dictionary<ulong, ObjectEntity>();

        private PZObjectManager()
        {
        }

        public ObjectEntity AddObject(ObjectEntity obj)
        {
            obj.ObjectId = _nextObjectId++;
            _listObject.Add(obj.ObjectId, obj);
            return obj;
        }

        public void RemoveObject(ulong id)
        {
            if (_listObject.ContainsKey(id))
                _listObject[id].Remove = true;
        }

        public void SendMessage(IMessage<MessageType> message, GameTime gameTime)
        {
            if (message.DestinationObjectId == 0)
            {
                List<ObjectEntity> backList = new List<ObjectEntity>();
                foreach (var item in _listObject)
                {
                    item.Value.OnMessage(message, gameTime);
                    backList.Add(item.Value);
                }

                // Delete die object
                while (backList.Count != 0)
                {
                    if (backList[0].Remove)
                        _listObject.Remove(backList[0].ObjectId);

                    backList.RemoveAt(0);
                }
            }
            else
            {
                if (_listObject.ContainsKey(message.DestinationObjectId))
                    _listObject[message.DestinationObjectId].OnMessage(message, gameTime);
            }
        }

        public IDictionary<ulong, ObjectEntity> GetObjects()
        {
            return this._listObject;
        }
    }
}
