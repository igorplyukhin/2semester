#!/usr/bin/env python3
from collections import Counter
import re


def make_stat(filename):
    vowel_letters = "аеёиоуэыюяАЕЁИОУЭЫЮЯ"
    male_exceptions = ["Илья", "Лёва", "Никита", "Алехандро"]
    female_exceptions = ["Любовь"]
    stat = {'general': {"male": Counter(), "female": Counter()}}
    current_year = ""
    with open(filename, encoding="cp1251") as f:
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
                    if name in female_exceptions \
                       or name[len(name) - 1] in vowel_letters and name not in male_exceptions \
                    else "male"
                stat[current_year][gender].update({name: 1})
                stat['general'][gender].update({name: 1})
    return stat


def extract_years(stat):
    return [x for x in reversed(list(stat.keys())) if x != 'general']


def extract_general(stat):
    male_stat = extract_general_male(stat)
    female_stat = extract_general_female(stat)
    return sorted(male_stat + female_stat, key=lambda item: item[1], reverse=True)


def extract_general_male(stat):
    return sorted(stat['general']['male'].items(), key=lambda item: item[1], reverse=True)


def extract_general_female(stat):
    return sorted(stat['general']['female'].items(), key=lambda item: item[1], reverse=True)


def extract_year(stat, year):
    male_stat = extract_year_male(stat, year)
    female_stat = extract_year_female(stat, year)
    return sorted(male_stat + female_stat, key=lambda item: item[1], reverse=True)


def extract_year_male(stat, year):
    return sorted(dict(stat[str(year)]['male']).items(), key=lambda item: item[1], reverse=True)


def extract_year_female(stat, year):
    return sorted(dict(stat[str(year)]['female']).items(), key=lambda item: item[1], reverse=True)


if __name__ == '__main__':
    print(extract_years(make_stat("home.html")))
