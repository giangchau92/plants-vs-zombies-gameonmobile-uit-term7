using GameDesignTool.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameDesignTool
{
    public partial class frmZombie : Form
    {
        public frmZombie()
        {
            InitializeComponent();
        }

        private void frmZombie_Load(object sender, EventArgs e)
        {
            this.resetZFields();
        }

        private void btnMake_Click(object sender, EventArgs e)
        {
            string result = string.Format(this.zombieFormat.Text,
                this.txtZName.Text,
                this.txtZSpeed.Text,
                this.txtZType.Text,
                this.txtZAttackRate.Text,
                this.txtZWalkRate.Text,
                this.txtZDeathRate.Text,
                this.txtZAttackX.Text,
                this.txtZAttackY.Text,
                this.txtZWalkX.Text,
                this.txtZWalkY.Text,
                this.txtZDeathX.Text,
                this.txtZDeathY.Text,
                this.txtZHP.Text,
                this.txtZDamage.Text);

            this.rTxtZResult.Text += Environment.NewLine;
            this.rTxtZResult.Text += Environment.NewLine;
            this.rTxtZResult.Text += result;
            this.rTxtZResult.SelectAll();
            this.resetZFields();
        }

        private void resetZFields()
        {
            this.txtZName.Clear();
            this.txtZDamage.Clear();
            this.txtZHP.Clear();
            this.txtZSpeed.Clear();
            this.txtZAttackX.Clear();
            this.txtZAttackY.Clear();
            this.txtZDeathX.Clear();
            this.txtZDeathY.Clear();
            this.txtZWalkX.Clear();
            this.txtZWalkY.Clear();
        }

        private void btnLMake_Click(object sender, EventArgs e)
        {
            string result = string.Empty;
            if (this.rTxtLResult.Text.Length !=  0)
            {
                result += "\t</Level>";
                result += Environment.NewLine;
                result += Environment.NewLine;
            }

            result += string.Format("\t<!--{0}-->", this.txtLName.Text);
            result += Environment.NewLine;
            result += "\t<Level>";
            result += Environment.NewLine;
            result += string.Format("\t\t<Name>{0}</Name>", this.txtLName.Text);
            result += Environment.NewLine;
            result += string.Format("\t\t<Background>{0}</Background>", this.txtLBackground.Text);

            this.rTxtLResult.Text += result;
        }

        private void btnWAdd_Click(object sender, EventArgs e)
        {
            string result = string.Empty;
            string nZombiesFrom, nZombiesTo;
            string GDelayFrom, GDelayTo;
            List<string> zombies;

            try
            {
                string[] temp = this.txtWNZombies.Text.Split('-');
                nZombiesFrom = temp[0];
                nZombiesTo = nZombiesFrom;
                if (temp.Length > 1)
                {
                    nZombiesTo = temp[1];
                }

                temp = this.txtWZombies.Text.Split(',');
                zombies = new List<string>(temp.Length);
                string[] zombieList = this.rTxTZombieList.Lines;
                for (int i = 0; i < temp.Length; ++i)
                {
                    int zIndex = Convert.ToInt32(temp[i]);
                    if (zombieList.Length > zIndex)
                    {
                        zombies.Add(zombieList[zIndex]);
                    }
                }

                temp = this.txtWGDelay.Text.Split('-');
                GDelayFrom = temp[0];
                GDelayTo = nZombiesFrom;
                if (temp.Length > 1)
                {
                    GDelayTo = temp[1];
                }

                result += string.Format(this.waveHeaderFormat.Text, this.txtWName.Text);
                for (int i = 0; i < zombies.Count; ++i)
                {
                    result += string.Format("\t\t\t<Zombie><Value>xml_{0}_zombie</Value></Zombie>", zombies[i]);
                    result += Environment.NewLine;
                }
                result += string.Format(this.waveFooterFormat.Text, nZombiesFrom, nZombiesTo, GDelayFrom, GDelayTo, this.txtWBegin.Text, this.txtWEnd.Text);

            }
            catch (Exception)
            {

            }

            this.rTxtLResult.Text += result;
            this.rTxtLResult.Text += Environment.NewLine;
            this.rTxtLResult.Text += Environment.NewLine;
            this.resetLFields();
        }

        private void resetLFields()
        {
            this.txtWNZombies.Clear();
            this.txtWZombies.Clear();
            this.txtWGDelay.Clear();
            this.txtWBegin.Clear();
            this.txtWEnd.Clear();
        }

        private void btnTop_Click(object sender, EventArgs e)
        {
            this.TopMost = !this.TopMost;
        }

        private void btnLScript_Click(object sender, EventArgs e)
        {
            try
            {
                string[] scripts = this.rTxtLResult.Lines;
                this.rTxtLResult.Clear();
                for (int i = 0; i < scripts.Length; ++i)
                {
                    string[] tabs = scripts[i].Split('\t');
                    if (tabs[0] != "")  // new level
                    {
                        this.txtLName.Text = tabs[0];
                        this.txtLBackground.Text = tabs[1];

                        this.btnLMake_Click(this, null);
                    }
                    else
                    {
                        this.txtWName.Text = "Wave " + tabs[2];
                        this.txtWNZombies.Text = tabs[3];
                        this.txtWZombies.Text = tabs[4];
                        this.txtWGDelay.Text = tabs[5];
                        this.txtWBegin.Text = tabs[6];
                        this.txtWEnd.Text = tabs[7];

                        this.btnWAdd_Click(this, null);
                    }
                }

                this.btnLMake_Click(this, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnZScript_Click(object sender, EventArgs e)
        {
            try
            {
                string[] scripts = this.rTxtZResult.Lines;
                this.rTxtZResult.Clear();
                for (int i = 0; i < scripts.Length; ++i)
                {
                    string[] tabs = scripts[i].Split('\t');
                    try
                    {
                        this.txtZName.Text = tabs[0];
                        this.txtZType.Text = tabs[1];
                        this.txtZSpeed.Text = tabs[2];
                        this.txtZHP.Text = tabs[3];
                        this.txtZDamage.Text = tabs[4];
                        this.txtZAttackRate.Text = tabs[5];
                        this.txtZDeathRate.Text = "0.04";
                        this.txtZWalkRate.Text = "0.04";
                        this.txtZAttackX.Text = tabs[6];
                        this.txtZAttackY.Text = tabs[7];
                        this.txtZDeathX.Text = tabs[8];
                        this.txtZDeathY.Text = tabs[9];
                        this.txtZWalkX.Text = tabs[10];
                        this.txtZWalkY.Text = tabs[11];

                        this.btnMake_Click(this, null);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
