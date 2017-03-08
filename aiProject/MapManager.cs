using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torque3D.Util;

namespace RealAI
{
    public class MapManager
    {

        float X;
        float Y;
        float Gamma;

        MapElement[,] map;
        int size;
        int tickMod;

        public MapManager(int size = 50, int tickModulus = 0)
        {
            X = 0.0f; Y = 0.0f; Gamma = 0.0f;
            this.size = size;
            this.tickMod = tickModulus;
            map = new MapElement[size * 2, size * 2];
            for (int x = 0; x < size * 2; x++)
            {
                for (int y = 0; y < size * 2; y++)
                {
                    map[x, y] = new Unknown();
                }
            }
        }

        private int getIntCoordinate(double d)
        {
            return ((int)d) + size;
        }

        private MapElement getElement(double x_coordinate, double y_coordinate)
        {
            int x = (int)x_coordinate; int y = (int)y_coordinate;
            x += size; y += size;
            if (x >= 2 * size || y >= 2 * size || y < 0 || x < 0) { return new OutOfMap(); } //Out of bounds
            return map[x, y];
        }

        private void setElement(double x_coordinate, double y_coordinate, MapElement e, bool onlyIfUnknown = true)
        {
            int x = (int)x_coordinate; int y = (int)y_coordinate;
            x += size; y += size;
            if (x >= size * 2 || y >= size * 2 || y < 0 || x < 0) { Console.Out.Write("Error in setElement in mapManager (" + x + "," + y + ")");  return; } //Fail gracefully
            if (onlyIfUnknown)
            {
                if (map[x, y] is Unknown)
                {
                    map[x, y] = e;
                }
            }
            else
            {
                map[x, y] = e;
            }

        }

        public List<Tile> pathfindFromTo(double xFrom, double yFrom, double xTo, double yTo)
        {
            return null;
        }

        public void tick(FeatureVector v, Brain b)
        {
            //Update player position and rotation:
            X += v.DeltaMovedX;
            Y += v.DeltaMovedY;
            Gamma += v.DeltaRot;
            if(Gamma < 0)
            {
                Gamma = (float) (2 * Math.PI - Gamma);
            }
            Gamma = (float)(Gamma % (2 * Math.PI));

            updateMap(v, b);

            if (tickMod > 0 && v.TickCount % tickMod == 0) { Console.Out.WriteLine(renderMap()); }
        }

        private void updateMap(FeatureVector v, Brain b)
        {
            //Obviously we know what we are standing on:
            setElement(X, Y, new Walkable());
            //Now to extract info from the FeatureVector
            double xoffset = Math.Cos(Gamma);
            double yoffset = Math.Sin(Gamma);


            setElement(X + v.DistanceToObstacleLeft * xoffset, Y + v.DistanceToObstacleLeft * yoffset, new Obstacle());
            setElement(X + v.DistanceToObstacleRight * xoffset, Y + v.DistanceToObstacleRight * yoffset, new Obstacle());

            
            if(((int) xoffset) == 0)
            {
                for(int i=(int)v.DistanceToObstacleLeft; i > 1; i--)
                {
                    setElement(X, Y + i * yoffset, new Walkable());
                }
                for (int i = (int)v.DistanceToObstacleRight; i > 1; i--)
                {
                    setElement(X, Y + i * yoffset, new Walkable());
                }
            }

            if (((int)yoffset) == 0)
            {
                for (int i = (int)v.DistanceToObstacleLeft; i > 1; i--)
                {
                    setElement(X + i * xoffset, Y, new Walkable());
                }
                for (int i = (int)v.DistanceToObstacleRight; i > 1; i--)
                {
                    setElement(X + i * xoffset, Y, new Walkable());
                }
            }

            double d;
            d = v.DistanceToObstacleLeft;
            while (Math.Abs(d * xoffset) > 1 && Math.Abs(d * yoffset) > 1)
            {
                setElement(X + d * xoffset, Y + d * yoffset, new Walkable());
                d--;
            }
            d = v.DistanceToObstacleRight;
            while (Math.Abs(d * xoffset) > 1 && Math.Abs(d * yoffset) > 1)
            {
                setElement(X + d * xoffset, Y + d * yoffset, new Walkable());
                d--;
            }
        }

        public static double Rad2Deg(double rad)
        {
            return rad * 180 / Math.PI;
        }


        private string renderMap()
        {
            /*      Left
               Down       Up
                    Right
            */
            StringBuilder mapStr = new StringBuilder("Player Position: (" + X + ", " + Y + ") Rotation: " + Gamma + " (" + Rad2Deg(Gamma) + " degrees) \n");
            for (int y = -size; y < size; y++)
            {
                for (int x = -size; x < size; x++)
                {
                    if (x == (int)X && y == (int)Y)
                    {
                        mapStr.Append("@");
                    }
                    else
                    {
                        mapStr.Append("" + getElement(x, y).getCharRepresentation() + "");
                    }

                }
                mapStr.Append("\n");
            }
            return mapStr.ToString();
        }
    }

    public enum TileType { Walkable, Obstacle, Unknown, OutOfMap };

    public class Tile //This is the class that holds the map element, so we can get "onUpdate" events
    {
        private TileType mapElement;
        
        Tile(TileType e)
        {
            this.mapElement = e;
        }

        public string getCharRepresentation()
        {
            switch (mapElement)
            {
                case TileType.Unknown: return "?";
                case TileType.Walkable: return " ";
                case TileType.OutOfMap: return " ";
                case TileType.Obstacle: return "X";
                default: return "#";
            }
        }

        public void changeType(TileType changeTo)
        {

        }
    }

    interface MapElement
    {
        string getCharRepresentation();
    }

    class Walkable : MapElement
    {
        public string getCharRepresentation()
        {
            return " ";
        }
    }

    class Obstacle : MapElement
    {
        public string getCharRepresentation()
        {
            return "X";
        }
    }

    class Unknown : MapElement
    {
        public string getCharRepresentation()
        {
            return "?";
        }
    }

    class OutOfMap : MapElement
    {
        public string getCharRepresentation()
        {
            return " ";
        }
    }

}
