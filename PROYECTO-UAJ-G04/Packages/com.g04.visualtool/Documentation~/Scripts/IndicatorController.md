## Indicator Controller

Será el encargado de gestionar los indicadores, lo primero que hace es asegurarse de que solo existe uno en la escena. Luego durante el transcurso del juego se encargará de crear, actualizar y eliminar los indicadores.


## Valores editables

* **int Range(1,100)** *Circle Size* 

 Hace referencia al porcentaje de pantalla que se desea que ocupen los indicadores.

* **Transform** *Player* 

 Referencia a la posicion del objeto que desea saber en que direccion se encuentran los sonidos  

## Métodos públicos

### AskForID()
#### Descripción
---
Gestiona la ID del indicador y garantiza que no haya dos iguales siempre y cuando haya menos de 18,446,744,073,709,551,615
#### Devuelve
---
```c
UInt64 
```
## ClearAll()
##### Descripción
---
Se encargá de borrar todos los indicadores que existen actualmente.

#### Devuelve
---
```c#
void
```

### ReceiveSound(IndicatorInfo cSound)
#### Descripción
---
Añade el sonido a la cola de sonidos que se deben procesar en el próximo ciclo
#### Parámetros
---
[IndicatorInfo](./IndicatorInfo.md) ***cSound*** debe contener la informacion del indicador 
#### Devuelve
---
```c#
void
```

### AddIndicator(UInt64 id,GameObject go)
#### Descripción
---
Añade un indicador seteando su padre y lo activa en la escena
#### Parámetros
---
* **UInt64** *id* 

Id del indicador que se desea generar debe haberse pedido antes mediante AskForID() a este script, si no se hace puede darse que dos Indicadores tengan la misma ID lo que derivaría en un fallo en ejecucción.

* **GameObject** *go* 

El gameobject que representa el indicador.
#### Devuelve
---
```c#
void
```

### RemoveIndicartor(UInt64 id)
#### Descripción
---
Se encargá de borrar todos los indicadores que existen actualmente.
#### Parámetros
---
**UInt64** ***id*** añade el indicador cuya id coincide a la lista de indicadores a eliminar
#### Devuelve
---
```c#
void
```
