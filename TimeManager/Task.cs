/*
 *  -- Project --
 *  Project: TimeManager  
 *  File: Task.cs
 *  Date: 11-08-2023 (DD-MM-YYYY)
 *  
 *  -- Credit --
 *  Name: Callum D Stevenson
 *  Position: Creator
 */

namespace TimeManager
{
    // Task class to store the text, selected and complete flag
    internal class Task
    {
        // _selceted and _complete flag have default values of false
        public Task(string _taskName, bool _selected = false, bool _complete = false)
        {
            // Initalise all the member variables with values
            m_TaskName = _taskName;
            m_Selected = _selected;
            m_Complete = _complete;
        }

        // Member variables
        public string m_TaskName;
        public bool m_Selected;
        public bool m_Complete;
    }
}
