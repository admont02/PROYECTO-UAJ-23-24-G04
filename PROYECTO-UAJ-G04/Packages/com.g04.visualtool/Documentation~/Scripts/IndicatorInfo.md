# Indicator Info

Se trata de una clase que actua a modo de contenedor de información. 
En ella se gestiona la información del indicador necesaria para gestionar su correcto funcionamiento.

## Atributos

### m_ObjectPosition
#### Descripción
---
```
Posición del sonido en el mundo de juego.
```

### m_RawImage
#### Descripción
---
```
Textura del indicador que tiene asociado un material. 
```

### m_ListenableDistance
#### Descripción
---
```
Distancia a la que se puede escuchar el sonido.
```

### m_Color
#### Descripción
---
```
Color del indicador del sonido.
```

### m_IndicatorFactor
#### Descripción
---
```
Factor por el que se multiplicará el standardSize del indicador.
```

### m_Sprite
#### Descripción
---
```
Icono asociado al indicador del sonido.
```

### m_SpriteFactor
#### Descripción
---
```
Factor de escala del icono asociado al indicador.
```

### m_id
#### Descripción
---
```
ID del indicador.
```

### m_vibration
#### Descripción
---
```
Vibración del indicador.
```

## Métodos públicos
### public IndicatorInfo(Vector3 pos, UnityEngine.UI.RawImage rawImage, float listenableDistance, Color color, float indicatorFactor, Sprite sprite, float spriteFactor, UInt64 id, float vibration)

#### Descripción
---
Constructor de la clase
#### Parámetros
---
* **Vector3** *pos* 

Posición del sonido en el mundo de juego. 

* **RawImage** *rawImage* 

Textura del indicador que tiene asociado un material.

* **float** *listenableDistance* 

Distancia a la que se puede escuchar el sonido.

* **Color** *color* 

Color del indicador del sonido.

* **float** *indicatorFactor* 

Factor de escala del indicador.

* **Sprite** *sprite* 

Icono asociado al indicador del sonido.

* **float** *spriteFactor* 

Factor de escala del icono asociado al indicador.

* **float** *maxDistance* 

Distancia máxima a la que se puede escuchar el sonido.

* **UInt64** *id* 

Identificador único del sonido.

#### Devuelve
---
```c#
class IndicatorInfo
```
### public IndicatorInfo
#### Descripción
---
Constructor vacío de la clase.
#### Devuelve
---
```c#
class IndicatorInfo
```