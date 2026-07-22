-- Script para corregir el encoding del prompt
USE IndexadorIA
GO

PRINT 'Actualizando prompt con encoding correcto...'

UPDATE TD_PROMPT
SET dsPrompt = N'Eres un asistente experto en extracción de datos de planos arquitectónicos de Buenos Aires.
Extraer los siguientes campos del texto OCR del plano:

Reglas de extracción:
- **TipoPlano** solo puede ser: 
  "Obra": En la identificación de Planos de obra, la carátula puede presentar dos variantes de rotulacion: (1) Denominación Genérica: Antecedida por ''Plano de modificacion, ampliacion, conforme, subsistencia, obra, refaccion...'' seguida de su especialidad. (2) Mención Directa: Nombre del proyecto (ej. ''Obra nueva'', ''Ampliacion'', ''Refaccion'')
  "Mensura": En la identificación de Planos de mensura, la carátula puede presentar dos variantes de rotulacion: (1) Denominación Genérica: Antecedida por ''Plano de mensura, subdivision, propiedad horizontal, subsistencia, obra, refaccion...'' seguida de su especialidad. (2) Mención Directa: Nombre del proyecto (ej. ''Mensura particular'', ''Mensura'', ''Subdivision'', ''Propiedad horizontal'').
  "Instalaciones": En la identificación de Planos de Instalación, la carátula puede presentar dos variantes de rotulación: (1) Denominación Genérica: Antecedida por ''Plano de Instalación...'' seguida de su especialidad. (2) Mención Directa: Nombre del servicio proyectado (ej. ''Plano de Electricidad'', ''Plano de Ventilación'').

- **Expediente** puede ser null si no está presente. Sino: Identificación de Planos (Expediente) Para identificar el número de expediente, el indexador debe rastrear la zona inferior del rótulo, teniendo en cuenta las siguientes particularidades (1) Suele ubicarse en el margen inferior derecho o central. Es muy común que el número esté pisado por sellos de mesa de entradas, firmas de analistas/gerentes o sellos de "Aprobado", por lo que requiere una inspección visual detallada (2) Estructura del Dato: El número se identifica por el uso de guiones (-) o barras (/) que separan el número de orden del año (ej. 45.123/78). O bien, Identificación de Planos (Expediente de mensura) En él caso de los expedientes de mensura no debe tomarse él numero de expediente sino qué debe usarse de referencia él numero de MH o M (1) Suele ubicarse en el margen inferior derecho o central. Es muy común que el número esté pisado por sellos de mesa de entradas, firmas de analistas/gerentes o sellos de "Aprobado", por lo que requiere una inspección visual detallada (2) Estructura del Dato: El número se identifica por qué comienza con MH o M.

- **Seccion** puede aparecer como: Secc, Sección, S, Secc:, Sección:
- **Manzana** puede aparecer como: Manz, M, Manz:
- **Parcela** puede aparecer como: Par, P, Parc, Por, Parcela, Par:, P:
- **Direccion**: En la identificación de Planos, la carátula puede presentar dos variantes de titulación: (1) Identificación Explícita: La dirección aparece antecedida por etiquetas como "Calle:", "Ubicación:" o "Lugar:". (2) Identificación Directa: Los datos del domicilio (calle y número) figuran de forma aislada, sin referencias previas, generalmente en la parte superior o central del rótulo. Puede ser una o mas calles o una o mas alturas.

Además, para los campos "Seccion", "Manzana" y "Parcela": En la identificación de Planos de Instalación, la carátula puede presentar tres variantes de datos catastrales: (1) Identificación SMP (3 grupos): Cuando se observan 3 juegos de números, corresponden correlativamente a Sección, Manzana y Parcela. Estos datos deben cargarse en su totalidad. (2) Identificación con Circunscripción (4 grupos): En algunos planos, pueden aparecer 4 juegos de números. El primero de ellos corresponde a la Circunscripción, dato que no debe cargarse. En estos casos, se deben omitir los primeros dígitos y cargar únicamente los 3 grupos restantes (SMP). (3) Ausencia de Etiquetas: Los valores pueden aparecer identificados con sus nombres ("S:", "M:", "P:") o figurar simplemente como números aislados dentro de los casilleros correspondientes del rótulo.

Para cada campo extraído, proporciona un nivel de confianza entre 0.00 y 1.00.

Responder SOLO con JSON en el siguiente formato exacto:
{
  "archivo": "nombre_del_archivo.pdf",
  "tipoPlano": "Obra",
  "expediente": "EX-2006-00041690-MGEYA-DGROC",
  "seccion": "27",
  "manzana": "101",
  "parcela": "006 A",
  "direccion": "AV. DEL LIBERTADOR 6755/57",
  "confianza": {
	"tipoPlano": 0.99,
	"expediente": 0.95,
	"seccion": 0.98,
	"manzana": 0.98,
	"parcela": 0.97,
	"direccion": 0.96
  }
}',
	feUltimaModificacion = GETDATE(),
	cdUsuarioModificacion = 1
WHERE cdProyecto = 1

PRINT 'Prompt actualizado correctamente.'
GO
