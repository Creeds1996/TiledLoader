using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// Author: Nathan (Creeds1996)
/// Date: 15/10/16
/// Description: This file is used to load in maps that are created with the program Tiled.
/// Tiled: www.mapeditor.org
/// Currently supported tile layer formats: XML, CSV (Support for Base64 is planned in a future update).
/// </summary>
namespace RPG.MapLoader
{
    /// <summary>
    /// Class the defines all the variables for a map layer.
    /// </summary>
    public class MapLayer
    {
        public string m_sName { get; set; } // Name of the layer that has been set in Tiled.
        public int m_iWidth { get; set; } // Width - Stores the amount of tile across the layer is.
        public int m_iHeight { get; set; } // Height - Stores the amoung of tile high the layer is.
        public float m_fOpacity { get; set; } // Opacity of the elements of the map layer.
        public float m_fTileOffsetX { get; set; } // Offset of a tile in the 'x' axis.
        public float m_fTileOffsetY { get; set; } // Offset of a tile in the 'y' axis.
        public bool m_bVisible { get; set; } // Defines if a map layer is visible or not.
        public Tile[] m_Tiles { get; set; } // Array that contains all of the tiles from a maplayer.

        /// <summary>
        /// Adds support for the CSV file format.
        /// </summary>
        /// <param name="a_Layer">XmlNode that contains the attributes from a Map Layer.</param>
        /// <param name="a_iMapWidth">The amount of tiles wide the map is.</param>
        /// <param name="a_iMapHeight">The amount of tiles high the map is.</param>
        /// <returns></returns>
        public static MapLayer LoadCSV(XmlNode a_Layer, int a_iMapWidth, int a_iMapHeight)
        {
            MapLayer Result = new MapLayer();

            if (a_Layer.Attributes.GetNamedItem("name") != null)
                Result.m_sName = a_Layer.Attributes.GetNamedItem("name").Value;
            else
                throw new Exception("Layers need to have a name.");
            
            if (a_Layer.Attributes.GetNamedItem("width") != null)
                Result.m_iWidth = int.Parse(a_Layer.Attributes.GetNamedItem("width").Value);
            else
                throw new Exception("Unable to read - Map Width");
            
            if (a_Layer.Attributes.GetNamedItem("height") != null)
                Result.m_iHeight = int.Parse(a_Layer.Attributes.GetNamedItem("height").Value);
            else
                throw new Exception("Unable to read - Map Height");
            
            if (a_Layer.Attributes.GetNamedItem("opacity") != null)
                Result.m_fOpacity = float.Parse(a_Layer.Attributes.GetNamedItem("opacity").Value);
            
            if (a_Layer.Attributes.GetNamedItem("offsetx") != null)
                Result.m_fTileOffsetX = float.Parse(a_Layer.Attributes.GetNamedItem("offsetx").Value);
            
            if (a_Layer.Attributes.GetNamedItem("offsety") != null)
                Result.m_fTileOffsetY = float.Parse(a_Layer.Attributes.GetNamedItem("offsety").Value);

            if (a_Layer.Attributes.GetNamedItem("visible") != null)
                Result.m_bVisible = Convert.ToBoolean(a_Layer.Attributes.GetNamedItem("visible").Value);
            else
                Result.m_bVisible = true;

            Result.m_Tiles = new Tile[a_iMapHeight * a_iMapWidth];

            int _Index = 0;

            foreach (string _Tile in a_Layer.FirstChild.InnerText.Split(','))
            {
                Result.m_Tiles[_Index] = new Tile(int.Parse(_Tile));
                _Index++;
            }

            return Result;
        }

