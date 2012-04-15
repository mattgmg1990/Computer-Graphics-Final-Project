/**
 * Functions for generating Perlin noise.
 **/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace PerlinNoise
{
    class PerlinNoise
    {

        #region Demo

        private static void ImageBlend()
        {
            Color[][] image1 = LoadImage("grass_and_dirt.png");
            Color[][] image2 = LoadImage("snow.png");
            Color[][] image3 = LoadImage("perlin_noise.png");

            int width = image1.Length;
            int height = image1[0].Length;

            float[][] perlinNoise = NoiseToFloat(image3);

            Color[][] perlinImage = BlendImages(image1, image2, perlinNoise);

            SaveImage(perlinImage, "perlin_noise_blended.png");
        }


        public static void Main(string[] args)
        {
            ImageBlend();
        }

        #endregion

        #region Reusable Functions

        public static float Interpolate(float x0, float x1, float alpha)
        {
            return x0 * (1 - alpha) + alpha * x1;
        }

        public static void SaveImage(Color[][] image, string fileName)
        {
            int width = image.Length;
            int height = image[0].Length;

            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    bitmap.SetPixel(i, j, image[i][j]);
                }
            }

            bitmap.Save(fileName);
        }

        public static Color[][] LoadImage(string fileName)
        {
            Bitmap bitmap = new Bitmap(fileName);
            
            int width = bitmap.Width;
            int height = bitmap.Height;

            Color[][] image = GetEmptyArray<Color>(width, height);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    image[i][j] = bitmap.GetPixel(i, j);
                }
            }

            return image;
        }

        public static float[][] NoiseToFloat(Color[][] noise)
        {
            int width = noise.Length;
            int height = noise[0].Length;

            float[][] floatingNoise = GetEmptyArray<float>(width, height);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    floatingNoise[i][j] = noise[i][j].GetBrightness();
                }
            }

            return floatingNoise;
        }

        public static T[][] GetEmptyArray<T>(int width, int height)
        {
            T[][] image = new T[width][];

            for (int i = 0; i < width; i++)
            {
                image[i] = new T[height];
            }

            return image;
        }

        public static Color[][] BlendImages(Color[][] image1, Color[][] image2, float[][] perlinNoise)
        {
            int width = image1.Length;
            int height = image1[0].Length;

	        Color[][] image = GetEmptyArray<Color>(width, height); //an array of colours for the new image
            
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    image[i][j] = Interpolate(image1[i][j], image2[i][j], perlinNoise[i][j]);
                }
            }

	        return image;
        }

        #endregion
    }
}