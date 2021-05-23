using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using static System.Console;

namespace _1_6_1_KhakatonProg
{
    class Program
    {

        static FileInfo fileInfo;

        /// <summary>
        /// Ввод чисел и проверка соответствия условиям
        /// </summary>
        /// <param name="paramMax">Максимальное значение для вводимого числа</param>
        /// <param name="paramMin">Минимальное значение для вводимого числа</param>
        /// <returns>Введенное число</returns>
        static int NumInput(int paramMin, int paramMax)
        {
            int input; // Хранилище для введенного числа
            while (true)
            {
                if (int.TryParse(ReadLine(), out input) && input >= paramMin && input <= paramMax) break;
                else Write($"Число должно быть целым от {paramMin} до {paramMax}, попробуйте еще раз: ");
            }
            WriteLine();

            return input;
        }

        /// <summary>
        /// Ввод и проверка пути файла
        /// </summary>
        /// <returns>Путь к файлу</returns>
        static string PathInput()
        {
            string filepath; // Хранилище для пути к файлу

            Write("Введите путь к текстовому файлу c числом: ");
            while (true)
            {
                filepath = ReadLine();
                // Проверка наличия файла по указанному пути (и правильности самого пути)
                if (File.Exists(filepath)) break;
                else Write($"Неправильно указан путь либо файл отсутствует, попробуйте еще раз: ");
            }
            WriteLine();

            fileInfo = new FileInfo(filepath); // Запись данных указанного файла для дальнейшего использования 

            return filepath;
        }

        /// <summary>
        /// Считывание числа из файла и проверка
        /// </summary>
        /// <returns>Считанное из файла число</returns>
        static int ReadNumber(string filepath)
        {
            int number; // Хранилище для числа
            while (true)
            {
                // Проверка полученного из файла числа
                if (int.TryParse(File.ReadAllText(filepath).Replace("_",""), out number) 
                    && number >= 1 && number <= 1_000_000_000) break;
                else
                {
                    Write($"Число должно быть целым от 1 до 1_000_000_000, запишите в файл нужное число и введите путь к нему:\n");
                    filepath = PathInput(); // При неверной записи числа программа просит снова ввести путь к этому или другому файлу
                }
            }
            WriteLine($"Получено число - {number}\n");

            return number;
        }

        static void Main(string[] args)
        {
            DateTime dtime; // Переменная для подсчета времени работы программы

            while (true)
            {
                #region Начальные переменные

                string finFileName = "Groups_of_numbers";

                byte groupsNum = 0; // Кол-во групп чисел
                int gFirst = 2; // Переменная для обозначения границ групп 
                int subNum = 1; // Переменная для записи чисел от 1 до Х
                int number = ReadNumber(PathInput()); // Конечное число, полученное из файла
                byte percentMult = 1;
                #region Настройка множителя
                if (number <= 2_000_000) percentMult = 25;
                else if (number <= 35_000_000) percentMult = 10;
                else if (number <= 200_000_000) percentMult = 5;
                else if (number <= 1_000_000_000) percentMult = 1;
                #endregion
                int percent = (number / 100) * percentMult; // Параметр для отображения прогресса вычислений

                #endregion

                WriteLine("Выберите режим работы\n" +
                    "1 - Вывод кол-ва групп\n" +
                    "2 - Подсчет и запись заполненных групп в файл");
                Write("Выбор: ");
                byte mode = (byte)NumInput(1,2); // Выбор режима работы (Ну тут и так понятно)

                dtime = DateTime.Now; // Запоминаем нынешнее время системы

                for (int lNum = 1; lNum <= number; lNum *= 2) // Подсчет кол-ва групп
                {
                    groupsNum++;
                }

                WriteLine("\n------------------------------\\/\n"); // Рамочки (Почему бы и нет)
                #region Режим вывода кол-ва групп

                WriteLine($"Кол-во групп - {groupsNum}");

                #endregion
                WriteLine("\n----------------\n");

                #region Режим записи в файл

                if (mode == 2)
                {
                    // Открываем поток для записи чисел в файл
                    using (StreamWriter stwr = new StreamWriter($"{fileInfo.DirectoryName}\\{finFileName}.txt",
                       false, Encoding.ASCII, 65_536)) // При большем буфере программа работает немного быстрее (хоть и чуть-чуть)
                    {
                        byte pers = 0;
                        int progress = percent;
                        for (byte yus = 1; yus <= groupsNum; yus++) // Перебор групп
                        {
                            stwr.Write($"\n|-{yus}-|>>> "); // Начало группы

                            while (subNum < gFirst && subNum <= number) // Перебор чисел в группах
                            {
                                stwr.Write($"{subNum} "); // Записываем число
                                if (subNum == progress)
                                {
                                    pers += percentMult;
                                    WriteLine($"|-{pers}%-|");
                                    progress += percent;
                                }
                                subNum++; // Переходим к следующему
                            }

                            gFirst *= 2; // Переход к начальному числу следующей группы для ориентировки
                        }
                    }
                    WriteLine($"\nЗапись результата в файл {finFileName}.txt успешно завершена");
                    WriteLine("\n------------------------------/\\\n");
                }

                #endregion

                TimeSpan span = DateTime.Now.Subtract(dtime); // Отнимаем сохраненное время от текущего
                WriteLine($"Процесс занял {span.TotalMilliseconds} миллисек.\n"); // Выводим на экран сколько времени прошло

                #region Архивация готового файла

                if (mode == 2)
                {
                    Write("Хотите архивировать готовый файл?  1 - Да | 0 - Нет\n" +
                        "Выбор: ");
                    if (NumInput(0, 1) == 1)
                    {
                        WriteLine("\n------------------------------\\/\n");
                        // Ориентировачная оценка требуемого времени
                        WriteLine($"Пожалуйста подождите, это займет примерно {Math.Round(span.TotalSeconds * 1.23)} сек.\n");
                        dtime = DateTime.Now; // Запоминаем нынешнее время системы

                        // Поток чтения исходного файла
                        using (FileStream fist = new FileStream($"{fileInfo.DirectoryName}\\{finFileName}.txt", FileMode.Open))
                        {
                            // Поток записи архивированного файла
                            using (FileStream fist_2 = File.Create($"{fileInfo.DirectoryName}\\{finFileName}.zip"))
                            {
                                // Поток архивации файла
                                using (GZipStream gzips = new GZipStream(fist_2, CompressionMode.Compress))
                                {
                                    fist.CopyTo(gzips); // Копирование данных из одного потока в другой
                                    WriteLine($"Сжатие файла \"{finFileName}.txt\" завершено\n" +
                                        $"Исходный вес файла - {fist.Length}\n" +
                                        $"Конечный вес файла - {fist_2.Length}");
                                }
                            }
                        }
                        WriteLine("\n------------------------------/\\\n");
                        span = DateTime.Now.Subtract(dtime); // Отнимаем сохраненное время от текущего
                        WriteLine($"Процесс занял {span.TotalMilliseconds} миллисекунд\n"); // Выводим на экран сколько времени прошло
                    }
                }

                #endregion

                #region Повтор или выход

                WriteLine("Запустить заново? 1 - Повтор | 0 - Выход");
                Write("Выбор: ");
                if (NumInput(0, 1) == 0) break; // Завершение программы
                WriteLine();

                #endregion
            }
        }
    }
}