        /// <summary>
        /// Adds support for the XML file format.
        /// </summary>
        /// <param name="a_Layer">XmlNode that contains the attributes from a Map Layer.</param>
        /// <param name="a_iMapWidth">The amount of tiles wide the map is.</param>
        /// <param name="a_iMapHeight">The amount of tiles high the map is.</param>
        /// <returns></returns>
        public static MapLayer LoadXML(XmlNode a_Layer, int a_iMapWidth, int a_iMapHeight)
        {
            MapLayer Result = new MapLayer();

            if (a_Layer.Attributes.GetNamedItem("name") != null)
                Result.m_sName = a_Layer.Attributes.GetNamedItem("name").Value;
            else
                throw new Exception("Layers need to have a name.");

            if (a_Layer.Attributes.GetNamedItem("width") != null)
                Result.m_iWidth = int.Parse(a_Layer.Attributes.GetNamedItem("width").Value);
            else
                throw new Exception("Unable to read - Map Width");

            if (a_Layer.Attributes.GetNamedItem("height") != null)
                Result.m_iHeight = int.Parse(a_Layer.Attributes.GetNamedItem("height").Value);
            else
                throw new Exception("Unable to read - Map Height");

            if (a_Layer.Attributes.GetNamedItem("opacity") != null)
                Result.m_fOpacity = float.Parse(a_Layer.Attributes.GetNamedItem("opacity").Value);

            if (a_Layer.Attributes.GetNamedItem("offsetx") != null)
                Result.m_fTileOffsetX = float.Parse(a_Layer.Attributes.GetNamedItem("offsetx").Value);

            if (a_Layer.Attributes.GetNamedItem("offsety") != null)
                Result.m_fTileOffsetY = float.Parse(a_Layer.Attributes.GetNamedItem("offsety").Value);

            if (a_Layer.Attributes.GetNamedItem("visible") != null)
                Result.m_bVisible = Convert.ToBoolean(a_Layer.Attributes.GetNamedItem("visible").Value);
            else
                Result.m_bVisible = true;

            Result.m_Tiles = new Tile[a_iMapWidth * a_iMapHeight];

            int _Index = 0;

            foreach (XmlNode _Tile in a_Layer.FirstChild)
            {
                Result.m_Tiles[_Index] = new Tile(int.Parse(_Tile.Attributes.GetNamedItem("gid").Value));
                _Index++;
            }

            return Result;
        }
    }

    /// <summary>
    /// Class that defines all the variables of a game tile.
    /// </summary>
    public class Tile
    {
        public int m_iTileSetID { get; set; } // Used to identify which tileset to use.
        public int m_iGid { get; set; } // Used to identify which sprite to render.

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public Tile()
        {

        }

        /// <summary>
        /// Constructor that sets a value to 'm_iGid'.
        /// </summary>
        /// <param name="a_iGid">'Gid' value is assigned by Tiled to represent which tile to render.</param>
        public Tile(int a_iGid)
        {
            m_iGid = a_iGid;
        }
    }

    /// <summary>
    /// Defines all the variables that belong to a Tileset.
    /// </summary>
    public class TileSet
    {
        public string m_sName { get; set; } // Name of the tileset.
        public int m_iFirstGid { get; set; } // First Gid from this tileset.
        public int m_iTileWidth { get; set; } // Width of a game tile.
        public int m_iTileHeight { get; set; } // Height of a game tile.
        public int m_iTileCount { get; set; } // Amount of tiles in this tileset.
        public int m_iColumns { get; set; } // Amount of tile columns in this tileset.
        public int m_iTileOffsetX { get; set; } // Tile offset in the 'X' axis.
        public int m_iTileOffsetY { get; set; } // Tile offset in the 'Y' axis.
        public int m_iSpacing { get; set; } // Tile Spacing
        public int m_iMargin { get; set; } // Margin around Tile
        public Texture2D m_Image { get; set; } // Texture of this tileset.

        /// <summary>
        /// Default constructor.
        /// </summary>
        public TileSet()
        {
            
        }

        /// <summary>
        /// Constructor that takes in an image. 
        /// </summary>
        /// <param name="a_Image">Image of the tileset.</param>
        public TileSet(Texture2D a_Image)
        {
            m_Image = a_Image;
        }

        public Rectangle getTileRect(Tile _Tile)
        {
            Rectangle Result = new Rectangle();

            int _Gid = _Tile.m_iGid - m_iFirstGid;

            int _NumColumns = m_Image.Width / (m_iTileWidth + m_iSpacing);
            int _Column = _Gid / _NumColumns;
            int _NumRows = m_iTileHeight / (m_iTileHeight + m_iSpacing);
            int _Row = _Gid % _NumColumns;

            Result.X = _Row * m_iTileWidth + _Row * m_iSpacing + m_iMargin;
            Result.Y = _Column * m_iTileHeight + _Column * m_iSpacing + m_iMargin;
            Result.Width = m_iTileWidth;
            Result.Height = m_iTileHeight;

            return Result;
        }

