using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using WindowsFormsApp1;
using ExtendedRichTextBox;
using System.IO;
namespace PHONE
{
    public partial class Form1 : Form
    {
        public const String MYPHONENUMBER = "3860070379797";
        public static Form1 f1 = null;
        public static Form2 f2 = null;
        public const int MAX = 1000000;
        public static String ProgramEXEPath = null;
        public static String DataPath = null;


        public class CPHONE_NUMBER
        {
            public String PhoneNumber;
            public IPAddress ip;
        }

        public static CPHONE_NUMBER[] PhoneNumbers;

        public static IPAddress[] Servers;


        private string currentFile;
        public Form1()
        {
            InitializeComponent();
            f1 = this;

            currentFile = "";
            this.Text = "Editor: New Document";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadSettings();
            PhoneNumbers = (CPHONE_NUMBER[])cs.LoadFile(sender, e, "PHONE NUMBERS.bin");
            if (PhoneNumbers == null)
            {
                PhoneNumbers = new CPHONE_NUMBER[MAX];
                PhoneNumbers[0] = new CPHONE_NUMBER();
                PhoneNumbers[0].PhoneNumber = MYPHONENUMBER;
                PhoneNumbers[0].ip = IPAddress.Loopback;
            }
            F2Show(sender, e);
            Centrala.Server.ServerPort65534();
            Centrala.ExchangePhoneNumbersWithAllP2PNetwork();
            DisplayAllPhoneNumbers(sender, e);
        }

