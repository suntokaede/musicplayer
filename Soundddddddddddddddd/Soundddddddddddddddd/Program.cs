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
        private List<string> musicList;
        private bool exitFlag = false;
        private static readonly string[] ALLOWED_EXTENSION_TYPE = { ".wav", ".mp3", ".mp4", ".m4v" };

        public NAudioControl()
        {
            this.waveOut = new WaveOut();
        }

        /// <summary>
        /// ミュージックフォルダから音楽ファイルの一覧を取得します。
        /// その後、コマンドラインに表示しリストに追加します。
        /// </summary>
        private void showAndMakeMusicList()
        {
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            string[] fileList = Directory.GetFiles(directory);
            musicList = fileList
                .Where(path => ALLOWED_EXTENSION_TYPE.Contains(Path.GetExtension(path)))
                .ToList();
            for(int i = 0,n = musicList.Count;i < n;i++){
                string fileName = Path.GetFileName(musicList[i]);
                Console.WriteLine("{0}: {1}", i, fileName);
            }   
        }
        
        /// <summary>
        /// ユーザーが選択したファイルのパスを返す
        /// </summary>
        /// <returns>選択されたファイルのパス</returns>
        private string getPath()
        {
            while (true)
            {
                string n = Console.ReadLine();
                int num;
                if (int.TryParse(n, out num))
                {
                    if (num >= 0 && num < musicList.Count) return musicList[num];
                }
                Console.WriteLine("Invalid Input");
            }
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
                else
                //その他
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
        /// メイン関数
        /// </summary>
        public void main()
        {
            while (true)
            {
                showAndMakeMusicList();
                string path = getPath();
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
