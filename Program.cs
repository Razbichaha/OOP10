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
        private Thread _threadKey;

        private bool _continueCicle = true;
        private bool _addFish = false;
        private bool _deleteFish = false;

        internal void Game()
        {
            _aquarium.AddFish(_random.Next(_config.MinimumFish, _config.MaximumFish));
            _message.ShowQuantityFish(ShowAllQuantityFish());
            _threadKey = new Thread(StartKeyboardThread);
            _threadKey.Start();

            while (_continueCicle)
            {
                _aquarium.CommitlifeCycleFish();
                ShowDisplayFishStats(_aquarium.GetMassivFishs());

                if (_addFish == true)
                {
                    _aquarium.AddFish(1);
                    _message.ShowQuantityFish(ShowAllQuantityFish());
                    _addFish = false;
                }
                if (_deleteFish == true)
                {
                    DeleteFish();
                    _deleteFish = false;
                }
                Thread.Sleep(200);
            }
        }

        private void DeleteFish()
        {
            int id = 0;

            if (_message.EnterId(_aquarium.GetListId(), ref id))
            {
                _aquarium.DeleteFish(id);
            }
        }

        private void StartKeyboardThread()
        {
            while (_continueCicle)
            {
                ConsoleKeyInfo key = Console.ReadKey();

                if (key.Key == ConsoleKey.Escape)
                {
                    _continueCicle = false;
                }

                if (key.Key == ConsoleKey.Enter)
                {
                    _addFish = true;

                }

                if (key.Key == ConsoleKey.Backspace)
                {
                    _deleteFish = true;
                }
            }
        }

        private int ShowAllQuantityFish()
        {
            return _aquarium.GetQuantityFishAll();
        }

        private void ShowDisplayFishStats(Fish[] fishs)
        {
            for (int i = 0; i < fishs.Length; i++)
            {
                _message.ShowDisplayFishStats(fishs[i], i + 1);
            }
        }
    }

    class Config
    {
        private ConsoleColor[] _colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));
        private char[] _skin = { '#', '@', '%', '$', '&', '№' };

        private int _maximumFish = 10;
        private int _minimumFish = 5;
        private int _maximumLifeTime = 100;
        private int _minimumLifeTime = 40;
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

        internal int GetQuantityColorFish()
        {
            return Colors.Length;
        }
    }

    class Message
    {
        private Config _config = new();

        internal bool EnterId(List<int> idAll, ref int id)
        {
            int tempId = id;
            bool thereIsId = false;
            SetCursorPositionMessag();
            Console.Write("Введите ID рыбы для удаления - ");
            string idString = Console.ReadLine();

            if (int.TryParse(idString, out tempId))
            {
                if (idAll.Exists(x => x == tempId))
                {
                    id = tempId;
                    thereIsId = true;
                }
                else
                {
                    Console.Write("Введенный Id отсуесевует.");
                }
            }
            else
            {
                Console.Write("Вы ввели не число попробуйте еще раз.");
            }
            return thereIsId;
        }

        internal void ShowDisplayFishStats(Fish fish, int numberLine)
        {
            string died;
            int indent = numberLine + 5;

            if (fish.Died == true)
                died = "Мертвая";
            else
                died = "Живая";

            Console.SetCursorPosition(_config.MessageCoordinateLeftTop[0], _config.MessageCoordinateLeftTop[1] + indent);
            Console.Write($"Id - {fish.Id} Жизнь - {fish.Healt}");
            Console.Write(" Скин - ");
            Console.ForegroundColor = fish.colorFish;
            Console.Write(fish.Skin);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($" Статус - {died}     ");
            Console.WriteLine();
        }

        internal void Clear()
        {
            Console.Clear();
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
            Console.WriteLine();
            Console.Write($"Всего рыб в аквариуме - {allFish}");
            Console.WriteLine();
            Console.WriteLine("Для выхода нажмите esc.");
            Console.WriteLine("Чтобы добавить рыбу, нажмите Enter.");
            Console.WriteLine("Чтобы удалить рыбу, нажмите Bacspace");
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
        private Fish[] _aquarium = new Fish[0];
        private List<Fish> _allFish = new List<Fish>();
        private int _lastId = 0;

        internal List<int> GetListId()
        {
            List<int> tempId = new List<int>();

            foreach (Fish fish in _aquarium)
            {
                tempId.Add(fish.Id);
            }
            return tempId;
        }

        internal void CommitlifeCycleFish()
        {
            bool thereAreLive = true;

            for (int i = 0; i < _aquarium.Length; i++)
            {
                _aquarium[i].RemoveOneLife();
            }

            while (thereAreLive)
            {
                foreach (Fish fish in _aquarium)
                {
                    if (fish.Died == true)
                    {
                        RemoveFish(fish);
                        break;
                    }
                }

                for (int i = 0; i < _aquarium.Length; i++)
                {
                    if (_aquarium[i].Died == false | i == _aquarium.Length - 1 | _aquarium.Length == 0)
                    {
                        thereAreLive = false;
                    }
                }

                if (_aquarium.Length==0)
                {
                    thereAreLive = false;
                }
            }
        }

        internal void AddFish(int quantityFish)
        {
            int tempId = _lastId;

            if (_aquarium.Length < _config.MaximumFish)
            {
                for (int i = tempId; i < quantityFish + tempId; i++)
                {
                    _lastId++;
                    Fish fish = new Fish(i);

                    AddFishToAquarium(fish);
                    _allFish.Add(fish);
                }
            }
            else
            {
                _message.ShowErrorMaximumFish();
            }
        }

        internal void DeleteFish(int id)
        {
            foreach (Fish fish in _aquarium)
            {
                if (fish.Id == id)
                {
                    RemoveFish(fish);
                    break;
                }
            }
        }

        internal int GetQuantityFishAll()
        {
            return _allFish.Count;
        }

        internal Fish[] GetMassivFishs()
        {
            Fish[] tempFishs = new Fish[_aquarium.Length];

            for (int i = 0; i < _aquarium.Length; i++)
            {
                tempFishs[i] = _aquarium[i];
            }
            return tempFishs;
        }

        private void AddFishToAquarium(Fish fish)
        {
            Fish[] tempFish = new Fish[_aquarium.Length + 1];

            for (int i = 0; i < _aquarium.Length; i++)
            {
                tempFish[i] = _aquarium[i];
            }
            _aquarium = tempFish;
            tempFish[_aquarium.Length - 1] = fish;
        }

        private void RemoveFish(Fish fish)
        {
            Fish[] tempFishs = new Fish[_aquarium.Length - 1];
            int tt = 0;

            for (int i = 0; i < _aquarium.Length; i++)
            {
                if (_aquarium[i] != fish)
                {
                    tempFishs[tt] = _aquarium[i];
                    tt++;
                }
            }
            _aquarium = tempFishs;
            _message.Clear();
            _message.ShowQuantityFish(_aquarium.Length);
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

        internal Fish(int id)
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
                KillFish();
            }
        }

        private void KillFish()
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
