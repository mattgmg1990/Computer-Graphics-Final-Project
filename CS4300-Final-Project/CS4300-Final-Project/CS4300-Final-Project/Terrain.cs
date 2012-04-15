using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace CS4300_Final_Project
{
    class Terrain
    {
        // As a starting point, we used the terrain tutorial at http://www.riemers.net/eng/Tutorials/XNA/Csharp/Series1/Terrain_from_file.php
        // We adapted and added to this code for use in our project

        // A texture to hold the heightmap image when we load it
        private Texture2D heightMap;

        // A grass texture to paint the scene with
        private Texture2D texture;

        // The vertices of the terrain
        private VertexPositionNormalTexture[] vertices;
        // The indices of the terrain
        private int[] indices;

        // Holds the width and height of this terrain
        private int terrainWidth;
        private int terrainHeight;

        // Holds the height data when it is loaded
        private float[,] heightData;

        public Terrain(ContentManager content)
        {
            // Load the heightmap and then create the texture
            // The content manager will create an array of color objects to store in the heightmap
            heightMap = content.Load<Texture2D>("heightmap");
            // Load the grass texture
            texture = content.Load<Texture2D>("snowy_ground");
            load();
        }

        /// <summary>
        /// Load the height data for the terrain that will be drawn
        /// </summary>
        private void LoadHeightData()
        {
            terrainWidth = heightMap.Width;
            terrainHeight = heightMap.Height;

            Color[] heightMapColors = new Color[terrainWidth * terrainHeight];

            // Store the color data for the heightmap texture
            heightMap.GetData(heightMapColors);

            // Height data is stored based on the red value of each pixel of the height map (divided by 5 to scale it)
            // This is because the darker the pixel is, the higher red value it will have and thus the higher it should be
            heightData = new float[terrainWidth, terrainHeight];
            for (int x = 0; x < terrainWidth; x++)
                for (int y = 0; y < terrainHeight; y++)
                    heightData[x, y] = heightMapColors[x + y * terrainWidth].R / 5.0f;
        }

        /// <summary>
        /// Set up the vertices for the triangles that will be drawn
        /// </summary>
        private void SetUpVertices()
        {
            vertices = new VertexPositionNormalTexture[terrainWidth * terrainHeight];
            for (int x = 0; x < terrainWidth; x++)
            {
                for (int y = 0; y < terrainHeight; y++)
                {
                    vertices[x + y * terrainWidth].Position = new Vector3(x, heightData[x, y], -y);
                    vertices[x + y * terrainWidth].TextureCoordinate.X = (float)x / 30.0f;
                    vertices[x + y * terrainWidth].TextureCoordinate.Y = (float)y / 30.0f;
                }
            }
        }

        /// <summary>
        /// Set up the indices for the triangles that will be drawn
        /// </summary>
        private void SetUpIndices()
        {
            indices = new int[(terrainWidth - 1) * (terrainHeight - 1) * 6];
            int counter = 0;
            for (int y = 0; y < terrainHeight - 1; y++)
            {
                for (int x = 0; x < terrainWidth - 1; x++)
                {
                    int lowerLeft = x + y * terrainWidth;
                    int lowerRight = (x + 1) + y * terrainWidth;
                    int topLeft = x + (y + 1) * terrainWidth;
                    int topRight = (x + 1) + (y + 1) * terrainWidth;

                    indices[counter++] = topLeft;
                    indices[counter++] = lowerRight;
                    indices[counter++] = lowerLeft;

                    indices[counter++] = topLeft;
                    indices[counter++] = topRight;
                    indices[counter++] = lowerRight;
                }
            }
        }

        /// <summary>
        /// Draws this terrain
        /// </summary>
        /// <param name="device">The device that will be used to draw it</param>
        public void draw(GraphicsDevice device)
        {
            device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length, indices, 0, indices.Length / 3, VertexPositionNormalTexture.VertexDeclaration);
        }

        /// <summary>
        /// Load this terrain object. Sets up the vertices and indices for the triangle that will be drawn
        /// </summary>
        public void load()
        {
            LoadHeightData();
            SetUpVertices();
            SetUpIndices();
        }

        /// <summary>
        /// Gets the texture that is used to texture this terrain
        /// </summary>
        /// <returns>The texture</returns>
        public Texture2D getTexture()
        {
            return texture;
        }

        public float getHeight(int worldX, int worldY)
        {
            int terrainX = worldX;
            int terrainY = -1 * (worldY);

            return heightData[terrainX, terrainY];
        }
    }
}
