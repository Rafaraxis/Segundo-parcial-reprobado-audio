using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Efectos
{
    class Delay : ISampleProvider
    {
        private ISampleProvider fuente;

        public int offsetTiempoMS;

        List<float> muestras = new List<float>();

        public Delay(ISampleProvider fuente)
        {
            this.fuente = fuente;
            offsetTiempoMS = 600;
            //50ms - 5000ms
            Factordelay = 0.5f;

        }

        public WaveFormat WaveFormat
        {
            get
            {
                return fuente.WaveFormat;
            }
        }

        private float factordelay;
        public float Factordelay
        {
            get
            {
                return factordelay;
            }
            set
            {
                if (value > 1)
                    factordelay = 1;
                else if (value < 0)
                    factordelay = 0;
                else
                    factordelay = value;
            }
        }


        public Delay(ISampleProvider fuente, float factordelay)
        {
            this.fuente = fuente;
            Factordelay = factordelay;
        }


        //Offset es el numero de muestras leídas hasta ahorita
        public int Read(float[] buffer, int offset, int count)
        {
            //Calculo de tiempos
            var read = fuente.Read(buffer, offset, count);
            float tiempoTranscurrido =
               (float) muestras.Count / (float)fuente.WaveFormat.SampleRate;
            int muestrasTranscurridas = muestras.Count;
            float tiempoTranscurridoMS = tiempoTranscurrido * 1000;
            int numMuestrasOffsetTiempo = (int)
                (((float)offsetTiempoMS / 1000.0f) * (float)fuente.WaveFormat.SampleRate);

            //Añadir muestras a nuestro buffer
            for (int i = 0; i < read; i++)
            {
                muestras.Add(buffer[i]);
            }


            //Modificar muestras

            if ( tiempoTranscurridoMS > offsetTiempoMS)
            {
                for (int i = 0; i < read; i++)
                {
                    buffer[i] +=
                        muestras[muestrasTranscurridas+
                        i-numMuestrasOffsetTiempo];
                }
            }

            
            return read;
        }
    }
}
