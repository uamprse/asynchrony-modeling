using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Lab7;

class Program
{
    static void Main()
    {
        int numEmitter = 1;
        int numHandlers = 3;
        int frequency = 100;
        int randomness = 10;
        int probabilityDelay = 50;
        int predefinedEventsCount = 2; 

        EventEmitter emitter = new EventEmitter();
        List<EventHandler> handlers = new List<EventHandler>();

        Task[] handlerTasks = new Task[numHandlers];


        for (int i = 0; i < numHandlers; i++)
        {
            EventHandler handler = new EventHandler(i);
            handlers.Add(handler);
            var handlerTask = handler.StartHandling(probabilityDelay);
            handlerTasks[i] = handlerTask;
        }

        emitter.EventEmitted += (sender, e) =>
        {
            for (int i = 0; i < numEmitter; i++)
            {
                Console.WriteLine($"{DateTime.Now:HH:mm:ss} - преподаватель {i} сгенерировал задание {e.Name}");
                File.AppendAllText("log.txt", $"{DateTime.Now:HH:mm:ss} - преподаватель {i} сгенерировал задание {e.Name}\n");
                foreach (var handler in handlers)
                {
                    handler.EnqueueEvent(e);
                }
            }

        };


        emitter.GenerateEvents(predefinedEventsCount);
        var emitterTask = emitter.StartEmitting(frequency, randomness);
        Thread.Sleep(5000);
        emitter.StopEmitting();
        emitterTask.Wait();

        foreach (var handler in handlers)
        {
            handler.StopHandling();
            handler.ReportStatistics();
            handler.ReportQueueLength();
            handler.ReportComplexMetrics();
        }


        Task.WaitAll(handlerTasks);

        Console.ReadLine();
    }

    //класс обертка для очереди для работы с локами +++
    //переделать на возвращаемое значение таск  ++
    //убрать в цикле вайл бесконечный цикл +++

}
