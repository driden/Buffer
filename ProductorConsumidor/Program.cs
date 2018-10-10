using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProductorConsumidor
{
    class Program
    {
        static void Main(string[] args)
        {
            var todas = new List<Thread>();

            var buffer = new Buffer<string>(10);

            for (int i = 0; i < 10; i++)
            {
                var t = new Thread(() => buffer.Insertar($"Texto producido por {i}"));
                t.Name = $"productor{i}";  
                todas.Add(t);

                t.Start();
            }

            for (int i = 0; i < 5; i++)
            {
                Thread t = new Thread(() =>
                {
                    while (true)
                    {
                        string leer;
                        buffer.Extraer(out leer);
                        Console.WriteLine(leer); 

                        Thread.Sleep(1000);
                    }
                });
                t.Name = $"consumidor{i}";
                t.Start();
                todas.Add(t);
                
            }


            todas = todas.OrderBy(x => Guid.NewGuid()).ToList();

            // Esperar
            foreach (var thread in todas)
            {
                //if (thread.ThreadState != (ThreadState.Stopped | ThreadState.StopRequested))
                //    continue;
                thread.Join();
            }

            Console.WriteLine("key");
            Console.ReadLine();
        }
    }
}
