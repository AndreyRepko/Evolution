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
        private int _generation;
        private SeenItem _focusItem;
        /*
        private int _initialMaxAge = 100;
        private int _initialMaxEnergy = 5000;
        private int _initialStartEnergy = 3000;
        private int _initialExpendEnergyToRotate = 5;
        private int _initialExpendEnergyToReplicate = 2;
        private int _initialEnergyToEat = 5;
        private int _initialPowerDirection = 1;
        private int _initialCostOfMovementTwo = 10;
        private int _initialCostOfMovementOne = 2; */

        public Zavr(Func<Zavr, IZavrWorldInteraction> world, ZavrSetup setup)
        {
            _world = world(this);
            _age = 1;
            _state = true;
            _maxAge = setup.InitialMaxAge;
            _maxEnergy = setup.InitialMaxEnergy;
            _energy = setup.InitialStartEnergy;
            _focusItem = null;

            _myChilds = 0;
        }

        public Zavr(int age, bool state, int energy, int speed, Directions direction, int myChilds, int generation, int sight,
            Func<Zavr, IZavrWorldInteraction> world, ZavrSetup setup) : this(world, setup)
        {
            _age = age;
            _state = state;
            _speed = speed;
            _direction = direction;
            _energy = energy;
            _sight = sight;
            _myChilds = myChilds;
            _generation = generation;
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

        public int Generation
        {
            get => _generation;
            private set
            {
                if (_generation != value)
                {
                    _generation = value;
                    NotifyPropertyChanged(nameof(Generation));
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

        public static Zavr GetRandomZavr(Func<Zavr,IZavrWorldInteraction> world, ZavrSetup setup)
        {
            var zavr = new Zavr(world, setup);
            zavr._sight = RandomNumberGenerator.GetInt32(1, 10);
            zavr._speed = RandomNumberGenerator.GetInt32(1, 5);
            return zavr;
        }


        public void NextTurn(bool isNormalTurn, ZavrSetup setup)
        {
            SeenItems items = GetAroundItems();

            if (items.Any())
            {
                if (_world.CanEat())
                {
                    EatFood(setup);
                }
                else
                {
                    //Stage 2 : Choose the most valuable tree and go into the direction
                    if (_focusItem == null || items.All(item => item.Item != _focusItem.Item))
                    {
                        _focusItem = items.OrderByDescending(x => x.Nutrition).First();
                    }

                    if (_focusItem.Item is IVictim victim)
                        _world.MarkItemAsVictim(victim, this);

                    var chosenSpeed = RandomNumberGenerator.GetInt32(1, _speed + 1);
                    var direction = items.Where(item => item.Item == _focusItem.Item).Single().Where;

                    MakeMove(chosenSpeed, direction, setup); // это походить
                    //If we jump to the food would be fair to eat it ;)
                    if (_world.CanEat())
                    {
                        EatFood(setup);
                    }
                }
            }
            else
            {
                var RandomChoice = RandomNumberGenerator.GetInt32(1, 4);
                if (RandomChoice == 1)
                {
                    var temp = _direction + setup.InitialPowerDirection;
                    if (temp > Directions.UpLeft) //Cruel hack, but should work
                        temp = Directions.Up;
                    _direction = temp;
                    _energy -= setup.InitialExpendEnergyToRotate;
                }
                else
                {
                    var chosenSpeed = RandomNumberGenerator.GetInt32(1, _speed + 1);

                    MakeMove(chosenSpeed, (Directions)RandomNumberGenerator.GetInt32(1, 9), setup);
                }
            }

            if (ZavrSurvied())
            {
                Age++;

                if (Energy >= _maxEnergy)
                {
                    var newSpeed = GetNewSpeed(_speed);
                    var newSight = GetNewSight(_sight);
                    var newGeneration = GetNewGeneration(_generation);
                    _world.SpawnNewZavr(newSpeed, newSight, newGeneration, (Directions)RandomNumberGenerator.GetInt32(1, 9));
                    Energy -= _maxEnergy / setup.InitialExpendEnergyToReplicate;
                    MyChilds++;
                }
            }
            else
            {
                KillZavr();
            }
        }

        private SeenItems GetAroundItems()
        {
            var items = _world.WhatZavrCanSee(_sight, _direction);
            _energy -= CostOfSight(_sight);
            return items;
        }

        private int CostOfSight(int sight)
        {
            var energy = (int)Math.Pow(sight, 2);
            return energy;
        }

        private int GetNewGeneration(int _generation)
        {
            var newGeneration = _generation++;

            newGeneration++;
            return newGeneration;
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
            _world.MarkZavrAsDead();
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

        private void EatFood(ZavrSetup setup)
        {
            var food = _world.EatVegetable();
            _energy += food * setup.InitialEnergyToEat; //ToDo: well, how much should we add here?
        }

        private void MakeMove(in int chosenSpeed, Directions itemDirection, ZavrSetup setup)
        {

            _world.MoveZavr(itemDirection, chosenSpeed);
            _energy -= CostOfMovement(chosenSpeed, setup);
        }

        private int CostOfMovement(in int chosenSpeed, ZavrSetup setup)
        {
            var energy = (int)Math.Pow(chosenSpeed, setup.InitialCostOfMovementOne) * setup.InitialCostOfMovementTwo;
            return energy;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
