using System;
using System.Collections.Generic;
using System.Threading;

namespace OOP10
{
    //    Есть аквариум, в котором плавают рыбы.
    //    В этом аквариуме может быть максимум определенное кол-во рыб.
    //    Рыб можно добавить в аквариум или рыб можно достать из аквариума.
    //    (программу делать в цикле для того, чтобы рыбы могли “жить”)
    //Все рыбы отображаются списком, у рыб также есть возраст.
    //За 1 итерацию рыбы стареют на определенное кол-во жизней и могут умереть.
    //Рыб также вывести в консоль, чтобы можно было мониторить показатели.

    class Program
    {
        static void Main(string[] args)
        {
            ProgramCore programCore = new ProgramCore();

            programCore.Game();

            Console.ReadLine();
        }
    }



    class ProgramCore
    {
        private Message _message = new();
        private Aquarium _aquarium = new();
        private Config _config = new();
        private Random _random = new();
        private Renderer _renderer = new();
        private Statistic _statistic = new();


        internal void Game()
        {
            _aquarium.AddFish(_random.Next(_config.MinimumFish, _config.MaximumFish));
            _message.ShowQuantityFish(_statistic.AllQuantityFish(_aquarium));
            SetanchorPointAquarium();
            _renderer.CreateAquarium();
            _statistic.DisplayFishStats(_aquarium.GetListFishs());

            while (true)
            {

                _aquarium.lifeCycleFish();
                _statistic.DisplayFishStats(_aquarium.GetListFishs());


                Thread.Sleep(100);
            }

        }

        private void SetanchorPointAquarium()
        {
            int quantityFish = _aquarium.GetQuqntityFishOfAquarium();
            int additionalLines = 2;

            _config.SetanchorPointAquarium(quantityFish + additionalLines);
        }

    }

    class Renderer
    {
        private Config _config = new();


        internal void ShowFish(Aquarium aquarium)
        {
            Random random = new Random();

            //foreach (var item in aquarium)
            //{

            //}

        }

        internal void CreateAquarium()
        {
            char wall = '|';
            char bottom = '_';
            int correction = 1;

            for (int i = _config.AnchorPointAquarium[1]; i < _config.AnchorPointAquarium[1] + _config.AquariumSizeLeftTop[1]; i++)
            {
                Console.SetCursorPosition(_config.AnchorPointAquarium[0], i);
                Console.Write(wall);
                Console.SetCursorPosition(_config.AnchorPointAquarium[0] + _config.AquariumSizeLeftTop[0], i);
                Console.Write(wall);
            }

            for (int i = _config.AnchorPointAquarium[0] + 1; i < _config.AnchorPointAquarium[0] + _config.AquariumSizeLeftTop[0]; i++)
            {
                Console.SetCursorPosition(i, _config.AnchorPointAquarium[1] + _config.AquariumSizeLeftTop[1] - correction);
                Console.Write(bottom);
            }
        }

    }

    class Statistic
    {
        // private ProgramCore _programCore = new();// рекурсия
        // private Aquarium _aquarium = new();//рекурсия
        private Message _message = new();

        // private int _allQuantityFish = 0;



        internal void DisplayFishStats(List<Fish> fishs)
        {
            for (int i = 0; i < fishs.Count; i++)
            {
                _message.DisplayFishStats(fishs[i], i + 1);
            }

        }

        internal int AllQuantityFish(Aquarium aquarium)
        {
            return aquarium.GetQuantityFishAll();
        }


    }

