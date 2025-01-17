﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexgrid
{
    public class GenericHexGrid<T, V> 
        where T : ITile, new()
        where V : new()
    {
        private int _N;

        private T[,] _tiles;
        private List<T> _tilesList;

        private V[,,] _vertices;

        public GenericHexGrid(int N) 
        {
            int q, r, q2, r2, z;
            T t;
            Directions.AxialDirection d;

            _N = N;

            _tiles = new T[2 * N + 1, 2 * N + 1];
            _tilesList = new List<T>();

            _vertices = new V[2 * N + 3, 2 * N + 3, 2];

            // Create and store all tiles
            for (q = -_N; q <= _N; q++)
            {
                for (r = Math.Max(-_N, -q - _N); r <= Math.Min(_N, -q + _N); r++)
                {
                    t = new T();
                    t.q = q;
                    t.r = r;
                    _tiles[r + _N, q + _N] = t;
                    _tilesList.Add(t);

                    // Create and store vertices
                    foreach ((int, int, Directions.AxialDirection) vec in Directions.AxialVertexVector)
                    {
                        q2 = q + vec.Item1;
                        r2 = r + vec.Item2;
                        d = vec.Item3;
                        if (GetVertex(q2, r2, d) == null)
                        {
                            z = 0;
                            if (d == Directions.AxialDirection.S) z = 1;
                            _vertices[r + _N + 1, q + _N + 1, z] = new V();
                        }
                    }

                }
            }

        }

        public T GetTile(int q, int r)
        {
            Console.WriteLine("{0:D} {1:D} ", q, r);
            return _tiles[r + _N, q + _N];
        }
        public List<T> GetTiles()
        {
            return _tilesList;
        }

        public List<T> GetNeighbors(int q, int r)
        {
            int q2, r2;
            List<T> neighbors = new List<T>();

            for (int i = 0; i < Directions.AxialTileVector.GetLength(0); i++)
            {
                q2 = q + Directions.AxialTileVector[i].Item1;
                r2 = r + Directions.AxialTileVector[i].Item2;
                if (IsValidCord(q2, r2))
                    neighbors.Add(GetTile(q2, r2));                
            }
            return neighbors;
        }

        public V GetVertex(int q, int r, Directions.AxialDirection d)
        {
            int z = 0;
            if (d == Directions.AxialDirection.S) z = 1;
            return _vertices[r + _N + 1, q + _N + 1, z];
        }

        public bool IsValidCord(int q, int r)
        {
            if (q + r + (-q - r) == 0 && q >= -_N && q <= _N && r >= -_N && r <= _N)
                return true;
            return false;
        }

        public Point AxialToPixel(Layout layout, T tile)
        {
            Orientation M = layout.orientation;
            double x = (M.f0 * tile.q + M.f1 * tile.r) * layout.size.x;
            double y = (M.f2 * tile.q + M.f3 * tile.r) * layout.size.y;
            return new Point(x + layout.origin.x, y + layout.origin.y);
        }

        public List<Point> AllFlatAxialToPixel(Layout layout)
        {
            List<Point> points = new List<Point>();
            foreach(T tile in _tilesList) {
                points.Add(AxialToPixel(layout, tile));
            }
            return points;
        }

    }
}
