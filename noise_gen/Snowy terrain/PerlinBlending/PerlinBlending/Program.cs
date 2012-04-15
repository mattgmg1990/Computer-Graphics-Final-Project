using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace PerlinBlending
{
    class Program
    {
        static void Main(string[] args)
        {
            ImageBlend();
        }

        private static void ImageBlend()
        {
            Color[][] image1 = LoadImage("img/grass_and_dirt.png");
            Color[][] image2 = LoadImage("img/snow.png");
            Color[][] image3 = LoadImage("img/perlin_noise.png");

            int width = image1.Length;
            int height = image1[0].Length;

            float[][] perlinNoise = NoiseToFloat(image3);

            Color[][] perlinImage = BlendImages(image2, image1, perlinNoise);

            SaveImage(perlinImage, "img/perlin_noise_blended.png");
        }

        public static Color Interpolate(Color col0, Color col1, float alpha)
        {
            float beta = 1 - alpha;
            return Color.FromArgb(
                255,
                (int)(col0.R * alpha + col1.R * beta),
                (int)(col0.G * alpha + col1.G * beta),
                (int)(col0.B * alpha + col1.B * beta));
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
    }
}
