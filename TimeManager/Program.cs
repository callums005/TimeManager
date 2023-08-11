/*
 *  -- Project --
 *  Project: TimeManager  
 *  File: Program.cs
 *  Date: 11-08-2023 (DD-MM-YYYY)
 *  
 *  -- Credit --
 *  Name: Callum D Stevenson
 *  Position: Creator
 */


namespace TimeManager
{
    internal class Program
    {
        // List of tasks
        static List<Task> m_Tasks = new();
        // Index of task currently selected
        static int m_SelectedTask = 0;

        // Enum of all the possible screens
        enum Screen
        {
            Tasks, // Main screen
            NewTask, // Add task screen
            DeleteTask // Delete task screen
        }

        // Save tasks to file function
        static void Save()
        {
            try
            {
                List<string> saveList = new List<string>();

                // Loops through each task in the array to convert the task array into a string array
                foreach (Task t in m_Tasks)
                {
                    // Converts complete bool to a string 1 or 0 to be put in file
                    string comp = t.m_Complete ? "1" : "0";

                    // Add task to the list with the task text and weather its marked as complete, seperated by an arrow (→) 
                    saveList.Add($"{t.m_TaskName}→{comp}");
                }

                // Deletes the save file
                File.Delete(@".\tasks.txt");

                // Writes the converted task array to the file
                File.WriteAllLines(@".\tasks.txt", saveList.ToArray());
            }
            catch (Exception e)
            {}
        }

        // Event called when the application ends
        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            // Calls the save function before the program closes
            Save();
        }

        // Called when the application is started
        static void Main(string[] args)
        {
            // Subscribes the CurrentDomain_ProccessExit function to the ProcessExit event
            // This is to trigger the save function within the code before the application closes
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);

            // Screen to display this cycle
            Screen screen = Screen.Tasks;
            // Weather the input handler should be ignored this cycle
            bool ignoreInputHandleThisCycle = false;
            // Sets the console cursor invisible
            Console.CursorVisible = false;

