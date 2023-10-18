using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Tiled2Unity
{
    public class TiledMap : MonoBehaviour
    {
        public enum MapOrientation
        {
            Orthogonal,
            Isometric,
            Staggered,
            Hexagonal,
        }

        public enum MapStaggerAxis
        {
            X,
            Y,
        }

        public enum MapStaggerIndex
        {
            Odd,
            Even,
        }

        public MapOrientation Orientation = MapOrientation.Orthogonal;
        public MapStaggerAxis StaggerAxis = MapStaggerAxis.X;
        public MapStaggerIndex StaggerIndex = MapStaggerIndex.Odd;
        public int HexSideLength = 0;

        public int NumLayers = 0;
        public int NumTilesWide = 0;
        public int NumTilesHigh = 0;
        public int TileWidth = 0;
        public int TileHeight = 0;
        public float ExportScale = 1.0f;

        // Note: Because maps can be isometric and staggered we simply can't multiply tile width (or height) by number of tiles wide (or high) to get width (or height)
        // We rely on the exporter to calculate the width and height of the map
        public int MapWidthInPixels = 0;
        public int MapHeightInPixels = 0;

        // Background color could be used to set the camera clear color to get the same effect as in Tiled
        public Color BackgroundColor = Color.black;

        public float LevelDirection = 1.0f;

        public float GetMapWidthInPixelsScaled()
        {
            return this.MapWidthInPixels * this.transform.lossyScale.x * this.ExportScale;
        }

        public float GetMapHeightInPixelsScaled()
        {
            return this.MapHeightInPixels * this.transform.lossyScale.y * this.ExportScale;
        }

        public Rect GetMapRect()
        {
            Vector2 pos_w = this.gameObject.transform.position;
            float width = this.MapWidthInPixels;
            float height = this.MapHeightInPixels;
            return new Rect(pos_w.x, pos_w.y - height, width, height);
        }

        public Rect GetMapRectInPixelsScaled()
        {
            Vector2 pos_w = this.gameObject.transform.position;
            float widthInPixels = GetMapWidthInPixelsScaled();
            float heightInPixels = GetMapHeightInPixelsScaled();
            return new Rect(pos_w.x, pos_w.y - heightInPixels, widthInPixels, heightInPixels);
        }

        public bool AreTilesStaggered()
        {
            // Hex and Iso Staggered maps both use "staggered" tiles
            return this.Orientation == MapOrientation.Staggered || this.Orientation == MapOrientation.Hexagonal;
        }

        private void OnDrawGizmosSelected()
        {
            Vector3 pos_w = this.gameObject.transform.position;
            Vector3 topLeft = Vector3.zero + pos_w;
            Vector3 topRight = new Vector3(GetMapWidthInPixelsScaled(), 0) + pos_w;
            Vector3 bottomRight = new Vector3(GetMapWidthInPixelsScaled(), -GetMapHeightInPixelsScaled()) + pos_w;
            Vector3 bottomLeft = new Vector3(0, -GetMapHeightInPixelsScaled()) + pos_w;

            // To make gizmo visible, even when using depth-shader shaders, we decrease the z depth by the number of layers
            float depth_z = -1.0f * this.NumLayers;
            pos_w.z += depth_z;
            topLeft.z += depth_z;
            topRight.z += depth_z;
            bottomRight.z += depth_z;
            bottomLeft.z += depth_z;

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);
        }

        [ContextMenu("Update tile components")]
        void UpdateTileComponents()
        {
            string csvPath = string.Format("LevelCSVs/{0}", gameObject.name);
            TextAsset csvFile = Resources.Load(csvPath, typeof(TextAsset)) as TextAsset;
            if (csvFile != null)
            {
                const float tileUnitSize = 1.2f;
                Transform objectsParent = transform.GetChild(0);
                // Create grounds parent
                GameObject groundsParent = new GameObject("Grounds");
                groundsParent.transform.parent = objectsParent;
                // Create coins parent
                GameObject coinsParent = new GameObject("Coins");
                coinsParent.transform.parent = objectsParent;
                // Triggers parent
                GameObject triggersParent = new GameObject("Triggers");
                triggersParent.transform.parent = objectsParent;
                // Load prefabs
                GameObject coinPrefab = Resources.Load("Coin", typeof(GameObject)) as GameObject;
                GameObject directionPrefab = Resources.Load("DirectionTrigger", typeof(GameObject)) as GameObject;
                GameObject timePrefab = Resources.Load("TimeTrigger", typeof(GameObject)) as GameObject;
                GameObject finishPrefab = Resources.Load("LevelFinish", typeof(GameObject)) as GameObject;
                GameObject jumperPrefab = Resources.Load("Jumper", typeof(GameObject)) as GameObject;
                // Create obstacles parent
                GameObject obstaclesParent = new GameObject("Obstacles");
                obstaclesParent.transform.parent = objectsParent;
                // Split csv files content on line endings
                string[] records = csvFile.text.Split('\n');
                for(int i = 0; i < records.Length; i++)
                {
                    // Split each line on commas
                    string[] fields = records[i].Split(',');
                    for (int j = 0; j < fields.Length; j++ )
                    {
                        int tileIndex = -1;
                        // Not empty tile
                        if(int.TryParse(fields[j], out tileIndex) && tileIndex != -1)
                        {
                            Vector3 obsPos = new Vector3(tileUnitSize * 0.5f + j * tileUnitSize, -tileUnitSize * 0.5f - i * tileUnitSize, 0);
                            switch(tileIndex)
                            {
                                case 1: // Grounds
                                    {
                                        // Create new object
                                        GameObject obstacle = new GameObject("Ground");
                                        BoxCollider2D col2d = obstacle.AddComponent<BoxCollider2D>();
                                        obstacle.tag = "Ground";
                                        // Set transform params
                                        obstacle.transform.parent = groundsParent.transform;
                                        obstacle.transform.position = obsPos;
                                        obstacle.transform.localScale = new Vector3(tileUnitSize, tileUnitSize, tileUnitSize);
                                        // Start/Finish grounds
                                        if(j == 0 || j == fields.Length - 1)
                                        {
                                            col2d.size = new Vector2(5, 1);
                                            col2d.offset = new Vector2((j == 0 ? -2 : 2), 0);
                                            // Level finish
                                            if(j == fields.Length - 1)
                                            {
                                                GameObject lf = Instantiate(finishPrefab, obsPos + new Vector3(2 * tileUnitSize, tileUnitSize * 0.5f, 0), Quaternion.identity) as GameObject;
                                                lf.transform.parent = triggersParent.transform;
                                                BoxCollider2D bc = lf.GetComponent<BoxCollider2D>();
                                                bc.offset = new Vector2(0, bc.size.y * 0.5f);
                                            }
                                        }
                                    }
                                    break;
                                case 2: // Coins
                                    {
                                        GameObject coin = Instantiate(coinPrefab, obsPos, Quaternion.identity);
                                        coin.transform.parent = coinsParent.transform;
                                        coin.transform.localScale = coinPrefab.transform.localScale;
                                    }
                                    break;
                                case 3: case 4: case 5: case 6: case 10: // Obstacles
                                    {
                                        // Create new object
                                        GameObject obstacle = new GameObject("Obstacle");
                                        BoxCollider2D col2d = obstacle.AddComponent<BoxCollider2D>();
                                        col2d.isTrigger = true;
                                        obstacle.tag = "Obstacle";
                                        // Set transform params
                                        obstacle.transform.parent = obstaclesParent.transform;
                                        obstacle.transform.position = obsPos;
                                    }
                                    break;
                                case 7: // Jump trigger
                                    {
                                        GameObject jmp = Instantiate(jumperPrefab, obsPos, Quaternion.identity);
                                        jmp.transform.parent = triggersParent.transform;
                                        BoxCollider2D bc = jmp.GetComponent<BoxCollider2D>();
                                        bc.offset = new Vector2(0, bc.size.y * 0.5f);
                                    }
                                    break;
                                case 25: case 26: // Direction trigger right
                                    {
                                        GameObject dt = Instantiate(directionPrefab, obsPos - new Vector3(0, tileUnitSize * 0.5f, 0), Quaternion.identity);
                                        dt.transform.parent = triggersParent.transform;
                                        dt.GetComponent<DirectionSwitcher>()._targetDirection = (tileIndex == 25 ? -1 : 1);
                                        BoxCollider2D bc = dt.GetComponent<BoxCollider2D>();
                                        bc.offset = new Vector2(0, bc.size.y * 0.5f);
                                    }
                                    break;
                                case 28 : case 29: case 30: case 31: // Time trigger(fast)
                                    {
                                        GameObject tt = Instantiate(timePrefab, obsPos - new Vector3(0, tileUnitSize * 0.5f, 0), Quaternion.identity);
                                        tt.transform.parent = triggersParent.transform;
                                        tt.GetComponent<TimeTrigger>()._targetTime = ((tileIndex == 29 || tileIndex == 31) ? 2.0f : 1.0f);
                                        BoxCollider2D bc = tt.GetComponent<BoxCollider2D>();
                                        bc.offset = new Vector2(0, bc.size.y * 0.5f);
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }

    }

}
