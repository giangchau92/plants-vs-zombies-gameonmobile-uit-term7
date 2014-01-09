using Microsoft.Xna.Framework;
using PlantsVsZombies.GameComponents;
using PlantsVsZombies.GameComponents.Components;
using PlantsVsZombies.GameCore;
using PlantsVsZombies.GameObjects;
using SCSEngine.Audio;
using SCSEngine.Mathematics;
using SCSEngine.Serialization;
using SCSEngine.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PlantsVsZombies.GameCore.Level
{
    public enum LevelState
    {
        BEGIN, WAVING, ENDWAVE, END
    }
    public class Level : ISerializable
    {
        public string Name { get; set; }
        public List<Wave> Waves { get; set; }
        public bool IsFirstZombie { get; set; }
        public string Background { get; set; }

        private LevelState _currentState;
        private TimeSpan _currentTime;
        private int _currentWave;

        public LevelState LevelState
        {
            get { return _currentState; }
        }

        public Level()
        {
            Waves = new List<Wave>();
            _currentState = LevelState.BEGIN;
            _currentTime = TimeSpan.Zero;
            _currentWave = 0;
            IsFirstZombie = true;
            ////Debug.WriteLine("LEVEL: Begin Wave");
        }


        public void Update(PZBoard board, GameTime gameTime)
        {
            if (_currentState == LevelState.BEGIN)
            {
                if (_currentTime >= Waves[_currentWave].TimeBeginWave)
                {
                    ////Debug.WriteLine("LEVEL: Start Wave " + _currentWave);
                    _currentState = LevelState.WAVING;
                    _currentTime = TimeSpan.Zero;
                }
                else
                    _currentTime += gameTime.ElapsedGameTime;
            }
            else if (_currentState == LevelState.WAVING)
            {
                Wave wave = Waves[_currentWave];
                if (wave.CurrentState == WaveState.WAVING)
                {
                    wave.Update(board, gameTime);
                }
                else if (wave.CurrentState == WaveState.END)
                {
                    _currentState = LevelState.ENDWAVE;
                }
            }
            else if (_currentState == LevelState.ENDWAVE) // Delay giua cac Wave
            {
                    if (_currentTime >= Waves[_currentWave].TimeNextWave)
                    {
                        _currentWave++; // Tang wave
                        if (_currentWave >= Waves.Count) // Het wave
                            _currentState = LevelState.END;
                        else
                        {
                            _currentState = LevelState.WAVING;
                            _currentTime = TimeSpan.Zero;
                        }
                    }
                    else
                    {
                        _currentTime += gameTime.ElapsedGameTime;
                        //Debug.WriteLine("LEVEL: Delay Wave ");
                    }
            }
            else if (_currentState == LevelState.END)
            {
                //Debug.WriteLine("LEVEL: End LEvel");
            }
        }

        public void Serialize(ISerializer serializer)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(IDeserializer deserializer)
        {
            Name = deserializer.DeserializeString("Name");
            Background = deserializer.DeserializeString("Background");
            var waveDers = deserializer.DeserializeAll("Wave");
            foreach (var item in waveDers)
            {
                Wave wave = new Wave();
                wave.Deserialize(item);
                wave.Owner = this;
                Waves.Add(wave);
            }
        }

        public Level Clone()
        {
            Level clone = new Level();
            clone.Name = Name;
            foreach (var item in Waves)
            {
                Wave wave = item.Clone();
                wave.Owner = clone;
                clone.Waves.Add(wave);
            }

            return clone;
        }
    }

    public enum WaveState
    {
        WAVING, END
    }
    public class Wave : ISerializable
    {
        private Sound _soundFirst;
        public string Name { get; set; }
        public List<string> Zombies { get; set; }
        public int NumberFrom { get; set; }
        public int NumberTo { get; set; }
        public Level Owner { get; set; }
        public bool IsFirstZombie { get; set; }
        private double _timeWave;
        public TimeSpan TimeWave
        {
            get
            {
                return TimeSpan.FromSeconds(_timeWave);
            }
        }
        private double _timeNextWave;
        public TimeSpan TimeNextWave
        {
            get
            {
                return TimeSpan.FromSeconds(_timeNextWave);
            }
        }
        private double _timeBeginWave;
        public TimeSpan TimeBeginWave
        {
            get
            {
                return TimeSpan.FromSeconds(_timeBeginWave);
            }
        }
        private double _timeNextZombieFrom;
        public double TimeNextZombieFrom
        {
            get
            {
                return _timeNextZombieFrom;
            }
        }
        private double _timeNextZombieTo;
        public double TimeNextZombieTo
        {
            get
            {
                return _timeNextZombieTo;
            }
        }


        private int _numberZombie = 0;
        private int _currentZombie = 0;
        private TimeSpan _currentTime;
        private TimeSpan _nextZombieTime;

        private WaveState _currentState;
        public WaveState CurrentState
        {
            get { return _currentState; }
        }

        public Wave()
        {
            Zombies = new List<string>();
            _currentState = WaveState.WAVING;
            _currentTime = TimeSpan.Zero;
            _nextZombieTime = TimeSpan.Zero;
            IsFirstZombie = true;
        }

        public void Update(PZBoard board, GameTime gameTime)
        {
            if (_numberZombie == 0)
            {
                _numberZombie = GRandom.RandomInt(NumberFrom, NumberTo);//rand.Next(NumberFrom, NumberTo);
            }

            if (_currentState == WaveState.WAVING)
            {
                if (_nextZombieTime == TimeSpan.Zero)
                {
                    _nextZombieTime = TimeSpan.FromSeconds(GRandom.RandomDouble(TimeNextZombieFrom, TimeNextZombieTo)); // Calc time to drop zombie
                    //Debug.WriteLine("WAVE: Tha ZOMBIE trong {0} giay", _nextZombieTime);
                }

                if (_currentTime >= _nextZombieTime) // Tha zombie
                {

                    string zombieString = Zombies[GRandom.RandomInt(Zombies.Count)];//rand.Next(0, Zombies.Count)];
                    ObjectEntity obj = GameObjectCenter.Instance.CreateObject(zombieString);
                    int row = GRandom.RandomInt(4);
                    //Debug.WriteLine("LEVEL: Tha ZOMBIE at row {0}", row);
                    board.AddObjectAt(obj, row, 10);
                    _currentTime = TimeSpan.Zero;
                    _nextZombieTime = TimeSpan.Zero;
                    if (IsFirstZombie && Owner.IsFirstZombie)
                    {
                        SCSServices.Instance.AudioManager.PlaySound(_soundFirst, false, true);
                        Owner.IsFirstZombie = false;
                        IsFirstZombie = false;
                    }
                    _currentZombie++;
                }
                else
                    _currentTime += gameTime.ElapsedGameTime;

                // Kiem tra da tha het zombie chua
                if (_currentZombie >= _numberZombie)
                    _currentState = WaveState.END;
            }
        }

        public void Serialize(ISerializer serializer)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(IDeserializer deserializer)
        {
            Name = deserializer.DeserializeString("Name");
            // Zombie
            var zombieDesers = deserializer.DeserializeAll("Zombie");
            foreach (var zombieDer in zombieDesers)
            {
                Zombies.Add(zombieDer.DeserializeString("Value"));
            }
            // Number
            var numberDer = deserializer.SubDeserializer("Number");
            NumberFrom = numberDer.DeserializeInteger("From");
            NumberTo = numberDer.DeserializeInteger("To");
            
            _timeNextWave = deserializer.DeserializeDouble("TimeNextWave");
            _timeBeginWave = deserializer.DeserializeDouble("TimeBeginWave");
            _timeNextZombieFrom = deserializer.DeserializeDouble("TimeNextZombieFrom");
            _timeNextZombieTo = deserializer.DeserializeDouble("TimeNextZombieTo");
        }

        public Wave Clone()
        {
            Wave clone = new Wave();
            clone.Name = Name;
            foreach (var item in Zombies)
            {
                clone.Zombies.Add(item);
            }
            clone.NumberFrom = NumberFrom;
            clone.NumberTo = NumberTo;
            clone._timeNextWave = _timeNextWave;
            clone._timeBeginWave = _timeBeginWave;
            clone._timeNextZombieFrom = _timeNextZombieFrom;
            clone._timeNextZombieTo = _timeNextZombieTo;
            clone._soundFirst = SCSServices.Instance.ResourceManager.GetResource<Sound>("Sounds/Evillaugh");
            return clone;
        }
    }
}
