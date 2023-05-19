using System;
using System.Diagnostics;
using TaskTwoThreeTree;

TwoThreeTree<int> two3tree = new TwoThreeTree<int>();
var countInputNumbers = 10000;
var rand = new Random();
var stopwatch = new Stopwatch();
var inputValuesPath = @"C:\Users\2695\Downloads\Telegram Desktop\TaskTwoThreeTree\TaskTwoThreeTree\inputValues.txt";
var timesAndIterationsOfAddingPath = @"C:\Users\2695\Downloads\Telegram Desktop\TaskTwoThreeTree\TaskTwoThreeTree\timesAndIterationsOfAdding.txt";
var timesAndIterationsOfFindingPath = @"C:\Users\2695\Downloads\Telegram Desktop\TaskTwoThreeTree\TaskTwoThreeTree\timesAndIterationsOfFinding.txt";
var timesAndIterationsOfRemovingPath = @"C:\Users\2695\Downloads\Telegram Desktop\TaskTwoThreeTree\TaskTwoThreeTree\timesAndIterationsOfRemoving.txt";
if (File.Exists(inputValuesPath)) File.Delete(inputValuesPath);
using (StreamWriter sw = File.CreateText(inputValuesPath))
{
    for (int i = 0; i < countInputNumbers; i++)
    {
        sw.Write(rand.Next(100) + " ");// Запись в файл
    }
}

string[] numbers;
using (StreamReader sr = File.OpenText(inputValuesPath))
{
    numbers = File.ReadAllText(inputValuesPath).Split(); // Чтение из файла в массив строк
}

List<int> elementsToFind = new List<int>();
var j = 0;
for (int i = 0; i < 100; i++)
{
    elementsToFind.Add(int.Parse(numbers[rand.Next(j, j+100)]));
    j += 100;
}

List<int> elementsToRemove = new List<int>();
j = 0;
for (int i = 0; i < 1000; i++)
{
    elementsToRemove.Add(int.Parse(numbers[rand.Next(j, j+10)]));
    j += 10;
}

if (File.Exists(timesAndIterationsOfAddingPath)) File.Delete(timesAndIterationsOfAddingPath);
using (StreamWriter sw = File.CreateText(timesAndIterationsOfAddingPath))
{
    for (int i = 0; i < numbers.Length - 1; i++)
    {
        stopwatch.Start();
        two3tree.Add(int.Parse(numbers[i]));
        stopwatch.Stop();
        sw.WriteLine("{0} element. Time: {1} Iterations count: {2}", i + 1, stopwatch.Elapsed,
            two3tree.IterationsCountOfAdding);
    }
}
stopwatch.Restart();
if (File.Exists(timesAndIterationsOfFindingPath)) File.Delete(timesAndIterationsOfFindingPath);
using (StreamWriter sw = File.CreateText(timesAndIterationsOfFindingPath))
{
    for (int i = 0; i < elementsToFind.Count; i++)
    {
        stopwatch.Start();
        two3tree.Find(elementsToFind[i]);
        stopwatch.Stop();
        sw.WriteLine("{0} element. Time: {1} Iterations count: {2}", i + 1, stopwatch.Elapsed,
            two3tree.IterationsCountOfFinding);
    }
}
stopwatch.Restart();
if (File.Exists(timesAndIterationsOfRemovingPath)) File.Delete(timesAndIterationsOfRemovingPath);
using (StreamWriter sw = File.CreateText(timesAndIterationsOfRemovingPath))
{
    for (int i = 0; i < elementsToRemove.Count; i++)
    {
        stopwatch.Start();
        two3tree.Remove(elementsToRemove[i]);
        stopwatch.Stop();
        sw.WriteLine("{0} element. Time: {1} Iterations count: {2}", i + 1, stopwatch.Elapsed,
            two3tree.IterationsCountOfRemoving);
    }
}