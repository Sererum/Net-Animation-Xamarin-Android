using Network.Classes.DataNet;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Network.Classes.NetStructure
{
    class Part
    {
        private Vector2 _position;
        private Vector2 _velocity;

        private int _ticsBeforeCollision;
        private float _sizeGridCells;
        private int _lengthConnect;

        private List<Part> _listNeighbours;
        private int _countNeighbours;

        private int _extraTics;

        public Part (Vector2 position, Vector2 velocity)
        {
            _position = position;
            _velocity = velocity;

            _sizeGridCells = NetState.LengthConnect * 3;

            _ticsBeforeCollision = GetTicsBeforeCollision();
            _listNeighbours = new List<Part>();
            _lengthConnect = NetState.LengthConnect;

            _extraTics = 80 / NetState.SpeedParts;
        }

        public Vector2 Position
        {
            get { return _position; }
        }

        public List<Part> ListNeighbours
        {
            get { return _listNeighbours; }
        }

        public int CountNeighbours
        {
            get { return _countNeighbours; }
        }

        public void UpdatePosition (ref List<Part>[,] gridParts)
        {
            _position += _velocity;
            _ticsBeforeCollision--;

            int startGridY = (int) (_position.Y / _sizeGridCells);
            int startGridX = (int) (_position.X / _sizeGridCells);

            AddPartInGrid(startGridY, startGridX, ref gridParts);

            if (_position.Y + _lengthConnect > startGridY * _sizeGridCells)
                AddSectorPartInGrid(startGridY + 1, startGridX, ref gridParts, true);
            if (_position.Y - _lengthConnect < startGridY * (_sizeGridCells - 1))
                AddSectorPartInGrid(startGridY - 1, startGridX, ref gridParts, true);

            if (_position.X + _lengthConnect > startGridX * _sizeGridCells)
                AddSectorPartInGrid(startGridY, startGridX + 1, ref gridParts, false);
            if (_position.X - _lengthConnect < startGridX * (_sizeGridCells - 1))
                AddSectorPartInGrid(startGridY, startGridX - 1, ref gridParts, false);

            if (_ticsBeforeCollision == 0)
            {
                UpdatePositionWithCollision();
                _ticsBeforeCollision = GetTicsBeforeCollision();
            }
        }

        public void CalculateNeighbours (List<Part>[,] gridParts)
        {
            _listNeighbours.Clear();

            foreach (Part part in gridParts[(int) (_position.Y / _sizeGridCells), (int) (_position.X / _sizeGridCells)])
            {
                if (IsNeighbour(part.Position))
                    _listNeighbours.Add(part);
            }
            _countNeighbours = _listNeighbours.Count;
        }

        private void UpdatePositionWithCollision ()
        {
            if (_position.Y < 0)
                _position.Y = Data.ScreenHeight + Math.Abs(_extraTics * _velocity.Y);
            else if (_position.Y > Data.ScreenHeight)
                _position.Y = - Math.Abs(_extraTics * _velocity.Y);

            if (_position.X < 0)
                _position.X = Data.ScreenWidth + Math.Abs(_extraTics * _velocity.X);
            else if (_position.X > Data.ScreenWidth)
                _position.X = - Math.Abs(_extraTics * _velocity.X);
        }

        private int GetTicsBeforeCollision ()
        {
            int ticsToLeft = 32767, ticsToRight = 32767, ticsToUp = 32767, ticsToDown = 32767;

            if (_velocity.Y < 0)
                ticsToUp = (int) Math.Abs(_position.Y / _velocity.Y);
            else
                ticsToDown = (int) Math.Abs((Data.ScreenHeight - _position.Y) / _velocity.Y);

            if (_velocity.X < 0)
                ticsToLeft = (int) Math.Abs(_position.X / _velocity.X);
            else
                ticsToRight = (int) Math.Abs((Data.ScreenWidth - _position.X) / _velocity.X);

            return Math.Min(Math.Min(ticsToUp, ticsToDown), Math.Min(ticsToLeft, ticsToRight)) + _extraTics;
        }

        private bool IsNeighbour (Vector2 otherPosition)
        {
            return NetState.RightLengthConnect(_position.X, _position.Y, otherPosition.X, otherPosition.Y);
        }

        private void AddPartInGrid(int row, int column, ref List<Part>[,] gridParts)
        {
            try
            { gridParts[row, column].Add(this); }
            catch (IndexOutOfRangeException) { } 
        }

        private void AddSectorPartInGrid(int row, int column, ref List<Part>[,] gridParts, bool upOrDownBorder)
        {
            if (upOrDownBorder)
            {
                AddPartInGrid(row, column-1, ref gridParts);
                AddPartInGrid(row, column, ref gridParts);
                AddPartInGrid(row, column+1, ref gridParts);
            }
            else
            {
                AddPartInGrid(row-1, column, ref gridParts);
                AddPartInGrid(row, column, ref gridParts);
                AddPartInGrid(row+1, column, ref gridParts);
            }
        }
    }
}