from perlin_noise import *
from PIL import Image

# Utility class that will help us dictate the general color scheme
# of randomly generated textures. The color values are 3-tuples 
# (or 4-tuples if using RGBA scheme) and each value in the tuple should
# be between [0, 1]
class Gradient:
	def __init__(self, color1, color2):
		if (len(color1) != len(color2)):
			raise Exception('Colors must have equal number of arguments')
		else:
			self.color1 = color1
			self.color2 = color2

	def mix_colors(self, t):
		new_color = []
		
		for ch1, ch2 in zip(self.color1, self.color2):
			new_color.append((1 - t)*ch1 + t*ch2)

		return new_color

# Returns a 2d array of RGB values based on the gradient and the
# given noise values
def map_gradient(gradient, noise):
	color_grid = []
	width = len(noise)
	height = len(noise[0])

	for i in range(width):
		color_grid.append([])
		for j in range(height):
			color_grid[i].append(gradient.mix_colors(noise[i][j]))
	
	return color_grid

# Takes rgb values that range form [0,1] and converts them so that
# they range from [0, 255]. Also returns a tuple since that's how
# PIL reads color values when generating an image.
def int_sequence(seq):
	new_seq = []
	for i in seq:
		new_seq.append(int(255 * i))		

	return tuple(new_seq)

# Blends pixel with a white color
def blend_pixel_with_white(seq1, alpha):
	l1 = list(seq1)
	return map(lambda x: int(x*(1-alpha) + 255*alpha), l1)

# Converts a 2D grid to an image on the disk. 
# The grid must contain float values (ranged 0 to 1).
def generate_texture(color_grid, fname):
	width = len(color_grid)
	height = len(color_grid[0])
	image = Image.new('RGBA', (width, height))
	pixels = image.load()
	seq = []

	for i in range(width):
		for j in range(height):
			seq = int_sequence(color_grid[i][j])
			pixels[i,j] = seq

	image.save('2d_noise/' + fname)   

# Interpolates between two textures with the given perlin noise values
# texture 1 and texture 2 are PIL Image objects
def blend_textures(texture1, p_noise, width, height):
	b_texture = Image.new('RGBA', (width, height))
	b_pixels = b_texture.load()
	t1_pixels = texture1.load()

	for i in range(width):
		for j in range(height):
			# blend_pixel(t1_pixels[i,j], t2_pixels[i,j], p_noise[i][j])
			alpha = p_noise[i][j]
			new_pixel = blend_pixel_with_white(t1_pixels[i,j], p_noise[i][j])
			b_pixels[i,j] = tuple(new_pixel)

	b_texture.save('blended_texture.png')

def normalize_rgb(rgb):
	rgb[0] = rgb[0] / 255.0
	rgb[1] = rgb[1] / 255.0
	rgb[2] = rgb[2] / 255.0
	print rgb
