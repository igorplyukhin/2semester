#!/usr/bin/env python3
from collections import namedtuple
import re
import requests
import sys


def format_title(title):
    return title.split('|')[0].replace(' ', '_')


def get_content(title):
    """
    Функция возвращает содержимое вики-страницы name из русской Википедии.
    В случае ошибки загрузки или отсутствия страницы возвращается None.
    """
    try:
        f = requests.get("https://ru.wikipedia.org/w/"
                     "api.php?action=parse&page={}&prop=wikitext&formatversion=2&format=json".format(title)).json()
    except:
        f = "error"
        pass

    if "error" in f:
        return None
    else:
        Page = namedtuple("Page", "title content")
        return Page(title=format_title(f["parse"]["title"]), content=f["parse"]["wikitext"])


def extract_content(page):
    """
    Функция принимает на вход содержимое страницы и возвращает 2-элементный
    tuple, первый элемент которого — номер позиции, с которой начинается
    содержимое статьи, второй элемент — номер позиции, на котором заканчивается
    содержимое статьи.
    Если содержимое отсутствует, возвращается (0, 0).
    """
    return (0, len(page))


def extract_links(page):
    """
    Функция принимает на вход содержимое страницы и начало и конец интервала,
    задающего позицию содержимого статьи на странице и возвращает все имеющиеся
    ссылки на другие вики-страницы без повторений и с учётом регистра.
    """
    return [format_title(x) for x in re.findall(r"\[\[(.*?)\]\]", page)]


def find_chain(start, finish):
    """
    Функция принимает на вход название начальной и конечной статьи и возвращает
    список переходов, позволяющий добраться из начальной статьи в конечную.
    Первым элементом результата должен быть start, последним — finish.
    Если построить переходы невозможно, возвращается None.
    """
    current_title = start
    chain = [current_title]
    links = [current_title]
    while current_title != finish:
        j = None
        if finish in links:
            j = get_content(finish)
        else:
            for link in links:
                j = get_content(link)
                if j is not None and j.title not in chain and "Файл" not in j.title:
                    break
        current_title = j.title
        links = extract_links(j.content)
        chain.append(current_title)
        print(current_title)
    return chain

def main():
    find_chain(sys.argv[1], "Философия")
    #print(extract_links(get_content("Компьютерная_программа").content))


if __name__ == '__main__':
    main()

