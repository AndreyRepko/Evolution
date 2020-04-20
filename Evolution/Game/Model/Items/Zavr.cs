using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Evolution.Game.Model.Positions;

namespace Evolution.Game.Model.Items
{
    public class Zavr : IBeing
    {
        private int _sight;
        private readonly IZavrWorldInteraction _world;
        private int _age;
        private int _energy;
        private bool _state;
        private int _speed;
        private Directions _direction;
        private int _maxAge;
        private int _maxEnergy;
        private int _myChilds;

        public Zavr(IZavrWorldInteraction world)
        {
            _world = world;
            _age = 1;
            _state = true;
            _maxAge = 100;
            _maxEnergy = 5000;
            _energy = 3000;
            _myChilds = 0;
        }

        public Zavr(int age, bool state, int energy, int speed, Directions direction, int myChilds, int sight, 
            IZavrWorldInteraction world) : this(world)
        {
            _age = age;
            _state = state;
            _speed = speed;
            _direction = direction;
            _energy = energy;
            _sight = sight;
            _myChilds = myChilds;
        }

        /// <summary>
        /// Gets or sets the range of sight.
        /// It defines the range object can see at most.
        /// Must be in 1..10 range.
        /// </summary>
        public int Sight
        {
            get => _sight;
            set
            {
                if (_sight != value)
                {
                    _sight = value;
                    NotifyPropertyChanged(nameof(Sight));
                }
            }
        }

        public int Age
        {
            get => _age;
            private set
            {
                if (_age != value)
                {
                    _age = value;
                    NotifyPropertyChanged(nameof(Age));
                }
            }
        }

        public int MyChilds
        {
            get => _myChilds;
            private set
            {
                if (_myChilds != value)
                {
                    _myChilds = value;
                    NotifyPropertyChanged(nameof(MyChilds));
                }
            }
        }

        public int Energy
        {
            get => _energy;
            set
            {
                if (value < 0 || value > 100000)
                    throw new InvalidDataException("enerenergy must be in 1..1 range");
                _energy = value;
            }
        }

        public bool State
        {
            get => _state;
            set
            {
                if (value != _state)
                {
                    _state = value;
                    NotifyPropertyChanged(nameof(State));
                }
            }
        }
        
        public int Speed
        {
            get => _speed;
            set
            {
                if (_speed != value)
                {
                    _speed = value;
                    NotifyPropertyChanged(nameof(Speed));
                }
            }
        }
        
        public Directions Direction
        {
            get => _direction;
        }

        public Position WeakPosition
        {
            get
            {
                return _world.GetPosition(this);
            }
        }

        public BeingType Type => BeingType.Zavr;

        public static Zavr GetRandomZavr(IZavrWorldInteraction world)
        {
            var zavr = new Zavr(world);
            zavr._sight = RandomNumberGenerator.GetInt32(1, 10);
            zavr._speed = RandomNumberGenerator.GetInt32(1, 5);
            return zavr;
        }


        public void NextTurn(bool isNormalTurn)
        {
            //Okay let's do something :)
            //Here come the algorithm (v 1.0)
            //Check for the food - is no then turn
            //If yes - try to it, if fail
            //go into the direction of the food

            //Stage 1 : Look around
            var items = _world.WhatZavrCanSee(this, _sight, _direction);

            if (items.Any())
            {
                if (_world.CanEat(this))
                {
                    EatFood();
                }
                else
                {
                    //Stage 2 : Choose the most valuable tree and go into the direction
                    var item = items.OrderByDescending(x => x.nutrition).First();

                    _world.MarkItemAsVictim(item.item, this);

                    var chosenSpeed = RandomNumberGenerator.GetInt32(1, _speed + 1);

                    MakeMove(chosenSpeed, item.where); // это походить
                    //If we jump to the food would be fair to eat it ;)
                    if (_world.CanEat(this))
                    {
                        EatFood();
                    }
                }
            }
            else
            {
                var RandomChoice = RandomNumberGenerator.GetInt32(1, 4);
                if (RandomChoice == 1)
                {
                    var temp = _direction + 1;
                    if (temp > Directions.UpLeft) //Cruel hack, but should work
                        temp = Directions.Up;
                    _direction = temp;
                    Energy -= 5;
                }
                else
                {
                    var chosenSpeed = RandomNumberGenerator.GetInt32(1, _speed + 1);

                    MakeMove(chosenSpeed, (Directions)RandomNumberGenerator.GetInt32(1, 9));
                }
            }

            if (ZavrSurvied())
            {
                Age++;

                if (Energy >= _maxEnergy)
                {
                    var newSpeed = GetNewSpeed(_speed);
                    var newSight = GetNewSight(_sight);
                    _world.SpawnNewZavr(this, newSpeed, newSight, (Directions)RandomNumberGenerator.GetInt32(1, 9));
                    Energy -= _maxEnergy / 2;
                    MyChilds++;
                }
            }
            else
            {
                KillZavr();
            }
        }

        private int GetNewSight(int sight)
        {
            var newSight = sight + RandomTransformer.GetChange();
            if (newSight < 1)
            {
                newSight = 1;
            }
            return newSight;
        }

        private int GetNewSpeed(int speed)
        {
            var newSpeed = speed + RandomTransformer.GetChange();
            if (newSpeed < 1)
            {
                newSpeed = 1;
            }
            return newSpeed;
        }

        private void KillZavr()
        {
            State = false;
            _world.MarkZavrAsDead(this);
        }

        private bool ZavrSurvied()
        {
            if (_energy <= 0)
            {
                return false;
            }
            else if (_age >= _maxAge)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void EatFood()
        {
            var food = _world.EatVegetable(this);
            _energy += food * 5; //ToDo: well, how much should we add here?
        }

        private void MakeMove(in int chosenSpeed, Directions itemDirection)
        {

            _world.MoveZavr(this, itemDirection, chosenSpeed);
            _energy -= CostOfMovement(chosenSpeed);
        }

        private int CostOfMovement(in int chosenSpeed)
        {
            var energy = (int)Math.Pow(chosenSpeed, 2) * 10;
            return energy;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
