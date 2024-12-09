import pytest
import math  
from pytest import approx
from math_functions import multiply, divide, distance, quadratic_equation, geometric_sum, count_words, find_substring, to_uppercase

# Фикстура для чтения текста из файла
@pytest.fixture
def sample_text():
    with open("sample_text.txt", "r") as f:
        return f.read()

# Класс для тестирования математических функций
class TestMathFunctions:
    @pytest.mark.parametrize("a, b, expected", [
        (2, 3, 6),
        (-1, 5, -5),
        (0, 10, 0),
        (-3, -3, 9)
    ])
    def test_multiply(self, a, b, expected):
        assert multiply(a, b) == expected

    @pytest.mark.parametrize("a, b, expected", [
        (6, 3, 2),
        (5, 2, 2.5),
        (10, 2, 5)
    ])
    def test_divide(self, a, b, expected):
        assert divide(a, b) == expected

    @pytest.mark.parametrize("x1, y1, x2, y2, expected", [
        (0, 0, 3, 4, 5),
        (1, 1, 1, 1, 0),
        (-1, -1, 1, 1, math.sqrt(8))
    ])
    def test_distance(self, x1, y1, x2, y2, expected):
        assert distance(x1, y1, x2, y2) == pytest.approx(expected)

    @pytest.mark.parametrize("a, b, c, expected", [
        (1, -3, 2, (2.0, 1.0)),
        (1, -2, 1, (1.0, 1.0)),
        (1, 2, 5, None)
    ])
    def test_quadratic_equation(self, a, b, c, expected):
        result = quadratic_equation(a, b, c)
        if expected is None:
            assert result is None
        else:
            assert set(result) == set(expected)

    @pytest.mark.parametrize("a, r, n, expected", [
        (1, 2, 3, 7),
        (1, 1, 5, 5),
        (2, 3, 4, 80),
        (1, 0.5, 10, pytest.approx(1.998046875, rel=1e-8))
    ])
    def test_geometric_sum(self, a, r, n, expected):
        assert geometric_sum(a, r, n) == expected


# Класс для тестирования текстовых функций
class TestStringFunctions:
    def test_count_words(self, sample_text):
        assert count_words(sample_text) == 19

    @pytest.mark.parametrize("substring, expected", [
        ("sample text", True),
        ("Pytest", True),
        ("missing", False)
    ])
    def test_find_substring(self, sample_text, substring, expected):
        assert find_substring(sample_text, substring) == expected

    def test_to_uppercase(self, sample_text):
        result = to_uppercase(sample_text)
        assert result.isupper()
        assert "HELLO" in result

    #pip install pytest
    #cd C:\Users\Пользователь\source\repos\Lab10\Lab10
    #pytest test_math_functions.py
