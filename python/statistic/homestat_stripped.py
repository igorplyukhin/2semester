#!/usr/bin/env python3
from collections import Counter
import re


def make_stat(filename):
    """
    Функция вычисляет статистику по именам за каждый год с учётом пола.
    """
    vowel_letters = "аеёиоуэыюяАЕЁИОУЭЫЮЯ"
    exceptions = ["Илья", "Лёва", "Никита"]
    stat = {'general': {"male": Counter(), "female": Counter()}}
    current_year = ""
    with open(filename, encoding="cp1251") as f:
        # print(re.findall(r'<a href=(?:.+?)>(.+?)</a>', f.read()))
        for line in f:
            s = re.findall(r'<h3>(.+?)</h3>', line)
            if len(s) > 0:
                stat[s[0]] = {"male": Counter(), "female": Counter()}
                current_year = s[0]
                continue
            s = re.findall(r'<a href=(?:.+?)>(.+?)</a>', line)
            if len(s) > 0:
                name = s[0].split()[1]
                gender = "female" \
                    if name[len(name) - 1] in vowel_letters and name not in exceptions \
                    else "male"
                stat[current_year][gender].update({name: 1})
                stat['general'][gender].update({name: 1})
    return stat


def extract_years(stat):
    """
    Функция принимает на вход вычисленную статистику и выдаёт список годов,
    упорядоченный по возрастанию.
    """
    return list(stat.keys())[::-1]


def extract_general(stat):
    """
    Функция принимает на вход вычисленную статистику и выдаёт список tuple'ов
    (имя, количество) общей статистики для всех имён.
    Список должен быть отсортирован по убыванию количества.
    """
    male_stat = extract_general_male(stat)
    female_stat = extract_general_female(stat)
    return sorted(male_stat + female_stat, key=lambda item: item[1], reverse=True)


def extract_general_male(stat):
    """
    Функция принимает на вход вычисленную статистику и выдаёт список tuple'ов
    (имя, количество) общей статистики для имён мальчиков.
    Список должен быть отсортирован по убыванию количества.
    """
    return sorted(stat['general']['male'].items(), key=lambda item: item[1], reverse=True)


def extract_general_female(stat):
    """
    Функция принимает на вход вычисленную статистику и выдаёт список tuple'ов
    (имя, количество) общей статистики для имён девочек.
    Список должен быть отсортирован по убыванию количества.
    """

    return sorted(stat['general']['female'].items(), key=lambda item: item[1], reverse=True)


def extract_year(stat, year):
    """
    Функция принимает на вход вычисленную статистику и год.
    Результат — список tuple'ов (имя, количество) общей статистики для всех
    имён в указанном году.
    Список должен быть отсортирован по убыванию количества.
    """
    male_stat = extract_year_male(stat, year)
    female_stat = extract_year_female(stat, year)
    return sorted(male_stat + female_stat, key=lambda item: item[1], reverse=True)


def extract_year_male(stat, year):
    """
    Функция принимает на вход вычисленную статистику и год.
    Результат — список tuple'ов (имя, количество) общей статистики для всех
    имён мальчиков в указанном году.
    Список должен быть отсортирован по убыванию количества.
    """
    return sorted(dict(stat[str(year)]['male']).items(), key=lambda item: item[1], reverse=True)


def extract_year_female(stat, year):
    """
    Функция принимает на вход вычисленную статистику и год.
    Результат — список tuple'ов (имя, количество) общей статистики для всех
    имён девочек в указанном году.
    Список должен быть отсортирован по убыванию количества.
    """
    return sorted(dict(stat[str(year)]['female']).items(), key=lambda item: item[1], reverse=True)


if __name__ == '__main__':
    print(extract_general(make_stat("home.html")))
