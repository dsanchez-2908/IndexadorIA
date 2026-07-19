# Corrección: Error de Conversión Int64 a Int32

## 🐛 Problema Reportado

**Error al crear lotes:**
```
Error al crear lote: Unable to cast object of type System.Int64 to type System.Int32
```

---

## 🔍 Causa Raíz

### SQL Server devuelve tipos diferentes según el contexto:

1. **Secuencias (`SEQUENCE`)**: Devuelven `BIGINT` (equivalente a `Int64` en C#)
2. **Identity con `OUTPUT INSERTED.cdLote`**: También puede devolver `BIGINT`

### El código original intentaba casting directo:

```csharp
// ❌ INCORRECTO - Falla si SQL Server devuelve BIGINT
return (int)comando.ExecuteScalar();
int cdLote = (int)cmdLote.ExecuteScalar();
```

Este casting directo (`(int)`) **falla** cuando el objeto es `Int64` porque no es compatible para conversión directa.

---

## ✅ Solución Implementada

Usar `Convert.ToInt32()` en lugar de casting directo:

```csharp
// ✅ CORRECTO - Maneja conversión de diferentes tipos numéricos
var resultado = comando.ExecuteScalar();
return Convert.ToInt32(resultado);

var resultadoLote = cmdLote.ExecuteScalar();
int cdLote = Convert.ToInt32(resultadoLote);
```

### ¿Por qué funciona?

- `Convert.ToInt32()` maneja conversiones entre tipos numéricos compatibles
- Convierte `Int64` → `Int32` de forma segura (si el valor cabe en Int32)
- También maneja `null`, `string`, `decimal`, etc.

---

## 📝 Archivos Modificados

### `IndexadorIA/03_Datos/LoteDAL.cs`

**Cambios realizados:**

1. **Método `ObtenerSiguienteSecuencia()`**:
   ```csharp
   // ANTES
   return (int)comando.ExecuteScalar();

   // DESPUÉS
   var resultado = comando.ExecuteScalar();
   return Convert.ToInt32(resultado);
   ```

2. **Método `CrearLote()`**:
   ```csharp
   // ANTES
   int cdLote = (int)cmdLote.ExecuteScalar();

   // DESPUÉS
   var resultadoLote = cmdLote.ExecuteScalar();
   int cdLote = Convert.ToInt32(resultadoLote);
   ```

---

## 🧪 Validación

### Compilación: ✅ Exitosa

La solución compila correctamente sin errores ni advertencias.

### Prueba funcional recomendada:

1. Ejecutar la aplicación
2. Ir a "Planos de GCBA" → "Preparación de Lotes"
3. Seleccionar algunos archivos
4. Configurar "Archivos por Lote" (ej: 5)
5. Hacer clic en "Crear Lotes"
6. **Verificar**: Los lotes deben crearse sin error

---

## 📊 Comparación de Métodos de Conversión

| Método | Int64 → Int32 | null | string | decimal | Recomendado |
|--------|---------------|------|--------|---------|-------------|
| `(int)` casting | ❌ Error | ❌ Error | ❌ Error | ❌ Error | ❌ No |
| `Convert.ToInt32()` | ✅ OK | ✅ 0 | ✅ OK* | ✅ OK | ✅ **Sí** |
| `int.Parse()` | ❌ Error | ❌ Error | ✅ OK* | ❌ Error | ⚠️ Solo strings |

*Siempre que el formato sea válido

---

## 💡 Buenas Prácticas

### Al trabajar con datos de SQL Server:

1. **Usar `Convert.ToXXX()` para conversiones de tipos numéricos**
   - Es más seguro y flexible
   - Maneja diferentes tipos de origen

2. **Evitar casting directo con `ExecuteScalar()`**
   - El tipo devuelto puede variar según la BD
   - IDENTITY, SEQUENCE, y funciones agregadas pueden devolver tipos inesperados

3. **Validar valores antes de convertir (opcional)**:
   ```csharp
   var resultado = comando.ExecuteScalar();
   if (resultado == null || resultado == DBNull.Value)
	   throw new Exception("No se pudo obtener la secuencia");
   return Convert.ToInt32(resultado);
   ```

---

## 🔄 Estado de los Problemas

| # | Problema | Estado | Observaciones |
|---|----------|--------|---------------|
| 1 | Contador seleccionados en cero | ✅ Resuelto | Eventos agregados al DataGridView |
| 2 | Error "Invalid object name 'SEQ_LOTE'" | ✅ Resuelto | Scripts SQL ejecutados |
| 3 | Error casting Int64 a Int32 | ✅ Resuelto | Usar Convert.ToInt32() |

---

## ✅ Resultado Esperado

Después de esta corrección:

✅ La secuencia `SEQ_LOTE` se lee correctamente  
✅ Los lotes se crean sin errores de tipo  
✅ Los nombres de lote se generan secuencialmente: `LOTE_00000001`, `LOTE_00000002`, etc.  
✅ Los archivos página cambian a estado 2 (Pendiente de Preparar imágenes)  
✅ Las relaciones se guardan en `TD_LOTE_ARCHIVOS`  

**La funcionalidad de Preparación de Lotes ahora debe funcionar completamente.** 🎉
