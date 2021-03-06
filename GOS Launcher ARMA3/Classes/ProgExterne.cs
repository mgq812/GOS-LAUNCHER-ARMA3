﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GOSLauncherA3
{
    class ProgExterne
    {
        static public void ValideProgExt()
        {
            ValideFRAPS();
            ValideTRACKIR();
            ValideTEAMSPEAKTeamForce();
        }

        // FRAPS

        static public void ValideFRAPS()
        {
            if (testFrapsExist() != "") GOSLauncherCore.fenetrePrincipale.checkBox11.Enabled = true;
        }
        static public string testFrapsExist()
        {
            string valeur = "";
            try
            {
                //ouvrir le dossier principal                   
                RegistryKey cle = Registry.CurrentUser;
                //ouvrir les sous répertoires du dossier principal, le second argument est un booleen permettant de modifier la clé    
                RegistryKey subcle = cle.OpenSubKey("Software\\Fraps3", false);
                valeur = subcle.GetValue("Directory").ToString();
                //fermer le registre   
                cle.Close();
            }
            catch
            {
            }

            return valeur;
        }
        static public void lancerFraps()
        {
            // lancement FRAPS
            if (GOSLauncherCore.fenetrePrincipale.checkBox11.Enabled && GOSLauncherCore.fenetrePrincipale.checkBox11.Checked)
            {
                if (Process.GetProcessesByName("fraps").Length == 0)
                {
                    Process frapsProcess = new Process();
                    frapsProcess.StartInfo.Verb = "runas";
                    frapsProcess.StartInfo.UseShellExecute = true;
                    frapsProcess.StartInfo.FileName = testFrapsExist() + @"\fraps.exe";
                    frapsProcess.Start();
                }
            }
        }

        // TRACK IR
        static public void ValideTRACKIR()
        {
            if (testTrackirExist() != "") GOSLauncherCore.fenetrePrincipale.checkBox12.Enabled = true;
        }
        static public void lancerTrackIR()
        {
            if (GOSLauncherCore.fenetrePrincipale.checkBox12.Enabled && GOSLauncherCore.fenetrePrincipale.checkBox12.Checked)
            {
                // lancement TRACKIR
                if (Process.GetProcessesByName("TrackIR5").Length == 0)
                {
                    try
                    {
                        Process trackirProcess = new Process();
                        trackirProcess.StartInfo.UseShellExecute = true;
                        trackirProcess.StartInfo.Verb = "runas";
                        trackirProcess.StartInfo.FileName = testTrackirExist() + @"\TrackIR5.exe";
                        trackirProcess.Start();
                    }
                    catch
                    {
                    }
                }
            }
        }
        static public string testTrackirExist()
        {
            string valeur = "";
            try
            {
                //ouvrir le dossier principal                   
                RegistryKey cle = Registry.CurrentUser;
                //ouvrir les sous répertoires du dossier principal, le second argument est un booleen permettant de modifier la clé    
                RegistryKey subcle = cle.OpenSubKey("Software\\NaturalPoint\\NaturalPoint\\NPClient Location", false);
                valeur = subcle.GetValue("Path").ToString();
                //fermer le registre   
                cle.Close();
            }
            catch
            {
            }

            return valeur;
        }

        // TEAMSPEAK Task Force
        static public void ValideTEAMSPEAKTeamForce()
        {

            GOSLauncherCore.fenetrePrincipale.button18.Enabled = false;
            GOSLauncherCore.fenetrePrincipale.button19.Enabled = false;
            if (testTeamSpeakTaskForceExist())
            {
                GOSLauncherCore.fenetrePrincipale.button18.Enabled = true;
                GOSLauncherCore.fenetrePrincipale.button19.Enabled = true;
                GOSLauncherCore.fenetrePrincipale.button18.Visible = true;
                GOSLauncherCore.fenetrePrincipale.button19.Visible = true;
            }
            else
            {

                try
                {
                    if (GOSLauncherCore.isGOSValid())
                    {
                        ReinstallTS3TaskForce();
                    };
                }
                catch
                {
                    var infoBox = MessageBox.Show("TS3 a besoin d'etre a nouveau initialisé. Quittez TEAMSPEAK et relancer le LAUNCHER SVP", "TS3 doit etre réinitialisé", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }
           
        }
        static public bool testTeamSpeakTaskForceExist()
        {
            try
            {
                if (File.Exists(GOSLauncherCore.cheminARMA3 + @"\userconfig\GOS-LauncherA3\TeamSpeak3\config\plugins\task_force_radio_win64.dll"))  //Si le fichier existe 
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
            }
            return false;
        }
        static public void lancerTeamspeak3TaskForceINTERCLAN(string adresseTEAMSPEAK,string passwordTEAMSPEAK)
        {
            Process[] ts364bit = Process.GetProcessesByName("ts3client_win64");
            Process[] ts332bit = Process.GetProcessesByName("ts3client_win32");

            if (ts364bit.Length == 0 && ts332bit.Length == 0)
            {
                //test l'existence d'un process
                // lance TASK FORCE
                Process ts3TaskForce = new Process();
                // Activation de l'envoi des événements
                ts3TaskForce.StartInfo.UseShellExecute = true;
                ts3TaskForce.StartInfo.Verb = "runas";
                ts3TaskForce.StartInfo.FileName = GOSLauncherCore.cheminARMA3 + @"\userconfig\GOS-LauncherA3\TeamSpeak3\ts3client_win64.exe";
                ts3TaskForce.StartInfo.Arguments = "ts3server://" + adresseTEAMSPEAK + "?password=" + passwordTEAMSPEAK;
                //ts3TaskForce.StartInfo.CreateNoWindow = true;
                ts3TaskForce.Start();
            }
            else
            {
                var infoBox = MessageBox.Show("Impossible de lancer le TS3 (code d'erreur #CONHARD 00x277H). Vous semblez avoir TS3 deja lancé sur votre ordinateur.", "Erreur TS3 en cours d'execution", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        static public void lancerTeamspeak3TaskForce()
        {
            Process[] ts364bit = Process.GetProcessesByName("ts3client_win64");
            Process[] ts332bit = Process.GetProcessesByName("ts3client_win32");

            if (ts364bit.Length == 0 && ts332bit.Length == 0)
            {
                //test l'existence d'un process
                // lance TASK FORCE
                Process ts3TaskForce = new Process();
                // Activation de l'envoi des événements
                ts3TaskForce.StartInfo.UseShellExecute = true;
                ts3TaskForce.StartInfo.Verb = "runas";
                ts3TaskForce.StartInfo.FileName = GOSLauncherCore.cheminARMA3 + @"\userconfig\GOS-LauncherA3\TeamSpeak3\ts3client_win64.exe";
                ts3TaskForce.StartInfo.Arguments = "ts3server://ts3.clan-gos.fr?password=welcome";
                //ts3TaskForce.StartInfo.CreateNoWindow = true;
                ts3TaskForce.Start();
            }
            else
            {
                var infoBox = MessageBox.Show("Impossible de lancer le TS3 dédié à TASK FORCE (code d'erreur #PoWeTACH :xTCON). Vous semblez avoir TS3 deja lancé sur votre ordinateur.", "Erreur TS3 en cours d'execution", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        static public void ReinstallTS3TaskForce()
        {
            try
            {
                // EFFACE REPERTOIRE PERIMES

                if (File.Exists(GOSLauncherCore.cheminARMA3 + @"\userconfig\task_force_radio\radio_keys.hpp"))
                {
                    Directory.Delete(GOSLauncherCore.cheminARMA3 + @"\@GOS\@TEMPLATE\@task_force_radio\userconfig\task_force_radio\", true);
                };

                if (File.Exists(GOSLauncherCore.cheminARMA3 + @"\userconfig\GOS-LauncherA3\TeamSpeak3\plugins\radio-sounds\on.wav"))
                {
                    Directory.Delete(GOSLauncherCore.cheminARMA3 + @"\userconfig\GOS-LauncherA3\TeamSpeak3\plugins\radio-sounds\", true);
                };

                if (File.Exists(GOSLauncherCore.cheminARMA3 + @"\userconfig\GOS-LauncherA3\TeamSpeak3\plugins\radio-sounds-old\on.wav"))
                {
                    Directory.Delete(GOSLauncherCore.cheminARMA3 + @"\userconfig\GOS-LauncherA3\TeamSpeak3\plugins\radio-sounds-old\", true);
                };
                if (File.Exists(GOSLauncherCore.cheminARMA3 + @"\userconfig\GOS-LauncherA3\TeamSpeak3\plugins\radio-sounds-new\on.wav"))
                {
                    Directory.Delete(GOSLauncherCore.cheminARMA3 + @"\userconfig\GOS-LauncherA3\TeamSpeak3\plugins\radio-sounds-new\", true);
                };

                if (File.Exists(GOSLauncherCore.cheminARMA3 + @"\userconfig\GOS-LauncherA3\TeamSpeak3\plugins\task_force_radio_win64.dll"))
                {
                    File.Delete(GOSLauncherCore.cheminARMA3 + @"\userconfig\GOS-LauncherA3\TeamSpeak3\plugins\task_force_radio_win64.dll");
                };
                //   
                GOSLauncherCore.CopyDir(GOSLauncherCore.cheminARMA3 + @"\@GOS\@CLIENT\TeamSpeak3\nouvelleversion\TeamSpeak3\", GOSLauncherCore.cheminARMA3 + @"\userconfig\GOS-LauncherA3\");
                GOSLauncherCore.CopyDir(GOSLauncherCore.cheminARMA3 + @"\@GOS\@TEMPLATE\@task_force_radio\TeamSpeak 3 Client\plugins\", GOSLauncherCore.cheminARMA3 + @"\userconfig\GOS-LauncherA3\TeamSpeak3\config\");
                GOSLauncherCore.CopyDir(GOSLauncherCore.cheminARMA3 + @"\@GOS\@TEMPLATE\@task_force_radio\userconfig\task_force_radio\", GOSLauncherCore.cheminARMA3 + @"\userconfig\");
  
                var infoBox = MessageBox.Show("Le TS3 dédié à TASK FORCE RADIO a été reinstallé dans sa derniere version.", "TS3 reinstallé", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                var infoBox = MessageBox.Show("Impossible de reinstaller le TS3 dédié au TASK FORCE RADIO (code d'erreur #PoWeTACH :xTCON). Vous semblez avoir TS3 deja lancé sur votre ordinateur.", "Erreur : TS3 en cours d'execution", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }


    }
}
