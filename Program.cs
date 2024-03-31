   
// TODO: declare a constant to represent the max size of the values
// and dates arrays. The arrays must be large enough to store
// values for an entire month.
int physicalSize = 31;
int logicalSize = 0;

double minValue = 0.0;
double maxValue = 100;
double[] values = new double[physicalSize];
string[] dates = new string[physicalSize];

bool goAgain = true;
  while (goAgain)
  {
    try
    {
      DisplayMainMenu();
      string mainMenuChoice = Prompt("\nEnter a Main Menu Choice: ").ToUpper();
      if (mainMenuChoice == "L")
        logicalSize = LoadFileValuesToMemory(dates, values);
      if (mainMenuChoice == "S")
        SaveMemoryValuesToFile(dates, values, logicalSize);
      if (mainMenuChoice == "D")
        DisplayMemoryValues(dates, values, logicalSize);
      if (mainMenuChoice == "A")
        logicalSize = AddMemoryValues(dates, values, logicalSize);
      if (mainMenuChoice == "E")
        EditMemoryValues(dates, values, logicalSize);
      if (mainMenuChoice == "Q")
      {
        goAgain = false;
        throw new Exception("Bye, hope to see you again.");
      }
      if (mainMenuChoice == "R")
      {
        while (true)
        {
          if (logicalSize == 0)
					  throw new Exception("No entries loaded. Please load a file into memory");
          DisplayAnalysisMenu();
          string analysisMenuChoice = Prompt("\nEnter an Analysis Menu Choice: ").ToUpper();
          if (analysisMenuChoice == "A")
            FindAverageOfValuesInMemory(values, logicalSize);
          if (analysisMenuChoice == "H")
            FindHighestValueInMemory(values, logicalSize);
          if (analysisMenuChoice == "L")
            FindLowestValueInMemory(values, logicalSize);
          if (analysisMenuChoice == "G")
            GraphValuesInMemory(dates, values, logicalSize);
          if (analysisMenuChoice == "R")
            throw new Exception("Returning to Main Menu");
        }
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine($"{ex.Message}");
    }
  }

void DisplayMainMenu()
{
	Console.WriteLine("\nMain Menu");
	Console.WriteLine("L) Load Values from File to Memory");
	Console.WriteLine("S) Save Values from Memory to File");
	Console.WriteLine("D) Display Values in Memory");
	Console.WriteLine("A) Add Value in Memory");
	Console.WriteLine("E) Edit Value in Memory");
	Console.WriteLine("R) Analysis Menu");
	Console.WriteLine("Q) Quit");
}

void DisplayAnalysisMenu()
{
	Console.WriteLine("\nAnalysis Menu");
	Console.WriteLine("A) Find Average of Values in Memory");
	Console.WriteLine("H) Find Highest Value in Memory");
	Console.WriteLine("L) Find Lowest Value in Memory");
	Console.WriteLine("G) Graph Values in Memory");
	Console.WriteLine("R) Return to Main Menu");
}

string Prompt(string prompt)
{
  bool inValidInput = true;
  string response = "";
  while (inValidInput)
  {
    try
    {
      Console.Write(prompt);
      response = Console.ReadLine().Trim();
      if(string.IsNullOrEmpty(response))
      {
        throw new Exception($"Empty Input: Please enter something.");
      }
      inValidInput = false;
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex.Message);
    }
  }
  return response;
}

string GetFileName()
{
	string fileName = "";
	do
	{
		fileName = Prompt("Enter file name including .csv or .txt: ");
	} while (string.IsNullOrWhiteSpace(fileName));
	return fileName;
}

int LoadFileValuesToMemory(string[] dates, double[] values)
{
	string fileName = GetFileName();
	int logicalSize = 0;
	string filePath = $"./data/{fileName}";
	if (!File.Exists(filePath))
		throw new Exception($"The file {fileName} does not exist.");
	string[] csvFileInput = File.ReadAllLines(filePath);
	for(int i = 0; i < csvFileInput.Length; i++)
	{
		Console.WriteLine($"lineIndex: {i}; line: {csvFileInput[i]}");
		string[] items = csvFileInput[i].Split(',');
		for(int j = 0; j < items.Length; j++)
		{
			Console.WriteLine($"itemIndex: {j}; item: {items[j]}");
		}
		if(i != 0)
		{
		dates[logicalSize] = items[0];
    values[logicalSize] = double.Parse(items[1]);
    logicalSize++;
		}
	}
  Console.WriteLine($"Load complete. {fileName} has {logicalSize} data entries");
	return logicalSize;
}

void DisplayMemoryValues(string[] dates, double[] values, int logicalSize)
{
	if(logicalSize == 0)
		throw new Exception($"No Entries loaded. Please load a file to memory or add a value in memory");
	Console.WriteLine($"\nCurrent Loaded Entries: {logicalSize}");
	Console.WriteLine($"   Date     Value");
	for (int i = 0; i < logicalSize; i++)
		Console.WriteLine($"{dates[i]}   {values[i]}");
}

double PromptDoubleBetweenMinMax(String prompt, double min, double max)
{
  bool inValidInput = true;
  double num = 0;
  while (inValidInput)
  {
    try
    {
      Console.Write($"{prompt} between {min:n2} and {max:n2}: ");
      num = double.Parse(Console.ReadLine());
      if (num < min || num > max)
        throw new Exception($"Invalid. Must be between {min} and {max}.");
      inValidInput = false;
    }
    catch (Exception ex)
    {
      Console.WriteLine($"{ex.Message}");
    }
  }
  return num;
}

