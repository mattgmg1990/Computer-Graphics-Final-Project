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

	image.save(fname)   

