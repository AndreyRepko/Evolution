using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Windows.Documents;

namespace Evolution.Game.Model
{
    public class Zavr : IBeing
    {
        private int _sight;
        private Position _position;
        private int _age;
        private int _enerenergy;
        private bool _state;
        private int _maxSpeed;
        private Directions _direction;
        private int _maxAge;

        public Zavr(Position position)
        {
            _position = position;
            _age = 1;
            _state = true;
            _maxAge = 100;
        }

        public Zavr(Position position, int age, bool state, int energy, int maxSpeed, Directions direction) : this(position)
        {
            _age = age;
            _state = state;
            _maxSpeed = maxSpeed;
            _direction = direction;
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
                if (value < 1 || value > 10)
                    throw new InvalidDataException("Sight must be in 1..10 range");
                _sight = value;
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

        public int Energy
        {
            get => _enerenergy;
            set
            {
                if (value < 0 || value > 10000)
                    throw new InvalidDataException("enerenergy must be in 1..1 range");
                _enerenergy = value;
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
            get => _maxSpeed;
            set
            {
                if (value < 1 || value > 5)
                    throw new InvalidDataException("speed must be in 5..5 range");
                _maxSpeed = value;
            }
        }

        public Directions Direction
        {
            get => _direction;
        }

        /// <summary>
        /// Position of the Zavr
        /// </summary>
        public Position Position
        {
            get => _position;
            set
            {
                if (!_position.Equals(value))
                {
                    _position = value;
                    NotifyPropertyChanged(nameof(Position));
                }

            }
        }

        public BeingType Type => BeingType.Zavr;

        public void ChangePosition(int x, int y)
        {
            //ToDo: implement check on speed, if we can move to the new position form the Zavr limitations
            // point of view
            _position.X = x;
            _position.Y = y;
        }

        public static Zavr GetRandomZavr(Position position)
        {
            var zavr = new Zavr(position);
            zavr._sight = RandomNumberGenerator.GetInt32(1, 10);
            zavr._enerenergy = 3000;
            zavr._maxSpeed = RandomNumberGenerator.GetInt32(1, 5);
            return zavr;
        }


        public void NextTurn(bool isNormalTurn, IZavrWorldInteraction world)
        {
            //Okay let's do something :)
            //Here come the algorithm (v 1.0)
            //Check for the food - is no then turn
            //If yes - try to it, if fail
            //go into the direction of the food

            //Stage 1 : Look around
            var items = world.WhatZavrCanSee(Position, _sight, _direction);

            if (items.Any())
            {
                if (world.CanEat(Position))
                {
                    EatFood(world);
                }
                else
                {
                    //Stage 2 : Choose the most valuable tree and go into the direction
                    var item = items.OrderByDescending(x => x.nutrition).First();

                    world.MarkItemAsVictim(item.item, this);

                    var chosenSpeed = RandomNumberGenerator.GetInt32(1, _maxSpeed + 1);

                    MakeMove(world, chosenSpeed, item.where); // это походить
                    //If we jump to the food would be fair to eat it ;)
                    if (world.CanEat(Position))
                    {
                        EatFood(world);
                    }
                }
            }
            else
            {
                var RandomChoice = RandomNumberGenerator.GetInt32(1, 3);
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
                      var chosenSpeed = RandomNumberGenerator.GetInt32(1, _maxSpeed + 1);

                    MakeMove(world, chosenSpeed, (Directions)RandomNumberGenerator.GetInt32(1, 9));
                }
            }

            if (ZavrSurvied())
            {
                Age++;
            }
            else
            {
                KillZavr(world);
            }

            NotifyPropertyChanged();
        }

        private void KillZavr(IZavrWorldInteraction world)
        {
            State = false;
            world.MarkZavrAsDead(Position);
        }

        private bool ZavrSurvied()
        {
            if (_enerenergy == 0)
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

        private void EatFood(IZavrWorldInteraction worldInformation)
        {
            var food = worldInformation.EatVegetable(Position);
            _position.X = food.position.X;
            _position.Y = food.position.Y;
            _enerenergy += food.nutriotion * 10; //ToDo: well, how much should we add here?
        }

        private void MakeMove(IZavrWorldInteraction worldInformation, in int chosenSpeed, Directions itemDirection)
        {
            Position newPosition;
            switch (itemDirection)
            {
                case Directions.Up:
                    newPosition = new Position(Position.X, Position.Y - chosenSpeed);
                    break;
                case Directions.UpRight:
                    newPosition = new Position(Position.X + chosenSpeed, Position.Y - chosenSpeed);
                    break;
                case Directions.Right:
                    newPosition = new Position(Position.X + chosenSpeed, Position.Y);
                    break;
                case Directions.DownRight:
                    newPosition = new Position(Position.X + chosenSpeed, Position.Y + chosenSpeed);
                    break;
                case Directions.Down:
                    newPosition = new Position(Position.X, Position.Y + chosenSpeed);
                    break;
                case Directions.DownLeft:
                    newPosition = new Position(Position.X - chosenSpeed, Position.Y + chosenSpeed);
                    break;
                case Directions.Left:
                    newPosition = new Position(Position.X - chosenSpeed, Position.Y);
                    break;
                case Directions.UpLeft:
                    newPosition = new Position(Position.X - chosenSpeed, Position.Y - chosenSpeed);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(itemDirection), itemDirection, null);
            }

            worldInformation.CorrectPositionToAllowed(ref newPosition);

            Position = newPosition;
            _enerenergy -= CostOfMovement(chosenSpeed);
        }

        private int CostOfMovement(in int chosenSpeed)
        {
            if (chosenSpeed == 1) return 10;
            if (chosenSpeed == 2) return 30;
            if (chosenSpeed == 3) return 60;
            if (chosenSpeed == 4) return 100;
            if (chosenSpeed == 5) return 150;
            throw new ArgumentOutOfRangeException(nameof(chosenSpeed), $"Speed is too big {chosenSpeed}");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
