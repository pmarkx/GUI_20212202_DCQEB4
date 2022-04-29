﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Models
{
    public class MapBackedByList : IMap
    {
        private List<GameObject> MapList;
        private int maxX;
        private int maxY;

        public MapBackedByList(int indexX, int indexY)
        {
            maxX = indexX;
            maxY = indexY;
            MapList = new List<GameObject>();
        }
        public GameObject this[int index1, int index2]
        {
            get
            {
                return MapList.FirstOrDefault(x => x.XPosition == index1 && x.YPosition == index2) ?? new Floor(index1, index2);
            }
            set
            {
                value.XPosition = index1;
                value.YPosition = index2;
                MapList.Add(value);
            }
        }

        public void CheckDie()
        {
            for (int i = 0; i < MapList.Count; i++)
            {
                if (!MapList[i].IsLive)
                    MapList.Remove(MapList[i]);
            }
        }

        public IEnumerator<GameObject> GetEnumerator()
        {
            return MapList.GetEnumerator();
        }

        public int GetLength(int dimension)
        {
            return dimension switch
            {
                0 => maxX,
                1 => maxY,
                _ => 0
            };
        }

        public (int X, int Y) IndexOf(Func<GameObject, bool> condition)
        {
            foreach (var item in MapList)
            {
                if (condition(item))
                    return (item.XPosition, item.YPosition);
            }
            return (-1, -1);
        }

        public void PopulateMapFromStreamReader(StreamReader streamReader, Player thePlayer)
        {
            if (!streamReader.EndOfStream)
            {
                for (int i = 0; i < GetLength(0); i++)
                {
                    string line = streamReader.ReadLine();
                    for (int j = 0; j < GetLength(1); j++)
                    {
                        switch (line[j])
                        {
                            case 'f':
                                //this[i, j] = new Floor(i, j);
                                break;
                            case 'm':
                                this[i, j] = new Mine(i, j);
                                break;
                            case 'w':
                                this[i, j] = new Wall(i, j);
                                break;
                            case 'e':
                                this[i, j] = new Enemy(i, j);
                                break;
                            case 'p':
                                this[i, j] = thePlayer;
                                break;
                        }
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void CollisionDetect()
        {
            foreach (var item in MapList)
            {
                if (MapList.Count(x => x.XPosition == item.XPosition && x.YPosition == item.YPosition) > 1)
                {
                    item.Collided(MapList.Where(x => x.XPosition == item.XPosition && x.YPosition == item.YPosition));
                }
            }
        }
    }
}