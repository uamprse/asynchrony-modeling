using System;

namespace Lab7;

public class WrapperQueue
{
    public object _lockObj = new();
    public static int CountQueue(Queue<Event> eventQueue)
    {
        lock (eventQueue)
        {
            return eventQueue.Count;
        }
    }

    public static Event DequeueQueue(Queue<Event> eventQueue)
    {
        lock (eventQueue)
        {
            return eventQueue.Dequeue();
        }
    }
}

