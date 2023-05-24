using ExtractPdf.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtractPdf
{
    public partial class frmPrincipal : Form
    {
        Funciones confunci = new Funciones();

        private String[] rutasdoctospdf = new String[] { }; //Arreglo de rutas de pdfs dentro del folder seleccionado

        List<String> scanspdfs = new List<String>();
        private String[] scanspdfsarr = new String[] { }; //Arreglo de rutas de pdfs dentro del folder seleccionado


        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
                textBox1.Enabled = iconButton1.Enabled = true;  
            else
                textBox1.Enabled = iconButton1.Enabled = false; textBox1.Text = "";
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            //--Limpiamos labes--//
            this.label1.Text = "";
            this.label2.Text = "";
            this.label3.Text = "";
            this.groupBox1.Text = "Name folder";

            this.textBox1.Enabled = false;
            this.iconButton1.Enabled = false;
            this.checkBox2.Checked = true;

            //--Para cargar carpeta con todos los pdfs y subcarpetas--//
            this.checkBox3.Enabled = false;
            this.textBox2.Enabled = false;
            this.iconButton4.Enabled = false;

        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            //confunci.textoentreparentesis(confunci.Name_Folder);
            //string ponumeros = confunci.Tex_Parentesis;

            //string abc = string.Empty;
            //string namefolder = confunci.textohastacaracter(confunci.Name_Folder, "(");
            //confunci.extraedatanamefolder(namefolder);

            //foreach (var elemt in confunci.namesfolders)
            //{
            //    abc += $"{elemt} - {ponumeros}";
            //}

            //this.label3.Text = abc;
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            confunci.getpathfolder();
            textBox1.Text = confunci.Path_Folder;
            groupBox1.Text = $"Información de la carpeta";
            label1.Text = $"Nombre: {confunci.Name_Folder}";
            label2.Text = $"Elementos en la carpeta: {confunci.Len_Folder.ToString()}";


            confunci.textoentreparentesis(confunci.Name_Folder);
            string ponumeros = confunci.Tex_Parentesis;

            string abc = string.Empty;
            string namefolder = confunci.textohastacaracter(confunci.Name_Folder, "(");
            confunci.extraedatanamefolder(namefolder);

            foreach (var elemt in confunci.namesfolders)
            {
                abc += $"{elemt} - {ponumeros} \n";
            }
            this.label3.Text = abc;

            //--Obtenemos arreglo de rutas de los pdf dentro de la carpeta seleccionada--//
            rutasdoctospdf = confunci.rutaspdfolder(confunci.Path_Folder, confunci.Len_Folder);
           

            //--Aqui tenemos la lista con los datos de los pdf escaneados--//
            scanspdfs = confunci.scandoctosdelfolder(rutasdoctospdf);//*******scanspds = CONTIENDE LOS DATOS DE LOS PDFS ESCANEADOS********//


            scanspdfsarr = scanspdfs.ToArray();
            dataGridView2.DataSource = scanspdfsarr;
            MessageBox.Show("assa");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
