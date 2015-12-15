//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************
using System;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using Windows.Media.Capture;
using Windows.Media.Devices;
using Windows.Storage;

namespace AudioTest
{
    public partial class MainForm : Form
    {
        #region Fields

        private static ResourceManager LocRM;
        private MediaCapture mediaCapture;
        private string soundFile = "AudioTest.wav";
        private string soundFilePath;
        private Windows.Media.MediaProperties.MediaEncodingProfile encodingProfile;
        private bool recording; //flag: if currently recording
        private bool recorded; //flag: if recording complete

        #endregion // Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the MainForm form class.
        /// </summary>
        public MainForm()
        {
            LocRM = new ResourceManager("win81FactoryTest.AppResources.Res", typeof(win81FactoryTest.TestForm).Assembly);

            InitializeComponent();
            InitializePlayingAudio();
            InitializeRecording();
            SetString();
            this.FormBorderStyle = FormBorderStyle.None;//Full screen and no title
            this.WindowState = FormWindowState.Maximized;
            PassBtn.Visible = false;
            ResetBtn.Visible = false;

        }

        #endregion // Constructor

        /// <summary>
        /// Initialize Playback function. Set audio file path (soundFilePath) from application arguments. 
        /// </summary>
        private void InitializePlayingAudio()
        {
            if (Program.ProgramArgs == null)
            {

                PlayAudioLbl.Text = LocRM.GetString("AudioFileNotFound");
                InstructionLbl.Text = LocRM.GetString("AudioSpeakerRecord");

                PlayBtn.Visible = false;
                NextBtn.Visible = false;
            }
            else if (Program.ProgramArgs.Count < 2)
            {
                PlayAudioLbl.Text = LocRM.GetString("AudioFileNotFound");
                InstructionLbl.Text = LocRM.GetString("AudioSpeakerRecord");

                PlayBtn.Visible = false;
                NextBtn.Visible = false;
            }
            else
            {
                soundFilePath = Program.ProgramArgs[1];

                PlayAudioLbl.Text = LocRM.GetString("AudioPlayAudio") + ":";
                InstructionLbl.Text = LocRM.GetString("AudioSpeakerPlay");

                PlayAudioLbl.Enabled = true;
                PlayBtn.Enabled = true;
                NextBtn.Enabled = true;
                RecordBtn.Enabled = false;
                RecordingLbl.Enabled = false;
            }
        }

        /// <summary>
        /// Initialize Recording function. 
        /// </summary>
        private async void InitializeRecording()
        {
            mediaCapture = new Windows.Media.Capture.MediaCapture();
            
            mediaCapture.Failed += MediaCaptureFailed;

            var settings = new MediaCaptureInitializationSettings();
            settings.StreamingCaptureMode = StreamingCaptureMode.Audio;

            settings.AudioDeviceId = MediaDevice.GetDefaultAudioCaptureId(AudioDeviceRole.Default);

            await mediaCapture.InitializeAsync(settings);

            encodingProfile = Windows.Media.MediaProperties.MediaEncodingProfile.CreateWav(
                Windows.Media.MediaProperties.AudioEncodingQuality.Auto);

            recorded = false; //no audio recorded yet
            recording = false; //recording not started
        }

        /// <summary>
        /// Start recording. Set audio file path (soundFilePath) to Windows.Storage.KnownFolders.MusicLibrary
        /// </summary>
        private async void StartCapture()
        {
            recording = true;
            StorageFile storageFile = await Windows.Storage.KnownFolders.MusicLibrary.CreateFileAsync(
                              soundFile, Windows.Storage.CreationCollisionOption.GenerateUniqueName);
            soundFilePath = storageFile.Path;
            await mediaCapture.StartRecordToStorageFileAsync(encodingProfile, storageFile);
        }

        /// <summary>
        /// Stop recording
        /// </summary>
        private async void StopCapture()
        {
            recording = false;
            await mediaCapture.StopRecordAsync();
            recorded = true;
        }

