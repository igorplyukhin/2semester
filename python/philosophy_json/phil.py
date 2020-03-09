#!/usr/bin/env python3
from collections import namedtuple
import re
import requests
import sys


def format_title(title):
    return title.split('|')[0].replace(' ', '_')


def get_content(title):
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
    return (0, len(page))


def extract_links(page):
    return [format_title(x) for x in re.findall(r"\[\[(.*?)\]\]", page)]


def find_chain(start, finish):
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