        private void F2Show(object sender, EventArgs e)
        {
            if (f2 == null)
            {
                f2 = new Form2(f1);
                f2.ShowDialog(f1);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            cs.SaveFile(sender, e, "PHONE NUMBERS.bin", PhoneNumbers);
        }

        private void NewToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (rtbDoc.Modified == true)
                {
                    System.Windows.Forms.DialogResult answer;
                    answer = MessageBox.Show("Save current document before creating new document?", "Unsaved Document", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (answer == System.Windows.Forms.DialogResult.No)
                    {
                        currentFile = "";
                        this.Text = "Editor: New Document";
                        rtbDoc.Modified = false;
                        rtbDoc.Clear();
                        return;
                    }
                    else
                    {
                        SaveToolStripMenuItem_Click(this, new EventArgs());
                        rtbDoc.Modified = false;
                        rtbDoc.Clear();
                        currentFile = "";
                        this.Text = "Editor: New Document";
                        return;
                    }
                }
                else
                {
                    currentFile = "";
                    this.Text = "Editor: New Document";
                    rtbDoc.Modified = false;
                    rtbDoc.Clear();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }




        private void OpenToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (rtbDoc.Modified == true)
                {
                    System.Windows.Forms.DialogResult answer;
                    answer = MessageBox.Show("Save current file before opening another document?", "Unsaved Document", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (answer == System.Windows.Forms.DialogResult.No)
                    {
                        rtbDoc.Modified = false;
                        OpenFile();
                    }
                    else
                    {
                        SaveToolStripMenuItem_Click(this, new EventArgs());
                        OpenFile();
                    }
                }
                else
                {
                    OpenFile();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }




        private void OpenFile()
        {
            try
            {
                OpenFileDialog1.Title = "RTE - Open File";
                OpenFileDialog1.DefaultExt = "rtf";
                OpenFileDialog1.Filter = "Rich Text Files|*.rtf|Text Files|*.txt|HTML Files|*.htm|All Files|*.*";
                OpenFileDialog1.FilterIndex = 1;
                OpenFileDialog1.FileName = string.Empty;

                if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
                {

                    if (OpenFileDialog1.FileName == "")
                    {
                        return;
                    }

                    string strExt;
                    strExt = System.IO.Path.GetExtension(OpenFileDialog1.FileName);
                    strExt = strExt.ToUpper();

                    if (strExt == ".RTF")
                    {
                        rtbDoc.LoadFile(OpenFileDialog1.FileName, RichTextBoxStreamType.RichText);
                    }
                    else
                    {
                        System.IO.StreamReader txtReader;
                        txtReader = new System.IO.StreamReader(OpenFileDialog1.FileName);
                        rtbDoc.Text = txtReader.ReadToEnd();
                        txtReader.Close();
                        txtReader = null;
                        rtbDoc.SelectionStart = 0;
                        rtbDoc.SelectionLength = 0;
                    }

                    currentFile = OpenFileDialog1.FileName;
                    rtbDoc.Modified = false;
                    this.Text = "Editor: " + currentFile.ToString();
                }
                else
                {
                    MessageBox.Show("Open File request cancelled by user.", "Cancelled");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }




        private void SaveToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (currentFile == string.Empty)
                {
                    SaveAsToolStripMenuItem_Click(this, e);
                    return;
                }

                try
                {
                    string strExt;
                    strExt = System.IO.Path.GetExtension(currentFile);
                    strExt = strExt.ToUpper();
                    if (strExt == ".RTF")
                    {
                        rtbDoc.SaveFile(currentFile);
                    }
                    else
                    {
                        System.IO.StreamWriter txtWriter;
                        txtWriter = new System.IO.StreamWriter(currentFile);
                        txtWriter.Write(rtbDoc.Text);
                        txtWriter.Close();
                        txtWriter = null;
                        rtbDoc.SelectionStart = 0;
                        rtbDoc.SelectionLength = 0;
                    }

                    this.Text = "Editor: " + currentFile.ToString();
                    rtbDoc.Modified = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "File Save Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }


        }


        private void SaveAsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {

            try
            {
                SaveFileDialog1.Title = "RTE - Save File";
                SaveFileDialog1.DefaultExt = "rtf";
                SaveFileDialog1.Filter = "Rich Text Files|*.rtf|Text Files|*.txt|HTML Files|*.htm|All Files|*.*";
                SaveFileDialog1.FilterIndex = 1;

                if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
                {

                    if (SaveFileDialog1.FileName == "")
                    {
                        return;
                    }

                    string strExt;
                    strExt = System.IO.Path.GetExtension(SaveFileDialog1.FileName);
                    strExt = strExt.ToUpper();

                    if (strExt == ".RTF")
                    {
                        rtbDoc.SaveFile(SaveFileDialog1.FileName, RichTextBoxStreamType.RichText);
                    }
                    else
                    {
                        System.IO.StreamWriter txtWriter;
                        txtWriter = new System.IO.StreamWriter(SaveFileDialog1.FileName);
                        txtWriter.Write(rtbDoc.Text);
                        txtWriter.Close();
                        txtWriter = null;
                        rtbDoc.SelectionStart = 0;
                        rtbDoc.SelectionLength = 0;
                    }

                    currentFile = SaveFileDialog1.FileName;
                    rtbDoc.Modified = false;
                    this.Text = "Editor: " + currentFile.ToString();
                    MessageBox.Show(currentFile.ToString() + " saved.", "File Save");
                }
                else
                {
                    MessageBox.Show("Save File request cancelled by user.", "Cancelled");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }




        private void ExitToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (rtbDoc.Modified == true)
                {
                    System.Windows.Forms.DialogResult answer;
                    answer = MessageBox.Show("Save this document before closing?", "Unsaved Document", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (answer == System.Windows.Forms.DialogResult.Yes)
                    {
                        return;
                    }
                    else
                    {
                        rtbDoc.Modified = false;
                        Application.Exit();
                    }
                }
                else
                {
                    rtbDoc.Modified = false;
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }




        private void SelectAllToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                rtbDoc.SelectAll();
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to select all document content.", "RTE - Select", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void CopyToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                rtbDoc.Copy();
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to copy document content.", "RTE - Copy", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void CutToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                rtbDoc.Cut();
            }
            catch
            {
                MessageBox.Show("Unable to cut document content.", "RTE - Cut", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void PasteToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                rtbDoc.Paste();
            }
            catch
            {
                MessageBox.Show("Unable to copy clipboard content to document.", "RTE - Paste", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void SelectFontToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!(rtbDoc.SelectionFont == null))
                {
                    FontDialog1.Font = rtbDoc.SelectionFont;
                }
                else
                {
                    FontDialog1.Font = null;
                }
                FontDialog1.ShowApply = true;
                if (FontDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    rtbDoc.SelectionFont = FontDialog1.Font;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }




        private void FontColorToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                ColorDialog1.Color = rtbDoc.ForeColor;
                if (ColorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    rtbDoc.SelectionColor = ColorDialog1.Color;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }




        private void BoldToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!(rtbDoc.SelectionFont == null))
                {
                    System.Drawing.Font currentFont = rtbDoc.SelectionFont;
                    System.Drawing.FontStyle newFontStyle;

                    newFontStyle = rtbDoc.SelectionFont.Style ^ FontStyle.Bold;

                    rtbDoc.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newFontStyle);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }




        private void ItalicToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!(rtbDoc.SelectionFont == null))
                {
                    System.Drawing.Font currentFont = rtbDoc.SelectionFont;
                    System.Drawing.FontStyle newFontStyle;

                    newFontStyle = rtbDoc.SelectionFont.Style ^ FontStyle.Italic;

                    rtbDoc.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newFontStyle);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }





        private void UnderlineToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!(rtbDoc.SelectionFont == null))
                {
                    System.Drawing.Font currentFont = rtbDoc.SelectionFont;
                    System.Drawing.FontStyle newFontStyle;

                    newFontStyle = rtbDoc.SelectionFont.Style ^ FontStyle.Underline;

                    rtbDoc.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newFontStyle);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }





        private void NormalToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!(rtbDoc.SelectionFont == null))
                {
                    System.Drawing.Font currentFont = rtbDoc.SelectionFont;
                    System.Drawing.FontStyle newFontStyle;
                    newFontStyle = FontStyle.Regular;
                    rtbDoc.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newFontStyle);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }




        private void PageColorToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                ColorDialog1.Color = rtbDoc.BackColor;
                if (ColorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    rtbDoc.BackColor = ColorDialog1.Color;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }




        private void mnuUndo_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (rtbDoc.CanUndo)
                {
                    rtbDoc.Undo();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }




        private void mnuRedo_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (rtbDoc.CanRedo)
                {
                    rtbDoc.Redo();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }




        private void LeftToolStripMenuItem_Click_1(object sender, System.EventArgs e)
        {
            try
            {
                rtbDoc.SelectionAlignment = HorizontalAlignment.Left;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }




        private void CenterToolStripMenuItem_Click_1(object sender, System.EventArgs e)
        {
            try
            {
                rtbDoc.SelectionAlignment = HorizontalAlignment.Center;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }




        private void RightToolStripMenuItem_Click_1(object sender, System.EventArgs e)
        {
            try
            {
                rtbDoc.SelectionAlignment = HorizontalAlignment.Right;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }




        private void AddBulletsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                rtbDoc.BulletIndent = 10;
                rtbDoc.SelectionBullet = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }




        private void RemoveBulletsToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                rtbDoc.SelectionBullet = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }




        private void mnuIndent0_Click(object sender, System.EventArgs e)
        {
            try
            {
                rtbDoc.SelectionIndent = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }




        private void mnuIndent5_Click(object sender, System.EventArgs e)
        {
            try
            {
                rtbDoc.SelectionIndent = 5;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }




        private void mnuIndent10_Click(object sender, System.EventArgs e)
        {
            try
            {
                rtbDoc.SelectionIndent = 10;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }




        private void mnuIndent15_Click(object sender, System.EventArgs e)
        {
            try
            {
                rtbDoc.SelectionIndent = 15;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }




        private void mnuIndent20_Click(object sender, System.EventArgs e)
        {
            try
            {
                rtbDoc.SelectionIndent = 20;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }

        }




        private void FindToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            /*
            try
            {
                frmFind f = new frmFind(this);
                f.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
            */
        }




        private void FindAndReplaceToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            /*
            try
            {
                frmReplace f = new frmReplace(this);
                f.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
            */
        }




        private void PreviewToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            /*
            try
            {
                PrintPreviewDialog1.Document = PrintDocument1;
                PrintPreviewDialog1.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
            */
        }




        private void PrintToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            /*
            try
            {
                PrintDialog1.Document = PrintDocument1;
                if (PrintDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    PrintDocument1.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
            */
        }




        private void mnuPageSetup_Click(object sender, System.EventArgs e)
        {
            /*
            try
            {
                PageSetupDialog1.Document = PrintDocument1;
                PageSetupDialog1.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
            */
        }




        private void InsertImageToolStripMenuItem_Click(object sender, System.EventArgs e)
        {

            OpenFileDialog1.Title = "RTE - Insert Image File";
            OpenFileDialog1.DefaultExt = "rtf";
            OpenFileDialog1.Filter = "Bitmap Files|*.bmp|JPEG Files|*.jpg|GIF Files|*.gif";
            OpenFileDialog1.FilterIndex = 1;
            OpenFileDialog1.ShowDialog();

            if (OpenFileDialog1.FileName == "")
            {
                return;
            }

            try
            {
                string strImagePath = OpenFileDialog1.FileName;
                Image img;
                img = Image.FromFile(strImagePath);
                Clipboard.SetDataObject(img);
                DataFormats.Format df;
                df = DataFormats.GetFormat(DataFormats.Bitmap);
                if (this.rtbDoc.CanPaste(df))
                {
                    this.rtbDoc.Paste(df);
                }
            }
            catch
            {
                MessageBox.Show("Unable to insert image format selected.", "RTE - Paste", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void rtbDoc_SelectionChanged(object sender, EventArgs e)
        {

        }


        private void tbrSave_Click(object sender, System.EventArgs e)
        {
            SaveToolStripMenuItem_Click(this, e);
        }


        private void tbrOpen_Click(object sender, System.EventArgs e)
        {
            OpenToolStripMenuItem_Click(this, e);
        }


        private void tbrNew_Click(object sender, System.EventArgs e)
        {
            NewToolStripMenuItem_Click(this, e);
        }


        private void tbrBold_Click(object sender, System.EventArgs e)
        {
            BoldToolStripMenuItem_Click(this, e);
        }


        private void tbrItalic_Click(object sender, System.EventArgs e)
        {
            ItalicToolStripMenuItem_Click(this, e);
        }


        private void tbrUnderline_Click(object sender, System.EventArgs e)
        {
            UnderlineToolStripMenuItem_Click(this, e);
        }


        private void tbrFont_Click(object sender, System.EventArgs e)
        {
            SelectFontToolStripMenuItem_Click(this, e);
        }


        private void tbrLeft_Click(object sender, System.EventArgs e)
        {
            rtbDoc.SelectionAlignment = HorizontalAlignment.Left;
        }


        private void tbrCenter_Click(object sender, System.EventArgs e)
        {
            rtbDoc.SelectionAlignment = HorizontalAlignment.Center;
        }


        private void tbrRight_Click(object sender, System.EventArgs e)
        {
            rtbDoc.SelectionAlignment = HorizontalAlignment.Right;
        }


        private void tbrFind_Click(object sender, System.EventArgs e)
        {
            /*
            frmFind f = new frmFind(this);
            f.Show();
            */
        }


        private void tspColor_Click(object sender, EventArgs e)
        {
            FontColorToolStripMenuItem_Click(this, new EventArgs());
        }


        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (rtbDoc.Modified == true)
                {
                    System.Windows.Forms.DialogResult answer;
                    answer = MessageBox.Show("Save current document before exiting?", "Unsaved Document", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (answer == System.Windows.Forms.DialogResult.No)
                    {
                        rtbDoc.Modified = false;
                        rtbDoc.Clear();
                        return;
                    }
                    else
                    {
                        SaveToolStripMenuItem_Click(this, new EventArgs());
                    }
                }
                else
                {
                    rtbDoc.Clear();
                }
                currentFile = "";
                this.Text = "Editor: New Document";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            InsertImageToolStripMenuItem_Click(sender, e);
        }

        public void LoadSettings()
        {
            ProgramEXEPath = Application.StartupPath;         
            DataPath = (ProgramEXEPath + "\\Data\\");
            if (!(Directory.Exists(DataPath)))
            {
                Directory.CreateDirectory(DataPath);
            }
        }

        public void DisplayAllPhoneNumbers(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            for (int i = 0; i < MAX; i++)
            {
                if (PhoneNumbers != null)
                {
                    if (PhoneNumbers[i] != null)
                    {
                        if ((PhoneNumbers[i].PhoneNumber != null) && (PhoneNumbers[i].PhoneNumber != ""))
                        {
                            comboBox1.Items.Add(PhoneNumbers[i].PhoneNumber);
                        }
                    }
                }
            }
        }
    }
}
