import matplotlib.pyplot as plt
from random import random

data = []
for x in range(100):
	data.append(random())

plt.plot(range(100),data, 'ro')
plt.savefig('plot')
