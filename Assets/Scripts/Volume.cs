using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if UNITY_STANDALONE
using System.Windows.Forms;
#endif
public class Volume : MonoBehaviour
{
    //Para establecer la rotacion del Volumen
    public void setRotationY(float value){
        Vector3 rotacionActual = transform.eulerAngles;
        rotacionActual.y = value-180;
        transform.eulerAngles = rotacionActual;
    }
    public void setRotationX(float value){
        Vector3 rotacionActual = transform.eulerAngles;
        rotacionActual.x = value-90;
        transform.eulerAngles = rotacionActual;
    }

    //Para el material del volumen
    public Material material;

    /* TRANSFER FUNCTION*/
    public Texture2D[] textures;

    public void setTexture(int value){
        var texture_ = textures[value];
        texture_.Apply(false);
        material.SetTexture("_Transfer", texture_);
    }
    
    private string[] extensionsPng = { "png file", "png"};
    public void readTexture(){
        string selectedpath = "";
#if UNITY_EDITOR
        selectedpath = EditorUtility.OpenFilePanelWithFilters("Selecciona un archivo png", "", extensionsPng);
#elif UNITY_STANDALONE
        using (OpenFileDialog openFileDialog = new OpenFileDialog())
        {
            openFileDialog.Filter = "Archivos PNG (*.png)|*.png";
            openFileDialog.Title = "Selecciona un archivo PNG";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedpath = openFileDialog.FileName;
            }
        }
#endif
        changeTexture(selectedpath);
    }

    void changeTexture(string path){
        if(!File.Exists(path)) return;

        // Cargar la imagen PNG en un arreglo de bytes
        byte[] imageBytes = System.IO.File.ReadAllBytes(path);

        // Crear un objeto Texture2D
        Texture2D texture = new Texture2D(2, 2);

        // Cargar la imagen en el objeto Texture2D
        texture.LoadImage(imageBytes);

        // Asignar la textura a un material o componente que la requiera
        material.SetTexture("_Transfer", texture);
    }

    /* VOLUMEN */
    public Texture3D[] volumes;

    public void setVolume(int value){
        var texture_ = volumes[value];
        texture_.Apply(false);
        material.SetTexture("_Volume", texture_);
    }


    public static List<Texture2D> listaTexturas;

    private string[] extensionsDicom = { "dicom file", "dcm"};
    string error;

    void Start()
    {
        listaTexturas  = new List<Texture2D>();
    }
    public void ReadDicom(){
        string selectedpath = "";
#if UNITY_EDITOR
        selectedpath = EditorUtility.OpenFilePanelWithFilters("Selecciona un archivo DICOM", "", extensionsDicom);
#elif UNITY_STANDALONE
        using (OpenFileDialog openFileDialog = new OpenFileDialog())
        {
            openFileDialog.Filter = "Archivos DICOM (*.dcm)|*.dcm";
            openFileDialog.Title = "Selecciona un archivo DICOM";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedpath = openFileDialog.FileName;
            }
        }
#endif
        changeVolume(selectedpath);
        }

    private void changeVolume(string path){
        if(!File.Exists(path)) return;
        string nombre, numero, extension;
        DividirNombreArchivo(path, out nombre, out numero, out extension);
        int i = 1;
        var newpath = nombre + i + extension;
        while(File.Exists(newpath)){
            //Cargamos la imagen en una textura2D
            var image = new Dicom.Imaging.DicomImage(newpath);
            var tex2d = image.RenderImage().As<Texture2D>();

            listaTexturas.Add(tex2d);

            //Seguimos iterando en todos los archivos
            i++;
            newpath = nombre + i + extension;
        }
        if(!Convert()) Debug.Log(error);
    }

    private void DividirNombreArchivo(string nombreArchivo, out string nombre, out string numero, out string extension)
    {
        int primerDigito = EncuentraPrimerDigito(nombreArchivo);
        int ultimoDigito = EncuentraUltimoDigito(nombreArchivo);

        nombre = nombreArchivo.Substring(0, primerDigito);
        numero = nombreArchivo.Substring(primerDigito, ultimoDigito - primerDigito + 1);
        extension = nombreArchivo.Substring(ultimoDigito + 1);
    }

    private int EncuentraPrimerDigito(string cadena)
    {
        for (int i = 0; i < cadena.Length; i++)
        {
            if (char.IsDigit(cadena[i]))
            {
                return i;
            }
        }
        return -1;
    }

    private int EncuentraUltimoDigito(string cadena)
    {
        for (int i = cadena.Length - 1; i >= 0; i--)
        {
            if (char.IsDigit(cadena[i]))
            {
                return i;
            }
        }
        return -1;
    }

    bool Convert()
    {
        listaTexturas.Reverse();
        if (listaTexturas.Count == 0)
        {
            error = "no image";
            return false;
        }

        var w = listaTexturas[0].width;
        var h = listaTexturas[0].height;
        var d = listaTexturas.Count;
        var format = listaTexturas[0].format;
        var colors = new Color32[w * h * d];

        for (int i = 0; i < d; ++i)
        {
            var tex2d = listaTexturas[i];
            if (tex2d.width != w || tex2d.height != h)
            {
                error = "texture size error";
                return false;
            }
            if (tex2d.format != format)
            {
                error = "texture format error";
                return false;
            }
            tex2d.GetPixels32().CopyTo(colors, w * h * i);
        }

        Texture3D tex3d = new Texture3D(w, h, d, format, false);
        tex3d.SetPixels32(colors);
        tex3d.Apply();
        
        string nombrePropiedad = "_Volume";

        // Asignar la nueva textura al material
        material.SetTexture(nombrePropiedad, tex3d);
        return true;
    }
}
