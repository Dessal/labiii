import pytest
import math  
from pytest import approx
from math_functions import multiply, divide, distance, quadratic_equation, geometric_sum

# Тесты для умножения
def test_multiply():
    assert multiply(2, 3) == 6
    assert multiply(-1, 5) == -5
    assert multiply(0, 10) == 0
    assert multiply(7, 0) == 0
    assert multiply(-3, -3) == 9

# Тесты для деления
def test_divide():
    assert divide(6, 3) == 2
    assert divide(5, 2) == 2.5
    assert divide(10, 2) == 5
    with pytest.raises(ValueError):
        divide(1, 0)  # Деление на ноль

# Тесты для расстояния между точками
def test_distance():
    assert distance(0, 0, 3, 4) == 5
    assert distance(1, 1, 1, 1) == 0
    assert distance(0, 0, 0, 0) == 0
    assert distance(-1, -1, 1, 1) == math.sqrt(8)
    assert distance(1, 2, 4, 6) == 5

# Тесты для квадратного уравнения
def test_quadratic_equation():
    assert set(quadratic_equation(1, -3, 2)) == {2.0, 1.0}  # Корни 2 и 1
    assert quadratic_equation(1, -2, 1) == (1.0, 1.0)  # Двойной корень
    assert quadratic_equation(1, 0, -1) == (1.0, -1.0)  # Корни 1 и -1
    assert quadratic_equation(1, 2, 5) == None  # Нет корней
    assert quadratic_equation(2, -4, 2) == (1.0, 1.0)  # Двойной корень

def test_geometric_sum():
    assert geometric_sum(1, 2, 3) == approx(7, rel=1e-9)
    assert geometric_sum(1, 1, 5) == approx(5, rel=1e-9)
    assert geometric_sum(2, 3, 4) == approx(80, rel=1e-9)
    assert geometric_sum(5, 0, 5) == approx(25, rel=1e-9)
    assert geometric_sum(1, 0.5, 10) == approx(1.998046875, rel=1e-7)  # Исправлена точность


    #pip install pytest
    #cd C:\Users\Пользователь\source\repos\Lab9\Lab9
    #pytest test_math_functions.py