    class Config
    {
        private ConsoleColor[] _colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));
        private char[] _skin = { '#', '@', '%', '$', '&', '№' };


        private int _maximumFish = 10;
        private int _minimumFish = 2;
        private int _maximumLifeTime = 100;
        private int _minimumLifeTime = 50;
        private int[] _messageCoordinateLeftTop = { 1, 1 };
        private int[] _aquariumSizeLeftTop = { 60, 20 };
        private int[] _anchorPointAquarium = { 2, 5 };

        internal int[] AquariumSizeLeftTop { get => _aquariumSizeLeftTop; }
        internal int[] AnchorPointAquarium { get => _anchorPointAquarium; }
        internal int[] MessageCoordinateLeftTop { get => _messageCoordinateLeftTop; }
        internal ConsoleColor[] Colors { get => _colors; }
        internal char[] Skin { get => _skin; }
        internal int MaximumFish { get => _maximumFish; }
        internal int MinimumFish { get => _minimumFish; }
        internal int MaximumLifeTime { get => _maximumLifeTime; }
        internal int MinimumLifeTime { get => _minimumLifeTime; }


        internal void SetanchorPointAquarium(int left)
        {
            _anchorPointAquarium[0] = left;
        }

        internal int GetQuantityColorFish()
        {
            return Colors.Length;
        }


    }

    class Message
    {
        private Config _config = new();
        // private Statistic _statistic = new(); рекурсия

        internal void DisplayFishStats(Fish fish, int numberLine)
        {
            string died;

            if (fish.Died == true)
                died = "Мертвая";
            else
                died = "Живая";

            Console.SetCursorPosition(_config.MessageCoordinateLeftTop[0], _config.MessageCoordinateLeftTop[1] + numberLine);

            Console.Write($"Id - {fish.Id} Жизнь - {fish.Healt}");
            Console.Write(" Скин - ");
            Console.ForegroundColor = fish.colorFish;
            Console.Write(fish.Skin);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($" Статус - {died}");
        }

        internal void ShowErrorMaximumFish()
        {
            SetCursorPositionMessag();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Вы достигли максимума рыб в аквариуме.");
            Console.ForegroundColor = ConsoleColor.White;
        }

        internal void ShowQuantityFish(int allFish)
        {
            SetCursorPositionMessag();
            Console.Write($"Всего рыб рожденно - {allFish}");
        }

        private void SetCursorPositionMessag()
        {
            Console.SetCursorPosition(_config.MessageCoordinateLeftTop[0], _config.MessageCoordinateLeftTop[1]);
        }
    }

    class Aquarium
    {
        private Config _config = new();
        private Message _message = new();
        private List<Fish> _aquarium = new List<Fish>();
        private List<Fish> _allFish = new List<Fish>();
        private int _lastId = 0;

        internal int NumberOfFish { get; private set; }

        internal void lifeCycleFish()
        {
            int[] idDead = new int[0];

            for (int i = 0; i < _aquarium.Count; i++)
            {
                _aquarium[i].RemoveOneLife();

                //if (_aquarium[i].Died == true)
                //{
                //    int[] tempId = new int[idDead.Length + 1];
                //    idDead = tempId;
                //    idDead[idDead.Length-1] = i;
                //   // break;
                //}
            }

            bool tt = true;

            while (tt)
            {
                foreach (Fish fish in _aquarium)
                {
                    if (fish.Died == true)
                    {
                       // tt = true;
                      //  _aquarium.Remove(fish);
                        break;
                    }
                       
                }
                foreach (Fish fish in _aquarium)
                {
                    if (fish.Died != true)
                    {
                        tt = false;
                    }

                }


            }

        }

        internal void SetNumberOfFish(int numberFish)
        {
            NumberOfFish = numberFish;
        }
        internal void AddFish(int quantityFish)
        {
            if (quantityFish < _config.MaximumFish)
            {
                for (int i = 0; i < quantityFish; i++)
                {
                    Fish fish = new Fish(i);
                    _aquarium.Add(fish);
                    _allFish.Add(fish);
                    _lastId++;
                }
            }
            else
            {
                _message.ShowErrorMaximumFish();
            }
        }

        internal int GetQuqntityFishOfAquarium()
        {
            return _aquarium.Count;
        }
        internal int GetQuantityFishAll()
        {
            return _allFish.Count;
        }

        internal List<Fish> GetListFishs()
        {
            List<Fish> tempFishs = new();

            foreach (Fish fish in _aquarium)
            {
                tempFishs.Add(fish);
            }
            return tempFishs;
        }
    }

    class Fish
    {
        private Config _config = new();
        Random _random = new();

        internal int Id { get; private set; }
        internal int Healt { get; private set; }

        internal int PositionLeft { get; private set; }

        internal int PositionTop { get; private set; }

        internal char Skin { get; private set; }

        internal bool Died { get; private set; }

        internal ConsoleColor colorFish { get; private set; }

        public Fish(int id)
        {
            Id = id;
            Healt = GenerateHealt();
            Skin = GenerateSkin();
            colorFish = GenerateColor();
            Died = false;
            GeneratePointSpavn();
        }

        internal void RemoveOneLife()
        {
            Healt--;

            if (Healt <= 0)
            {
                Healt = 0;
                Dead();
            }
        }

        private void Dead()
        {
            Died = true;
        }

        private int GenerateHealt()
        {
            int healt = _random.Next(_config.MinimumLifeTime, _config.MaximumLifeTime);
            return healt;
        }

        private ConsoleColor GenerateColor()
        {
            ConsoleColor color;

            color = _config.Colors[(_random.Next(0, _config.GetQuantityColorFish()))];
            if (color == ConsoleColor.Black)
            {
                color = ConsoleColor.DarkBlue;
            }
            return color;
        }

        private char GenerateSkin()
        {
            char skin = _config.Skin[_random.Next(0, _config.Skin.Length)];
            return skin;
        }

        private void GeneratePointSpavn()
        {
            int minimumNumberRandomLeft = _config.AnchorPointAquarium[0] + 1;
            int maximumNumberRandomLeft = _config.AnchorPointAquarium[0] + _config.AquariumSizeLeftTop[0] - 1;
            int minimumNumberRandomTop = _config.AnchorPointAquarium[1];
            int maximumNumberRandomTop = _config.AnchorPointAquarium[1] + _config.AquariumSizeLeftTop[1] - 1;

            PositionLeft = _random.Next(minimumNumberRandomLeft, maximumNumberRandomLeft);
            PositionTop = _random.Next(minimumNumberRandomTop, maximumNumberRandomTop);

        }


    }



}
