using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProductorConsumidor
{
    public class Buffer<T>
    {
        private T[] datos = null;
        private readonly int N = -1;
        private int cant, prox_ins, prox_elim = 0;

        // Condiciones
        private SemaphoreSlim no_lleno; //= new Mutex(false, "no_lleno"); // Espero que no este lleno
        private SemaphoreSlim no_vacio; // = new Mutex(false, "no_vacio"); // Espero que no este vacio

        public Buffer(int bufferSize)
        {
            datos = new T[bufferSize];
            N = bufferSize;
            no_lleno = new SemaphoreSlim(1); // Espero que no este lleno
            no_vacio = new SemaphoreSlim(1); // Espero que no este vacio
        }

        public void Insertar(T x)
        {
            if (cant == N)
                no_lleno.Wait();

            datos[prox_ins] = x;
            prox_ins = (prox_ins + 1) % N;
            cant++;
            Console.WriteLine("add: {0}", Thread.CurrentThread.Name);

            no_vacio.Release();

        }

        public void Extraer(out T x)
        {
            if (cant < 1)
                no_vacio.Wait();

            x = datos[prox_elim];
            prox_elim = (prox_elim + 1) % N;
            cant--;
            Console.WriteLine("cons: {0}", Thread.CurrentThread.Name);

            no_lleno.Release();
        }
    }
}