string PromptDate(String prompt)
{
  bool inValidInput = true;
  DateTime date = DateTime.Today;
  Console.WriteLine(date);
  while (inValidInput)
  {
    try 
    {
      Console.Write(prompt);
      date = DateTime.Parse(Console.ReadLine());
      Console.WriteLine(date);
      inValidInput = false;
    }
    catch (Exception ex)
    {
      Console.WriteLine($"{ex.Message}");
    }
  }
  return date.ToString("MM-dd-yyyy");
}

double FindHighestValueInMemory(double[] values, int logicalSize)
{
    if (logicalSize == 0)
        throw new Exception("No entries loaded. Please load a file into memory.");

    double highestValue = values[0];

    for (int i = 1; i < logicalSize; i++)
    {
        if (values[i] > highestValue)
        {
            highestValue = values[i];
        }
    }

    Console.WriteLine($"The highest value in memory is: {highestValue}");
    return highestValue;
}

double FindLowestValueInMemory(double[] values, int logicalSize)
{
	if (logicalSize == 0)
        throw new Exception("No entries loaded. Please load a file into memory.");

    double lowestValue = values[0];

    for (int i = 1; i < logicalSize; i++)
    {
        if (values[i] < lowestValue)
        {
            lowestValue = values[i];
        }
    }

    Console.WriteLine($"The lowest value in memory is: {lowestValue}");
    return lowestValue;
}

void FindAverageOfValuesInMemory(double[] values, int logicalSize)
{
	if (logicalSize == 0)
        throw new Exception("No entries loaded. Please load a file into memory.");

    double valuesSum = values[0];

    for (int i = 1; i < logicalSize; i++)
    {
        valuesSum = values[i] + valuesSum;
    }
    
    double valuesAverage = 0;
    valuesAverage = valuesSum / logicalSize;

    Console.WriteLine($"The average of the values in memory is: {valuesAverage:n5}");
}

void SaveMemoryValuesToFile(string[] dates, double[] values, int logicalSize)
{
    string fileName = Prompt("Enter a file name including .csv or .txt: ");
    string filePath = $"./data/{fileName}";

    if (logicalSize == 0)
        throw new Exception("No entries loaded. Please load a file into memory.");
    if (logicalSize > 1)
        Array.Sort(dates, values, 0, logicalSize);

    string[] csvlines = new string[logicalSize + 1];
    csvlines[0] = "dates,values";
    for (int i = 1; i <= logicalSize; i++)
    {
        csvlines[i] = $"{dates[i-1]},{values[i-1].ToString()}";
    }

    File.WriteAllLines(filePath, csvlines);
    Console.WriteLine($"Save complete. {fileName} has {logicalSize} entries.");

    // StreamWriter writer;
    // writer = new StreamWriter(filePath);
    // writer.WriteLine("dates,values");
    // for (int i = 0; i < logicalSize; i++)
    // {
    //     writer.WriteLine($"{dates[i]},{values[i]}");
    // }
    // writer.Close();
}

int AddMemoryValues(string[] dates, double[] values, int logicalSize)
{
	double value = 0.0;
  string dateString = "";

  dateString = PromptDate("Enter date format mm-dd-yyyy (eg 11-23-2023): ");
  bool found = false;
  for (int i = 0; i < logicalSize; i++)
    if (dates[i].Equals(dateString))
      found = true;
    if (found == true)
      throw new Exception($"{dateString} is already in memory. Edit entry instead.");
    value = PromptDoubleBetweenMinMax($"Enter a double value", minValue, maxValue);
    dates[logicalSize] = dateString;
    values[logicalSize] = value;
    logicalSize++;
    return logicalSize;
}

void EditMemoryValues(string[] dates, double[] values, int logicalSize)
{
	double value = 0.0;
  string dateString = "";
  int foundIndex = 0;

  if (logicalSize == 0)
    throw new Exception($"No Entries loaded. Please load a file to memory or add a value in memory");
  dateString = PromptDate("Enter date format mm-dd-yyyy (eg 11-23-2023): ");
  bool found = false;
  for (int i = 0; i < logicalSize; i++)
    if (dates[i].Equals(dateString))
    {
      found = true;
      foundIndex = i;
    }
  if (found == false)
    throw new Exception($"{dateString} is not in memory. Either load a file with the entry you wish to edit or add entry instead.");
  value = PromptDoubleBetweenMinMax($"Enter a double value", minValue, maxValue);
  values[foundIndex] = value;
}

void GraphValuesInMemory(string[] dates, double[] values, int logicalSize)
{
  if (logicalSize == 0)
    {
        throw new Exception("No entries loaded. Please load a file into memory.");
    }

    Console.WriteLine("\nGraph Values in Memory:");


    for (int dollar = 90; dollar >= 0; dollar -= 10)
    {
        Console.Write($"${dollar,-4}|");

        for (int day = 1; day <= 31; day++)
        {
            bool valueFound = false;
            double valueToGraph = 0.0;

            for (int i = 0; i < logicalSize; i++)
            {
                if (DateTime.TryParse(dates[i], out DateTime date))
                {
                    if (date.Day == day && (values[i] >= dollar && values[i] < dollar + 10))
                    {
                        valueFound = true;
                        valueToGraph = values[i];
                        break;
                    }
                }
            }


            // All the if else stuff is just for formating if day < 10 the spacing is different
            if (valueFound)
            {
                Console.Write($"  ${valueToGraph}");
            }
            else
            {
                Console.Write("     ");
            }
        }
        Console.WriteLine();
    }


    Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------------------------------------------------");
    Console.WriteLine("Days |    1    2    3    4    5    6    7    8    9   10   11   12   13   14   15   16   17   18   19   20   21   22   23   24   25   26   27   28   29   30   31");
}