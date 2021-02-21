/******************************************************************************/
/*    Copyright (c) 2021 MD Automotive Controls. Original Work.               */
/*    License: http://www.gnu.org/licenses/gpl.html GPL version 2 or higher   */
/******************************************************************************/
/* CONTEXT:APP                                                                */
/* PACKAGE TITLE:      Sequence Setup Wizard                                  */
/* DESCRIPTION:                                                               */
/* FILE NAME:          tclsSequenceSetupWizard.cs                             */
/* REVISION HISTORY:   22-02-2021 | 1.0 | Initial revision                    */
/*                                                                            */
/******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UDP
{
    public partial class tclsSequenceSetupWizard : Form
    {
        int miCylCount;
        int miSyncPointCount;
        int miSyncRepeatCount;
        bool mboSequential;
        bool mboDirectFire;

        public tclsSequenceSetupWizard(int cylCount, int syncPointCount, int syncRepeatCount)
        {
            InitializeComponent();

            miCylCount = cylCount;
            miSyncPointCount = syncPointCount;
            miSyncRepeatCount = syncRepeatCount;

            textBoxCylinders.Text = cylCount.ToString();
            textBoxSyncCount.Text = syncPointCount.ToString();

            if (syncRepeatCount == 1)
            {
                radioButton360.Checked = true;
                radioButtonBatchInjection.Checked = true;
                radioButtonWastedSpark.Checked = true;
                mboDirectFire = false;
                mboSequential = false;
            }
            else if (syncRepeatCount == 2)
            {
                radioButton720.Checked = true;
                radioButtonSeqInjection.Checked = true;

                if (cylCount <= 4)
                {
                    radioButtonDirectFire.Checked = true;
                }
                else
                {
                    radioButtonWastedSpark.Checked = true;
                }
            }

            Calculate();
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            Calculate();
        }

        private void Calculate()
        {
            try
            {
                ReloadInputs();
                int iStartFuelSequence;
                int iFuelSequenceStep;
                int iSparkSequenceStep;

                switch (miCylCount)
                {
                    case 4:
                        {
                            if (mboSequential == false)
                            {
                                textBoxFuelResAE.Text = "103";
                                textBoxFuelResBF.Text = "68";
                                textBoxFuelResCG.Text = "67";
                                textBoxFuelResDH.Text = "103";

                                textBoxFuelResAE.BackColor = Color.DarkSlateGray;
                                textBoxFuelResBF.BackColor = Color.LightGreen;
                                textBoxFuelResCG.BackColor = Color.LightGreen;
                                textBoxFuelResDH.BackColor = Color.DarkSlateGray;

                                int iSequence4B;

                                if (miSyncRepeatCount == 2)
                                {
                                    iSequence4B = 0x100 * (miSyncPointCount / 2) + (miSyncPointCount + miSyncPointCount / 2);
                                }
                                else
                                {
                                    iSequence4B = 0x100 * (miSyncPointCount / 2) + (miSyncPointCount / 2);
                                }

                                textBoxFuelSeqAE.Text = "0";
                                textBoxFuelSeqBF.Text = iSequence4B.ToString();
                                textBoxFuelSeqCG.Text = iSequence4B.ToString();
                                textBoxFuelSeqDH.Text = "0";

                                textBoxFuelSeqAE.BackColor = Color.DarkSlateGray;
                                textBoxFuelSeqBF.BackColor = Color.LightGreen;
                                textBoxFuelSeqCG.BackColor = Color.LightGreen;
                                textBoxFuelSeqDH.BackColor = Color.DarkSlateGray;

                                textBoxOutput.Text = "Wire injectors: 1st pair INJBF, 2nd pair INJCG\r\n";
                                textBoxOutput.Text += "No Arduino injector links needed\r\n";
                            }
                            else
                            {
                                iFuelSequenceStep = (miSyncRepeatCount * miSyncPointCount) / miCylCount;
                                iStartFuelSequence = (miSyncRepeatCount * miSyncPointCount) / 2;

                                textBoxFuelResAE.Text = "6";
                                textBoxFuelResBF.Text = "68";
                                textBoxFuelResCG.Text = "67";
                                textBoxFuelResDH.Text = "7";

                                textBoxFuelResAE.BackColor = Color.LightGreen;
                                textBoxFuelResBF.BackColor = Color.LightGreen;
                                textBoxFuelResCG.BackColor = Color.LightGreen;
                                textBoxFuelResDH.BackColor = Color.LightGreen;

                                int iFuelSequence = iStartFuelSequence;
                                int iSequence4S = 0xff00 + iFuelSequence;

                                textBoxFuelSeqAE.Text = iSequence4S.ToString();
                                iFuelSequence = (iFuelSequence + iFuelSequenceStep) % (miSyncPointCount * miSyncRepeatCount);
                                iSequence4S = 0xff00 + iFuelSequence;

                                textBoxFuelSeqBF.Text = iSequence4S.ToString();
                                iFuelSequence = (iFuelSequence + iFuelSequenceStep) % (miSyncPointCount * miSyncRepeatCount);
                                iSequence4S = 0xff00 + iFuelSequence;

                                textBoxFuelSeqCG.Text = iSequence4S.ToString();
                                iFuelSequence = (iFuelSequence + iFuelSequenceStep) % (miSyncPointCount * miSyncRepeatCount);
                                iSequence4S = 0xff00 + iFuelSequence;

                                textBoxFuelSeqDH.Text = iSequence4S.ToString();

                                textBoxFuelSeqAE.BackColor = Color.LightGreen;
                                textBoxFuelSeqBF.BackColor = Color.LightGreen;
                                textBoxFuelSeqCG.BackColor = Color.LightGreen;
                                textBoxFuelSeqDH.BackColor = Color.LightGreen;

                                textBoxOutput.Text = "Wire injectors: 1st to fire INJAE, 2nd to fire INJBF, 3rd to fire INJCG, 4th to fire INJDH\r\n";
                                textBoxOutput.Text += "Link Arduino PWM13 & A6, PWM10 & A7\r\n";
                            }

                            /* IGNITION */
                            if (mboDirectFire == false)
                            {
                                iSparkSequenceStep = miSyncPointCount / miCylCount;

                                textBoxSparkResAE.Text = "60";
                                textBoxSparkResBF.Text = "61";
                                textBoxSparkResCG.Text = "103";
                                textBoxSparkResDH.Text = "103";

                                textBoxSparkResAE.BackColor = Color.LightGreen;
                                textBoxSparkResBF.BackColor = Color.LightGreen;
                                textBoxSparkResCG.BackColor = Color.DarkSlateGray;
                                textBoxSparkResDH.BackColor = Color.DarkSlateGray;

                                int iSequence = 0;

                                if (miSyncRepeatCount == 2)
                                {
                                    iSequence = miSyncPointCount;
                                    textBoxSparkSeqAE.Text = iSequence.ToString();

                                    iSequence = 0x100 * miSyncPointCount / 2 + miSyncPointCount + miSyncPointCount / 2;
                                    textBoxSparkSeqBF.Text = iSequence.ToString();

                                    textBoxSparkSeqCG.Text = "0";
                                    textBoxSparkSeqDH.Text = "0";
                                }
                                else
                                {
                                    textBoxSparkSeqAE.Text = iSequence.ToString();

                                    iSequence = 0x100 * miSyncPointCount / 2 + miSyncPointCount / 2;
                                    textBoxSparkSeqBF.Text = iSequence.ToString();

                                    textBoxSparkSeqCG.Text = "0";
                                    textBoxSparkSeqDH.Text = "0";
                                }

                                textBoxSparkSeqAE.BackColor = Color.LightGreen;
                                textBoxSparkSeqBF.BackColor = Color.LightGreen;
                                textBoxSparkSeqCG.BackColor = Color.DarkSlateGray;
                                textBoxSparkSeqDH.BackColor = Color.DarkSlateGray;

                                textBoxOutput.Text += "Wire igniters: 1st to fire MOT1B, 2nd to fire MOT1A\r\n";
                                textBoxOutput.Text += "No Arduino igniter links needed\r\n";
                            }
                            else
                            {
                                textBoxSparkResAE.Text = "60";
                                textBoxSparkResBF.Text = "61";
                                textBoxSparkResCG.Text = "4";
                                textBoxSparkResDH.Text = "70";

                                textBoxSparkResAE.BackColor = Color.LightGreen;
                                textBoxSparkResBF.BackColor = Color.LightGreen;
                                textBoxSparkResCG.BackColor = Color.LightGreen;
                                textBoxSparkResDH.BackColor = Color.LightGreen;

                                int iSequence = 0;

                                iSequence = 0xff00;
                                textBoxSparkSeqAE.Text = iSequence.ToString();

                                iSequence = 0xff00 + (miSyncRepeatCount * miSyncPointCount) / 4;
                                textBoxSparkSeqBF.Text = iSequence.ToString();

                                iSequence = 0xff00 + (miSyncRepeatCount * miSyncPointCount) / 2;
                                textBoxSparkSeqCG.Text = iSequence.ToString();

                                iSequence = 0xff00 + (3 * (miSyncRepeatCount * miSyncPointCount) / 4);
                                textBoxSparkSeqDH.Text = iSequence.ToString();

                                textBoxSparkSeqAE.BackColor = Color.LightGreen;
                                textBoxSparkSeqBF.BackColor = Color.LightGreen;
                                textBoxSparkSeqCG.BackColor = Color.LightGreen;
                                textBoxSparkSeqDH.BackColor = Color.LightGreen;

                                textBoxOutput.Text += "Wire igniters: 1st to fire MOT1B, 2nd to fire MOT1A, 3rd to fire MOT2B, 4th to fire MOT2A\r\n";
                                textBoxOutput.Text += "Link Arduino PWM6 & A4, PWM7 & EXTINT\r\n";
                            }

                            break;
                        }

                    case 6:
                        {
                            if (mboSequential == false)
                            {
                                textBoxFuelResAE.Text = "6";
                                textBoxFuelResBF.Text = "68";
                                textBoxFuelResCG.Text = "67";
                                textBoxFuelResDH.Text = "103";

                                textBoxFuelResAE.BackColor = Color.LightGreen;
                                textBoxFuelResBF.BackColor = Color.LightGreen;
                                textBoxFuelResCG.BackColor = Color.LightGreen;
                                textBoxFuelResDH.BackColor = Color.DarkSlateGray;

                                int iSequence6B;

                                if (miSyncRepeatCount == 2)
                                {
                                    iSequence6B = 0x100 * (miSyncPointCount / 2) + (miSyncPointCount + miSyncPointCount / 2);
                                }
                                else
                                {
                                    iSequence6B = 0x100 * (miSyncPointCount / 2) + (miSyncPointCount / 2);
                                }

                                textBoxFuelSeqAE.Text = iSequence6B.ToString();
                                textBoxFuelSeqBF.Text = iSequence6B.ToString();
                                textBoxFuelSeqCG.Text = iSequence6B.ToString();
                                textBoxFuelSeqDH.Text = "0";

                                textBoxFuelSeqAE.BackColor = Color.LightGreen;
                                textBoxFuelSeqBF.BackColor = Color.LightGreen;
                                textBoxFuelSeqCG.BackColor = Color.LightGreen;
                                textBoxFuelSeqDH.BackColor = Color.DarkSlateGray;

                                textBoxOutput.Text = "Wire injectors: 1st pair INJAE, 2nd pair INJBF, 3rd pair INJCG\r\n";
                                textBoxOutput.Text += "Link Arduino PWM13 & A6\r\n";
                            }
                            else
                            {
                                iFuelSequenceStep = (miSyncRepeatCount * miSyncPointCount) / 3;
                                iStartFuelSequence = (miSyncRepeatCount * miSyncPointCount) / 2;

                                textBoxFuelResAE.Text = "6";
                                textBoxFuelResBF.Text = "68";
                                textBoxFuelResCG.Text = "67";
                                textBoxFuelResDH.Text = "7";

                                textBoxFuelResAE.BackColor = Color.LightGreen;
                                textBoxFuelResBF.BackColor = Color.LightGreen;
                                textBoxFuelResCG.BackColor = Color.LightGreen;
                                textBoxFuelResDH.BackColor = Color.DarkSlateGray;

                                int iFuelSequence = iStartFuelSequence;
                                int iSequence6S = 0xff00 + iFuelSequence;

                                textBoxFuelSeqAE.Text = iSequence6S.ToString();
                                iFuelSequence = (iFuelSequence + iFuelSequenceStep) % (miSyncPointCount * miSyncRepeatCount);
                                iSequence6S = 0xff00 + iFuelSequence;

                                textBoxFuelSeqBF.Text = iSequence6S.ToString();
                                iFuelSequence = (iFuelSequence + iFuelSequenceStep) % (miSyncPointCount * miSyncRepeatCount);
                                iSequence6S = 0xff00 + iFuelSequence;

                                textBoxFuelSeqCG.Text = iSequence6S.ToString();
                                iFuelSequence = (iFuelSequence + iFuelSequenceStep) % (miSyncPointCount * miSyncRepeatCount);
                                iSequence6S = 0xff00 + iFuelSequence;

                                textBoxFuelSeqDH.Text = "0";

                                textBoxFuelSeqAE.BackColor = Color.LightGreen;
                                textBoxFuelSeqBF.BackColor = Color.LightGreen;
                                textBoxFuelSeqCG.BackColor = Color.LightGreen;
                                textBoxFuelSeqDH.BackColor = Color.DarkSlateGray;

                                textBoxOutput.Text = "Wire injectors: 1st pair to fire INJAE, 2nd pair to fire INJBF, 3rd pair to fire INJCG, 4th to fire INJDH\r\n";
                                textBoxOutput.Text += "Link Arduino PWM6 & A5\r\n";
                            }
              

                            /* IGNITION */
                            textBoxSparkResAE.Text = "60";
                            textBoxSparkResBF.Text = "61";
                            textBoxSparkResCG.Text = "4";
                            textBoxSparkResDH.Text = "103";

                            textBoxSparkResAE.BackColor = Color.LightGreen;
                            textBoxSparkResBF.BackColor = Color.LightGreen;
                            textBoxSparkResCG.BackColor = Color.LightGreen;
                            textBoxSparkResDH.BackColor = Color.DarkSlateGray;

                            int iSequence = 0;

                            if (miSyncRepeatCount == 2)
                            {
                                iSequence = miSyncPointCount;
                                textBoxSparkSeqAE.Text = iSequence.ToString();

                                iSequence = 0x100 * miSyncPointCount / 3 + miSyncPointCount + miSyncPointCount / 3;
                                textBoxSparkSeqBF.Text = iSequence.ToString();

                                iSequence = 0x100 * 2 * miSyncPointCount / 3 + miSyncPointCount + 2 * miSyncPointCount / 3;
                                textBoxSparkSeqCG.Text = iSequence.ToString();

                                textBoxSparkSeqDH.Text = "0";
                            }
                            else
                            {
                                textBoxSparkSeqAE.Text = iSequence.ToString();

                                iSequence = 0x100 * miSyncPointCount / 3 + miSyncPointCount / 3;
                                textBoxSparkSeqBF.Text = iSequence.ToString();

                                iSequence = 0x100 * 2 * miSyncPointCount / 3 + 2 * miSyncPointCount / 3;
                                textBoxSparkSeqCG.Text = iSequence.ToString();

                                textBoxSparkSeqDH.Text = "0";
                            }

                            textBoxSparkSeqAE.BackColor = Color.LightGreen;
                            textBoxSparkSeqBF.BackColor = Color.LightGreen;
                            textBoxSparkSeqCG.BackColor = Color.LightGreen;
                            textBoxSparkSeqDH.BackColor = Color.DarkSlateGray;

                            textBoxOutput.Text += "Wire igniters: 1st pair to fire MOT1B, 2nd pair to fire MOT1A, 3rd pair to fire MOT2B\r\n";
                            textBoxOutput.Text += "Link Arduino PWM6 to A4\r\n";
                            break;
                        }

                    case 8:
                        {
                            if (mboSequential == false)
                            {
                                textBoxFuelResAE.Text = "6";
                                textBoxFuelResBF.Text = "68";
                                textBoxFuelResCG.Text = "67";
                                textBoxFuelResDH.Text = "7";

                                textBoxFuelResAE.BackColor = Color.LightGreen;
                                textBoxFuelResBF.BackColor = Color.LightGreen;
                                textBoxFuelResCG.BackColor = Color.LightGreen;
                                textBoxFuelResDH.BackColor = Color.LightGreen;

                                int iSequence8B;

                                if (miSyncRepeatCount == 2)
                                {
                                    iSequence8B = 0x100 * (miSyncPointCount / 2) + (miSyncPointCount + miSyncPointCount / 2);
                                }
                                else
                                {
                                    iSequence8B = 0x100 * (miSyncPointCount / 2) + (miSyncPointCount / 2);
                                }

                                textBoxFuelSeqAE.Text = iSequence8B.ToString();
                                textBoxFuelSeqBF.Text = iSequence8B.ToString();
                                textBoxFuelSeqCG.Text = iSequence8B.ToString();
                                textBoxFuelSeqDH.Text = iSequence8B.ToString();

                                textBoxFuelSeqAE.BackColor = Color.LightGreen;
                                textBoxFuelSeqBF.BackColor = Color.LightGreen;
                                textBoxFuelSeqCG.BackColor = Color.LightGreen;
                                textBoxFuelSeqDH.BackColor = Color.LightGreen;

                                textBoxOutput.Text = "Wire injectors: 1st pair INJAE, 2nd pair INJBF, 3rd pair INJCG, 4th pair INJDF\r\n";
                                textBoxOutput.Text += "Link Arduino PWM13 & A6, PWM10 & A7\r\n";
                            }
                            else
                            {
                                iFuelSequenceStep = (miSyncRepeatCount * miSyncPointCount) / 4;
                                iStartFuelSequence = (miSyncRepeatCount * miSyncPointCount) / 2;

                                textBoxFuelResAE.Text = "6";
                                textBoxFuelResBF.Text = "68";
                                textBoxFuelResCG.Text = "67";
                                textBoxFuelResDH.Text = "7";

                                textBoxFuelResAE.BackColor = Color.LightGreen;
                                textBoxFuelResBF.BackColor = Color.LightGreen;
                                textBoxFuelResCG.BackColor = Color.LightGreen;
                                textBoxFuelResDH.BackColor = Color.LightGreen;

                                int iFuelSequence = iStartFuelSequence;
                                int iSequence8S = 0xff00 + iFuelSequence;

                                textBoxFuelSeqAE.Text = iSequence8S.ToString();
                                iFuelSequence = (iFuelSequence + iFuelSequenceStep) % (miSyncRepeatCount * miSyncPointCount);
                                iSequence8S = 0xff00 + iFuelSequence;

                                textBoxFuelSeqBF.Text = iSequence8S.ToString();
                                iFuelSequence = (iFuelSequence + iFuelSequenceStep) % (miSyncRepeatCount * miSyncPointCount);
                                iSequence8S = 0xff00 + iFuelSequence;

                                textBoxFuelSeqCG.Text = iSequence8S.ToString();
                                iFuelSequence = (iFuelSequence + iFuelSequenceStep) % (miSyncRepeatCount * miSyncPointCount);
                                iSequence8S = 0xff00 + iFuelSequence;

                                textBoxFuelSeqDH.Text = iSequence8S.ToString();

                                textBoxFuelSeqAE.BackColor = Color.LightGreen;
                                textBoxFuelSeqBF.BackColor = Color.LightGreen;
                                textBoxFuelSeqCG.BackColor = Color.LightGreen;
                                textBoxFuelSeqDH.BackColor = Color.LightGreen;

                                textBoxOutput.Text = "Wire injectors: 1st pair to fire INJAE, 2nd pair to fire INJBF, 3rd pair to fire INJCG, 4th pair to fire INJDH\r\n";
                                textBoxOutput.Text += "Link Arduino PWM13 & A6, PWM10 & A7\r\n";
                            }

                            /* IGNITION */
                            textBoxSparkResAE.Text = "60";
                            textBoxSparkResBF.Text = "61";
                            textBoxSparkResCG.Text = "4";
                            textBoxSparkResDH.Text = "70";

                            textBoxSparkResAE.BackColor = Color.LightGreen;
                            textBoxSparkResBF.BackColor = Color.LightGreen;
                            textBoxSparkResCG.BackColor = Color.LightGreen;
                            textBoxSparkResDH.BackColor = Color.LightGreen;

                            int iSequence = 0;

                            if (miSyncRepeatCount == 2)
                            {
                                iSequence = miSyncPointCount;
                                textBoxSparkSeqAE.Text = iSequence.ToString();

                                iSequence = 0x100 * miSyncPointCount / 4 + miSyncPointCount + miSyncPointCount / 4;
                                textBoxSparkSeqBF.Text = iSequence.ToString();

                                iSequence = 0x100 * miSyncPointCount / 2 + miSyncPointCount + miSyncPointCount / 2;
                                textBoxSparkSeqCG.Text = iSequence.ToString();

                                iSequence = 0x100 * (3 * miSyncPointCount / 4) + miSyncPointCount + (3 * miSyncPointCount / 4);
                                textBoxSparkSeqDH.Text = iSequence.ToString();
                            }
                            else
                            {
                                textBoxSparkSeqAE.Text = iSequence.ToString();

                                iSequence = 0x100 * miSyncPointCount / 4 + miSyncPointCount / 4;
                                textBoxSparkSeqBF.Text = iSequence.ToString();

                                iSequence = 0x100 * miSyncPointCount / 2 + miSyncPointCount / 2;
                                textBoxSparkSeqCG.Text = iSequence.ToString();

                                iSequence = 0x100 * (3 * miSyncPointCount / 4) + (3 * miSyncPointCount / 4);
                                textBoxSparkSeqDH.Text = iSequence.ToString();
                            }

                            textBoxSparkSeqAE.BackColor = Color.LightGreen;
                            textBoxSparkSeqBF.BackColor = Color.LightGreen;
                            textBoxSparkSeqCG.BackColor = Color.LightGreen;
                            textBoxSparkSeqDH.BackColor = Color.LightGreen;

                            textBoxOutput.Text += "Wire igniters: 1st pair to fire MOT1B, 2nd pair to fire MOT1A, 3rd pair to fire MOT2B, 4th pair to fire MOT2A\r\n";
                            textBoxOutput.Text += "Link Arduino PWM6 & A4, PWM7 & EXTINT\r\n";


                            break;
                        }

                }
            }
            catch
            {
                MessageBox.Show("Data input error, please check values");
            }
        }

        private void ReloadInputs()
        {
            miCylCount = Convert.ToInt32(textBoxCylinders.Text);
            miSyncPointCount = Convert.ToInt32(textBoxSyncCount.Text);
            miSyncRepeatCount = radioButton360.Checked == true ? 1 : 2;

            if (miSyncRepeatCount == 1)
            {
                if (radioButtonSeqInjection.Checked == true)
                {
                    radioButtonSeqInjection.Checked = false;
                    radioButtonBatchInjection.Checked = true;
                    MessageBox.Show("Sequential injection not possible in 360 degree system!");
                }
                if (radioButtonDirectFire.Checked == true)
                {
                    radioButtonDirectFire.Checked = false;
                    radioButtonWastedSpark.Checked = true;
                    MessageBox.Show("Direct fire ignition not possible in 360 degree system!");
                }
            }

            if (miCylCount > 4)
            {
                if (radioButtonDirectFire.Checked == true)
                {
                    radioButtonDirectFire.Checked = false;
                    radioButtonWastedSpark.Checked = true;
                    MessageBox.Show("Direct fire ignition not supported for more than 4 cylinders");
                }
            }

            mboSequential = radioButtonSeqInjection.Checked == true;
            mboDirectFire = radioButtonDirectFire.Checked == true;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            bool boErr = false;

            int iSyncRepeats;
            int iCylCount;
            int iFuelAEResource;
            int iFuelBFResource;
            int iFuelCGResource;
            int iFuelDHResource;

            int iSparkAEResource;
            int iSparkBFResource;
            int iSparkCGResource;
            int iSparkDHResource;

            int iFuelAESequence;
            int iFuelBFSequence;
            int iFuelCGSequence;
            int iFuelDHSequence;

            int iSparkAESequence;
            int iSparkBFSequence;
            int iSparkCGSequence;
            int iSparkDHSequence;

            int iSyncRepeatsIDX = tclsASAM.iGetCharIndex("Sync Phase Repeats");
            int iCylCountIDX = tclsASAM.iGetCharIndex("Cyl Count");

            int iFuelAEResourceIDX = tclsASAM.iGetCharIndex("Fuel 1 IO Resource");
            int iFuelBFResourceIDX = tclsASAM.iGetCharIndex("Fuel 2 IO Resource");
            int iFuelCGResourceIDX = tclsASAM.iGetCharIndex("Fuel 3 IO Resource");
            int iFuelDHResourceIDX = tclsASAM.iGetCharIndex("Fuel 4 IO Resource");

            int iSparkAEResourceIDX = tclsASAM.iGetCharIndex("EST 1 IO Resource"); ;
            int iSparkBFResourceIDX = tclsASAM.iGetCharIndex("EST 2 IO Resource"); ;
            int iSparkCGResourceIDX = tclsASAM.iGetCharIndex("EST 3 IO Resource"); ;
            int iSparkDHResourceIDX = tclsASAM.iGetCharIndex("EST 4 IO Resource"); ;

            int iFuelAESequenceIDX = tclsASAM.iGetCharIndex("Injection Sequence AE");
            int iFuelBFSequenceIDX = tclsASAM.iGetCharIndex("Injection Sequence BF");
            int iFuelCGSequenceIDX = tclsASAM.iGetCharIndex("Injection Sequence CG");
            int iFuelDHSequenceIDX = tclsASAM.iGetCharIndex("Injection Sequence DH");

            int iSparkAESequenceIDX = tclsASAM.iGetCharIndex("Ignition Sequence AE");
            int iSparkBFSequenceIDX = tclsASAM.iGetCharIndex("Ignition Sequence BF");
            int iSparkCGSequenceIDX = tclsASAM.iGetCharIndex("Ignition Sequence CG");
            int iSparkDHSequenceIDX = tclsASAM.iGetCharIndex("Ignition Sequence DH");

            try
            {
                iSyncRepeats = radioButton360.Checked == true ? 1 : 2;
                iCylCount = Convert.ToInt32(textBoxCylinders.Text);


                iFuelAEResource = Convert.ToInt32(textBoxFuelResAE.Text);
                iFuelBFResource = Convert.ToInt32(textBoxFuelResBF.Text);
                iFuelCGResource = Convert.ToInt32(textBoxFuelResCG.Text);
                iFuelDHResource = Convert.ToInt32(textBoxFuelResDH.Text);

                iSparkAEResource = Convert.ToInt32(textBoxSparkResAE.Text);
                iSparkBFResource = Convert.ToInt32(textBoxSparkResBF.Text);
                iSparkCGResource = Convert.ToInt32(textBoxSparkResCG.Text);
                iSparkDHResource = Convert.ToInt32(textBoxSparkResDH.Text);

                iFuelAESequence = Convert.ToInt32(textBoxFuelSeqAE.Text);
                iFuelBFSequence = Convert.ToInt32(textBoxFuelSeqBF.Text);
                iFuelCGSequence = Convert.ToInt32(textBoxFuelSeqCG.Text);
                iFuelDHSequence = Convert.ToInt32(textBoxFuelSeqDH.Text);

                iSparkAESequence = Convert.ToInt32(textBoxSparkSeqAE.Text);
                iSparkBFSequence = Convert.ToInt32(textBoxSparkSeqBF.Text);
                iSparkCGSequence = Convert.ToInt32(textBoxSparkSeqCG.Text);
                iSparkDHSequence = Convert.ToInt32(textBoxSparkSeqDH.Text);

                tclsDataPage.u32SetWorkingData(tclsASAM.milstCharacteristicList[iFuelAEResourceIDX].u32Address,
                    (UInt32)iFuelAEResource);

                tclsDataPage.u32SetWorkingData(tclsASAM.milstCharacteristicList[iFuelBFResourceIDX].u32Address,
                    (UInt32)iFuelBFResource);

                tclsDataPage.u32SetWorkingData(tclsASAM.milstCharacteristicList[iFuelCGResourceIDX].u32Address,
                    (UInt32)iFuelCGResource);

                tclsDataPage.u32SetWorkingData(tclsASAM.milstCharacteristicList[iFuelDHResourceIDX].u32Address,
                    (UInt32)iFuelDHResource);

                tclsDataPage.u32SetWorkingData(tclsASAM.milstCharacteristicList[iSparkAEResourceIDX].u32Address,
                    (UInt32)iSparkAEResource);

                tclsDataPage.u32SetWorkingData(tclsASAM.milstCharacteristicList[iSparkBFResourceIDX].u32Address,
                    (UInt32)iSparkBFResource);

                tclsDataPage.u32SetWorkingData(tclsASAM.milstCharacteristicList[iSparkCGResourceIDX].u32Address,
                    (UInt32)iSparkCGResource);

                tclsDataPage.u32SetWorkingData(tclsASAM.milstCharacteristicList[iSparkDHResourceIDX].u32Address,
                    (UInt32)iSparkDHResource);

                tclsDataPage.u32SetWorkingData(tclsASAM.milstCharacteristicList[iFuelAESequenceIDX].u32Address,
                    (UInt32)iFuelAESequence);

                tclsDataPage.u32SetWorkingData(tclsASAM.milstCharacteristicList[iFuelBFSequenceIDX].u32Address,
                    (UInt32)iFuelBFSequence);

                tclsDataPage.u32SetWorkingData(tclsASAM.milstCharacteristicList[iFuelCGSequenceIDX].u32Address,
                    (UInt32)iFuelCGSequence);

                tclsDataPage.u32SetWorkingData(tclsASAM.milstCharacteristicList[iFuelDHSequenceIDX].u32Address,
                    (UInt32)iFuelDHSequence);

                tclsDataPage.u32SetWorkingData(tclsASAM.milstCharacteristicList[iSparkAESequenceIDX].u32Address,
                    (UInt32)iSparkAESequence);

                tclsDataPage.u32SetWorkingData(tclsASAM.milstCharacteristicList[iSparkBFSequenceIDX].u32Address,
                    (UInt32)iSparkBFSequence);

                tclsDataPage.u32SetWorkingData(tclsASAM.milstCharacteristicList[iSparkCGSequenceIDX].u32Address,
                    (UInt32)iSparkCGSequence);

                tclsDataPage.u32SetWorkingData(tclsASAM.milstCharacteristicList[iSparkDHSequenceIDX].u32Address,
                    (UInt32)iSparkDHSequence);

                tclsDataPage.u32SetWorkingData(tclsASAM.milstCharacteristicList[iSyncRepeatsIDX].u32Address,
                    (UInt32)iSyncRepeats);

                tclsDataPage.u8SetWorkingData(tclsASAM.milstCharacteristicList[iCylCountIDX].u32Address,
                    (byte)iCylCount);
            }
            catch
            {
                MessageBox.Show("An error occurred applying the new settings");
                boErr = true;
            }

            if (false == boErr)
            {
                Program.vNotifyProgramEvent(tenProgramEvent.enLoadCalibrationComplete, 0, "");
                this.Close();
            }      
        }

        private void textBoxSyncCount_TextChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("It is not recommended to change this setting - it is calculated from the sync points array");
        }
    }
}
