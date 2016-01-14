using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio;
using NAudio.Wave;

namespace Soundddddddddddddddd
{
    class NAudioControl
    {
        private WaveOut waveOut;
        private AudioFileReader reader;
        private float vol;

        public NAudioControl()
        { 
            play();            
        }

        /// <summary>
        /// 音量、終了、再生、停止、一時停止、再開
        /// </summary>
        private void control()
        {
            string i = Console.ReadLine();
            if (float.TryParse(i, out vol))
            {
                if (vol >= 0.0 && vol <= 1.0)
                {
                    waveOut.Volume = vol;
                }
                else
                {
                    Console.WriteLine("Invalid Input");
                }
            }
            else
            {
                switch (i)
                {
                    case "exit": return;
                    case "select":
                        waveOut.Stop();
                        waveOut.Dispose();
                        play();
                        break;
                    case "play":
                        waveOut.Play();
                        break;
                    case "stop":
                        waveOut.Stop();
                        break;
                    case "pause":
                        waveOut.Pause();
                        break;
                    case "resume":
                        waveOut.Resume();
                        break;
                    default:
                        Console.WriteLine("Invalid Input");
                        break;
                }
            }
            this.control();
        }

        /// <summary>
        /// カレントディレクトリ一覧を取得し、ファイルを選択した後選択したファイルのパスを返す
        /// </summary>
        /// <returns>選択されたファイルのパス</returns>
        private string select()
        {
            string cd = Directory.GetCurrentDirectory();
            string[] fileList = Directory.GetFiles(cd);
            string[] musicList = { };
            int count = 0;
            for (int n = 0, l = fileList.Length; n < l; n++)
            {
                string fileName = Path.GetFileName(fileList[n]);
                string ext = Path.GetExtension(fileName);
                if (ext == ".wav"|| ext == ".mp3"|| ext == ".mp4"|| ext == ".m4v") {
                    musicList = musicList.Concat(new string[] { fileName }).ToArray();
                    Console.WriteLine(string.Format("{0}: {1}", count, fileName));
                    count++;
                }
            }
            while (true)
            {
                string n = Console.ReadLine();
                int num;
                if (int.TryParse(n, out num))
                {
                    if(num >= 0 && num <= count) return musicList[num];
                }
                Console.WriteLine("Invalid Input");
            }
        }

        /// <summary>
        /// 初期化して再生
        /// </summary>
        private void play()
        {
            using (waveOut = new WaveOut())
            using (reader = new AudioFileReader(select()))
            {
                waveOut.Init(reader);
                waveOut.Play();
                control();
            }
        }
    }

    class Program
    {
        static void Main()
        {
            var n = new NAudioControl();
        }
    }
}
