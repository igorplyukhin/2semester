import os
import sys
import itertools


def project_stats(path, extensions):
    """
    Вернуть число строк в исходниках проекта.
    
    Файлами, входящими в проект, считаются все файлы
    в папке ``path`` (и подпапках), имеющие расширение
    из множества ``extensions``.

    """
    i = iter_filenames(path)
    i2 = with_extensions(extensions, i)
    return total_number_of_lines(i2)


def total_number_of_lines(filenames):
    """
    Вернуть общее число строк в файлах ``filenames``.
    """
    return sum(number_of_lines(x) for x in filenames)


def number_of_lines(filename):
    """ 
    Вернуть число строк в файле.
    """
    with open(filename) as f:
        return len(f.split("\n"))


def iter_filenames(path):
    return os.walk(path)


def with_extensions(extensions, filenames):
    """
    Оставить из итератора ``filenames`` только
    имена файлов, у которых расширение - одно из ``extensions``.    
    """
    return (x for x in filenames if get_extension(x) in extensions)


def get_extension(filename):
    """ Вернуть расширение файла """
    return filename.split('.')[1]


def print_usage():
    print("Usage: python project_sourse_stats_3.py <project_path>")


if __name__ == '__main__':
    # if len(sys.argv) != 2:
    #     print_usage()
    #     sys.exit(1)
    #
    # project_path = sys.argv[1]
    # print(project_stats(project_path, {'.cs'}))
    b = iter_filenames("NSimulator")
    a=0
