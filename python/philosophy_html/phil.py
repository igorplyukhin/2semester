#!/usr/bin/env python3
from urllib.request import urlopen
from urllib.parse import quote, unquote
import re


def get_content(name):
    url = "https://ru.wikipedia.org/wiki/{}".format(quote(name))
    try:
        with urlopen(url) as response:
            return response.read().decode('utf-8')
    except:
        return None


def extract_content(page):
    if page is None:
        return 0, 0
    return re.search("<p>", page).start(), re.search(r"(?s:.*)Категории", page).end()


def extract_links(page, begin, end):
    raw_links = re.findall(r"<a +href=[\",\']/wiki/(.+?)[\",\']", page[begin:end], re.IGNORECASE)
    links = []
    for link in raw_links:
        unquoted_link = unquote(link)
        if all(e not in unquoted_link for e in ["#", "category", ":"]) and unquoted_link not in links:
            links.append(unquoted_link)
    return links


def find_chain(start, finish):
    chain = [start]
    links = [start]
    current_title = start
    while current_title != finish:
        if finish in links:
            content = get_content(finish)
            current_title = finish
        else:
            for link in links:
                if link not in chain or link == start:
                    content = get_content(link)
                    if content is not None:
                        current_title = link
                        break
        if content is None:
            return
        bounds = extract_content(content)
        links = extract_links(content, bounds[0], bounds[1])
        chain.append(current_title)
    return chain


def main():
    find_chain("Философ", "Философия")


if __name__ == '__main__':
    main()
