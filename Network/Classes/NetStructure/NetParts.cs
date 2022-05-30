using Network.Classes.DataNet;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Network.Classes.NetStructure
{
    class NetParts
    {
        private List<Part> _parts;

        private List<Part> _newParts;
        private List<Part> _deathParts;

        private int _neighbours;

        private int gridHeight;
        private int gridWidth;

        private Random Random;

        private List<Part>[,] _gridParts;

        public NetParts ()
        {
            CreateNewNet();

            _newParts = new List<Part>();
            _deathParts = new List<Part>();

            Random = new Random();

            gridHeight = (int) (Data.ScreenHeight / (NetState.LengthConnect * 3)) + 1;
            gridWidth = (int) (Data.ScreenWidth / (NetState.LengthConnect * 3)) + 1;

            UpdateStateNet();
        }

        public List<Part> ListParts
        {
            get { return _parts; }
        }

        public void CreateNewNet ()
        {
            _parts = new List<Part>();
            InitNet(NetState.QuantityParts);
        }

        public void UpdateStateNet ()
        {
            _gridParts = null;
            _gridParts = new List<Part>[gridHeight, gridWidth];

            for (int i = 0; i < gridHeight; i++)
            {
                for (int j = 0; j < gridWidth; j++)
                    _gridParts[i, j] = new List<Part>();
            }
            
            foreach (Part part in _parts)
                part.UpdatePosition(ref _gridParts);

            foreach (Part part in _parts)
                part.CalculateNeighbours(_gridParts);
        }

        public void IterrateLiveCircle ()
        {
            foreach (Part part in _parts)
            {
                _neighbours = part.CountNeighbours;

                if (NetState.GetStatePart(_neighbours) == Data.Duplicate)
                {
                    if (Random.Next(Data.DuplicationRate) == 0)
                        _newParts.Add(new Part(part.Position, new Vector2(
                                (float) (NetState.SpeedParts / 2 * Random.NextDouble() * (Random.Next(2) == 1 ? -1 : 1)),
                                (float) (NetState.SpeedParts / 2 * Random.NextDouble() * (Random.Next(2) == 1 ? -1 : 1)))
                                ));
                }
                else if (NetState.GetStatePart(_neighbours) == Data.Death)
                {
                    if (Random.Next(Data.DeathRate) == 0)
                        _deathParts.Add(part);
                } 
            }

            foreach (Part part in _deathParts)
                _parts.Remove(part);

            _deathParts.Clear();

            if (_parts.Count > NetState.QuantityParts)
            {
                _newParts.Clear();
                return;
            }

            foreach (Part part in _newParts)
                _parts.Add(part);

            _newParts.Clear();
        }
        
        private void InitNet (int startQuantityParts)
        {
            Random Random = new Random();

            for (int i = 0; i < startQuantityParts; i++)
            {
                _parts.Add(new Part(
                    position: new Vector2(
                        (float) (Data.ScreenWidth * Random.NextDouble()), 
                        (float) (Data.ScreenHeight * Random.NextDouble())),
                    velocity: new Vector2(
                        (float) (NetState.SpeedParts / 2 * Random.NextDouble() * (Random.Next(2) == 1 ? -1 : 1)), 
                        (float) (NetState.SpeedParts / 2 * Random.NextDouble() * (Random.Next(2) == 1 ? -1 : 1)))
                    ));
            }
        }
    }
}