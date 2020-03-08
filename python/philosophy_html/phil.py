#!/usr/bin/env python3
from urllib.request import urlopen
from urllib.parse import quote, unquote
import re


def get_content(name):
    """
    Функция возвращает содержимое вики-страницы name из русской Википедии.
    В случае ошибки загрузки или отсутствия страницы возвращается None.
    """
    url = "https://ru.wikipedia.org/wiki/{}".format(quote(name))
    try:
        with urlopen(url) as response:
            return response.read().decode('utf-8')
    except:
        return None


def extract_content(page):
    """
    Функция принимает на вход содержимое страницы и возвращает 2-элементный
    tuple, первый элемент которого — номер позиции, с которой начинается
    содержимое статьи, второй элемент — номер позиции, на котором заканчивается
    содержимое статьи.
    Если содержимое отсутствует, возвращается (0, 0).
    """
    return


def extract_links(page, begin, end):
    """
    Функция принимает на вход содержимое страницы и начало и конец интервала,
    задающего позицию содержимого статьи на странице и возвращает все имеющиеся
    ссылки на другие вики-страницы без повторений и с учётом регистра.
    """
    raw_links = re.findall(r"<a +href=[\",\']/wiki/(.+?)[\",\']", page[begin:end], re.IGNORECASE)
    links = []
    for x in raw_links:
        l = unquote(x)
        if all(e not in l for e in ["#", "category", ":"]) and l not in links:
            links.append(l)
    return links


def find_chain(start, finish):
    """
    Функция принимает на вход название начальной и конечной статьи и возвращает
    список переходов, позволяющий добраться из начальной статьи в конечную.
    Первым элементом результата должен быть start, последним — finish.
    Если построить переходы невозможно, возвращается None.
    """
    chain = []
    current_title = start
    links = [start]
    while current_title != finish:
        content = None
        if finish in links:
            content = get_content(finish)
            current_title = finish
        else:
            for link in links:
                if link not in chain and "год" not in link:
                    content = get_content(link)
                    if content is not None:
                        current_title = link
                        break
        links = extract_links(content, 0, len(content) - 1)
        chain.append(current_title)
        print(current_title)
    return chain

def main():
    find_chain("Философия", "Философия")
    #print("Философия" in extract_links(get_content("Аристотель"), 0, 1000000))


if __name__ == '__main__':
    main()
