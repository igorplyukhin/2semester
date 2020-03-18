using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
	public class ParsingTask
	{
		/// <param name="lines">все строки файла, которые нужно распарсить. Первая строка заголовочная.</param>
		/// <returns>Словарь: ключ — идентификатор слайда, значение — информация о слайде</returns>
		/// <remarks>Метод должен пропускать некорректные строки, игнорируя их</remarks>
		public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
		{
			var slides = new Dictionary<int, SlideRecord>();
			var isFirstIteration = true;
			foreach (var e in lines)
			{
				if (isFirstIteration)
				{
					isFirstIteration = false;
					continue;
				}

				try
				{
					var splittedLine = e.Split(';');
					var slideId = int.Parse(splittedLine[0]);
					var slideType = (SlideType)Enum.Parse(typeof(SlideType), splittedLine[1]);
					var unitTitle = splittedLine[2];
					slides.Add(slideId, new SlideRecord(slideId, slideType, unitTitle));
				}
				catch (Exception exception)
				{
				}
			}

			return slides;
		}

		/// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
		/// <param name="slides">Словарь информации о слайдах по идентификатору слайда. 
		/// Такой словарь можно получить методом ParseSlideRecords</param>
		/// <returns>Список информации о посещениях</returns>
		/// <exception cref="FormatException">Если среди строк есть некорректные</exception>
		public static IEnumerable<VisitRecord> ParseVisitRecords(
			IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
		{
			throw new NotImplementedException();
		}
	}
}