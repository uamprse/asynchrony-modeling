using System;
using Lab7;

namespace Lab7
{
    public class EventEmitter
    {
        private int totalEventsGenerated = 0;

        public int TotalEventsGenerated => totalEventsGenerated;

        private bool isRunning = true;
        public event EventHandler<Event> EventEmitted;

        private int eventID = 0;

        public async Task StartEmitting(int frequencyMilliseconds, int randomness)
        {
            Random random = new Random();
            while (isRunning)
            {
                await Task.Delay(frequencyMilliseconds + random.Next(-randomness, randomness));
                eventID++;
                EventEmitted?.Invoke(this, new Event { ID = eventID, Name = $"task {eventID}" });
            }
        }

        public void GenerateEvents(int numEvents)
        {
            for (int i = 0; i < numEvents; i++)
            {
                eventID++;
                totalEventsGenerated++;
                EventEmitted?.Invoke(this, new Event { ID = eventID, Name = $"task {eventID}" });
            }
        }

        public void StopEmitting()
        {
            isRunning = false;
        }

        public void ReportStatistics(int numEmitter)
        {
            Console.WriteLine($"Общее количество сгенерированных заданий: {totalEventsGenerated * numEmitter}");
        }

    }
}

