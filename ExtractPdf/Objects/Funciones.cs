using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace ExtractPdf.Objects
{
    public class Funciones
    {

        public string[] namesfolders;

        private string ultimo_error;
        private string path_folder;
        private string name_folder;
        private string tex_parentesis;
        private int len_folder;

        public string Ultimo_Error { get { return ultimo_error; } }
        public string Path_Folder { get { return path_folder; } }
        public string Name_Folder { get { return name_folder; } }
        public string Tex_Parentesis { get { return tex_parentesis; } }
        public int Len_Folder { get { return len_folder; } }

        //--Por carpeta general--//

        private string dato_general;
        public string Dato_General { get { return dato_general; } set { dato_general = value; } }

        public string strAdate(String str)
        {
            DateTime d = Convert.ToDateTime(str);
            return d.ToString();
        }

        public String getpathfolder()
        {
            //creamos ofd para seleccionar archivo
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "All Files (*.*)|*.*";
            ofd.Multiselect = true;
            ofd.CheckFileExists = false;
            ofd.CheckPathExists = true;
            ofd.FileName = "Select folder";

            ////mostramos ofd
            //DialogResult result = ofd.ShowDialog();

            //if (result == DialogResult.OK)
            //{
            //    //obtener la ruta openfiledialog
            //    path_folder = ofd.FileName;

            //    name_folder = namelastfolder(path_folder);

            //    path_folder = textohastacaracter(path_folder, "\\");
                
            //}

            //usamos FolderBrowserDialog para poder obtener la ruta del folder
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.SelectedPath = System.IO.Path.GetDirectoryName(path_folder);
                DialogResult folderesult = fbd.ShowDialog();

                if (folderesult == DialogResult.OK)
                {
                    //Obtenemos ruta de la carpeta
                    path_folder = fbd.SelectedPath;

                    name_folder = namelastfolder(path_folder);

                    //path_folder = textohastacaracter(path_folder, "\\");

                    len_folder = lenConfolder(path_folder);
                }
            }

            return path_folder;
        }

        //--Comienza a buscar el caracter desde el principio--//
        public string textohastacaracter(String cadena, String caracter)
        {
            string retcadena = string.Empty;
            int indicecaracter = cadena.IndexOf(caracter); //enpieza a buscar el caracter desde el final

            if (indicecaracter >= 0)
            {
                retcadena = cadena.Substring(0, indicecaracter);
            }
            return retcadena;
        }

        //--Comienza a buscar el caracter desde el final--//
        public string textodesdecaracter(String cadena, String caracter)
        {
            string retcadena = string.Empty;

            int indicecaracter = cadena.LastIndexOf(caracter); //enpieza a buscar el caracter desde el final

            if (indicecaracter >= 0)
            {
                retcadena = cadena.Substring(0, indicecaracter);
            }
            return retcadena;
        }
        public string namelastfolder(String path)
        {
            string namefol = string.Empty;

            //Se obtiene la ruta del directorio padre de la ruta del archivo y utilizando el método Path.GetFileName, se obtiene el nombre del último folder en esa ruta.
            //namefol = Path.GetFileName(Path.GetDirectoryName(path));
            namefol = System.IO.Path.GetFileName(path.TrimEnd(System.IO.Path.DirectorySeparatorChar));

            return namefol;
        }
        public int lenConfolder(String path)
        {
            int countfol;

            //Se obtiene la ruta del directorio padre de la ruta del archivo y utilizando el método Path.GetFileName, se obtiene el nombre del último folder en esa ruta.
            countfol = Directory.GetFileSystemEntries(path).Length;

            return countfol;
        }
        public void textoentreparentesis(String texto)
        {
            string patron = @"\((.*?)\)";

            MatchCollection coincidencias = Regex.Matches(texto, patron);

            foreach (Match coincidencia in coincidencias)
            {
                tex_parentesis = coincidencia.Groups[1].Value;
                
            }
        }
        //--Crea lista de textos de en donde son separados por una coma y una y al final--//
        public void extraedatanamefolder(String namefolder)
        {
            string copytoname = namefolder;
            
            string[] anames = copytoname.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] bnames = anames[anames.Length - 1].Split(new char[] { 'y' }, StringSplitOptions.RemoveEmptyEntries);
            string[] cnames = anames.Where(e => e != anames[anames.Length - 1]).ToArray();

            namesfolders = cnames.Concat(bnames).ToArray();
            
        }
        //--Extrae rutas de los pdf que estan dentro de una carpeta en la ruta dada--//
        public String[] rutaspdfolder(String pathfolder, int lendoctos)
        {
            String[] paths = new string[lendoctos];

            paths = Directory.GetFiles(pathfolder, "*.pdf");

            return paths;
        }
        //--Scanner pdf-escanea todo el texto del pdf--//
        public List<String> scandoctosdelfolder(String[] rutasdedoctos)
        {
            List<String> textospdfs = new List<String>();
            int lendoctos = rutasdedoctos.Length;
            //string textosdocto = string.Empty;

            //--Definimos el total de doctos a leer--//
            for (int i = 0; i < lendoctos; i++)
            {
                textospdfs.Add(scanpdfonlylines(rutasdedoctos[i]));
            }

            return textospdfs;
        }
        //--Scanea--//
        public String scanpdfonly(String pathpdf)
        {
            string textpd = string.Empty;

            using (PdfReader read = new PdfReader(pathpdf))
            {
                //--Recorremos cada hoja del documento y extraemos su texto de hoja--//
                for (int i = 1; i <= read.NumberOfPages; i++)
                {
                    ITextExtractionStrategy ites = new SimpleTextExtractionStrategy();
                    string pagetext = PdfTextExtractor.GetTextFromPage(read, i, ites);
                    textpd += pagetext;
                }
            }

            return textpd;
        }

        public String scanpdfonlylines(String pathpdf)
        {
            StringBuilder sb = new StringBuilder();
            string textpd = string.Empty;

            using (PdfReader read = new PdfReader(pathpdf))
            {
                //--Recorremos cada hoja del documento y extraemos su texto de hoja--//
                for (int i = 1; i <= read.NumberOfPages; i++)
                {
                    ITextExtractionStrategy ites = new SimpleTextExtractionStrategy();
                    string pagetext = PdfTextExtractor.GetTextFromPage(read, i, ites);

                    // Separar el texto en líneas
                    string[] lineas = pagetext.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                    // Iterar sobre cada línea y hacer algo con ella
                    foreach (string linea in lineas)
                    {
                        // Hacer algo con cada línea (por ejemplo, mostrarla en la consola)
                        textpd += linea;
                        //Console.WriteLine(linea);
                    }
                    //textpd += pagetext;
                }
            }

            return textpd;
        }
    }
}
