# math_functions.py

import math

# Функция умножения
def multiply(a, b):
    return a * b

# Функция деления
def divide(a, b):
    if b == 0:
        raise ValueError("Division by zero is not allowed.")
    return a / b

# Функция для нахождения расстояния между двумя точками
def distance(x1, y1, x2, y2):
    return math.sqrt((x2 - x1)**2 + (y2 - y1)**2)

# Функция для решения квадратного уравнения
def quadratic_equation(a, b, c):
    discriminant = b**2 - 4*a*c
    if discriminant < 0:
        return None  # Нет реальных решений
    sqrt_discriminant = math.sqrt(discriminant)
    x1 = (-b + sqrt_discriminant) / (2*a)
    x2 = (-b - sqrt_discriminant) / (2*a)
    return (x1, x2)  # Всегда возвращаем кортеж

# Функция для суммы геометрической прогрессии
def geometric_sum(a, r, n):
    if r == 1:
        return a * n  # Если r = 1, сумма - это просто n * a
    if r == 0:
        return a * n  # Если r = 0, все члены прогрессии будут равны a, сумма = n * a
    return a * (1 - r**n) / (1 - r)
