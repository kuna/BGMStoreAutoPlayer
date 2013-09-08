using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.IO;

using System.Threading;

// 1. 쓰레딩화
// 2. 자동 플레이 가능하게 하기
// 3. save 버튼 만들기 (or auto save)

namespace BGMStoreAutoPlayer
{
    public partial class Form1 : Form
    {
        private BGMStore bgm;
        private Player p;
        private bool updateSong = true;
        private bool autoplay = false;

        private int songlen;

        public Form1()
        {
            InitializeComponent();
            bgm = new BGMStore();
            p = new Player();
            timer1.Interval = 500;
            timer1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            refresh();
        }

        private void refresh()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            label1.Text = "새 곡을 로딩 중입니다...";
            //Thread t = new Thread(new ThreadStart(loadSong));
            //t.Start();
            loadSong();
            UIControl();
        }

        private delegate void myDelegate();
        private void UIControl()
        {
            button1.Enabled = true;
            button2.Enabled = true;
            this.Text = bgm.title;
            label1.Text = bgm.title;
            // temp
            textBox1.Text = bgm.musicurl + "\r\n" + bgm.filename + "\r\n" + bgm.title;
        }

        private void loadSong()
        {
            updateSong = false;
            p.Close();

            bgm.parse();

            bgm.download();

            p.Play("tempfile");
            songlen = trackBar1.Maximum = p.GetSongLength();

            this.Invoke(new myDelegate(UIControl));
            updateSong = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.IO.File.Copy("tempfile", bgm.filename);
            label1.Text = bgm.filename + " 으로 저장 되었습니다.";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            trackBar1.Value = p.GetCurentMilisecond();
            if (updateSong && autoplay && !p.IsPlaying())
            {
                refresh();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (autoplay == true)
            {
                button1.Text = "Play";
                p.Stop();
                autoplay = false;
            }
            else
            {
                button1.Text = "Stop";
                autoplay = true;
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            p.SetPosition(trackBar1.Value);
        }
    }
}
