from perlin_noise import *
from texture_gen import *

from texture_gen import Gradient

def demo_2d():
	w = h = 512
	octaves = 9
	persistence = 0.5

	print 'Making Perlin noise...'
	p_noise = perlin_noise_2d(w, h, octaves, persistence)

	print 'Generating texture...'
	gradient = Gradient((0, 0, 1, 1), (1, 1, 1, 1))
	color_grid = map_gradient(gradient, p_noise)
	generate_texture(color_grid, 'sample_texture.png')

	print 'Done.'

demo_2d()
