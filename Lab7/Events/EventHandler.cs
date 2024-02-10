using System;

namespace Lab7
{
    public class EventHandler
    {
        public int ID { get; private set; }
        private Queue<Event> eventQueue;

        private int totalEventsProcessed = 0;
        private object _lockObj = new();

        private bool isRunning = true;

        public int TotalEventsProcessed => totalEventsProcessed;

        private List<long> taskCompletionTimes = new List<long>();

        public EventHandler(int id)
        {
            ID = id;
            eventQueue = new Queue<Event>();
        }

        public async Task HandleEvent(Event e, int processingTime)
        {
            var startTime = DateTime.Now;
            
            await Task.Delay(processingTime);
            totalEventsProcessed++;
            Console.WriteLine($"{DateTime.Now:HH:mm:ss} - студент {ID} выполнил задание {e.Name}");
            File.AppendAllText("log.txt", $"{DateTime.Now:HH:mm:ss} - студент {ID} выполнил задание {e.Name}\n");

            MeasureTaskCompletionTime(startTime); 
      
        }


        public void ReportStatistics()
        {
            Console.WriteLine($"Общее количество выполненных заданий студентом {ID}: {totalEventsProcessed}");
        }

        public void ReportQueueLength()
        {
            Console.WriteLine($"Длина очереди студента {ID}: {eventQueue.Count}");
        }


        public void EnqueueEvent(Event e)
        {
            lock (_lockObj)
            {
                eventQueue.Enqueue(e);
            }
        }

        public async Task StartHandling(int probabilityDelay)
        {
            Random random = new Random();
            while (isRunning) {
                if (WrapperQueue.CountQueue(eventQueue) > 0)
                {
                    Event e = WrapperQueue.DequeueQueue(eventQueue);
                    await HandleEvent(e, random.Next(100));
                    await Task.Delay(random.Next(probabilityDelay));
                }
                else
                {
                    await Task.Delay(100);
                }   
            }
        }

        private void MeasureTaskCompletionTime(DateTime startTime)
        {
            long completionTime = (long)(DateTime.Now - startTime).TotalMilliseconds;
            taskCompletionTimes.Add(completionTime);
        }

        public void ReportComplexMetrics()
        {
            if (taskCompletionTimes.Count > 0)
            {
                long averageCompletionTime = (long)taskCompletionTimes.Average();
                Console.WriteLine($"Среднее время выполнения заданий студентом {ID}: {averageCompletionTime} мс");

                long maxCompletionTime = taskCompletionTimes.Max();
                Console.WriteLine($"Максимальное время выполнения заданий студентом {ID}: {maxCompletionTime} мс");
            }
        }

        public void StopHandling()
        {
            isRunning = false;
        }
    }
}

