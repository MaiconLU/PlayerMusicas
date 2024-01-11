using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;

namespace form1
{
    public partial class Form4 : Form
    {
        private IWavePlayer waveOutDevice;
        private AudioFileReader audioFileReader;
        private Timer timer;
        public Form4()
        {
            InitializeComponent();
            waveOutDevice = new WaveOut();
            timer = new Timer();
            timer.Interval = 1000; // Atualizar a cada segundo
            timer.Tick += Timer_Tick;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "Metaphorsis";
            pictureBox1.Visible = true;
            // Obtém o caminho para a área de trabalho do usuário
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            // Combina o caminho da área de trabalho com o nome do arquivo de áudio
            string musicPath = System.IO.Path.Combine(desktopPath, "be happy.wav"); // Substitua pelo caminho do seu arquivo de áudio
            // Verifica se o arquivo de áudio existe
            if (System.IO.File.Exists(musicPath))
            {
                if (audioFileReader != null)
                {
                    audioFileReader.Dispose();
                }
                //Inicializa um novo leitor de arquivo de áudio com o caminho do arquivo de áudio
                audioFileReader = new AudioFileReader(musicPath);
                // Inicializa o dispositivo de saída de áudio e reproduz o arquivo de áudio
                waveOutDevice.Init(audioFileReader);
                waveOutDevice.Play();
                timer.Start();
            }
            else
            {
                MessageBox.Show("Arquivo de música não encontrado na área de trabalho.");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //para a reprodução
            waveOutDevice?.Stop();


        }

        private void button5_Click(object sender, EventArgs e)
        {
            Retroceder(TimeSpan.FromSeconds(10));
        }
        private void Retroceder(TimeSpan timeSpan)
        {
            if(audioFileReader != null)
            {
                // Calcula a nova posição
                long novaPosicao = Math.Max(0, audioFileReader.Position - (long)(audioFileReader.WaveFormat.SampleRate * timeSpan.TotalSeconds));

                // Define a nova posição
                audioFileReader.Position = novaPosicao;
            }
        }
        private void Avancar(TimeSpan timeSpan)
        {
            if (audioFileReader != null)
            {
                // Calcula a nova posição
                long novaPosicao = Math.Min(audioFileReader.Length, audioFileReader.Position + (long)(audioFileReader.WaveFormat.SampleRate * timeSpan.TotalSeconds));

                // Define a nova posição
                audioFileReader.Position = novaPosicao;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Avancar(TimeSpan.FromSeconds(20));
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (audioFileReader != null)
            {
                // Exibe o tempo decorrido no formato HH:mm:ss
                label3.Text = audioFileReader.CurrentTime.ToString(@"mm\:ss\.fff");
            }
        }
        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            waveOutDevice?.Stop();
            waveOutDevice?.Dispose();
            audioFileReader?.Dispose();
            timer.Stop(); // Para o cronômetro
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.meta3;
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.meta2;
        }
    }
}
