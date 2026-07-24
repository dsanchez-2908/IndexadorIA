# Configuración de Tesseract OCR

## Archivos de idioma requeridos

Para que el OCR funcione correctamente, debe descargar los archivos de idioma de Tesseract y colocarlos en la carpeta `tessdata` dentro del directorio de ejecución de la aplicación.

### Pasos de instalación:

1. **Descargar el archivo de idioma español:**
   - URL: https://github.com/tesseract-ocr/tessdata/raw/main/spa.traineddata
   - O desde: https://github.com/tesseract-ocr/tessdata

2. **(Opcional) Archivo de idioma OSD:** ya no es necesario para el giro automático de páginas (ver sección "Giro automático de páginas" más abajo). Solo agréguelo si desea usar el NuGet de Tesseract con OSD por algún otro motivo.

3. **Crear la carpeta tessdata en el proyecto:**
   - Colocar los archivos en `IndexadorIA/tessdata/` (ya versionada en el proyecto y configurada en el `.csproj` para copiarse al directorio de salida).
   - Alternativamente, colocarlos directamente en el directorio de ejecución: `IndexadorIA/bin/Debug/net10.0-windows/tessdata/`

4. **Copiar los archivos:**
   - `spa.traineddata` dentro de la carpeta `tessdata`

### Estructura de archivos:

```
IndexadorIA.exe
├── tessdata/
│   └── spa.traineddata          ← Archivo de idioma español (OCR)
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

### Giro automático de páginas (FrmSeparacionImagenes):

El giro automático **ya no usa el NuGet de Tesseract** (resultó impreciso). En su lugar, invoca directamente el ejecutable **`tesseract.exe` instalado en el sistema** (el mismo que usas manualmente por consola / con Python y `ocrmypdf`), ejecutando `tesseract <imagen> <salida> --psm 0` para obtener la orientación real de la página.

Requisitos para que el giro automático funcione:
- Tener **Tesseract OCR** instalado en la PC donde corre la aplicación (https://github.com/tesseract-ocr/tesseract/releases), con la opción "Add Tesseract to PATH" marcada.
- Tener el idioma **Spanish (spa)** instalado (el `--psm 0` usa el modelo `osd.traineddata` que ya viene incluido en la instalación oficial de Tesseract, no requiere descarga aparte).
- Verificar la instalación con `tesseract --version` desde la consola.

Si `tesseract.exe` no se encuentra en el PATH ni en las rutas de instalación típicas (`C:\Program Files\Tesseract-OCR\`), se registra un WARNING en el log y se omite el giro (no se rota ninguna página), igual que antes.

### Nota sobre Python/Tesseract standalone:

Como mencionaste que has tenido excelentes resultados con Tesseract mediante Python, esta implementación de .NET usa la librería `Tesseract` que internamente utiliza la misma API nativa de Tesseract. Los archivos traineddata son los mismos que usarías con Python.

---

**Importante:** Los archivos tessdata son archivos binarios grandes (~10-50 MB cada uno) y no se incluyen en el repositorio Git por su tamaño. Deben descargarse manualmente.
