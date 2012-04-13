from perlin_noise import *
from texture_gen import *

from texture_gen import Gradient

def generate_cloud_texture():
	w = h = 512
	octaves = 9
	persistence = .5

	print 'Making Perlin noise...'
	p_noise = perlin_noise_2d(w, h, octaves, persistence)

	print 'Generating cloud texture...'
	gradient = Gradient((0, 0, 1, 1), (1, 1, 1, 1))
	color_grid = map_gradient(gradient, p_noise)
	generate_texture(color_grid, 'cloud_texture.png')

	print 'Done.'

def wood_texture(width, height, layers):
	perlin = perlin_noise_2d(width, height, layers, .15)

	for i in range(width):
		for j in range(height):
			g = perlin[i][j] * 20
			perlin[i][j] = g - int(g)
	
	return perlin

def generate_wood_texture():
	w = h = 512
	octaves = 9

	print 'Making Perlin noise ...'
	p_noise = wood_texture(w, h, octaves)

	print 'Generating wood texture...'
	gradient = Gradient((.62, .32, .17, 1), (.38, .13, .07, 1))
	color_grid = map_gradient(gradient, p_noise)
	generate_texture(color_grid, 'wood_texture.png')

	print 'Done.'

def write_perlin_to_file(perlin, width, height):
	f = open('perlin.txt', 'w')
	for i in range(width):
		for j in range(height):
			if(j == height - 1):
				f.write(str(perlin[i][j]))
			else:
				f.write(str(perlin[i][j]) + ' ')
		f.write('\n')

	f.close()

generate_cloud_texture()