            // Loads previously saved tasks
            try
            {
                // File path
                string file = @".\tasks.txt";

                if (File.Exists(file))
                {
                    string[] lines = File.ReadAllLines(file);

                    // Counts the lines in the file
                    int count = 0;

                    foreach (string ln in lines)
                    {
                        // Splits the line at the arrow (→)
                        string[] items = ln.Split("→");
                        bool comp = false;

                        // If the second value is 1 then the task was previously marked as complete
                        if (items[1] == "1")
                            comp = true;

                        // Creates the task object and adds to the array of tasks, if it is the first task then the menu system should select it
                        m_Tasks.Add(new Task(items[0], _selected: (count == 0), _complete: comp));
                        
                        count++;
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                // Prints a developer set error message if there is an issue loading the file
                // The application will function normally without the loading of the file
                Console.WriteLine("Error Loading save file. Your tasks have not been recovered. Press any key to continue...");
                // Forces the user to press a key so they are able to read the message
                Console.ReadKey();
            }

            // Application loop
            while (true)
            {
                // Clears the screen at the start of each cycle
                Console.Clear();

                // Creates the title at the start of each cycle
                Output.CreateTitle("Time Manager");

                // ---------------------- OUTPUT ---------------------- 
                // Switches between the different screen states
                switch (screen)
                {
                    case Screen.Tasks:
                        // Creates information text to tell the user what keys to use
                        Output.CreateTitle("↑ = Up Arrow | ↓ = Down Arrow | N = New Task | D = Delete Task | S = Save to File | X = Toggle Complete | H = Home", true);
                        Console.WriteLine();

                        // Prints out all the tasks
                        foreach (Task o in m_Tasks)
                        {
                            string emblem = "";

                            // Decides what emblem to put at the start of the task depending on the completetion state or if the item is selected (the arrow)
                            if (o.m_Selected)
                                emblem = "→";
                            else if (o.m_Complete)
                                emblem = "X";
                            else if (!o.m_Complete)
                                emblem = " ";

                            // Sets default colours for the tasks
                            ConsoleColor background = ConsoleColor.Black;
                            ConsoleColor foreground = ConsoleColor.White;

                            // Makes the selected text black
                            if (o.m_Selected)
                                foreground = ConsoleColor.Black;
                            
                            // If the task is complete it sets the background colour to green, if not complete set to red
                            if (o.m_Complete)
                                background  = ConsoleColor.Green;
                            else if (!o.m_Complete)
                                background = ConsoleColor.Red;

                            // Prints out the text to the screen
                            Output.CreateTitle($"{emblem} {o.m_TaskName}", true, background,foreground);
                            Console.ResetColor();
                        }

                        break;
                    case Screen.NewTask:
                        // Creates information text to tell the user what keys to use
                        Output.CreateTitle("Enter = Create Task", true);
                        Console.WriteLine();
                        
                        // Creates a title to explain the screen
                        Output.CreateTitle("Create Task", background: ConsoleColor.Black, foreground: ConsoleColor.Gray);
                        
                        // Sets the cursor visible for the duration of text entry
                        Console.CursorVisible = true;
                        string? task = Console.ReadLine(); // Text entry
                        Console.CursorVisible = false;
                        
                        // Creates the task
                        m_Tasks.Add(new Task(task));
                        // Resets the selected index back to the top, this prevents any issues when a task is modified
                        m_SelectedTask = 0;
                        screen = Screen.Tasks;
                        // Sets the flag to ignore input handling at the end of the cycle
                        ignoreInputHandleThisCycle = true;
                        break;
                    case Screen.DeleteTask:
                        // Creates information text to tell the user what keys to use
                        Output.CreateTitle("D = Delete", true);
                        Console.WriteLine();

                        // Checks to ensure if there are any tasks that can be deleted, if not then reset the screen
                        if (m_Tasks.Count < 1)
                        {
                            screen = Screen.Tasks;
                            ignoreInputHandleThisCycle = true;

                            break;
                        }

                        // Creates a title to explain the screen
                        Output.CreateTitle($"Are you sure you would like to delete {m_Tasks[m_SelectedTask].m_TaskName}?", background: ConsoleColor.Black, foreground: ConsoleColor.Gray);

                        // Gets a key press
                        ConsoleKeyInfo key = Console.ReadKey();

                        // If the D key is pressed it will delete a task
                        if (key.Key == ConsoleKey.D)
                        {
                            // Removes the task from the list
                            m_Tasks.RemoveAt(m_SelectedTask);
                        }

                        // Resets the selected task index to prevent any quirky things happening
                        m_SelectedTask = 0;
                        //Resets the screen
                        screen = Screen.Tasks;
                        // Sets the flag to ignore input handling at the end of the cycle
                        ignoreInputHandleThisCycle = true;

                        break;
                }

                // ---------------------- INPUT ----------------------

                // If the flag is not set to ignore input then it will pause the program and get keyboard input
                if (!ignoreInputHandleThisCycle)
                {
                    try
                    {
                        // Gets keyboard input
                        ConsoleKeyInfo key = Console.ReadKey();

                        switch (key.Key)
                        {
                            // If the up arrow key is pressed then it moves the index up by 1, or back to the bottom if the current index is 0
                            case ConsoleKey.UpArrow:
                                if (m_SelectedTask == 0)
                                    m_SelectedTask = m_Tasks.Count - 1;
                                else
                                    m_SelectedTask--;
                                break;
                            // If the down arrow is pressed then it moves the index down by 1, or to the top of the list if the current index is at the end of the list
                            case ConsoleKey.DownArrow:
                                if (m_SelectedTask < m_Tasks.Count - 1)
                                    m_SelectedTask++;
                                else if (m_SelectedTask == m_Tasks.Count - 1)
                                    m_SelectedTask = 0;
                                break;
                            // Menu
                            // If the N key is pressed it triggers the new task screen
                            case ConsoleKey.N:
                                screen = Screen.NewTask;
                                break;
                            // If the D key is pressed it triggers the delete task screen
                            case ConsoleKey.D:
                                screen = Screen.DeleteTask;
                                break;
                            // If the S key is pressed it triggers the save function
                            case ConsoleKey.S:
                                Save();
                                break;
                            // If the X key is pressed it toggles the complete flag on the task currenty selected
                            case ConsoleKey.X:
                                if (m_Tasks.Count > 0)
                                    m_Tasks[m_SelectedTask].m_Complete = !m_Tasks[m_SelectedTask].m_Complete;
                                break;
                            // If the H key is pressed it will reset the screen to the home screen
                            case ConsoleKey.H:
                                screen = Screen.Tasks;
                                break;
                            // If the home key is pressed it will reset the screen to the home screen
                            case ConsoleKey.Home:
                                screen = Screen.Tasks;
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }

                // Resets the ignore input flag
                ignoreInputHandleThisCycle = false;

                // Makes all the tasks selected flag to false
                foreach (Task option in m_Tasks)
                    option.m_Selected = false;

                // Sets the indexed tasks selected flag to true
                if (m_Tasks.Count > 0)
                    m_Tasks[m_SelectedTask].m_Selected = true;
            }
        }
    }
}