using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LoadDicom : MonoBehaviour
{

    public static List<Texture2D> listaTexturas;
    public Material material;

    private string[] extensions = { "dicom file", "dcm"};
    string error;

    void Start()
    {
        listaTexturas  = new List<Texture2D>();
    }
    [ContextMenu("ReadDicom")]
    
    public void ReadDicom(){
        var selectedpath = "";
        #if UNITY_EDITOR
        selectedpath = EditorUtility.OpenFilePanelWithFilters("Selecciona un archivo dicom", "", extensions);
        #endif
        #if UNITY_STANDALONE
        //TODO WITH WINDOWS FILE SYSTEM
        #endif

        if(!File.Exists(selectedpath)) return;
        string nombre, numero, extension;
        DividirNombreArchivo(selectedpath, out nombre, out numero, out extension);
        int i = 1;
        var path = nombre + i + extension;
        while(File.Exists(path)){
            //Cargamos la imagen en una textura2D
            var image = new Dicom.Imaging.DicomImage(path);
            var tex2d = image.RenderImage().As<Texture2D>();

            listaTexturas.Add(tex2d);

            //Seguimos iterando en todos los archivos
            i++;
            path = nombre + i + extension;
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