        /// <summary>
        /// Playback audio file based on audio file path (soundFilePath)
        /// </summary>
        private void PlayBack()
        {
            WMPLib.IWMPMediaCollection medialist;
            WMPLib.IWMPMedia mediaSRC;
            try
            {
                medialist = Player1.mediaCollection;
                mediaSRC = medialist.add(soundFilePath);
                Player1.currentPlaylist.clear();
                Player1.currentPlaylist.appendItem(mediaSRC);
                Player1.Ctlcontrols.play();
            }
            catch (Exception ex)
            {
                DllLog.Log.LogError("Media play Failed: " + ex.Message);
                PlayBtn.Text = "Error";
                PlayBtn.BackColor = Color.Crimson;
            }
        }

        /// <summary>
        /// Control.Click Event handler. Where control is the Play button.
        /// This will play either the audio file from application argument (input audio file) or the recorded audio file
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void PlayBtn_Click(object sender, EventArgs e)
        {
            PlayBack();
        }

        /// <summary>
        /// Control.Click Event handler. Where control is the Record button.
        /// If state is "NOT RECORDING" & "NOT RECORDED" then application can start capture recording
        /// If state is "NOT RECORDING" & "COMPLETE RECORDED" then application can playback recorded file
        /// If state is "RECORDING" then application stops recording
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void RecordBtn_Click(object sender, EventArgs e)
        {
            if (!recording)
            {
                if (!recorded)
                {
                    this.BackColor = Color.Gray;
                    RecordBtn.Text = LocRM.GetString("AudioStop"); ;
                    StartCapture();
                }
                else
                {
                        PlayBack();
                        PassBtn.Visible = true;
                        ResetBtn.Visible = true;
                }
            }
            else
            {
                StopCapture();
                this.BackColor = Color.FromArgb(64, 64, 64);
                RecordBtn.Text = LocRM.GetString("AudioPlay"); ;
            }
        }


        /// <summary>
        /// Control.Click Event handler. Where control is the Reset/Retry button.
        /// Resets the test
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void ResetBtn_Click(object sender, EventArgs e)
        {
            recorded = false; //no audio recorded yet
            recording = false; //recording not started
            SetString();
            InitializePlayingAudio();
        }


        /// <summary>
        /// Control.Click Event handler. Where control is the Pass button.
        /// Pass the test and close app
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void PassBtn_Click(object sender, EventArgs e)
        {
            Program.ExitApplication(0);
        }


        /// <summary>
        /// Control.Click Event handler. Where control is the Fail button.
        /// Fail the test and close app
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void FailBtn_Click(object sender, EventArgs e)
        {
            Program.ExitApplication(255);
        }


        /// <summary>
        /// Control.Click Event handler. Where control is the Next button.
        /// Continue to record part of the test. 
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void NextBtn_Click(object sender, EventArgs e)
        {
            PlayBtn.Enabled = false;
            PlayAudioLbl.Enabled = false;
            NextBtn.Enabled = false;
            RecordBtn.Enabled = true;
            RecordingLbl.Enabled = true;
            InstructionLbl.Text = LocRM.GetString("AudioSpeakerRecord");

        }

        /// <summary>
        /// Windows.Media.Capture.MediaCapture fail Event handler.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void MediaCaptureFailed(MediaCapture sender, MediaCaptureFailedEventArgs e)
        {
            DllLog.Log.LogError("Media Capture Failed: " + e.Message);

            throw new InvalidOperationException(e.Message);
        }

        /// <summary>
        /// Initializes strings from SystemFunctionTestClass resources
        /// </summary>
        private void SetString()
        {
            Title.Text = LocRM.GetString("Audio") + LocRM.GetString("Test");
            PassBtn.Text = LocRM.GetString("Pass");
            FailBtn.Text = LocRM.GetString("Fail");
            ResetBtn.Text = LocRM.GetString("Retry");
            RecordingLbl.Text = LocRM.GetString("AudioRecording") + ":";
            RecordingNowLbl.Text = LocRM.GetString("AudioRecordingNow");
            RecordBtn.Text = LocRM.GetString("AudioStart");
            PlayBtn.Text = LocRM.GetString("AudioPlay");
            NextBtn.Text = LocRM.GetString("AudioNext");

        }

    }
}
