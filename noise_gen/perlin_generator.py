from perlin_noise import *
from texture_gen import *

from texture_gen import Gradient

# Generates a simple cloud texture from perlin_noise
def generate_sample_textures():
	w = h = 512
	octaves = 9

	# generate cloud texture
##	gradient = Gradient((1, 1, 1, 1), (0, 0, 1, 1))
##	p_noise = perlin_noise_2d(w, h, octaves, .5)
##	color_grid = map_gradient(gradient, p_noise)
##	generate_texture(color_grid, 'cloud_texture.png')

        # generate black and white texture
	gradient = Gradient((0, 0, 0, 1), (1, 1, 1, 1))
	p_noise = perlin_noise_2d(w, h, octaves, .3)
	color_grid = map_gradient(gradient, p_noise)
	generate_texture(color_grid, 'perlin_noise.png')

##        # generate grass and dirt ground texture
##	gradient = Gradient((.62, .32, .17, 1), ((34.0/255.0), (139.0/255.0), (34.0/255.0), 1))
##	p_noise = perlin_noise_2d(w, h, octaves, .5)
##	color_grid = map_gradient(gradient, p_noise)
##	generate_texture(color_grid, 'grass_and_dirt.png')

##	# generate snowy ground texture
##	gradient = Gradient((1, 1, 1, 1), ((34.0/255.0), (139.0/255.0), (34.0/255.0), 1))
##	p_noise = perlin_noise_2d(w, h, octaves, .5)
##	color_grid = map_gradient(gradient, p_noise)
##	generate_texture(color_grid, 'snowy_ground.png')

	# generate wood texture
	# gradient = Gradient((.62, .32, .17, 1), (.38, .13, .07, 1))
	# p_noise = wood_texture(512, 512, 9)
	# color_grid = map_gradient(gradient, p_noise)
	# generate_texture(color_grid, gradient)


# writes perlin noise values to a text file. Each value is delimited by
# a ' ' character and each row is delimited by a '\n' character
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

# Simple prompt which gives user the option to generate sample perlin noise texture
# or generate a text file with perlin noies values.
def prompt():
	print("please choose an option")
	print("\t1 - Generate sample texture")
	print("\t2 - Output perlin values to text file")

	selected = input('>')
	if (selected == 1):
		print("Generating texture, please wait...")
		generate_sample_textures()
	elif(selected == 2):
		width = input("specify width: ")
		height = input("specify height: ")
		persistence = input("specify persistence: ")
		print("Generting perlin noise values please wait")
		p_noise = perlin_noise_2d(width, height, 9, persistence)
		write_perlin_to_file(p_noise, width, height)
	else:
		prompt()

	print("Done")

prompt()
