import math
from random import random
import matplotlib.pyplot as plt

# ------------------------------------------------------------------------
# If you want to generate plots and experiment with the noise funciton,
# you must install matplotlib. Instructions on how to do so can be found
# here: http://matplotlib.sourceforge.net/users/installing.html. You must
# also uncomment the code in the perlin noise function.
# ------------------------------------------------------------------------

# Simply returns n random numbers between [0, 1]
def generate_white_noise(n):
	noise = []
	for x in range(0, n):
		noise.append(random())
	return noise

# Interpolation methods
def linear_interpolation(a, b, x):
	return a*(1-x) + b*x

def cosine_interpolation(a, b, x):
	ft = x * math.pi
	f = (1 - math.cos(ft)) * .5

	return a*(1-f) + b*f

# Returns a 3-tuple which is used as the input for the interpolation methods
def sample_points(x, t, max_x):
	x0 = x // t * t # '//' simply floors result of 'x / t'
	return x0, (x0 + t) % max_x, (x - x0) / t

# Interpolates between all of the base_noise values using interpolation_fun
def generate_smooth_noise(base_noise, interpolation_fun, octave):
	noise_len = len(base_noise)
	t = 2**octave
	smooth_noise = []
	
	for i in range(noise_len):	
		a, b, x = sample_points(i, t, noise_len)							
		smooth_noise.append(interpolation_fun(base_noise[a], base_noise[b], x))
	return smooth_noise
	

def perlin_noise_1d(layers, falloff, base_noise):
	perlin_noise = []
	for x in range(len(base_noise)):
		perlin_noise.append(0)

	r = 1
	for k in range(layers):
		r *= falloff
		smooth_noise = generate_smooth_noise(base_noise, linear_interpolation, layers - k - 1)
		# plt.axis([0, 100, 0, 1])
		# plt.plot(range(100), smooth_noise)
		# plt.savefig("octave_%s" % k)
		# plt.clf()

		for p in range(len(perlin_noise)):
			perlin_noise[p] += smooth_noise[p]*r

	# plt.axis([0, 100, 0, 1])
	# plt.plot(range(100),perlin_noise)
	# plt.savefig("perlin")
	return perlin_noise
			
# Debugging...
sample_noise = generate_white_noise(100)
perlin_noise = perlin_noise_1d(6, .5, sample_noise)

for x in perlin_noise:
	print x
