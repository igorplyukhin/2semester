#!/usr/bin/env python3
from urllib.request import urlopen
from urllib.parse import quote, unquote
import re
import sys


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
        unquoted_links = unquote(link)
        if all(e not in unquoted_links for e in ["#", "category", ":"]) and unquoted_links not in links:
            links.append(unquoted_links)
    return links


def find_chain(start, finish):
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
                if link not in chain:
                    content = get_content(link)
                    if content is not None:
                        current_title = link
                        break
        if content is None:
            return
        bounds = extract_content(content)
        links = extract_links(content, bounds[0], bounds[1])
        chain.append(current_title)
    if len(chain) == 0:
        chain.append(start)
    return chain


def main():
    print(find_chain(sys.argv[1], "Философия"))


if __name__ == '__main__':
    main()
