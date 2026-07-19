# Configuración de Tesseract OCR

## Archivos de idioma requeridos

Para que el OCR funcione correctamente, debe descargar los archivos de idioma de Tesseract y colocarlos en la carpeta `tessdata` dentro del directorio de ejecución de la aplicación.

### Pasos de instalación:

1. **Descargar el archivo de idioma español:**
   - URL: https://github.com/tesseract-ocr/tessdata/raw/main/spa.traineddata
   - O desde: https://github.com/tesseract-ocr/tessdata

2. **Crear la carpeta tessdata:**
   - En el directorio donde se ejecuta `IndexadorIA.exe`, crear una carpeta llamada `tessdata`
   - Ejemplo: `C:\Users\...\IndexadorIA\bin\Debug\net10.0-windows\tessdata\`

3. **Copiar el archivo:**
   - Colocar `spa.traineddata` dentro de la carpeta `tessdata`

### Estructura de archivos:

```
IndexadorIA.exe
├── tessdata/
│   └── spa.traineddata          ← Archivo de idioma español
├── pdfium.dll
└── ... (otros archivos)
```

### Idiomas adicionales (opcional):

Si desea procesar textos en otros idiomas, descargue los archivos correspondientes:
- Inglés: `eng.traineddata`
- Portugués: `por.traineddata`
- Etc.

### Verificación:

Al ejecutar el procesamiento de imágenes con OCR habilitado:
- ✅ Si encuentra tessdata: El OCR se ejecutará correctamente
- ❌ Si NO encuentra tessdata: Se generará un error indicando la ruta esperada

### Nota sobre Python/Tesseract standalone:

Como mencionaste que has tenido excelentes resultados con Tesseract mediante Python, esta implementación de .NET usa la librería `Tesseract` que internamente utiliza la misma API nativa de Tesseract. Los archivos traineddata son los mismos que usarías con Python.

---

**Importante:** Los archivos tessdata son archivos binarios grandes (~10-50 MB cada uno) y no se incluyen en el repositorio Git por su tamaño. Deben descargarse manualmente.
