﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GoogleMusicAPI;
using System.Net;
using System.Threading.Tasks;

namespace GoogleMusicTest
{
    public partial class GoogleTest : Form
    {
        API api = new API();
        public GoogleTest()
        {
            InitializeComponent();

            api.OnLoginComplete += OnGMLoginComplete;
            api.OnGetAllSongsComplete += GetAllSongsDone;
            api.OnCreatePlaylistComplete += api_OnCreatePlaylistComplete;
            api.OnGetPlaylistsComplete += new API._GetPlaylists(api_OnGetPlaylistsComplete);
        }

        void api_OnGetPlaylistsComplete(GoogleMusicPlaylists pls)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                foreach (GoogleMusicPlaylist pl in pls.UserPlaylists)
                {
                    lbPlaylists.Items.Add(pl.Title);
                }

                foreach (GoogleMusicPlaylist pl in pls.InstantMixes)
                {
                    lbPlaylists.Items.Add(pl.Title);
                }
            }));
        }

        void OnGMLoginComplete(object s, EventArgs e)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                this.Text += " -> Logged in";
            }));
        }

        void GetAllSongsDone(List<GoogleMusicSong> songs)
        {
            int num = 1;
            foreach (GoogleMusicSong song in songs)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = (num++).ToString();
                lvi.SubItems.Add(song.Title);
                lvi.SubItems.Add(song.Artist);
                lvi.SubItems.Add(song.Album);

                this.Invoke(new MethodInvoker( delegate {
                    lvSongs.Items.Add(lvi);
                }));
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            api.Login(tbEmail.Text, tbPass.Text);
        }

        void api_OnCreatePlaylistComplete(AddPlaylistResp resp)
        {
            if (resp.Success)
                MessageBox.Show("Created pl");
        }

        private void btnCreatePl_Click(object sender, EventArgs e)
        {
            api.AddPlaylist("Testing");
        }

        private void btnFetchSongs_Click(object sender, EventArgs e)
        {
            api.GetAllSongs(String.Empty);
        }

        private void btnGetPlaylists_Click(object sender, EventArgs e)
        {
            api.GetPlaylists();
        }
    }
}
