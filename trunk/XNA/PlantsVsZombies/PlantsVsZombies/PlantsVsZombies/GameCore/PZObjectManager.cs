using PlantsVsZombies.GameComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCSEngine.Utils.GameObject.Component;
using Microsoft.Xna.Framework;

namespace PlantsVsZombies.GameCore
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
                _listObject.Remove(id);
        }

        public void RemoveAllObject()
        {
            _listObject.Clear();
        }

        public void SendMessage(IMessage<MessageType> message, GameTime gameTime)
        {
            if (message.DestinationObjectId == 0)
            {
                IDictionary<ulong, ObjectEntity> listCopy = new Dictionary<ulong, ObjectEntity>(_listObject);
                foreach (var item in listCopy)
                {
                    if (message.Handled)
                        break;
                    item.Value.OnMessage(message, gameTime);
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

        public IDictionary<ulong, ObjectEntity> GetCopyObjects()
        {
            return new Dictionary<ulong, ObjectEntity>(this._listObject);
        }
    }
}
