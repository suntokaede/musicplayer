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
    class NAudioControl : IDisposable
    {
        private WaveOut waveOut;
        private bool exitFlag = false;

        public NAudioControl()
        {
            this.waveOut = new WaveOut();
        }

        /// <summary>
        /// コントロール(音量、終了、再生、停止、一時停止、再開）
        /// </summary>
        private void control()
        {
            while (true)
            {
                string i = Console.ReadLine();
                //音量
                if (i.IndexOf("vol") >= 0)
                {
                    string[] volStr = i.Split(' ');
                    float vol;
                    foreach (string s in volStr)
                    {
                        if (float.TryParse(s, out vol))
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
                    }
                }
                else//その他
                {
                    switch (i)
                    {
                        case "exit":
                            exitFlag = true;
                            return;
                        case "select":
                            return;
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
            }
        }

        /// <summary>
        /// ミュージックフォルダから音楽ファイルの一覧を取得し、ユーザーが選択したファイルのパスを返す
        /// </summary>
        /// <returns>選択されたファイルのパス</returns>
        private string select()
        {
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            string[] fileList = Directory.GetFiles(directory);
            List<string> musicList = new List<string>();
            for (int n = 0, l = fileList.Length; n < l; n++)
            {
                string fileName = Path.GetFileName(fileList[n]);
                string ext = Path.GetExtension(fileName);
                if (ext == ".wav"|| ext == ".mp3"|| ext == ".mp4"|| ext == ".m4v") {
                    Console.WriteLine(string.Format("{0}: {1}", musicList.Count, fileName));
                    musicList.Add(fileList[n]);
                }
            }
            while (true)
            {
                string n = Console.ReadLine();
                int num;
                if (int.TryParse(n, out num))
                {
                    if(num >= 0 && num < musicList.Count) return musicList[num];
                }
                Console.WriteLine("Invalid Input");
            }
        }

        /// <summary>
        /// メイン関数
        /// </summary>
        public void main()
        {
            while (true)
            {
                string path = select();
                using (var reader = new AudioFileReader(path))
                {
                    waveOut.Init(reader);
                    waveOut.Play();
                    control();
                }
                if (exitFlag) { return; }
            }
        }

        public void Dispose()
        {
            waveOut.Dispose();
        }
    }

    class Program
    {
        static void Main()
        {
            using (var n = new NAudioControl())
            {
                n.main();
            }
        }
    }
}
