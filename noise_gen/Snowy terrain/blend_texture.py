from PIL import Image

def blend_images(image1, image2, noise):
        im1 = Image.open(image1)
        pixel1 = im1.load()
        pixel1Size = im1.size
        
        im2 = Image.open(image2)
        pixel2 = im2.load()
        
        im3 = Image.open(noise)
        pixel3 = im3.load()

        imOut = Image.new('RGBA', pixel1Size)
        
        print pixel1[0,0]
        print im1.getpixel("00")
        print im1.getpixel("01")
        print pixel1Size[0]
        print pixel1Size[1]
        print pixel2
        print pixel3
        
        #image = Image.blend(im1, im2, 0.5)
        
        #image.save('blended_texture.png')


def interpolate(image1, image2, alpha):
        out = image1 * (1.0 - alpha) + image2 * alpha