        /// <summary>
        /// Loads a Tileset from an XmlNode that is handed in.
        /// </summary>
        /// <param name="a_Tileset">XmlNode that contains all the information about a tileset.</param>
        /// <returns>Returns a tileset with all the information from the a_Tileset XmlNode</returns>
        public static TileSet Load(XmlNode a_Tileset, ContentManager a_ContentManager)
        {
            TileSet Result = new TileSet();

            if (a_Tileset.Attributes.GetNamedItem("firstgid") != null)
                Result.m_iFirstGid = int.Parse(a_Tileset.Attributes.GetNamedItem("firstgid").Value);
            else
                throw new Exception("Unable to read - Firstgid");

            if (a_Tileset.Attributes.GetNamedItem("name") != null)
                Result.m_sName = a_Tileset.Attributes.GetNamedItem("name").Value;
            else
                throw new Exception("Unable to read - Name");

            if (a_Tileset.Attributes.GetNamedItem("tilewidth") != null)
                Result.m_iTileWidth = int.Parse(a_Tileset.Attributes.GetNamedItem("tilewidth").Value);
            else
                throw new Exception("Unable to read - Tilewidth");

            if (a_Tileset.Attributes.GetNamedItem("tileheight") != null)
                Result.m_iTileHeight = int.Parse(a_Tileset.Attributes.GetNamedItem("tileheight").Value);
            else
                throw new Exception("Unable to read - Tileheight");

            if (a_Tileset.Attributes.GetNamedItem("tilecount") != null)
                Result.m_iTileCount = int.Parse(a_Tileset.Attributes.GetNamedItem("tilecount").Value);
            else
                throw new Exception("Unable to read - Tilecount");

            if (a_Tileset.Attributes.GetNamedItem("columns") != null)
                Result.m_iColumns = int.Parse(a_Tileset.Attributes.GetNamedItem("columns").Value);
            else
                throw new Exception("Unable to read - Columns");

            if (a_Tileset.Attributes.GetNamedItem("spacing") != null)
                Result.m_iSpacing = int.Parse(a_Tileset.Attributes.GetNamedItem("spacing").Value);

            if (a_Tileset.Attributes.GetNamedItem("margin") != null)
                Result.m_iMargin = int.Parse(a_Tileset.Attributes.GetNamedItem("margin").Value);

            foreach (XmlNode _Child in a_Tileset.ChildNodes)
            {
                if (_Child.Name == "image")
                    Result.m_Image = a_ContentManager.Load<Texture2D>("./Graphics/" + Path.GetFileNameWithoutExtension(_Child.Attributes.GetNamedItem("source").Value));
                else
                    throw new Exception("Unable to read - Image");

                if (_Child.Name == "tileoffset")
                {
                    Result.m_iTileOffsetX = int.Parse(_Child.Attributes.GetNamedItem("x").Value);
                    Result.m_iTileOffsetY = int.Parse(_Child.Attributes.GetNamedItem("y").Value);
                }
            }

            return Result;
        }
    }

    /// <summary>
    /// Contains all the variables that relate to an object.
    /// </summary>
    public class GameObject
    {
        public string m_sName { get; set; } // Name of the Object
        public string m_sType { get; set; } // Type of the Object
        public int m_iX { get; set; } // Position X-Axis
        public int m_iY { get; set; } // Position Y-Axis
        public int m_iHeight { get; set; } // Height of the Object
        public int m_iWidth { get; set; } // Width of the Object
        public string m_sProperty { get; set; } // Stores an additional property that can be defined by the user.
    }

    /// <summary>
    /// Contains all the variables that relate to an object group.
    /// </summary>
    public class ObjectGroup
    {
        public string m_sName { get; set; } // Name of the object group.
        public Color m_Color { get; set; } // Color of objects in the object group.
        public List<Object> m_Objects { get; set; } // List of objects that belong to this group.

        /// <summary>
        /// Takes in a string that contains a Hex color and returns a Xna Color. Must start with '#'.
        /// </summary>
        /// <param name="a_sColorString">String that contains the Hex color. Must start with '#'</param>
        /// <returns>Returns a Xna Color from the Hex String</returns>
        public static Color HextoColor(string a_sColorString)
        {
            if (a_sColorString.Length == 7 && a_sColorString.Substring(0, 1) == "#")
            {
                Color Result = new Color();

                Result.R = byte.Parse(a_sColorString.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
                Result.G = byte.Parse(a_sColorString.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
                Result.B = byte.Parse(a_sColorString.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
                Result.A = 255;

                return Result;
            }
            else
                throw new Exception("String doesn't start with '#' or is not 7 characters in length.");
        }

        /// <summary>
        /// Loads in objects that belong to an Objectgroup
        /// </summary>
        /// <param name="a_ObjectGroup">XmlNode that contains all the information about a object group</param>
        /// <returns>Returns an Objectgroup that contains all the objects from a certain group.</returns>
        public static ObjectGroup Load(XmlNode a_ObjectGroup, ContentManager a_ContentManager)
        {
            ObjectGroup Result = new ObjectGroup();
            Result.m_Objects = new List<Object>();

            if (a_ObjectGroup.Attributes.GetNamedItem("color") != null)
                Result.m_Color = HextoColor(a_ObjectGroup.Attributes.GetNamedItem("color").Value);
            else
                Result.m_Color = new Color(255, 255, 255);

            if (a_ObjectGroup.Attributes.GetNamedItem("name") != null)
                Result.m_sName = a_ObjectGroup.Attributes.GetNamedItem("name").Value;
            else
                throw new Exception("Unable to read - Name");

            foreach (XmlNode _Object in a_ObjectGroup.ChildNodes)
            {
                GameObject _Result = new GameObject();

                if (_Object.Attributes.GetNamedItem("name") != null)
                    _Result.m_sName = _Object.Attributes.GetNamedItem("name").Value;
                else
                    throw new Exception("Unable to read - Name");

                if (_Object.Attributes.GetNamedItem("type") != null)
                    _Result.m_sType = _Object.Attributes.GetNamedItem("type").Value;
                else
                    throw new Exception("Unable to read - Type");

                if (_Object.Attributes.GetNamedItem("x") != null)
                    _Result.m_iX = int.Parse(_Object.Attributes.GetNamedItem("x").Value);
                else
                    throw new Exception("Unable to read - X");

                if (_Object.Attributes.GetNamedItem("y") != null)
                    _Result.m_iY = int.Parse(_Object.Attributes.GetNamedItem("y").Value);
                else
                    throw new Exception("Unable to read - Y");

                if (_Object.Attributes.GetNamedItem("width") != null)
                    _Result.m_iWidth = int.Parse(_Object.Attributes.GetNamedItem("width").Value);
                else
                    throw new Exception("Unable to read - Width");

                if (_Object.Attributes.GetNamedItem("height") != null)
                    _Result.m_iHeight = int.Parse(_Object.Attributes.GetNamedItem("height").Value);
                else
                    throw new Exception("Unable to read - Height");

                if (_Object.ChildNodes.Count > 0)
                    foreach (XmlNode _Children in _Object.ChildNodes)
                    {
                        foreach (XmlNode _Child in _Children)
                        {
                            if (_Child.Attributes.GetNamedItem("value") != null)
                                _Result.m_sProperty = _Child.Attributes.GetNamedItem("value").Value;
                        }
                    }

                Result.m_Objects.Add(_Result);
            }

            return Result;
        }
    }

    /// <summary>
    /// Defines all the variables that belong to a map.
    /// </summary>
    public class Map
    {
        public int m_iMapWidth { get; set; } // Stores the amount of tiles wide the map is.
        public int m_iMapHeight { get; set; } // Stores the amount of tiles high the map is.
        public int m_iTileWidth { get; set; } // Stores the width of a game tile.
        public int m_iTileHeight { get; set; } // Stores the height of a game tile.
        private List<MapLayer> m_Layers = new List<MapLayer>(); // List that contains all the map layers.
        private List<TileSet> m_Tileset = new List<TileSet>(); // List that contains all the map Tilesets.
        private List<ObjectGroup> m_Objects = new List<ObjectGroup>(); // List that contains all the map objects.

        /// <summary>
        /// Allows users to access the objects contained within the map.
        /// </summary>
        /// <returns>Returns the list of objects contained within this map.</returns>
        public List<ObjectGroup> getObjects()
        {
            return m_Objects;
        }

        /// <summary>
        /// Allows users to access the map layers contained within the map.
        /// </summary>
        /// <returns>Returns the list of MapLayers contained within the map.</returns>
        public List<MapLayer> getMapLayers()
        {
            return m_Layers;
        }

        /// <summary>
        /// Allows users to access the TileSets contained within the map.
        /// </summary>
        /// <returns>Returns the list of TileSets contained within the map.</returns>
        public List<TileSet> getTileSets()
        {
            return m_Tileset;
        }

        /// <summary>
        /// Hands in a tile and this function returns the image that was used in Tiled.
        /// </summary>
        /// <param name="_Tile">Used to determine the correct image.</param>
        /// <returns>Returns the correct image for a Tile.</returns>
        private Texture2D getTexture(Tile _Tile)
        {
            for (int i = 0; i < m_Tileset.Count; i++)
            {
                if (_Tile.m_iGid >= m_Tileset[i].m_iFirstGid && _Tile.m_iGid <= m_Tileset[i].m_iTileCount)
                    _Tile.m_iTileSetID = i;
                    return m_Tileset[i].m_Image;
            }

            return null;
        }

        /// <summary>
        /// Loads all the information from the map file.
        /// </summary>
        /// <param name="a_sFileLocation">Directory for the map file.</param>
        public void Load(string a_sFileLocation, ContentManager a_ContentManager)
        {
            XmlDocument _Map = new XmlDocument();
            _Map.Load(a_sFileLocation);
            XmlElement _MapRoot = _Map.DocumentElement;

            if (_MapRoot.Attributes.GetNamedItem("width") != null)
                m_iMapWidth = int.Parse(_MapRoot.Attributes.GetNamedItem("width").Value);
            else
                throw new Exception("Unable to read - Map Width");

            if (_MapRoot.Attributes.GetNamedItem("height") != null)
                m_iMapHeight = int.Parse(_MapRoot.Attributes.GetNamedItem("height").Value);
            else
                throw new Exception("Unable to read - Map Height");

            if (_MapRoot.Attributes.GetNamedItem("tilewidth") != null)
                m_iTileWidth = int.Parse(_MapRoot.Attributes.GetNamedItem("tilewidth").Value);
            else
                throw new Exception("Unable to read - Tile Width");

            if (_MapRoot.Attributes.GetNamedItem("tileheight") != null)
                m_iTileHeight = int.Parse(_MapRoot.Attributes.GetNamedItem("tileheight").Value);
            else
                throw new Exception("Unable to read - Tile Height");

            XmlNodeList _Layers = _MapRoot.SelectNodes("layer");
            XmlNodeList _Tilesets = _MapRoot.SelectNodes("tileset");
            XmlNodeList _ObjectGroups = _MapRoot.SelectNodes("objectgroup");

            foreach (XmlNode _Layer in _Layers)
            {
                if (_Layer.LastChild.Name == "data" && _Layer.LastChild.Attributes.GetNamedItem("encoding") != null)
                    m_Layers.Add(MapLayer.LoadCSV(_Layer, m_iMapWidth, m_iMapHeight));
                else
                    m_Layers.Add(MapLayer.LoadXML(_Layer, m_iMapWidth, m_iMapHeight));
            }

            foreach (XmlNode _Tileset in _Tilesets)
            {
                m_Tileset.Add(TileSet.Load(_Tileset, a_ContentManager));
            }

            foreach (XmlNode _ObjectGroup in _ObjectGroups)
            {
                m_Objects.Add(ObjectGroup.Load(_ObjectGroup, a_ContentManager));
            }
        }

        public void Draw(SpriteBatch a_SpriteBatch)
        {
            int _Row = 0;
            int _Column = 0;

            foreach (MapLayer _Layer in m_Layers)
            {
                foreach (Tile _Tile in _Layer.m_Tiles)
                {
                    if (_Row == m_iMapWidth)
                    { 
                        _Row = 0;
                        _Column++;
                    }

                    if (_Tile.m_iGid != 0 && _Layer.m_bVisible)
                        a_SpriteBatch.Draw(getTexture(_Tile), null, new Rectangle(_Row * 32, _Column * 32, m_Tileset[_Tile.m_iTileSetID].m_iTileWidth, m_Tileset[_Tile.m_iTileSetID].m_iTileHeight), m_Tileset[_Tile.m_iTileSetID].getTileRect(_Tile), null, 0.0f, null, null);

                    _Row++;
                }
                _Row = 0;
                _Column = 0;
            }
        }

        public void Close()
        {
            foreach (TileSet _Tileset in m_Tileset)
            {
                _Tileset.m_Image.Dispose();
            }
        }
    }
}
